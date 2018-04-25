using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Configuration;

using SamplesModel.Entities;
using SamplesController.Services;
using SampleBrowser.Common;
using SampleBrowser.SamplesModel.Entities;

namespace SamplesViewModel.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            List<Sample> allSamples = SamplesService.GetAllSamples(Constants.XML_SOURCE_FILE_PATH);
            AllSearchResult = new ObservableCollection<SearchResult>(SamplesService.GetAllSearchResults(allSamples));
        }

        private ObservableCollection<SearchResult> allSearchResult;
        public ObservableCollection<SearchResult> AllSearchResult
        {
            get
            {
                return allSearchResult;
            }
            set
            {
                if (allSearchResult != value)
                {
                    allSearchResult = value;
                    NotifyPropertyChanged("SampleList");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

}
