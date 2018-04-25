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
using SamplesViewModel.ViewModel;
using System.Configuration;
using System.Collections.ObjectModel;

using SamplesModel.Entities;
using SampleBrowser.Forms;
using SamplesModel.Enum;
using SampleBrowser.View;
using SamplesController.Services;
using SampleBrowser.SamplesModel.Entities;
using SampleBrowser.Lucene.LuceneSearcher;
using SampleBrowser.Common;
using SampleBrowser.UserControls;
using SampleBrowser.Common.MultiThread;
using System.ComponentModel;
using System.Threading;
using CodeBoxControl;
using CodeBoxControl.DecorationConditions;



namespace SampleBrowser
{
   
    public class MergeParam
    {
       public ObservableList<SearchResult> ol;
       public string strCondition;
       public string languagesCondition ;
       public string technologiesCondition;
       public int flag; //indicate add manuReset or not
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string txtCondition;
        public string languagesCondition;
        public string technologiesCondition;
        public string currecttabitem;
        public ManualResetEvent mre = new ManualResetEvent(true);
        public List<ManualResetEvent> mreList = new List<ManualResetEvent>();
        public MainWindow()
        {
            InitializeComponent();

            #region Loaded all samples from xml, will be removed
            //mainVM = new MainWindowViewModel();
            //this.DataContext = mainVM.AllSearchResult;           
            #endregion            

            this.searchControl.btnSearch.Click += new RoutedEventHandler(btnSearch_Click);
            this.searchControl.txtCondition.KeyDown += new KeyEventHandler(txtCondition_KeyDown);
        }


        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitialTabConatainer();
        }

        private void InitialTabConatainer()
        {
            this.GetSearchResult();

            #region Will need loaded some default tabs.


            MergeParam mp1 = new MergeParam();
            mp1.strCondition = "";
            //txtCondition = "";
            mp1.languagesCondition = "All Languages";
            mp1.technologiesCondition = "ASP.NET";
            string tabHeader = string.Format("C:{0} T:{1} L:{2}", "", "ASP.NET", "All Languages");
            this.CreateNewTabItem(tabHeader, mp1,2,2);


            MergeParam mp2 = new MergeParam();
            mp2.strCondition = "";
            //txtCondition = "";
            mp2.languagesCondition = "All Languages";
            mp2.technologiesCondition = "Silverlight";
            tabHeader = string.Format("C:{0} T:{1} L:{2}", "", "Silverlight", "All Languages");
            this.CreateNewTabItem(tabHeader, mp2,2,2);


            MergeParam mp3 = new MergeParam();
            mp3.strCondition = "";
            //txtCondition = "";
            mp3.languagesCondition = "C#";
            mp3.technologiesCondition = "All Technologies";
            tabHeader = string.Format("C:{0} T:{1} L:{2}", "", "All Technologies", "C#");
            this.CreateNewTabItem(tabHeader, mp3,2,2);


            MergeParam mp4 = new MergeParam();
            mp4.strCondition = "";
            //txtCondition = "";
            mp4.languagesCondition = "VB";
            mp4.technologiesCondition = "All Technologies";
            tabHeader = string.Format("C:{0} T:{1} L:{2}", "", "All Technologies", "VB");
            this.CreateNewTabItem(tabHeader, mp4,2,2);

            #endregion

            
          
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            #region Create Lucene Index

            //SamplesService.CreateLuceneIndex();
            //MessageBox.Show("Create index success.");

            #endregion

            this.GetSearchResult();

          
             
           //IEnumerable<CodeBoxControl.CodeBox> AllCodeBox = FindChildren<CodeBoxControl.CodeBox>(this);
            //CodeBoxControl.CodeBox cb = VisualTreeHelperEx.FindVisualChildByName<CodeBoxControl.CodeBox>(sampleCollectionsTabContainer, "document");
            
            //CodeBoxControl.DecorationConditions.DecorationCondition dc = new CodeBoxControl.DecorationConditions.DecorationCondition();
            //dc._txtCondition = "test";
            //foreach (CodeBoxControl.CodeBox codeBox in AllCodeBox)
            //{

            //    codeBox.SetValue(CodeBoxControl.CodeBox.DecorationConditionProperty, dc);
                //MessageBox.Show(string.Format("第{0}个按钮[{1}]的内容为：{2}", i, btn.Name, btn.Content));
            //}
           
        }

        void txtCondition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.GetSearchResult();
            }
        }

        /// <summary>
        /// According search codition to get search result
        /// </summary>
        void GetSearchResult()
        {
            txtCondition = this.searchControl.txtCondition.Text.Trim();
            languagesCondition = this.searchControl.cbBoxLanguage.Text;
            technologiesCondition = this.searchControl.cbBoxTechnology.Text;

            string tabHeader = string.Format("C:{0} T:{1} L:{2}", txtCondition.Trim(), technologiesCondition, languagesCondition);

            MergeParam mp=new MergeParam();
            mp.strCondition=txtCondition;
            mp.languagesCondition = languagesCondition;
            mp.technologiesCondition = technologiesCondition;
            this.CreateNewTabItem(tabHeader, mp,1,1);
        }

       

        /// <summary>
        /// Create new tab item according search codition
        /// </summary>
        /// <param name="tabHeader"></param>
        /// <param name="lst"></param>
        private void CreateNewTabItem(string tabHeader,MergeParam mp,int i,int j)   //dataSource may should be instead of List<Sample> lst
        {
            TabItem tabItem = new TabItem();

            Grid grd = new Grid();
            RowDefinition row1 = new RowDefinition() { Height = new GridLength(35) };
            ColumnDefinition column1 = new ColumnDefinition() { Width = new GridLength(90) };
            ColumnDefinition column2 = new ColumnDefinition { Width = new GridLength(20) };
            grd.RowDefinitions.Add(row1);
            grd.ColumnDefinitions.Add(column1);
            grd.ColumnDefinitions.Add(column2);

            TextBlock tb = new TextBlock();
            tb.Name = "itemTabName";
            tb.FontFamily = new FontFamily("Arial");
            tb.Foreground = Brushes.White;

            string tempTabHeader = tabHeader.Replace("C:", "").Replace("T:", "").Replace("L:", "")
                                            .Replace("All Languages", "").Replace("All Technologies", "").Trim();
            tb.Text = string.IsNullOrEmpty(tempTabHeader) ? "All" : tempTabHeader;
            tb.Tag = tabHeader.Trim();   //for hidden store tab header in order to when click the tab header can associated with search codition controls.

            Thickness tc = new Thickness(10, 5, 10, 5);
            tb.Margin = tc;
            tb.FontSize = 13;
            tb.ToolTip = tb.Text;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.SetValue(Grid.ColumnProperty, 0);
            grd.Children.Add(tb);

            Button bt = new Button();
            bt.Content = "×";
            Thickness btcMargin = new Thickness(0, 2, 0, 5);
            bt.Margin = btcMargin;
            Thickness btcPadding = new Thickness(0, 0, 0, 3);
            bt.Padding = btcPadding;

            bt.Width = 14;
            bt.Height = 15;
            bt.Background = Brushes.Gray;
            bt.BorderThickness = new Thickness(0);
            bt.HorizontalAlignment = HorizontalAlignment.Right;
            bt.VerticalAlignment = VerticalAlignment.Center;
            bt.Click += new RoutedEventHandler(itemHeaderButton_Click);
            bt.SetValue(Grid.ColumnProperty, 1);
            grd.Children.Add(bt);

            tabItem.Header = grd;
            tabItem.Width = 130;
            tabItem.Height = 35;

           
            
            ObservableList<SearchResult> ob = new ObservableList<SearchResult>(System.Windows.Threading.Dispatcher.CurrentDispatcher);
            SampleBrowser.UserControls.SampleListBox sampleListBox = new SampleBrowser.UserControls.SampleListBox(ob._observableCollection,mp.strCondition);
            
            tabItem.Content = sampleListBox;
            sampleListBox.Tag = mp.strCondition;
           
            this.sampleCollectionsTabContainer.Items.Add(tabItem);
            
            if(i==1)
            this.sampleCollectionsTabContainer.SelectedItem = tabItem;

     


            ScrollViewer sv = VisualTreeHelperEx.FindVisualChildByName<ScrollViewer>(sampleCollectionsTabContainer, "svTP");
            sv.ScrollToRightEnd();

            //var cb = new CodeBox();
            //CodeBoxControl.DecorationConditions.DecorationCondition dc = new CodeBoxControl.DecorationConditions.DecorationCondition();
            //dc._txtCondition = "DataTable";
            //cb.SetValue(CodeBox.DecorationConditionProperty, dc);
            //this.AddChild(cb);



            //BackgroundWorker _backgroundWorker = new BackgroundWorker();
            //_backgroundWorker.WorkerSupportsCancellation = true;
            //_backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            //_backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);

            

            Thread thrEven = new Thread(new ParameterizedThreadStart(OutPutEven));   
  


         

            mp.ol = ob;

            if (j == 2)
            {
                mp.flag = 2;//indicate add manuReset to block first before click tab control
            }
            else
            {
                mp.flag = 1;
            }


            //_backgroundWorker.RunWorkerAsync(mp);
            thrEven.Start(mp);   
            //ThreadPool.SetMaxThreads(10, 200);
            //ThreadPool.SetMinThreads(2, 40);
            //Mutex mtx = new Mutex(true);

            //ThreadPool.RegisterWaitForSingleObject(
            //mtx.ReleaseMutex();

            


            //tabItem.Tag = _backgroundWorker;

            tabItem.Tag = thrEven;
     
        }


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
      
        /// <summary>
        /// Tab container's tab item remove button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = ((((Button)e.Source).Parent as Grid).Parent) as TabItem;
            //currecttabitem = (string)ti.Tag;

            //click close button should cancel control backgroundworker via property tag
            if (ti.Tag != null)
            {
                //((BackgroundWorker)ti.Tag).CancelAsync();
                ((Thread)ti.Tag).Abort();
            }

            if (ti != null && ti != this.sampleCollectionsTabContainer.Items[0])
            {
                if (ti == (this.sampleCollectionsTabContainer.SelectedItem as TabItem))
                {
                    this.sampleCollectionsTabContainer.Items.Remove(ti);
                }
                else
                {
                    this.sampleCollectionsTabContainer.SelectedItem = ti;
                    this.sampleCollectionsTabContainer.Items.Remove(ti);
                }
            }
        }


        /// <summary>
        /// When click on a tab, the search condition associated with the tab should be restored to the search textbox and the two dropdownlists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sampleCollectionsTabContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.sampleCollectionsTabContainer.Items.Count > 1)
            {
                //IEnumerable<CodeBoxControl.CodeBox> AllCodeBox = FindChildren<CodeBoxControl.CodeBox>(searchControl);
                //ListBox sv = VisualTreeHelperEx.FindVisualChildByName<ListBox>((Visual)sender, "slb");
                //TextBlock tbb = VisualTreeHelperEx.FindVisualChildByName<TextBlock>((Visual)sv, "txtFilePath");
                
                //CodeBoxControl.DecorationConditions.DecorationCondition dc = new CodeBoxControl.DecorationConditions.DecorationCondition();
                //dc._txtCondition = "datatable";
                //if(dc !=null&&cb!=null)
                //cb.DecorationCondition = dc;

                string languagesString = string.Empty;
                string technologiesString = string.Empty;

                if (this.sampleCollectionsTabContainer.SelectedIndex == 0)
                {
                    this.searchControl.txtCondition.Text = string.Empty;
                    this.searchControl.cbBoxLanguage.Text = "All Languages";
                    this.searchControl.cbBoxTechnology.Text = "All Technologies";
                }
                else if (this.sampleCollectionsTabContainer.SelectedIndex > 0)
                {
                    TabItem ti = this.sampleCollectionsTabContainer.SelectedItem as TabItem;
                    Visual v = ti.Header as Visual;
                    TextBlock tb = VisualTreeHelper.GetChild(v, 0) as TextBlock;

                    //when index between 1 and 4 ,generics list including manualReset could set signal again
                    if (sampleCollectionsTabContainer.SelectedIndex > 0 && sampleCollectionsTabContainer.SelectedIndex<5)
                    {
                        mreList[sampleCollectionsTabContainer.SelectedIndex-1].Set();
                    
                    }

                    string tabHeaderContent = tb.Tag.ToString();
                    string result = tabHeaderContent;

                    int c = tabHeaderContent.IndexOf("T:");
                    string conditionString = tabHeaderContent.Substring(0, c).Replace("C:", "").Trim();
                    this.searchControl.txtCondition.Text = conditionString;

                    int t = tabHeaderContent.IndexOf("L:");
                    technologiesString = tabHeaderContent.Substring(c + 2, t - c - 2).Trim();
                    this.searchControl.cbBoxTechnology.Text = technologiesString;

                    languagesString = tabHeaderContent.Substring(t + 2).Trim();
                    this.searchControl.cbBoxLanguage.Text = languagesString;
                }

                this.ValidateLanguagesChechBoxIsCheckOrNot(languagesString);
                this.ValidateTechnologiesChechBoxIsCheckOrNot(technologiesString);
            }
        }

        private void btnPageDown_Click(object sender, RoutedEventArgs e)
        {
            int i = this.sampleCollectionsTabContainer.SelectedIndex;
            if (i != 0)
            {
                this.sampleCollectionsTabContainer.SelectedIndex = i - 1;
            }
        }

        private void btnPageUp_Click(object sender, RoutedEventArgs e)
        {
            int i = this.sampleCollectionsTabContainer.SelectedIndex;
            if (i < this.sampleCollectionsTabContainer.Items.Count - 1)
            {
                this.sampleCollectionsTabContainer.SelectedIndex = i + 1;
            }
        }
        private void OutPutEven(object lst)
        {

            //ObservableList<SearchResult> taskRequests = e.Argument as ObservableList<SearchResult>;

            MergeParam mp = lst as MergeParam;
            ManualResetEvent mreTemp = new ManualResetEvent(false);

            ObservableList<SearchResult> taskRequests = mp.ol;

            if (mp.flag == 2)
            {


                mreList.Add(mreTemp);
            }

            if (mp.flag == 2)
            {

                mreTemp.WaitOne();
            }

            foreach (SearchResult i in (new LuceneSearch()).SearchResultsIterList(mp.strCondition,
                                            mp.languagesCondition, mp.technologiesCondition))
            {
                if (i != null)
                {
                    SearchResult iTemp = new SearchResult();
                    iTemp.FilePath = i.FilePath;
                    iTemp.GroupParameter = i.GroupParameter;
                    iTemp.Samples = i.Samples;
                    iTemp.Score = i.Score;
                    iTemp.SlnPath = i.SlnPath;
                    iTemp.Snippet = i.Snippet;

                    //using (TimedLock timedLock = taskRequests.AcquireLock())
                    //{
                    taskRequests.Add(iTemp);

                    Thread.Sleep(50);
                    //}

                    mre.WaitOne();


                }
            }
        
        
        }
        public void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //ObservableList<SearchResult> taskRequests = e.Argument as ObservableList<SearchResult>;
            
            MergeParam mp= e.Argument as MergeParam;
            ManualResetEvent mreTemp=new ManualResetEvent(false);
            
            ObservableList<SearchResult> taskRequests = mp.ol;

            if (mp.flag == 2)
            { 
                

                mreList.Add(mreTemp);
            }

            if (mp.flag == 2)
            {

               mreTemp.WaitOne();
            }

            foreach (SearchResult i in (new LuceneSearch()).SearchResultsIterList(mp.strCondition,
                                            mp.languagesCondition, mp.technologiesCondition))
            {
                if (i != null)
                {
                    SearchResult iTemp = new SearchResult();
                    iTemp.FilePath = i.FilePath;
                    iTemp.GroupParameter = i.GroupParameter;
                    iTemp.Samples = i.Samples;
                    iTemp.Score = i.Score;
                    iTemp.SlnPath = i.SlnPath;
                    iTemp.Snippet = i.Snippet;

                    //using (TimedLock timedLock = taskRequests.AcquireLock())
                    //{
                        taskRequests.Add(iTemp);
                    
                        Thread.Sleep(50);
                    //}

                    mre.WaitOne();
                      
                
                }
            }
        }

        public void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //CodeBoxControl.CodeBox cbc = VisualTreeHelperEx.FindVisualChildByName<CodeBoxControl.CodeBox>(sampleListBox, "document");

          

        }

        /// <summary>
        /// Validate whether cbLanguages's checkbox should be checked or not
        /// </summary>
        /// <param name="languagesString"></param>
        void ValidateLanguagesChechBoxIsCheckOrNot(string languagesString)
        {
            string[] languagesArr = languagesString.Split(',');
            for (int i = 0; i < this.searchControl.cbBoxLanguage.Items.Count; i++)
            {
                ComboBoxItem item = this.searchControl.cbBoxLanguage.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem;
                CheckBox cb = VisualTreeHelperEx.FindVisualChildByName<CheckBox>(item, "lanCkBox");
                if (cb != null)
                {
                    if (languagesArr.Contains(cb.Content))
                    {
                        cb.IsChecked = true;
                    }
                    else
                    {
                        cb.IsChecked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Validate whether cbTechnologies's checkbox should be checked or not
        /// </summary>
        /// <param name="technologiesString"></param>
        void ValidateTechnologiesChechBoxIsCheckOrNot(string technologiesString)
        {
            string[] technologiesArr = technologiesString.Split(',');
            for (int i = 0; i < this.searchControl.cbBoxTechnology.Items.Count; i++)
            {
                ComboBoxItem item = this.searchControl.cbBoxTechnology.ItemContainerGenerator.ContainerFromIndex(i) as ComboBoxItem;
                CheckBox cb = VisualTreeHelperEx.FindVisualChildByName<CheckBox>(item, "techCkBox");
                if (cb != null)
                {
                    if (technologiesArr.Contains(cb.Content))
                    {
                        cb.IsChecked = true;
                    }
                    else
                    {
                        cb.IsChecked = false;
                    }
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            mre.Reset();
        }

        private void lblHomepage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://1code.codeplex.com");
        }

        private void lblRequest_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://1code.codeplex.com/wikipage?title=Request%20Code%20Sample%20from%20Microsoft%20All-In-One%20Code%20Framework&referringTitle=Documentation");
        }

        public Visual samplesListBox { get; set; }
    }
}
