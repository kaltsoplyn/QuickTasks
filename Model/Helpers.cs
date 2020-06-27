using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Shell32; // Reference: Microsoft Shell Controls and Automation

namespace QuickTasks.Model
{
    public static class Helpers
    {
        // convert Icon to ImageSource
        public static ImageSource IconToImageSource(System.Drawing.Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        // Get icon from path
        public static Icon GetIconFromPath(string path)
        {
            Icon _ico;
            try
            {
                _ico = System.Drawing.Icon.ExtractAssociatedIcon(path);
            }
            catch (Exception)
            {
                _ico = new Icon("pack://application/,,,/Images/burn.ico");
            }

            return _ico;
        }

        // Select new exe (task) from dialog
        public static string GetPathOfNewTask()
        {
            string def = "No file selected";
            string path = def;

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Executables (*.exe;*.lnk)|*.exe;*.lnk";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    path = dialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Exception occured:\n{0}", ex.Message), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                    return string.Empty;
                }
            }

            if (path == def) return string.Empty;

            if (Path.GetExtension(path) == ".lnk")
            {
                path = GetShortcutTarget(path);
                if (path == string.Empty)
                {
                    MessageBox.Show("Shortcut target could not be resolved.\nAn empty string is returned.", "Path not found",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return path;
                }
            }

            if (File.Exists(path) && Path.GetExtension(path) == ".exe")
            {
                return path;
            }
            else
            {
                MessageBox.Show(String.Format("Resolved path is not an executable:\n{0}", path), "Missing executable",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        // handle the case where the user selects a shortcut (lnk) from the dialog, instead of an exe 
        private static string GetShortcutTarget(string shortcut)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcut);
            string filenameOnly = System.IO.Path.GetFileName(shortcut);

            Shell32.Shell shell = new Shell32.Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
