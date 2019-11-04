using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientApp.BL;
using Emgu.CV;
using Emgu.CV.Structure;
using MLP.Models.OutputModels;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCapture Capture;
        private int Counter = 0;
        private string StartupFolder;
        private string Folder = "CameraSets";
        private string ProcessedFolder = "ProcessedImages";
        private string Separator = " ";
        private IEnumerable<NeuralNetworkDto> NeuralNetworks;
        private NeuralNetworkTrainingConfigDto TrainingConfig;
        private ImageProcessingConfigDto ImageProcessingConfig;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartupFolder = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
            Capture = new VideoCapture(1);
            NeuralNetworks = NeuralNetworkService.GetNeuralNetworks();

            cmbNeuralNetwork.ItemsSource = NeuralNetworks;
            cmbNeuralNetwork.DisplayMemberPath = "Name";
            SetDefaultImage();
            DeleteTemporalImages();
        }

        private void CmbNeuralNetwork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbPredictedObject.Items.Clear();
            var nn = cmbNeuralNetwork.SelectedValue;
            if(nn != null)
            {
                btnCreateFile.IsEnabled = true;
                TrainingConfig = TrainingService.GetTrainingConfig(((NeuralNetworkDto)nn).Id);
                ImageProcessingConfig = NeuralNetworkService.GetImageProcessingActiveConfig(((NeuralNetworkDto)nn).Id);
                cmbPredictedObject.ItemsSource = TrainingConfig.PredictedObjects;
                cmbPredictedObject.DisplayMemberPath = "ObjectName";
            }
            else
            {
                btnCreateFile.IsEnabled = false;
            }
        }

        private void CmbPredictedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbPredictedObject.SelectedValue != null)
            {
                btnCapture.IsEnabled = true;                
                Counter = 0;
                lblCount.Content = $"Count: {Counter}";
            }
            else
            {
                btnCapture.IsEnabled = false;                
            }
            
        }

        private void BtnCapture_Click(object sender, RoutedEventArgs e)
        {
            btnCapture.IsEnabled = false;
            var dir = GetImageSetFolder(((NeuralNetworkDto)cmbNeuralNetwork.SelectedValue).Id);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Image<Bgr, Byte> ColorImage = Capture.QueryFrame().ToImage<Bgr, Byte>();
            Bitmap bitmap = ColorImage.Bitmap;
            dir = System.IO.Path.Combine(dir, $"{((PredictedObjectDto)cmbPredictedObject.SelectedValue).ObjectName}_{DateTime.Now.ToString("MMddyyyyHHmmssffff")}.bmp");
            bitmap.Save(dir, ImageFormat.Bmp);
            ImageSource source = new BitmapImage(new Uri(dir));
            ctrCaptureImage.Source = source;
            lblCount.Content = $"Count: {++Counter}";
            btnCapture.IsEnabled = true;
        }

        private string GetImageSetFolder(Guid Id)
        {
            var dir = System.IO.Path.Combine(StartupFolder, Folder);
            dir = System.IO.Path.Combine(dir, Id.ToString());

            return dir;
        }

        private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            btnCapture.IsEnabled = false;
            var dir = GetImageSetFolder(((NeuralNetworkDto)cmbNeuralNetwork.SelectedValue).Id);

            if (!Directory.Exists(dir))
            {
                return;
            }

            string[] files = Directory.GetFiles(dir, "*.bmp");

            if(files.Count() > 0)
            {
                dir = System.IO.Path.Combine(dir, ProcessedFolder);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                List<string> finalFile = new List<string>();
                foreach(string file in files)
                {
                    var img = new Image<Bgr, byte>(file);
                    var imgProcessed = MLP.ImageProcessing.ClientApp.ProcessImageClientApp(img, ImageProcessingConfig);
                    var name = System.IO.Path.GetFileName(file);//.Split('_')[0];

                    var objPredited = TrainingConfig.PredictedObjects.Where(po => po.ObjectName == name.Split('_')[0]).FirstOrDefault();

                    finalFile.Add(
                        TrainingDatasetHelper.ListToString(MLP.ImageProcessing.ClientApp.ProcessImageClientAppBinarized(imgProcessed, ImageProcessingConfig), Separator) +
                        TrainingDatasetHelper.GetSetY(objPredited.Index, TrainingConfig.PredictedObjects.Count, Separator)
                        );

                    Bitmap bitmap = imgProcessed.Bitmap;
                    bitmap.Save(System.IO.Path.Combine(dir,name), ImageFormat.Bmp);
                }
                var to = System.IO.Path.Combine(TrainingConfig.TrainingDatabaseFileRoute, txtFileName.Text);

                using (StreamWriter writer = new System.IO.StreamWriter(to))
                {
                    foreach (var lineValue in finalFile)
                    {
                        writer.WriteLine(lineValue);
                    }
                }
            }            
        }

        private void BtnPreview_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultImage();
            Image<Bgr, Byte> ColorImage = Capture.QueryFrame().ToImage<Bgr, Byte>();            
            //Bitmap bitmap = ColorImage.Bitmap;
            Bitmap bitmap = MLP.ImageProcessing.ClientApp.ProcessImageClientApp(ColorImage, ImageProcessingConfig).Bitmap;
            
            var final = System.IO.Path.Combine(StartupFolder, $"Preview/preview_{DateTime.Now.ToString("MMddyyyyHHmmssffff")}.bmp");
            //if (File.Exists(final))
            //{
            //    File.Delete(final);
            //}
            bitmap.Save(final, ImageFormat.Bmp);

            //var bmi = new BitmapImage(new Uri(final));
            //bmi.CacheOption = BitmapCacheOption.OnLoad;

            ImageSource source = new BitmapImage(new Uri(final));
            ctrCaptureImage.Source = source;

            txtBase64.Text = Convert.ToBase64String(ColorImage.Bytes);
        }

        private void SetDefaultImage()
        {
            var defaultImage = System.IO.Path.Combine(StartupFolder, "default.png");
            ImageSource source = new BitmapImage(new Uri(defaultImage));
            ctrCaptureImage.Source = source;
        }

        private void DeleteTemporalImages()
        {
            string[] files = Directory.GetFiles(System.IO.Path.Combine(StartupFolder, "Preview"), "*.bmp");

            if (files.Count() > 0)
            {                
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
