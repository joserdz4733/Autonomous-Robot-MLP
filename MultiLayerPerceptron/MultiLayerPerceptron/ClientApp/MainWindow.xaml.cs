using ClientApp.Models;
using Emgu.CV;
using Emgu.CV.Structure;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INeuralNetworkRepoService _neuralNetworkRepoService;
        private readonly IImageProcessingConfigService _imageProcessingConfigService;
        private readonly MainConfig _config;

        private readonly VideoCapture _capture;
        private int _counter = 0;
        private IEnumerable<NeuralNetworkDto> _neuralNetworks;
        private NeuralNetworkTrainingConfigDto _trainingConfig;
        private ImageProcessingConfig _imageProcessingConfig;

        public MainWindow(INeuralNetworkRepoService neuralNetworkRepoService,
            IImageProcessingConfigService imageProcessingConfigService)
        {
            _neuralNetworkRepoService = neuralNetworkRepoService;
            _imageProcessingConfigService = imageProcessingConfigService;
            _config = GetConfig();
            _capture = new VideoCapture(_config.CamIndex);
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _neuralNetworks = await _neuralNetworkRepoService.GetNeuralNetworks();
            CmbNeuralNetwork.ItemsSource = _neuralNetworks;
            DeleteTemporalImages();
        }

        private async void CmbNeuralNetwork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbPredictedObject.Items.Clear();
            var selectedNetwork = CmbNeuralNetwork.SelectedValue;
            if (selectedNetwork == null)
            {
                BtnCreateFile.IsEnabled = false;
                return;
            }
            
            _trainingConfig =
                await _neuralNetworkRepoService.GetTrainingConfigDto(((NeuralNetworkDto) selectedNetwork).Id);
            _imageProcessingConfig = await
                _imageProcessingConfigService.GetActiveImageProcessingConfigByNetworkId(
                    ((NeuralNetworkDto) selectedNetwork).Id);
            if (_imageProcessingConfig != null)
            {
                ChkPreviewBinarized.IsEnabled = true;
                BtnCreateFile.IsEnabled = true;
            }
            CmbPredictedObject.ItemsSource = _trainingConfig.PredictedObjects;
        }

        private void CmbPredictedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbPredictedObject.SelectedValue != null)
            {
                BtnCapture.IsEnabled = true;
                UpdateCounter(true);
            }
            else
            {
                BtnCapture.IsEnabled = false;
            }
        }

        private void BtnCapture_Click(object sender, RoutedEventArgs e)
        {
            BtnCapture.IsEnabled = false;
            var dir = GetImageSetFolder(((NeuralNetworkDto) CmbNeuralNetwork.SelectedValue).Id);
            var bitmap = _capture.QueryFrame().ToBitmap();
            dir = Path.Combine(dir, GetBmpName(((PredictedObjectDto)CmbPredictedObject.SelectedValue).ObjectName));
            bitmap.Save(dir, ImageFormat.Bmp);
            ImageSource source = new BitmapImage(new Uri(dir));
            CtrCaptureImage.Source = source;
            UpdateCounter();
            BtnCapture.IsEnabled = true;
        }

        private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            BtnCapture.IsEnabled = false;
            var imageSetFolder = GetImageSetFolder(((NeuralNetworkDto) CmbNeuralNetwork.SelectedValue).Id);
            var files = Directory.GetFiles(imageSetFolder, "*.bmp");
            if (!files.Any())
            {
                return;
            }

            var processedFolder = GetAppFolder(_config.ProcessedFolder);
            var finalFile = new List<string>();
            foreach (var file in files)
            {
                var image = new Image<Bgr, byte>(file);
                var name = Path.GetFileName(file);
                var entries = image.ProcessImageMlp(_imageProcessingConfig);
                var objPredicted = _trainingConfig.PredictedObjects.Single(po => po.ObjectName == name.Split('_')[0]);
                finalFile.Add(string.Join(_config.Separator, entries) + GetResponseSet(objPredicted.Index,
                    _trainingConfig.PredictedObjects.Count, _config.Separator));

                if (!_config.SaveImage) continue;
                var imgToSave = image.ProcessImageClientApp(_imageProcessingConfig);
                var bitmapToSave = imgToSave.ToBitmap();
                bitmapToSave.Save(Path.Combine(processedFolder, name), ImageFormat.Bmp);
            }

            CreateDirectoryIfNotExist(_trainingConfig.TrainingDatabaseFileRoute);
            var to = Path.Combine(_trainingConfig.TrainingDatabaseFileRoute, TxtFileName.Text);
            using (var writer = new StreamWriter(to))
            {
                foreach (var lineValue in finalFile)
                {
                    writer.WriteLine(lineValue);
                }
            }
        }

        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            var colorImage = _capture.QueryFrame().ToImage<Bgr, byte>();
            var bitmap = ChkPreviewBinarized.IsChecked == true
                ? colorImage.ProcessImageClientApp(_imageProcessingConfig).ToBitmap()
                : colorImage.ToBitmap();
            var final = Path.Combine(GetAppFolder(_config.PreviewFolder), GetBmpName("preview"));
            bitmap.Save(final, ImageFormat.Bmp);

            ImageSource source = new BitmapImage(new Uri(final));
            CtrCaptureImage.Source = source;
            TxtBase64.Text = Convert.ToBase64String(colorImage.Bytes);
        }

        private static string GetResponseSet(int index, int qty, string separator)
        {
            var y = "";
            for (var j = 0; j < qty; j++)
            {
                if (index == j + 1)
                {
                    y += "1" + separator;
                }
                else
                {
                    y += "0" + separator;
                }
            }

            return y;
        }

        private void DeleteTemporalImages()
        {
            //File.Delete(GetAppFolder(_config.PreviewFolder));
            var files = Directory.GetFiles(GetAppFolder(_config.PreviewFolder), "*.bmp");
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        private string GetImageSetFolder(Guid id)
        {
            var dir = Path.Combine(GetAppFolder(_config.CameraSetFolder), id.ToString());
            return CreateDirectoryIfNotExist(dir);
        }

        private string GetAppFolder(string specialFolder)
        {
            var dir = Path.Combine(_config.DefaultRootFolder, specialFolder);
            return CreateDirectoryIfNotExist(dir);
        }

        private string CreateDirectoryIfNotExist(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        private void UpdateCounter(bool reset = false)
        {
            _counter = reset ? 0 : _counter + 1;
            LblCount.Content = $"Count: {_counter}";
        }

        private string GetBmpName(string preName) => $"{preName}_{DateTime.Now:MMddyyyyHHmmssffff}.bmp";

        private MainConfig GetConfig() =>
            new MainConfig
            {
                Separator = ConfigurationManager.AppSettings["DataSetSeparator"],
                CameraSetFolder = ConfigurationManager.AppSettings["DefaultCameraFolderName"],
                ProcessedFolder = ConfigurationManager.AppSettings["ProcessedFolderName"],
                PreviewFolder = ConfigurationManager.AppSettings["PreviewFolderName"],
                CamIndex = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCameraIndex"]),
                SaveImage = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveImagesAfterProcessedSet"]),
                DefaultRootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ConfigurationManager.AppSettings["RootFolderName"])
            };
    }
}
