   M S B A M L   `   `   `  ���� K  GStudentMarksheet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:StudentMarksheetStudentMarksheet  =#clr-namespace:StudentMarksheet.ViewStudentMarksheet.View  +    %StudentMarksheet.StudentMarksheetView   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:StudentMarksheet     _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  .view#clr-namespace:StudentMarksheet.View     AppIcon  � Icon#  [   � Title$ Student MarkSheet��$	��450��$	��800��$	��450��$	��800�� � WindowState$ 	Maximized=� � 
ResizeMode$ 	CanResize=�.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+���   GenericBorderStyle#��[ $��0��$��2q�._���  �� Orientation$ 
Horizontal=�$
��Left=�.+���   ListBtnStyle#��[ $��2q����� *FetchStudentsMarksCommand+���  � ColumnDefinitions �� $
��Auto ��� .+��� $��0��$
��Left=�$��Center=�$��25��$��20��  DisplayGridIcon#��[ �� $��Center=�$
��Left=�$��Fetch Student MarkSheet��$��1��$��4,0,0,0q��� #��[ $��2q����� *OpenFileBrowserCommand+���  �� $
��Auto ��� .+��� $��0��$
��Left=�$��Center=�$��25��$��20��  UploadCSVFileIcon#��[ �� $��Center=�$
��Left=�$��Upload Student Marksheet��$��1��$��4,0,0,0q�� $��1��{��� *IsFilterEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $��50 ��� $
��Auto �.+��� $ 
Horizontal=�$��0��$��2q�$
��Left=�.+��� $��Filter by Roll Number���� $��25��$	��100��$��2q����� *FilterRollNumber+$ PropertyChanged=�$ TwoWay=�.��*  $Microsoft.Xaml.Behaviors.Interaction	   	Behaviors	 2   ,StudentMarksheet.View.NumericTextBoxBehavior  .���� $ 
Horizontal=�$��1��$��2q�$
��Left=�.+��� $��Filter by Status���� $��25��$	��100��$��2q����� *ExamResultCollection+
 �� SelectedItem
 �� *SelectedFilterResult+$ PropertyChanged=�$ TwoWay=� �� Text �� *FilterStatus+$ PropertyChanged=�$ TwoWay=��� -    ��Name$ 	BtnFilter��$��25��$��25��$��2q�$��2��$
��Left=����� *FilterCommand+����   
FilterIcon#��[ �� -   $ BtnTitleClearFilter��$��25��$��25��$��2q�$��3��$
��Left=����� *ClearFilterCommand+����   ClearFilterIcon#��[ �� $��7��$��2q�$��Center=�$��DemiBold"����� *StudentGridCount+ �� StringFormat$" No. of Students Fetched: {0}��$ PropertyChanged=�$ TwoWay=��� $��2��$��2q�  DataGridBorderStyle#��[ ._�� .+�&   System.Windows.Controls.DataGrid  "   VerticalScrollBarVisibility$
 Auto=�$��5q�$	��250��$	��500��$   HorizontalScrollBarVisibility$
 Auto=�!   EnableColumnVirtualization$
 True��   GridLinesVisibility$ 
Horizontal=�   EnableRowVirtualization$
 True��   
IsReadOnly$
 True��$
��Auto��   CanUserResizeColumns$
 True��   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ *    AutoGenerateColumns$ False������ *StudentMarksheetCollection+$ TwoWay=�$ PropertyChanged=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Roll Number��   Width$
 .25* 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
RollNumber+  $
 Name��$	 .5*  �� *Name+  $ Gender��$
 .25*  �� *Gender+  $ Total Marks (500)��   
FontWeight$ DemiBold"�$
 .25*  �� *
TotalMarks+   	CellStyle ��  �� 
TargetType*  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value �� *
IsPassMark+$ PropertyChanged=�$ TwoWay=�  BoolBrushConverter  �� 	Converter#  [   $ Status��$ DemiBold"�$
 .25*  �� *Status+ �� "  .���  � �� �� *
IsPassMark+$ PropertyChanged=�$ TwoWay=�#  [ 4	  .System.Windows.Controls.DataGridTemplateColumn	  $ Subject Marks��$ * ! 	  CellTemplate! �� .R�  $��10q�$
 True��$	 All=�$ False������ *
MarkList+$ TwoWay=�$ PropertyChanged=�   $ Maths��$ *  �� *Maths+  $ Physics��$ *  �� *	Physics+  $ 	Chemistry��$ *  �� *	Chemistry+  $ Social Studies��$ *  �� *SocialStudies+  $ 2nd Language��$ *  �� *SecondLanguage+�� $��3��$��Center=�$��Center=�y��� *	IsLoading+$ PropertyChanged=�$ TwoWay=� 	 BoolVisibilityConverter#  [	 .+��� $��
Loading...��$��14(�$��Center=�� " � IsIndeterminate$
" True��$	��200��$��15��$��5q�