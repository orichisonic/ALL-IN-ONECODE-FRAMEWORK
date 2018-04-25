using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using SamplesModel.Entities;
using SamplesController.Services;

namespace SamplesViewModel.ViewModel
{
    public class SampleListViewModel : INotifyPropertyChanged
    {
        public SampleListViewModel()
        {
            
        }

        private ObservableCollection<Sample> sampleList;
        public ObservableCollection<Sample> SampleList
        {
            get
            {
                return sampleList;
            }
            set
            {
                if (sampleList != value)
                {
                    sampleList = value;
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
