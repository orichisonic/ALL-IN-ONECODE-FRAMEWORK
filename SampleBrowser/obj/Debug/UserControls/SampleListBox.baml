   M S B A M L   `   `   `  ����R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel  5      H DSampleBrowser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SampleBrowser.CommonSampleBrowser.Common 5      I ECodeBoxControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CodeBoxControl;assembly=CodeBoxControlCodeBoxControl 5      /clr-namespace:ColoredWordPadColoredWordPad 5	      7 clr-namespace:SampleBrowser.TestSampleBrowser.Test 5
      C&clr-namespace:SampleBrowser.PropertiesSampleBrowser.Properties 5      .   (SampleBrowser.UserControls.SampleListBox   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5      Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase      5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006      5      0c"clr-namespace:SampleBrowser.Common      5      @d2http://schemas.microsoft.com/expression/blend/2008      5      Bi4clr-namespace:CodeBoxControl;assembly=CodeBoxControl      5      ,cwpclr-namespace:ColoredWordPad      5	      2local clr-namespace:SampleBrowser.Test      5
      4p&clr-namespace:SampleBrowser.Properties      5      ���#808080  � 	Resources  5      )  #ColoredWordPad.SystemColorConverter   systemColorConverter( "ColoredWordPad.ColorBrushConverter  colorBrushConverter$  CodeBoxControl.CodeBoxSettings  coloredWordSettings   	BaseColor' !SampleBrowser.Properties.Settings   Default �� Source �� Path �� Mode �� 	Converter  sams �� Source �� SortDescriptions+   %System.ComponentModel.SortDescription	   PropertyName
   	Direction �� GroupDescriptions2  ,System.Windows.Data.PropertyGroupDescription   PropertyName �� 
TargetType �� Property �� Value   booleanToVisibilityConverter9  3SampleBrowser.Common.GroupNameToSampleNameConverter  groupToSampleNameConverter;  5SampleBrowser.Common.GroupNameToTechnologiesConverter   groupToTechnologiesConverter:	  4SampleBrowser.Common.GroupNameToDescriptionConverter  groupToDescriptionConverter:
  4SampleBrowser.Common.GroupNameToPicturePathConverter  groupToPicturePathConverter8  2SampleBrowser.Common.GroupNameToVsVersionConverter 	 groupToVsVersionConverter6  0SampleBrowser.Common.ListItemBackgroundConverter 
 listItemBackgroundConverter �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName  samplesGroup �� Value �� Binding � RowDefinitions �Name � ColumnDefinitions d�Name d� 	IsChecked �� ElementName��%�  &	        &	      &	 &     7  &	 |     '�� i    &	      &	     &	 &    &	 9    &	 L    &		 _    &	
 r    'd� �    &	 �    7  7  7 	 7  7    5   
   6#     5   
   6"     5   
    69   �� # Z $  DefaultBaseForegroungColor�$ TwoWay=�8   �� 5   
    6   ��  5        5      $	 Score��6&   $
 
Descending=�6;     5      $	 Samples.Name��6&     5      $	 Samples.VsVersion��6&    5        5      $ GroupParameter��6+   5      �� 5!   
   " ��6   .�5"      ��  � ��$ Stretch=�6;   �� 5#       � ��
 �  ��6+   �� 5$       � ��$ 14(�63   5%      �� 5'   
   6'     5(   
   6+     5)   
   6-   	  5*   
   6,   
  5+   
   6,     5,   
   6*     5-   
   6(   �� 5/   
   " d�6   .�50      ��  � ��$ Center=�6;   �� 51       � ��$ Center=�69   �� 52       � ��$ 1q�6(   �� 53       � ��$ 12��6'   �� 54       � ��$ 12��6&   �� 55       � �� 56      �� 57      " d�6#   .R�58      �� $	 img��6    -   5        $2��,/SampleBrowser;component/Images/Collapse.png��58   -   6c    59      P� 5:       �@d�	IsChecked$ FalseY�6;   . �5;   "   �� $	 img��6)    � ��$0 */SampleBrowser;component/Images/Expand.png��6L   5<      5?      5A      5B      �� 5D   
   " ��6    5E      �� 5F      $
 True��6E    6   �� $ IsBottomLevel�.a�5G      �� $ gridTemplate��6    � �� �#6262626R   5H      P� 5I       �@J�IsMouseOver . . �5J      �� $ gridTemplate��6    � �� �#3131316R   5K      .R�5M      �  5N      �� 5O      �� 5P      .+�5S      � $ gridTemplate��6   $��Center=�6-     5T      ���� 5U      '��       " ��6    .�5V      ��  � ��$ 14(�69   �� 5W       � ��
 �����6;   �� 5X       � ��$ Center=�6B   5Y      5Z       5[      �� 5\      �� 5]      �� 5^      �� 5_       5a      �� 5b      $
��Auto �6+   �� 5c      $��* �6+   �� 5d      $��50 �6+   �� 5e      $��50 �6+   .+�5h      d� $ btnShowHide��6#   $ FalseY�68   $	��3.5q�6J   �� 5i      $��1��6    $��3.5,0q�6�   $
��Bold"�6�   
���������60   �� $
 Name�8   6�   � 5l      $��1��6    $��1��60   $��3.5,0q�6=   .+�5m      �� $��Technologies:  ��6$   $
��11.2(�6;   
�������6K   �� 5n      $
��11.2(�6x   
���������6$   �� $
 Name�8  6�   5o      � 5p      $��1��6    $��2��60   $��3.5,0q�6=   .+�5q      �� $
��11.2(�6u   
���������6$   �� $
 Name�8  6�   5r      � 5s      $��1��6    $��3��60   $��3.5,0q�6=   .+�5t      �� $
��11.2(�6w   
���������6$   �� $
 Name�8  6�   5v      5y      �� 5{      $��1��6�   $��
11,0,0,3.5q�6�   y�6!   �� $ btnShowHide��$ 	IsChecked�8  6�   5|      5}      5      .��5�      �  5�   
   �� 5�      $
��Auto �6   �� 5�      $��0.622* �6   �� 5�      $��0.378* �6   .+�5�   
   �� -   5         ��Name$	 slb��5�      $��0q�6   $��1��6+   $��0q�6m   $��2��6x   ���	#FF333333��68   �� # [   5�       �� Color��%  (��       5�   "   * SystemColors.HighlightBrushKey+)(�� #     5�   "   *SystemColors.ControlBrushKey+)(�� F     5�   "   *$"SystemColors.HighlightTextBrushKey+)�� 5�      $ 	#FFE4E4E4��6T   �� 5�      $ 	#FFE4E4E4��6R   �� 5�      # Z��6X   5�        �� 
GroupStyle  5�      (  "System.Windows.Controls.GroupStyle  5�      !   ContainerStyle! 5�      �� 5�      " ��6    .�5�      ��  � �� 5�   "   �� 5�   &   " �� ResourceKey$" samplesGroup��65   5�      -   5        5�      5�      ��5�      �� 5�      " ��6   .�5�      ��  � ��
 �����5�      5�      ��5�      �� 5�      .R�5�      ]� -   5        .+�5�      �� .+�5�      �� # ��Name$# txtFilePath��6(   $
��Left=�6=   $
��Left=�6T   ���	#FF333333��6o   �� $ FilePath�6�     CodeBoxControl.CodeBox  5�      $  Name$	$ doc��6(   %   LineNumberMarginWidth$% 25Q�5�   �   
������ &   DecorationScheme>  8CodeBoxControl.Decorations.DecorationSchemeTypeConverter$& CSharp3 5�   B   '   DecorationConditionJ  DCodeBoxControl.DecorationConditions.DecorationConditionTypeConverter$'   6]   ( �� 
IsReadOnly( . $	��888��6�   )   BaseForeground) 5�      �� # [ $ 	BaseColor�# [ ��5�      �� # [ $ FontSize���5�      �� # [ $ 
FontFamily�*   CodeBoxBackground* 5�      �� # [ $ BackgroundColor�# [  ��5�      �� *	Snippet+6�   5�      5�      + �� Triggers+ 5�      ^� 5�      , ^� 
Conditions, 5�      �� 5�   "   - �� Binding- 6,   �� $ FilePath�. �� Value. 6N   Z� �� 5�   "   - 6,   �� *	Snippet+. 6H   Z� .0�5�      �� $ txtFilePath��6%    � y�$ 	Collapsed=�6T   �� 5�      $	 doc��6%    � y�$ 	Collapsed=�6L   5�      5�      5�      5�      5�      