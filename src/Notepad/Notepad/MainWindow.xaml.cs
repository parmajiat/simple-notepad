using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notepad
{
    public partial class MainWindow : Window
    {
        string fileName = null;
        bool dirty = false;
        public bool Dirty
        {
            get { return dirty; }
            set
            {
                if (value != dirty)
                {
                    dirty = value;
                }
                SetTitle();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            string path = AppDomain.CurrentDomain.BaseDirectory + "sample.txt";
            Editor.Text = System.IO.File.ReadAllText(path);
        }

        #region File

        private void NewClicked(object sender, RoutedEventArgs e)
        {
            if (Dirty)
            {
                var result = MessageBox.Show("Do you want to save the changes", "Notepad", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                    dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(dialog.FileName, Editor.Text);
                        Editor.Text = "";
                        fileName = null;
                        Dirty = false;
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    Editor.Text = "";
                    fileName = null;
                    dirty = false;
                }
            }
        }

        private void OpenClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = dialog.FileName;
                Editor.Text = System.IO.File.ReadAllText(fileName);
                Dirty = false;
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (fileName == null)
            {
                System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = dialog.FileName;
                    System.IO.File.WriteAllText(fileName, Editor.Text);
                    Dirty = false;
                }
            }
            else
            {
                System.IO.File.WriteAllText(fileName, Editor.Text);
                Dirty = false;
            }
        }

        private void SaveAsClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(dialog.FileName, Editor.Text);
            }
        }
        private void ExitClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region View

        private void RestoreDefaultZoomClicked(object sender, RoutedEventArgs e)
        {
            Editor.FontSize = 14;
        }

        private void ZoomOutClicked(object sender, RoutedEventArgs e)
        {
            if (Editor.FontSize > 1)
                Editor.FontSize--;
        }

        private void ZoomInClicked(object sender, RoutedEventArgs e)
        {
            Editor.FontSize++;
        }

        private void StatusBarClicked(object sender, RoutedEventArgs e)
        {
            StatusBar.Visibility = ((sender as MenuItem).IsChecked) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Format
        private void WordWrapClicked(object sender, RoutedEventArgs e)
        {
            if (Editor.TextWrapping == TextWrapping.Wrap)
            {
                Editor.TextWrapping = TextWrapping.NoWrap;
                (sender as MenuItem).IsChecked = false;
            }
            else
            {
                Editor.TextWrapping = TextWrapping.Wrap;
                (sender as MenuItem).IsChecked = true;
            }
        }

        private void FontClicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog dialog = new System.Windows.Forms.FontDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Editor.FontFamily = new FontFamily(dialog.Font.FontFamily.Name);
                Editor.FontSize = dialog.Font.Size;
                Editor.FontWeight = dialog.Font.Bold ? FontWeights.Bold : FontWeights.Normal;
            }
        }
        #endregion

        private void AboutClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple Notepad @ 2021 Parmajiat Foundation", "Calculator", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Dirty)
                Dirty = true;
        }

        void SetTitle()
        {
            Title = "Notepad";
            if (fileName != null)
            {
                Title = fileName + " - " + Title;
            }
            if (Dirty)
            {
                Title = "*" + Title;
            }
        }
    }
}
