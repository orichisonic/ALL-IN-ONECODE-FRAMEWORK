using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel ;
using CodeBoxControl.Decorations ;
namespace ColoredWordPad
{
    public static class ToolBarStatics
    {

     public static ObservableCollection< DecorationScheme> AvailableSchemes  
     {
         get
         {
             ObservableCollection<DecorationScheme> schemes =new  ObservableCollection<DecorationScheme>();
             //schemes.Add( new DecorationScheme() { Name = "None" });
             schemes.Add(DecorationSchemes.CSharp3);

             
             //schemes.Add(DecorationSchemes.SQLServer2008);
             //schemes.Add(DecorationSchemes.Xml);
             //schemes.Add(DecorationSchemes.Xaml);
             return schemes;
         }

     }


     public static ObservableCollection<Decoration> AvailableDecorations
     {
         get
         {
             ObservableCollection<Decoration> decorations = new ObservableCollection<Decoration>();

             return decorations;
         }
     }
    }
}
