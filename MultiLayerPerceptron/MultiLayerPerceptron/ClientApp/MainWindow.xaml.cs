using Emgu.CV;
using Emgu.CV.Structure;
using MLP.Models.OutputModels;
using MultiLayerPerceptron.Application.Extensions;
using MultiLayerPerceptron.Application.Interfaces;
using MultiLayerPerceptron.Contract.Dtos;
using MultiLayerPerceptron.Data.Entities;
using System;
using System.Collections.Generic;
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
        private readonly IImageProcessingService _imageProcessingService;
        private string _startupFolder;
        private VideoCapture _capture;
        private int _counter = 0;
        private IEnumerable<NeuralNetworkDto> _neuralNetworks;
        private NeuralNetworkTrainingConfigDto _trainingConfig;
        private ImageProcessingConfig _imageProcessingConfig;
        private const string Folder = "CameraSets";
        private const string ProcessedFolder = "ProcessedImages";
        private const string Separator = " ";
        private readonly bool _saveImage = true;

        public MainWindow(INeuralNetworkRepoService neuralNetworkRepoService,
            IImageProcessingConfigService imageProcessingConfigService, IImageProcessingService imageProcessingService)
        {
            _neuralNetworkRepoService = neuralNetworkRepoService;
            _imageProcessingConfigService = imageProcessingConfigService;
            _imageProcessingService = imageProcessingService;
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _startupFolder = new DirectoryInfo(Environment.CurrentDirectory).Parent?.Parent?.FullName;
            // TODO cam index by settings
            _capture = new VideoCapture();
            _neuralNetworks = await _neuralNetworkRepoService.GetNeuralNetworks();
            CmbNeuralNetwork.ItemsSource = _neuralNetworks;
            CmbNeuralNetwork.DisplayMemberPath = "Name";

            SetDefaultImage();
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

            BtnCreateFile.IsEnabled = true;
            _trainingConfig =
                await _neuralNetworkRepoService.GetTrainingConfigDto(((NeuralNetworkDto) selectedNetwork).Id);
            _imageProcessingConfig = await
                _imageProcessingConfigService.GetActiveImageProcessingConfigByNetworkId(
                    ((NeuralNetworkDto) selectedNetwork).Id);

            CmbPredictedObject.ItemsSource = _trainingConfig.PredictedObjects;
            CmbPredictedObject.DisplayMemberPath = "ObjectName";
        }

        private void CmbPredictedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbPredictedObject.SelectedValue != null)
            {
                BtnCapture.IsEnabled = true;
                _counter = 0;
                lblCount.Content = $"Count: {_counter}";
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

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var bitmap = _capture.QueryFrame().ToBitmap();
            dir = System.IO.Path.Combine(dir,
                $"{((PredictedObjectDto) CmbPredictedObject.SelectedValue).ObjectName}_{DateTime.Now:MMddyyyyHHmmssffff}.bmp");
            bitmap.Save(dir, ImageFormat.Bmp);
            ImageSource source = new BitmapImage(new Uri(dir));
            CtrCaptureImage.Source = source;
            lblCount.Content = $"Count: {++_counter}";
            BtnCapture.IsEnabled = true;
        }

        private string GetImageSetFolder(Guid id)
        {
            var dir = System.IO.Path.Combine(_startupFolder, Folder);
            dir = System.IO.Path.Combine(dir, id.ToString());

            return dir;
        }

        private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            BtnCapture.IsEnabled = false;
            var dir = GetImageSetFolder(((NeuralNetworkDto) CmbNeuralNetwork.SelectedValue).Id);

            if (!Directory.Exists(dir))
            {
                return;
            }

            var files = Directory.GetFiles(dir, "*.bmp");
            if (!files.Any())
            {
                return;
            }

            dir = System.IO.Path.Combine(dir, ProcessedFolder);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var finalFile = new List<string>();
            // TODO add setting for saving the picture or not
            foreach (var file in files)
            {
                var image = new Image<Bgr, byte>(file);

                var name = System.IO.Path.GetFileName(file);
                var entries = image.ProcessImageMlp(_imageProcessingConfig);
                var objPredicted = _trainingConfig.PredictedObjects.Single(po => po.ObjectName == name.Split('_')[0]);
                finalFile.Add(string.Join(Separator, entries) + GetResponseSet(objPredicted.Index,
                    _trainingConfig.PredictedObjects.Count, Separator));

                if (!_saveImage) continue;
                var imgToSave = image.ProcessImageClientApp(_imageProcessingConfig);
                var bitmapToSave = imgToSave.ToBitmap();
                bitmapToSave.Save(System.IO.Path.Combine(dir, name), ImageFormat.Bmp);
            }

            var to = System.IO.Path.Combine(_trainingConfig.TrainingDatabaseFileRoute, TxtFileName.Text);
            using (var writer = new System.IO.StreamWriter(to))
            {
                foreach (var lineValue in finalFile)
                {
                    writer.WriteLine(lineValue);
                }
            }
        }

        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultImage();
            var colorImage = _capture.QueryFrame().ToImage<Bgr, byte>();
            var bitmap = colorImage.ProcessImageClientApp(_imageProcessingConfig).ToBitmap();
            var final = System.IO.Path.Combine(_startupFolder,
                $"Preview/preview_{DateTime.Now:MMddyyyyHHmmssffff}.bmp");
            bitmap.Save(final, ImageFormat.Bmp);

            ImageSource source = new BitmapImage(new Uri(final));
            CtrCaptureImage.Source = source;
            TxtBase64.Text = Convert.ToBase64String(colorImage.Bytes);
        }

        private void SetDefaultImage()
        {
            var defaultImage = System.IO.Path.Combine(_startupFolder, "default.png");
            ImageSource source = new BitmapImage(new Uri(defaultImage));
            CtrCaptureImage.Source = source;
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
            var files = Directory.GetFiles(System.IO.Path.Combine(_startupFolder, "Preview"), "*.bmp");

            if (!files.Any())
            {
                return;
            }

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}
