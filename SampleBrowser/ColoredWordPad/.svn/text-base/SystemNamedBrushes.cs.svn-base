using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Reflection;
using System.Windows;

namespace ColoredWordPad
{
    public static class SystemNamedBrushes
    {
        private static readonly String[] sColorNames;
        private static Color[] sColors;
        private static   Brush[] sColorBrushes;

        static SystemNamedBrushes()
        {
            System.Reflection.PropertyInfo[] nCols = typeof(Colors).GetProperties();
            sColorNames = new String[nCols.Length];
            sColors = new Color[nCols.Length];
            sColorBrushes = new Brush[nCols.Length];
            for (int i = 0; i < nCols.Length; i++)
            {
                sColorNames[i] = nCols[i].Name;
                sColors[i] =(Color) nCols[i].GetValue(null,null);
                sColorBrushes[i] = new SolidColorBrush((Color)nCols[i].GetValue(null, null));
            }
        }

        public static string[] ColorNames
        {
            get { return sColorNames; }
        }

        public static Color[] Colors
        {
            get { return sColors; }
        }

        public static Brush[] ColorBrushes
        {
            get { return sColorBrushes; }
        }

        public static bool IsNamedColor(Color color)
        {
            return Array.IndexOf(sColors, color) != -1;
        }

        public static string  ColorName(Color color)
        {
            int index = Array.IndexOf(sColors, color);
            if ( index == -1)
            {
                return "";
            }
            else
            {
                return sColorNames[index];
            }

        }


        public static Color Color(string colorName)
        {

            int index = Array.IndexOf(sColors, sColorNames);
            if (index == -1)
            {
                return new Color();
            }
            else
            {
                return sColors[index];
            }


        }
    }
}

