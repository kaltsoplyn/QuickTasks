   M S B A M L   `   `   `  ����E  AQuickTasks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:QuickTasks
QuickTasks  5          QuickTasks.TitleBar   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:QuickTasks     5      -   5        $��
UCTitleBar��5         UCStyle#��[  5      .��5      �   � DataContext  6   ��  �� ElementName$ 
UCTitleBar�� � ColumnDefinitions 5   
   �� 5      $��6* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��TitleLbl��5        
titleStyle#��[ 6@    � ToolTip$ Double Click to rename��6d   $	��190��6�   ��6!   ��  �� Path$ Title�5      �� 5   
   -   5         ��Name$ UndoBtn��5   !   $��1��6   $	��↩��62     buttonStyle#��[ 6D   $��0 0 0 1q�6i   $
 Undo��6z   ��5      �� $
 Undo��� 5   
   -   5        $ RedoBtn��5   !   $��2��6   $	��↪��62   #��[ 6D   $��0 0 0 1q�6i   $
 Redo��6z   ��5      �� $
 Redo��� 5   
   -   5        $ AddBtn��5   !   $��3��6   $	��＋��61   $
��Bold"�6D   #��[ 6V   $��0 0 0 1q�6{   $ Add new task��6�   ��5       �� $	 Add��� 5!   
   -   5        $ 	ReloadBtn��5!   !   $��4��6   $
��🗘��64   #��[ 6H   $��0 0 0 1q�6m   $ Reload from disk��6~   ��5"      �� $ Reload��� 5#   
   -   5        $ SaveBtn��5#   !   $��5��6   $
��🖫��62   #��[ 6F   $��0 0 0 1q�6k   $ Save to disk��6|   ��5$      �� $
 Save��� 5%   
   -   5        $ QuitBtn��5%   !   $��6��6   $	��⨉��62   $
��Bold"�6E     
NoBtnStyle#��[ 6W   $
 Quit��6{   ��5&      �� $
 Quit���5'      �� $ WindowState� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" �	 � 	Resources	 5(      
 �� 
TargetType �� Property �� Value��%f   '��       �� 5)      "
 ��6   .�5*      ��  �@��CornerRadius$ 	0 0 10 10��65   5+      5,      5-      5/      50      