using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CodeBoxControl.Decorations;
using System.Windows.Media;
using System.Windows.Markup;
namespace CodeBoxControl.DecorationConditions
{
    public class DecorationConditionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }



        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {

            if (value is string)
            {

                string testString = (string)value;
                if (!testString.Equals("") )
                {
                    DecorationCondition dc = new DecorationCondition();
                    DecorationSchemes ds = new DecorationSchemes();
                    MultiRegexWordDecoration BlueWords = new MultiRegexWordDecoration();
                    BlueWords.Brush = new SolidColorBrush(Colors.Blue);
                    BlueWords.Words = ds.CSharpReservedWords(testString);
                    dc.BaseDecorations.Add(BlueWords);
                    return dc;
                }
                //else if (testString == "SQLServer2008" || testString == "SQL")
                //{
                //    return DecorationSchemes.SQLServer2008;

                //}
                //else if (testString == "DBML" || testString == "Dbml")
                //{
                //    return DecorationSchemes.Dbml;

                //}
                //else if (testString.ToUpper() == "XML")
                //{
                //    return DecorationSchemes.Xml ;

                //}
                //else if (testString.ToUpper() == "XAML")
                //{
                //    return DecorationSchemes.Xaml;

                //}


            }
            return base.ConvertFrom(context, culture, value);

        }


    }
}
