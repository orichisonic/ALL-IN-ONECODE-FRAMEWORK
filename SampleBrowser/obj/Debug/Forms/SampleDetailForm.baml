   M S B A M L   `   `   `  ����H  DSampleBrowser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SampleBrowser.ViewSampleBrowser.View  5   	   ;"clr-namespace:SampleBrowser.CommonSampleBrowser.Common  5   	   *    $SampleBrowser.Forms.SampleDetailForm   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   2local clr-namespace:SampleBrowser.View     5   	   0c"clr-namespace:SampleBrowser.Common     5   	     � Title$  SampleDetailForm��5   	   $	��590��6"   $	��770��6/    � WindowStartupLocation$ CenterScreen=�6;   ���#808080 � 	Resources 5      4   .SampleBrowser.Common.LanguageToStringConverter   languageToStringConverter;   5SampleBrowser.Common.GroupNameToTechnologiesConverter   groupToTechnologiesConverter��%:   &	        &	        5	   
   6&     5
   
   6-   5      .��5      � .+�5   
   �  � ShowGridLines .  $��5q�6%    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      .+�5      � $��0��6    5      �� 5      �� 5      �� 5      �� 5       5      �� 5      $	��100 �6'   �� 5       $��* �6'   �� 5!      $	��100 �6'   �� 5"      $��* �6'   .+�5%      �� -   5        $��
sampleName��5%      
�������$
��Bold"�6d   $��14(�6v   $��10,5,5,5q�6�   $��0��6�   $��0��6�   $��4��6�   ��6.   ��  �� Path$ Samples.Name�6�   �� 5'      $��	Language:��6   $��13(�6-   
�������$��1��6O   $��0��6\   $��10,5,5,5q�6l   �� 5(      -   5        $��sampleLanguage��5(      $��13(�6�   
�������$
��Left=�6�   $��1��6�   $��1��6�   $��0,5,5,5q�6�   ��62   �� $ Samples.Language� �� 	Converter# [  6�   �� 5*      $��Version:��6   $��13(�6-   
�������$��2��6O   $��0��6\   $��10,5,5,5q�6l   �� 5+      -   5        $��sampleVsVersion��5+      $��13(�63   
�������$��2��6U   $��1��6b   $��0,5,5,5q�6r   �� 5,      $�Visual Studio ��6    �� 5-      �6   �� $ Samples.VsVersion�5.      �� 50      $��Technologies:��6   $��13(�61   
�������$��1��6S   $��2��6`   $��5,5,5,5q�6p   �� 51      -   5        $��
sampleTech��51      $��1��6.   $��3��6;   $��13(�6�   
�������$��0,5,5,5q�6�   ��6K   �� $ GroupParameter�# [ 6�   �� 53      $��Difficulty:��6   $��13(�6/   
�������$��2��6Q   $��2��6^   $��5,5,5,5q�6n   �� 54      -   5        $��sampleDifficulty��54      $��13(�6]   
�������$��2��6   $��3��6�   $��0,5,5,5q�6�   ��64   �� $ Samples.Difficulty�6�   �� 56      $��Description:��6   $��13(�60   
�������$��3��6R   $��0��6_   $��10,5,5,5q�6o   �� 57      -   5        $��sampleDescription��57      $��3��65   $��1��6B   $��3��6R   $��13(�6�   
�������$��
0,5,146,-4q�6�   $��60��6�   $
��Wrap=�6�   $��WordEllipsis=�6�   ��6f   �� $ Samples.Description�6�   �� 58      -   5        $��btnOpenInVisual��58      $��Open In Visual Studio��60   
�������$��3��6�   $��3��6�   $��0,5,5,0q�6�   $
��10,5q�6�   $��Right=�6�   $	��Top=�6�   $��30��6�   59      � 5;      $��1��6    .  $
��5,10q�66    5<      �� 5=      $��* �6$    5?      �� 5@      �� 5A      .+�5D      �� $��2��6    J� SnapsToDevicePixels . $��	0,0.5,0,0q�6H   
�������._�5E      (  "System.Windows.Controls.WebBrowser  -   5        $��wbReadMe��5E   !   5F      �� 5G      $��2��6   $
��Auto=�63    . $��0,1,0,0q�6s   
�������.��5H      �� -	   5        $��	txtReadMe��5H      5I      5N      5O      5P      5Q      