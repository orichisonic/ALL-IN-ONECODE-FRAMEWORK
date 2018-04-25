using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SamplesModel.Entities;
using SamplesModel.Enum;

namespace SampleBrowser.SamplesModel.Entities
{
    public class SearchResult
    {
        public string FilePath { get; set; }

        public string SlnPath { get; set; }

        public string Snippet { get; set; }

        public Sample Samples { get; set; }

        public float Score { get; set; }

        private string _groupParameter;
        public string GroupParameter
        {
            get
            {
                if (Samples != null)
                {
                    string technologies = string.Empty;
                    foreach (Technology tec in Samples.Technologies)
                    {
                        technologies += EnumOperator.GetTechnologyStringValue(tec) + "-";
                    }

                    string[] arr = { Samples.Name, technologies.TrimEnd('-'), Samples.Description, Samples.PicturePath, Samples.VsVersion.ToString() };
                    _groupParameter = string.Join(",", arr);
                }
                return _groupParameter;
            }
            set
            {
                if (_groupParameter != value)
                {
                    _groupParameter = value;
                }
            }
        }
    }
}
