   M S B A M L   `   `   `  ���� E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks      QuickTasks.RenameDialog   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:QuickTasks     -   $��modRenameDialog��  � Title$  RenameDialog��$	��110��$	��250��   WindowStyle#��[  .���� $��Stretch=�$��10q� � DataContext ��  �� ElementName$ modRenameDialog��.+��� $��Enter new name:��$
��20 0q�$	��0 5q�  
labelStyle#��[ �� -   $��	RenameBox��$��	20 0 20 5q� �� 	MaxLength$ 20�� �� TextWrapping$ NoWrap=� � ToolTip$@ :Max 15 Characters. Alphanumerics and some symbols allowed.������ *	NewName+�  � ColumnDefinitions �� $	��19* ��� $	��11* ��� $	��30* �.+��� -   $��	CancelBtn��$��0��$��Cancel��$��	20 5 10 0q�$��2��  
NoBtnStyle#��[  � 	Resources  �� 
TargetType	 �� Property
 �� Value��%A   '��       �� " ��.��� 	 �@��CornerRadius$
 	0 10 0 10���� -   $��SaveBtn��$��2��$
��Save��$��	10 5 20 0q�  YesBtnStyle#��[  ��%A   '��       �� " ��.��� 	 �@��CornerRadius$
 	10 0 10 0��