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
using System.Windows.Navigation;
using System.Windows.Shapes;

using SampleBrowser.View;
using SampleBrowser.Forms;
using SampleBrowser.SamplesModel.Entities;
using System.Collections.ObjectModel;
using CodeBoxControl;
using CodeBoxControl.Decorations;
using SampleBrowser.Common.MultiThread;
using System.ComponentModel;
using SampleBrowser.Common;
using System.Runtime.InteropServices;
namespace SampleBrowser.UserControls
{
    /// <summary>
    /// parent windows form transfer three param to child form
    /// </summary>
    public class MergeParam
    {
        public ObservableList<SearchResult> ol;
        public string strCondition;
        public string languagesCondition;
        public string technologiesCondition;
    }

    /// <summary>
    /// Interaction logic for SampleListBox.xaml
    /// </summary>
    
    public partial class SampleListBox : UserControl
    {

        protected string Condition = null;
        public SampleListBox()
        {
            InitializeComponent();

            //this.samplesListBox.MouseDoubleClick += samplesListBox_MouseDoubleClick;
        }

        public SampleListBox(ObservableCollection<SearchResult> dataSource,string strCondition)
        {
            InitializeComponent();

            CollectionViewSource collectionviewsource= this.FindResource("sams") as CollectionViewSource;
            collectionviewsource.Source = dataSource;
            Condition = strCondition;
            

            //this.samplesListBox.MouseDoubleClick += samplesListBox_MouseDoubleClick;
        }

        

        void samplesListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            //string filePath = (this.samplesListBox.SelectedItem as SearchResult).FilePath;
            //MessageBox.Show(filePath);
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            CollectionViewGroup cvg = (sender as GroupItem).Content as CollectionViewGroup;
            
            GroupItem lbi = ((GroupItem)e.Source);

            
            GeneralTransform transform = lbi.TransformToAncestor(this);
            Point topLeft = transform.Transform(new Point(0, 0));
            Point bottomRight = transform.Transform(new Point(lbi.ActualWidth, 63.69));
         
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            //when mouse coordinate Falls in rectangular which don't expand,popup detailWinodws to show readme
            if (pX > topLeft.X && pX < bottomRight.X && pY > topLeft.Y && pY < bottomRight.Y)
            {
            
                if (((sender as GroupItem).Content as CollectionViewGroup).Items.Count > 0)
                {
                    SampleDetailForm frm = new SampleDetailForm(((sender as GroupItem).Content as CollectionViewGroup).Items[0] as SearchResult);
                    frm.Show();
                }
            }
        }

   


        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            CodeBox sv = SampleBrowser.Common.VisualTreeHelperEx.FindVisualChildByName<CodeBox>((DockPanel)sender, "doc");


            CodeBoxControl.DecorationConditions.DecorationCondition dc = new CodeBoxControl.DecorationConditions.DecorationCondition();
            DecorationSchemes ds = new DecorationSchemes();
            MultiRegexWordDecoration BlueWords = new MultiRegexWordDecoration();
            BlueWords.Brush = new SolidColorBrush(Color.FromRgb(191,124,0));


            string[] sArray = this.Tag.ToString().Split(' ');
            foreach (string i in sArray)

            BlueWords.Words = ds.CSharpReservedWords(i);


            dc.BaseDecorations.Add(BlueWords);
            
            sv.DecorationCondition = dc;
          
         
        }



        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
          //when Control image is loading, its property source should be replaced if no condition.
            Image img = e.Source as Image;
            string strImagePath = "/SampleBrowser;component/Images/Dot.png";
          BitmapImage image = new BitmapImage(new Uri(strImagePath, UriKind.Relative));
          if (Condition=="")
              img.Source = image;
     
        }


        /// <summary>
        /// iter child frame element from  parent element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<T> FindChildren<T>(DependencyObject parent) where T : class
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if (t != null)
                        yield return t;

                    var children = FindChildren<T>(child);
                    foreach (var item in children)
                        yield return item;
                }
            }
        }
    }
     
}


