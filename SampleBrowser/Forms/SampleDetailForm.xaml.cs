using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SamplesModel.Entities;
using SampleBrowser.SamplesModel.Entities;
using SampleBrowser.Common;
using System.IO;

namespace SampleBrowser.Forms
{
    /// <summary>
    /// Interaction logic for SampleDetailForm.xaml
    /// </summary>
    public partial class SampleDetailForm : Window
    {
        SearchResult _sr = null;
        public SampleDetailForm()
        {
            InitializeComponent();
        }

        public SampleDetailForm(SearchResult searchResult)
        {
            InitializeComponent();
            _sr = searchResult;
            this.DataContext = searchResult;

            this.InitialWebBrowserReadMeContent(searchResult);
            //this.wbVote.Navigate("http://1code.codeplex.com");

            this.Title = string.Concat(searchResult.Samples.Name, " Details");
        }

        private string InitialWebBrowserReadMeContent(SearchResult searchResult)
        {
            string[] webExtentions = { ".htm", ".html", ".mht" };

            this.wbReadMe.Visibility = Visibility.Collapsed;
            this.txtReadMe.Visibility = Visibility.Collapsed;

            string sourcePath = Constants.currentPath;
            string readMeDirectory = System.IO.Path.Combine(sourcePath,
                searchResult.Samples.VsVersion == 2008 ? "Visual Studio 2008" : "Visual Studio 2010", searchResult.Samples.Name);

            DirectoryInfo di = new DirectoryInfo(readMeDirectory);
            FileInfo[] fis = di.GetFiles("*", SearchOption.TopDirectoryOnly);
            string fileName = string.Empty;
            foreach (FileInfo fi in fis)
            {
                fileName = fi.Name;
                if (fileName.Replace(fi.Extension, "").ToLower() == "readme")
                {
                    StreamReader streamReader = new StreamReader(fi.FullName, Encoding.Default);

                    if (webExtentions.Contains(fi.Extension))
                    {
                        this.wbReadMe.Visibility = Visibility.Visible;
                        this.wbReadMe.NavigateToString(streamReader.ReadToEnd());
                    }
                    else
                    {
                        this.txtReadMe.Visibility = Visibility.Visible;
                        this.txtReadMe.Text = streamReader.ReadToEnd();
                    }
                    return null;
                }
            }
            return null;
        }

        private void btnOpenInVisual_Click(object sender, RoutedEventArgs e)
        {
            string slnPath = System.IO.Path.Combine(Constants.currentPath, _sr.SlnPath.Trim('\\'));
            try
            {
                System.Diagnostics.Process.Start(slnPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The file cannot be opened", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
