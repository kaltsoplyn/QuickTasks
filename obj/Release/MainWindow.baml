   M S B A M L   `   `   `  ���� E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks      QuickTasks.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:QuickTasks     -   $
��Main��  � Title$  
QuickTasks��$	��450��   WindowStyle#��[   � 	Resources   	BoolToVis��%   &	       �� .���� .+��� � $��0q�.+�   QuickTasks.TitleBar  -     Name$ 
ucTitleBar��$��Stretch=�   Title ��  �� Path$ 	TitleText�   Rename �� $ TitleRename�   Save �� $ 
SaveToDisk�   Reload �� $ ReloadFromDisk�
   Add �� $ AddItem�	   Redo	 �� $
 Redo�
   Undo
 �� $
 Undo�   Quit �� $
 Quit�]� -   $��10q� J� 	AllowDrop . y���  �� ElementName$ MainTaskList��$ ItemsSource.IsEmpty� �� 	Converter# [ .+��� -    ��Name$ EmptyListBanner�� �� TextAlignment$ Center=�$
��Wrap=�There is nothing here.�� Use "+" or drag and drop�� to add new tasks...�� -   $��ListContainer��$��0q�.+��� -   $��MainTaskList��$��0q�$��0q�  bgColor#��[ ���� *tasks+��Z� ����  �� 
TargetType" ��.���  �� Property � �� �� Value$ Stretch=���  � ��$	 0 3q���  � ��$ 0q����� .R�   QuickTasks.LVI     QuickTasks.LVI   	MouseMove LVI_Drag   Drop LVI_Drop$��0q� .    LVItem ��    Launch �� *DataContext.LaunchAndQuit+ �� RelativeSource ��  �� AncestorType$ ListViewL�   Rename �� *DataContext.Rename+ �� $ ListViewL�   Delete �� *DataContext.RemoveItem+ �� $ ListViewL�   QuickTasks.InfoBox  -     Name$ ucInfo��   QuickTasks.InfoBox   Drop$ LVI_Drop��   Message$  ��   LogLevel$  �� . 