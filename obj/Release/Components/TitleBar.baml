   M S B A M L   `   `   `  ���� E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks      QuickTasks.TitleBar   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     *localclr-namespace:QuickTasks     -   $��
UCTitleBar��   UCStyle#��[  .���   � DataContext  ��  �� ElementName$ 
UCTitleBar�� � ColumnDefinitions �� $��6* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� -   $��TitleLbl��  
titleStyle#��[  � ToolTip$ Double Click to rename��$	��190������  �� Path$ Title��� -    ��Name$ UndoBtn��$��1��$	��↩��  buttonStyle#��[ $��0 0 0 1q�$
 Undo������ $
 Undo��� -   $ RedoBtn��$��2��$	��↪��#��[ $��0 0 0 1q�$
 Redo������ $
 Redo��� -   $ AddBtn��$��3��$	��＋��$
��Bold"�#��[ $��0 0 0 1q�$ Add new task������ $	 Add��� -   $ 	ReloadBtn��$��4��$
��🗘��#��[ $��0 0 0 1q�$ Reload from disk������ $ Reload��� -   $ SaveBtn��$��5��$
��🖫��#��[ $��0 0 0 1q�$ Save to disk������ $
 Save��� -   $ QuitBtn��$��6��$	��⨉��$
��Bold"�  
NoBtnStyle#��[ $
 Quit������ $
 Quit����� $ WindowState� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" �	 � 	Resources	 
 �� 
TargetType �� Property �� Value��%A   '��       �� "
 ��.���  �@��CornerRadius$ 	0 0 10 10��