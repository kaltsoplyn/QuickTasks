   M S B A M L   `   `   `  ����E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks  5   	       QuickTasks.RenameDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:QuickTasks     5   	   -   5        $��modRenameDialog��5   	     � Title$  RenameDialog��6    $	��110��5	   	   $	��250��6      WindowStyle#��[  5
   	   .��5      �� $��Stretch=�6   $��10q�6-    � DataContext 69   ��  �� ElementName$ modRenameDialog��.+�5   
   �� $��Enter new name:��6   $
��20 0q�6*   $	��0 5q�68     
labelStyle#��[ 6F   6l   �� 5   
   -   5        $��	RenameBox��5      $��	20 0 20 5q�6#    �� 	MaxLength$ 20��5       �� TextWrapping$ NoWrap=�6!    � ToolTip$@ :Max 15 Characters. Alphanumerics and some symbols allowed.��67   ��5   6   �� *	NewName+5   5   � 5   
    � ColumnDefinitions 5      �� 5      $	��19* �6#   �� 5      $	��11* �6#   �� 5      $	��30* �6#   .+�5      �� -   5        $��	CancelBtn��5   %   $��0��6   $��Cancel��66   $��	20 5 10 0q�6G   $��2��5        
NoBtnStyle#��[ 6)    � 	Resources 5       �� 
TargetType	 �� Property
 �� Value��%f   '��       �� 5      " ��6   .�5      �� 	 �@��CornerRadius$
 	0 10 0 10��69   5      5      5      �� 5      -   5        $��SaveBtn��5   %   $��2��6   $
��Save��64   $��	10 5 20 0q�6C     YesBtnStyle#��[ 5       5       ��%f   '��       �� 5!      " ��6   .�5"      �� 	 �@��CornerRadius$
 	10 0 10 0��69   5#      5$      5%      5&      5'      5(      