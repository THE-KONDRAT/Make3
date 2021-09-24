using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        public static readonly DependencyProperty ProjectNameDP = DependencyProperty.Register(
            "ProjectName", typeof(string), typeof(Label));
        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameDP); }
            set
            {
                SetValue(ProjectNameDP, value);
            }
        }
        public ProjectControl()
        {
            InitializeComponent();
        }
    }
}
