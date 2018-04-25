using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using SamplesModel.Enum;


namespace SamplesModel.Entities
{
    public class Sample : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private DateTime? createDate;
        public DateTime? CreateDate
        {
            get { return createDate; }
            set
            {
                if (createDate != value)
                {
                    createDate = value;
                    NotifyPropertyChanged("CreateDate");
                }
            }
        }

        private DateTime? updateDate;
        public DateTime? UpdateDate
        {
            get { return updateDate; }
            set
            {
                if (updateDate != value)
                {
                    updateDate = value;
                    NotifyPropertyChanged("UpdateDate");
                }
            }
        }

        private Language? language;
        public Language? Language
        {
            get { return language; }
            set
            {
                if (language != value)
                {
                    language = value;
                    NotifyPropertyChanged("Language");
                }
            }
        }

        private int vsVersion;
        public int VsVersion
        {
            get { return vsVersion; }
            set
            {
                if (vsVersion != value)
                {
                    vsVersion = value;
                    NotifyPropertyChanged("VsVersion");
                }
            }
        }

        private List<string> tags;
        public List<string> Tags
        {
            get { return tags; }
            set
            {
                if (tags != value)
                {
                    tags = value;
                    NotifyPropertyChanged("Tags");
                }
            }
        }

        private string difficulty;
        public string Difficulty
        {
            get { return difficulty; }
            set
            {
                if (difficulty != value)
                {
                    difficulty = value;
                    NotifyPropertyChanged("Difficulty");
                }
            }
        }

        private string downloadLink;
        public string DownloadLink
        {
            get { return downloadLink; }
            set
            {
                if (downloadLink != value)
                {
                    downloadLink = value;
                    NotifyPropertyChanged("DownloadLink");
                }
            }
        }

        private string readMeLink;
        public string ReadMeLink
        {
            get { return readMeLink; }
            set
            {
                if (readMeLink != value)
                {
                    readMeLink = value;
                    NotifyPropertyChanged("ReadMeLink");
                }
            }
        }

        private string readMePath;
        public string ReadMePath
        {
            get { return readMePath; }
            set
            {
                if (readMePath != value)
                {
                    readMePath = value;
                    NotifyPropertyChanged("ReadMePath");
                }
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private List<Technology> technologies;
        public List<Technology> Technologies
        {
            get { return technologies; }
            set
            {
                if (technologies != value)
                {
                    technologies = value;
                    NotifyPropertyChanged("Technologies");
                }
            }
        }

        private List<string> referSamples;
        public List<string> ReferSamples
        {
            get { return referSamples; }
            set
            {
                if (referSamples != value)
                {
                    referSamples = value;
                    NotifyPropertyChanged("ReferSamples");
                }
            }
        }

        private string picturePath;
        public string PicturePath
        {
            get
            {
                switch (language)
                {
                    case SamplesModel.Enum.Language.CSharp:
                        return @"..\Images\CSharp.png";
                    case SamplesModel.Enum.Language.Cpp:
                        return @"..\Images\Cpp.png";
                    case SamplesModel.Enum.Language.VisualBasic:
                        return @"..\Images\VB.png";
                    //case SamplesModel.Enum.Language.Xaml:
                    //    return @"..\Images\Xaml.png";
                    default:
                        return @"..\Images\AppIcon.ico";
                }
            }
            set
            {
                if (picturePath != value)
                {
                    picturePath = value;
                    NotifyPropertyChanged("PicturePath");
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
