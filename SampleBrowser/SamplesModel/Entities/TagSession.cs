using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SampleBrowser.SamplesModel.Entities
{
    /// <summary>
    /// A class for store session informations in tabcontrol tab's tag.
    /// </summary>
    public class TagSession
    {
        public BackgroundWorker _backGroundWorker { get; set; }

        public string _searchCondition { get; set; }
    }
}
