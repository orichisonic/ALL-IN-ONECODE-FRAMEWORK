using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CodeBoxControl.DecorationConditions;
using CodeBoxControl.Decorations;
namespace CodeBoxControl.DecorationConditions
{
    [TypeConverter(typeof(DecorationConditionTypeConverter))]
    public class DecorationCondition
    {
        List<Decoration> mDecorations = new List<Decoration>();

        public List<Decoration> BaseDecorations
        {
            get { return mDecorations; }
            set { mDecorations = value; }
        }

        public string Name { get; set; }
        public string _txtCondition { get; set; }

        //public DecorationScheme(string txtCondition)
        //{
        //    _txtCondition = txtCondition;
        //}
    }
}
