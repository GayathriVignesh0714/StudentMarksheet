   M S B A M L   `   `   `  ����K  GStudentMarksheet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:StudentMarksheetStudentMarksheet  5   	   =#clr-namespace:StudentMarksheet.ViewStudentMarksheet.View  5   	   +    %StudentMarksheet.StudentMarksheetView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:StudentMarksheet     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   .view#clr-namespace:StudentMarksheet.View  5   	      AppIcon  � Icon#  [  5	       � Title$ Student MarkSheet��5
   	   $	��450��5   	   $	��800��6   $	��450��6"   $	��800��62    � WindowState$ 	Maximized=�6A    � 
ResizeMode$ 	CanResize=�6Y   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��   GenericBorderStyle#��[ 6   $��0��6=   $��2q�6J   ._�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�63   .+�5      ��   ListBtnStyle#��[ 6   $��2q�6?   ��6J   �� *FetchStudentsMarksCommand+��5      � 5       � ColumnDefinitions 5      �� 5   "   $
��Auto �63   �� 5   "   .+�5      �� $��0��6$   $
��Left=�64   $��Center=�6O   $��25��6j   $��20��6u     DisplayGridIcon#��[ 6�   �� 5      $��Center=�6(   $
��Left=�6C   $��Fetch Student MarkSheet��6^   $��1��6}   $��4,0,0,0q�6�   5      5       �� 5!      #��[ 6   $��2q�6?   ��6J   �� *OpenFileBrowserCommand+��5"      � 5#       5$      �� 5%   "   $
��Auto �63   �� 5&   "   .+�5(      �� $��0��6$   $
��Left=�64   $��Center=�6O   $��25��6j   $��20��6u     UploadCSVFileIcon#��[ 6�   �� 5)      $��Center=�6(   $
��Left=�6C   $��Upload Student Marksheet��6^   $��1��6~   $��4,0,0,0q�6�   5*      5,      5-      5.      � 5/   
   $��1��6   {�6   �� *IsFilterEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� 50      �� 51      $
��Auto �6#   �� 52      $
��Auto �6#   �� 53      $
��Auto �6#   �� 54      $
��Auto �6#   �� 55      $
��Auto �6#   �� 56      $
��Auto �6#   �� 57      $��50 �6#   �� 58      $
��Auto �6#   .+�5:      �� $ 
Horizontal=�6   $��0��62   $��2q�6B   $
��Left=�6M   .+�5;      �� $��Filter by Roll Number��6   �� 5<      $��25��6   $	��100��6&   $��2q�62   ��6>   �� *FilterRollNumber+$ PropertyChanged=�$ TwoWay=�.��6�   *  $Microsoft.Xaml.Behaviors.Interaction	   	Behaviors	 5=      2   ,StudentMarksheet.View.NumericTextBoxBehavior  5>      .��5?   /   5@      5A      �� 5B      $ 
Horizontal=�6   $��1��62   $��2q�6B   $
��Left=�6M   .+�5C      �� $��Filter by Status��6   �� 5D      $��25��6   $	��100��6'   $��2q�63   ��6>   �� *ExamResultCollection+
 �� SelectedItem
 5E      �� *SelectedFilterResult+$ PropertyChanged=�$ TwoWay=� �� Text 6z   �� *FilterStatus+$ PropertyChanged=�$ TwoWay=�5F      �� 5G      -   5         ��Name$ 	BtnFilter��5G      $��25��6(   $��25��63   $��2q�6@   $��2��6K   $
��Left=�6[   ��6v   �� *FilterCommand+��5H      �� 5I        
FilterIcon#��[ 6   5K      �� 5L      -   5        $ BtnTitleClearFilter��5L      $��25��63   $��25��6>   $��2q�6K   $��3��6V   $
��Left=�6f   ��6�   �� *ClearFilterCommand+��5M      �� 5N        ClearFilterIcon#��[ 6   5P      �� 5Q      $��7��5R      $��2q�6   $��Center=�6'   $��DemiBold"�6B   ��5Q      �� *StudentGridCount+ �� StringFormat$" No. of Students Fetched: {0}��$ PropertyChanged=�$ TwoWay=�5R   B   5S      �� 5T   
   $��2��6   $��2q�6     DataGridBorderStyle#��[ 6+   ._�5U      � .+�5V      &   System.Windows.Controls.DataGrid  "   VerticalScrollBarVisibility$
 Auto=�6   $��5q�6�   $	��250��6�   $	��500��6�   $   HorizontalScrollBarVisibility$
 Auto=�5W      !   EnableColumnVirtualization$
 True��67      GridLinesVisibility$ 
Horizontal=�6Y      EnableRowVirtualization$
 True��6z      
IsReadOnly$
 True��5X      $
��Auto��5Y         CanUserResizeColumns$
 True��6       ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6<      AutoGenerateColumns$ False��6L   ��5V      �� *StudentMarksheetCollection+$ TwoWay=�$ PropertyChanged=�   Columns 5Z      0  *System.Windows.Controls.DataGridTextColumn  5[      ,  &System.Windows.Controls.DataGridColumn   Header$ Roll Number��6-      Width$
 .25* 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *
RollNumber+6a     5\      $
 Name��6-   $	 .5* 6T    6;   �� *Name+6T     5]      $ Gender��6-   $
 .25* 6X    6=   �� *Gender+6X     5^      $ Total Marks (500)��6-      
FontWeight$ DemiBold"�6H   $
 .25* 6}    6^   �� *
TotalMarks+   	CellStyle 5_      �� 5`   "    �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6)   .�5a   &   ��  �� Property � �� �� Value 6C   �� *
IsPassMark+$ PropertyChanged=�$ TwoWay=�  BoolBrushConverter  �� 	Converter#  [ 5b   #   5d        5e      $ Status��6-   $ DemiBold"�6=   $
 .25* 6n    6S   �� *Status+ 5f      �� 5g   "   "  6)   .�5h   &   ��  � �� 6C   �� *
IsPassMark+$ PropertyChanged=�$ TwoWay=�#  [ 5i   #   5k      4	  .System.Windows.Controls.DataGridTemplateColumn	  5l      $ Subject Marks��61   $ * 6H   ! 	  CellTemplate! 5m      �� 5n   "   .R�5o   &     $��10q�6/   $
 True��6�   $	 All=�6�   $ False��6�   ��6;   �� *
MarkList+$ TwoWay=�$ PropertyChanged=� 5p   *     5q   *   $ Maths��6=   $ * 6f    6L   �� *Maths+6f     5r   *   $ Physics��6=   $ * 6j    6N   �� *	Physics+6j     5s   *   $ 	Chemistry��6=   $ * 6n    6P   �� *	Chemistry+6n     5t   *   $ Social Studies��6=   $ * 6w    6U   �� *SocialStudies+6w     5u   *   $ 2nd Language��6=   $ * 6v    6S   �� *SecondLanguage+6v   5w   '   5x   #   5z      5|      5}      5~      �� 5�   
   $��3��6   $��Center=�6"   $��Center=�6?   y�6Z   �� *	IsLoading+$ PropertyChanged=�$ TwoWay=� 	 BoolVisibilityConverter#  [	 .+�5�      �� $��
Loading...��6   $��14(�6*   $��Center=�68   � 5�      " � IsIndeterminate$
" True��6   $	��200��61   $��15��6=   $��5q�6I   5�      5�      5�      