using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControlLibrary.Control_Parts
{
    /// <summary>
    /// Логика взаимодействия для PathControl.xaml
    /// </summary>
    public partial class PathControl : UserControl, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        /*public delegate void OpenClicked(bool directory);
        public event OpenClicked OnOpenClicked;

        public delegate void ClearClicked();
        public event ClearClicked OnClearClicked;*/

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            "Path", typeof(string), typeof(PathControl));
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set
            {
                SetValue(PathProperty, value);
                OnPropertyChanged("Path");
            }
        }

        public static readonly DependencyProperty OpenCommandProperty = DependencyProperty.Register(
            "OpenCommand", typeof(ICommand), typeof(PathControl));
        public ICommand OpenCommand
        {
            get { return (ICommand)GetValue(OpenCommandProperty); }
            set
            {
                SetValue(OpenCommandProperty, value);
                OnPropertyChanged("OpenCommand");
            }
        }

        public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(
            "ClearCommand", typeof(ICommand), typeof(PathControl));
        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set
            {
                SetValue(ClearCommandProperty, value);
                OnPropertyChanged("ClearCommand");
            }
        }

        public PathMode InputPathMode { get; set; }

        public PathControl()
        {
            InitializeComponent();
            //Path = "testPath";
            //Directory = false;
        }

        public PathControl(bool directory)
        {
            InitializeComponent();

            //Directory = directory;
        }

        /*private UI_Helper.RelayCommand openCommand;
        public UI_Helper.RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      OnOpenClicked?.Invoke(Directory);
                  }
                  ));
            }
        }*/

        /*private UI_Helper.RelayCommand clearCommand;
        public UI_Helper.RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                  (clearCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      OnClearClicked?.Invoke();
                  }
                  ));
            }
        }*/

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public enum PathMode
        {
            File,
            Directory
        }
    }
}
