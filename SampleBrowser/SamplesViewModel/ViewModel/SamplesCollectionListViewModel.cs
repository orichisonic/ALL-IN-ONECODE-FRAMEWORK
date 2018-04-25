using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SamplesViewModel.ViewModel
{
    public class SamplesCollectionListViewModel : INotifyPropertyChanged
    {
        public SamplesCollectionListViewModel()
        {
 
        }

        private ObservableCollection<SampleCollectionViewModel> sampleCollectionList;
        public ObservableCollection<SampleCollectionViewModel> SampleCollectionList
        {
            get
            {
                return sampleCollectionList;
            }
            set
            {
                if (sampleCollectionList != value)
                {
                    sampleCollectionList = value;
                    NotifyPropertyChanged("SampleCollectionList");
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
