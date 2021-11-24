using Emgu.CV.Structure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Make3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.MainViewModel VM;
        public MainWindow()
        {
            InitializeComponent();
            if (LayerItemSource.Items != null)
            {
                if (LayerItemSource.Items.Count > 1)
                {
                    LayerItemSource.Items.Clear();
                }
            }
            VM = new ViewModels.MainViewModel();
            VM.Owner = this;
            this.DataContext = VM;
            VM.lpcProps = this.lpcProps;

            ColorProfile.ColorProfileVM cVm = new ColorProfile.ColorProfileVM();
            //cVm.MainArcWidth = 10;
            VM.cpVM = cVm;
            //cTest.DataContext = VM.cpVM;

            //cVm.MainArcWidth = 234;
            //VM.cpVM.MainArcWidth = 1004.2m;

            VM.OnChangeLayersDirection += ChangeLayersDirection;
            /*ollectionView v = CollectionViewSource.GetDefaultView((((ViewModel)this.DataContext).ProjectLayers));
            v.SortDescriptions.Clear();
            v.SortDescriptions.Add(new System.ComponentModel.SortDescription("Order", System.ComponentModel.ListSortDirection.Descending));
            v.Refresh();
            ProjectLayersItemSource.ItemsSource = v;*/

            InitializeLayerImages();

        }

        #region UI Actions

        #region initialization

        
        private void InitializeLayerImages()
        {
            List<Image> layerButs = new List<Image>();
            
            int w = 32;
            int h = w;
            int str = w / 8;
            byte[] px = new byte[h * str];
            byte[] px1 = new byte[h * str];
            byte[] px2 = new byte[h * str];
            byte[] px3 = new byte[h * str];
            bool sw = false;
            bool sw1 = false;
            for (int i = 0; i < px.Length; i++)
            {
                px[i] = 0;
                px1[i] = 1;
                if (Convert.ToDouble(i) % 8.0 == 0)
                {
                    sw = !sw;
                }
                if (Convert.ToDouble(i) % w == 0)
                {
                    sw1 = !sw1;
                }

                if (sw)
                {
                    px2[i] = 0;
                }
                else
                {
                    px2[i] = 1;
                }

                if (sw1)
                {
                    px3[i] = 1;
                }
                else
                {
                    px3[i] = 0;
                }

            }

            // Try creating a new image with a custom palette.
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.Red);
            colors.Add(System.Windows.Media.Colors.Blue);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);

            // Creates a new empty image with the pre-defined palette
            var a = BitmapSource.Create(w,h,96,96,
                PixelFormats.Indexed1,
                myPalette,px,str);

            var b = BitmapImage.Create(w, h, 96, 96,
                PixelFormats.Indexed1,
                new BitmapPalette(new List<Color> { Colors.Transparent }), px1, str);
            var c = BitmapImage.Create(w, h, 96, 96,
                PixelFormats.Indexed1,
                myPalette, px2, str);
            var d = BitmapImage.Create(w, h, 96, 96,
                PixelFormats.Indexed1,
                new BitmapPalette(new List<Color> { Colors.Transparent }), px3, str);
            //imgTreko.Source = a;
            //imgTreko3D.Source = b;
            //imgTrekoSwitch.Source = c;
            //imgStickySymbol.Source = d;
        }

        private void CreateButtonsXML()
        {
            List<UIElement> elements = new List<UIElement>();

            List<UIElement> favorites = new List<UIElement>();

            bool contSep = LayersPanel.Children.OfType<Separator>().Any();
            bool Sep = false;
            

            foreach (UIElement el in LayersPanel.Children)
            {
                if (contSep)
                {
                    if (el.GetType() == typeof(Separator))
                    {
                        Sep = true;
                    }

                    if (el.GetType() == typeof(Image))
                    {
                        if (Sep)
                        {
                            elements.Add(el);
                        }
                        else favorites.Add(el);
                    }
                }
                else
                {
                    if (el.GetType() == typeof(Image))
                    {
                        elements.Add(el);
                    }
                }
            }

        }

        private void TurnOnFavorites()
        {
            if (LayersPanel.Children.OfType<Separator>().Any())
            {
                //LayersPanel.Children.IndexOf(x => x.GetType(Separator));
            }
        }
        private void TurnOffFavorites()
        {

        }
        #endregion
        #endregion

        class ElementsInfo
        {
            public string PanelNane { get; set; }
            public bool Favorites { get; set; }

        }

        class ElementInfo
        {
            public int Order { get; set; }
            public string Name { get; set; }

            //Add animation?
            public string ToolTipText { get; set; }

        }

        private void ChangeLayersDirection(ListSortDirection dir)
        {
            CollectionViewSource cvs = (CollectionViewSource)this.FindResource("LayersCVS");
            SortDescription sd = cvs.SortDescriptions[0] == null ? new SortDescription() : cvs.SortDescriptions[0];
            if (sd.PropertyName != null)
            {
                String pn = sd.PropertyName;
                sd = new SortDescription(pn, dir);
                cvs.SortDescriptions[0] = sd;
                /*if (sd.Direction != dir)
                {
                    cvs.SortDescriptions[0] = sd;
                }
                else
                {
                    //cvs.DeferRefresh;
                }*/

            }
        }

        private void LayersPanel_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        // команда создания нового проекта
        private UI_Helper.RelayCommand newProjectCommand;
        public UI_Helper.RelayCommand NewProjectCommand
        {
            get
            {
                return newProjectCommand ??
                  (newProjectCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.NewProject(this);
                  }
                  ));
            }
        }

        // команда открытия проекта из файла
        private UI_Helper.RelayCommand openProjectCommand;
        public UI_Helper.RelayCommand OpenProjectCommand
        {
            get
            {
                return openProjectCommand ??
                  (openProjectCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.OpenProjectFromFile(this);
                  }
                  ));
            }
        }

        // команда сохранеия проекта
        private UI_Helper.RelayCommand saveProjectCommand;
        public UI_Helper.RelayCommand SaveProjectCommand
        {
            get
            {
                return saveProjectCommand ??
                  (saveProjectCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.SaveProject(this);
                  }
                  ));
            }
        }

        // команда сохранеия проекта как (в новый файл)
        private UI_Helper.RelayCommand saveProjectAsCommand;
        public UI_Helper.RelayCommand SaveProjectAsCommand
        {
            get
            {
                return saveProjectAsCommand ??
                  (saveProjectAsCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.SaveProjectAs(this);
                  }
                  ));
            }
        }

        // команда выхода из программы
        private UI_Helper.RelayCommand exitCommand;
        public UI_Helper.RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                  (exitCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.Exit(this);
                      /*bool allowExit = VM.Exit(this);
                      if (allowExit)
                      {
                          this.Close();
                      }*/
                  }
                  ));
            }
        }

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            ControlLibrary.ProjectControl pc = new ControlLibrary.ProjectControl();
            ViewModels.NewProjectWindow pcw = new ViewModels.NewProjectWindow();
            pcw.Height = 500;
            pcw.Width = 400;
            pcw.Owner = this;
            pcw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //pcw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            /*SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            pcw.Background = brush;*/
            //pcw.Content = pc;
            pcw.ShowDialog();
        }

        #region Layers
        // команда выхода из программы
        private UI_Helper.RelayCommand showPreviewCommand;
        public UI_Helper.RelayCommand ShowPreviewCommand
        {
            get
            {
                return showPreviewCommand ??
                  (showPreviewCommand = new UI_Helper.RelayCommand(obj =>
                  {
                      if (VM == null)
                      {
                          VM = new ViewModels.MainViewModel();
                      }
                      VM.ShowSelectedLayerPreview();
                      /*bool allowExit = VM.Exit(this);
                      if (allowExit)
                      {
                          this.Close();
                      }*/
                  }
                  ));
            }
        }
        #endregion
    }
}
