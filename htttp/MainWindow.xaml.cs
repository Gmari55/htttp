using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace htttp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Regex onlyNumbers = new Regex("[^0-9.-]+");
      
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddHistoryItem(string fileName)
        {
            historyList.Items.Add($"{DateTime.Now.ToShortTimeString()}: {fileName}");
        }
        private async void DownloadBtnClick(object sender, RoutedEventArgs e)
        {
            int width = int.Parse(WightTxtbox.Text);
            int height = int.Parse(HeightTxtbox.Text);
            string category = Categories.Text;
            string url = @$"https://source.unsplash.com/random/{width}x{height}/?{category}&1.png";


            string name = Guid.NewGuid().ToString();

            string extension = Path.GetExtension(url);

            string dest = Path.Combine(FolderTxtBox.Text, $"{name}.{extension}");


            if (!Directory.Exists(FolderTxtBox.Text))
                Directory.CreateDirectory(FolderTxtBox.Text);

            WebClient webClient = new WebClient();

            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.QueryString.Add("file", dest);
            webClient.DownloadFileAsync(new Uri(url), dest);




            AddHistoryItem(url);
        }

        private void WebClient_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.Title = $"Download Completed";
            string fileIdentifier = ((System.Net.WebClient)(sender)).QueryString["file"];
          
           
                Uri fileUri = new Uri(fileIdentifier);
                img.Source = new BitmapImage(fileUri);
            
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
               FolderTxtBox.Text= dialog.SelectedPath;
            }
           

        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            return !onlyNumbers.IsMatch(text);
        }
    }
}
