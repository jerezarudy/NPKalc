   M S B A M L   `   `   `  ????D  @NPKalc_v3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:NPKalc_v3.Views.CalculatorNPKalc_v3.Views.Calculator  5   	   *    $NPKalc_v3.Views.Calculator.SMSWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:NPKalc_v3.Views.Calculator     5   	   -   5          ? Title$  Send SMS??5   	    ? WindowStyle$ 
ToolWindow=?6   $	??400??5
   	   $	??600??6    ? WindowStartupLocation$ CenterScreen=?6"    ? 
ResizeMode$ NoResize=?6G   .??5      ? $??20q?6    ? ColumnDefinitions 5   
   ?? 5      $	??100 ?6   ?? 5       ? RowDefinitions 5   
   ?? 5      $??28 ?6   ?? 5      $??10 ?6   ?? 5      $??28 ?6   ?? 5      $??10 ?6   ?? 5      ?? 5      $??10 ?6   ?? 5      $??50 ?6   .+?5   
   ?? $??Phone Number:??6   $??Center=?6)   $
??Left=?6D   ?? 5   
   -   5         ??Name$ txtPhoneNumber??5   #   $??1??6   6#   ?? 5   
   $??Message:??6   $??Center=?6$   $
??Left=?6?   $??2??6Z   ?? 5   
   -   5        $ 
txtMessage??5      $??4??6   $??2??63    !  MaterialDesignOutlinedTextBox#??[  6G    ?? AcceptsReturn .  ?? TextWrapping$
 Wrap=?6?   ?? 5    
   -   5        $??6??5       $??1??6   $	??100??6C   $??Right=?6O   $
??Send??6k   
??? ? ?
??? ? ?6?   5!      5"      