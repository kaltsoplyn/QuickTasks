   M S B A M L   `   `   `  ����E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks  5   	       QuickTasks.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:QuickTasks     5   	   -   5        $
��Main��5   	     � Title$  
QuickTasks��5	   	   $	��450��6      WindowStyle#��[  5
   	    � 	Resources 5        	BoolToVis��%   &	       �� 5   
   6'   5      .��5      �� .+�5   
   �� � 5   
   $��0q�6   .+�5         QuickTasks.TitleBar  -   5          Name$ 
ucTitleBar��5      $��Stretch=�61      Title 5      ��  �� Path$ 	TitleText�   Rename 5      �� $ TitleRename�   Save 5      �� $ 
SaveToDisk�   Reload 5      �� $ ReloadFromDisk�
   Add 5      �� $ AddItem�	   Redo	 5      �� $
 Redo�
   Undo
 5      �� $
 Undo�   Quit 5      �� $
 Quit�5      ]� 5   
   -   5        $��10q�5       J� 	AllowDrop . y�6    ��  �� ElementName$ MainTaskList��$ ItemsSource.IsEmpty� �� 	Converter# [ .+�5      �� -   5         ��Name$ EmptyListBanner��5       �� TextAlignment$ Center=�61   $
��Wrap=�6H   There is nothing here.�� 5   -   Use "+" or drag and drop�� 5   .   to add new tasks...5!      5"      �� 5#   
   -   5        $��ListContainer��5#      $��0q�6*   .+�5$      �� -   5        $��MainTaskList��5$   5   $��0q�6I   $��0q�6U     bgColor#��[ 6`   ��6   �� *tasks+��5%      Z� ��5&      �� 5'       �� 
TargetType" ��6   .�5(      ��  �� Property � �� �� Value$ Stretch=�6G   �� 5)       � ��$	 0 3q�64   �� 5*       � ��$ 0q�63   5+      ��5-      �� 5.      .R�5/         QuickTasks.LVI     QuickTasks.LVI   	MouseMove LVI_Drag55         Drop LVI_Drop57      $��0q�54       .    LVItem 50      ��    Launch 51      �� *DataContext.LaunchAndQuit+ �� RelativeSource ��  �� AncestorType$ ListViewL�   Rename 52      �� *DataContext.Rename+ �� $ ListViewL�   Delete 53      �� *DataContext.RemoveItem+ �� $ ListViewL�57      59      5;      5<         QuickTasks.InfoBox  5=   
   -   5          Name$ ucInfo��5=         QuickTasks.InfoBox   Drop$ LVI_Drop��6P      Message$  ��6(      LogLevel$  ��63    . 6P   5>      5?      