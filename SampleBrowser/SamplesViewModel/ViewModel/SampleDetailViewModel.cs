using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SamplesModel.Entities;

namespace SamplesViewModel.ViewModel
{
    public class SampleDetailViewModel : INotifyPropertyChanged
    {
        public SampleDetailViewModel()
        { }

        private Sample sampleDetail;

        public Sample SampleDetail
        {
            get { return sampleDetail; }
            set
            {
                if (sampleDetail != value)
                {
                    sampleDetail = value;
                    NotifyPropertyChanged("SampleDetail");
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
