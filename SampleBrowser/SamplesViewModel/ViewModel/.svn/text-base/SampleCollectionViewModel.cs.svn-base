using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

using SamplesModel.Entities;

namespace SamplesViewModel.ViewModel
{
    public class SampleCollectionViewModel : INotifyPropertyChanged
    {
        public SampleCollectionViewModel()
        {
 
        }

        public SampleCollectionViewModel(ObservableCollection<Sample> allSamples, string condition)
        {
            if (string.IsNullOrEmpty(condition))
                Samples = allSamples;
        }

        private ObservableCollection<Sample> samples;

        public ObservableCollection<Sample> Samples
        {
            get { return samples; }
            set 
            {
                if (samples != value)
                {
                    samples = value;
                    NotifyPropertyChanged("Samples");
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
