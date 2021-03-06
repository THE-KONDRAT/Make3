using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ViewModels
{
    public class MainViewModel : DependencyObject, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        #region Events
        public delegate void ChangeLayersDirection(ListSortDirection dir);
        public event ChangeLayersDirection OnChangeLayersDirection;
        #endregion

        StatusBar statusBar;
        public StatusBar StatusBar
        {
            get { return statusBar; }
            set
            {
                statusBar = value;
                OnPropertyChanged("StatusBar");
            }
        }

        public Window Owner { get; set; }

        public ObservableCollection<UIElement> LayerPanelElements { get; set; }
        List<string> LayerPanelFavorites { get; set; }

        List<UIElement> LayerPanelLayers { get; set; }

        /// <summary>
        /// List of loaded projects
        /// </summary>
        private List<ProjectClassLib.Project> Projects { get; set; }

        /// <summary>
        /// Index of current project in list
        /// </summary>
        private byte? SelectedProject { get; set; }

        private ObservableCollection<ControlLibrary.LayerControl> projectLayers;
        /// <summary>
        /// Collection of Layer controls of current project
        /// </summary>
        public ObservableCollection<ControlLibrary.LayerControl> ProjectLayers
        {
            get
            {
                return projectLayers;
                //return projectLayers == null ? null : new ObservableCollection<ControlLibrary.LayerControl>(projectLayers.OrderByDescending(lc => lc.Order));
            }
            set
            {
                projectLayers = value;
                OnPropertyChanged("ProjectLayers");
            }
        }

        //internal ControlLibrary.LayerPropertiesControl lpcProps;
        ListSortDirection LayersSortDirection;

        #region Selected Layer Properties
        public static readonly DependencyProperty LayerSelectedNameProperty = DependencyProperty.Register(
            "LayerSelectedName", typeof(string), typeof(MainViewModel));
        public string LayerSelectedName
        {
            get { return (string)GetValue(LayerSelectedNameProperty); }
            set
            {
                SetValue(LayerSelectedNameProperty, value);
                OnPropertyChanged("LayerSelectedName");
            }
        }

        public ControlLibrary.LayerPropertiesControl lpcProps;

        public static readonly DependencyProperty LayerSelectedProperty = DependencyProperty.Register(
            "LayerSelected", typeof(Layers.Layer), typeof(MainViewModel));
        public Layers.Layer LayerSelected
        {
            get { return (Layers.Layer)GetValue(LayerSelectedProperty); }
            set
            {
                SetValue(LayerSelectedProperty, value);
                OnPropertyChanged("LayerSelected");
            }
        }

        public static readonly DependencyProperty LayerSelectedPreviewProperty = DependencyProperty.Register(
            "LayerSelectedPreview", typeof(ImageSource), typeof(MainViewModel));
        public ImageSource LayerSelectedPreview
        {
            get { return (ImageSource)GetValue(LayerSelectedPreviewProperty); }
            set
            {
                SetValue(LayerSelectedPreviewProperty, value);
                OnPropertyChanged("LayerSelectedPreview");
            }
        }

        public static readonly DependencyProperty LayerSelectedMaskProperty = DependencyProperty.Register(
            "LayerSelectedMask", typeof(ImageSource), typeof(MainViewModel));
        public ImageSource LayerSelectedMask
        {
            get { return (ImageSource)GetValue(LayerSelectedMaskProperty); }
            set
            {
                SetValue(LayerSelectedMaskProperty, value);
                OnPropertyChanged("LayerSelectedMask");
            }
        }
        #endregion

        public ColorProfile.ColorProfileVM cpVM;

        public MainViewModel()
        {
            cpVM = new ColorProfile.ColorProfileVM();
            //LayerPanel = new StackPanel();
            LayerPanelElements = new ObservableCollection<UIElement>();
            StatusBar = new StatusBar();
            CreateLayerButtons();
            Projects = new List<ProjectClassLib.Project>();

            LayersSortDirection = ListSortDirection.Descending;
            OnChangeLayersDirection?.Invoke(LayersSortDirection);

            SetStatusBarContent();

            //lpcProps = new ControlLibrary.LayerPropertiesControl();
        }




        #region Layer panel
        public void CreateLayerButtons()
        {
            List<string> layers = LayerPanelOperations.GetAllLayers();
            bool favorites = false;

            foreach (string layer in layers)
            {
                if (LayerPanelLayers == null)
                {
                    LayerPanelLayers = new List<UIElement>();
                }
                LayerPanelLayers.Add(CreateLayerButton(layer, false));
                LayerPanelElements.Add(LayerPanelLayers.Last());
            }

            LayerPanelFavorites = LayerPanelOperations.LoadFavoriteLayers();
            favorites = LayerPanelFavorites == null ? false : true;

            if (favorites)
            {
                LayerPanelOperations.CreateFavorites(LayerPanelElements, LayerPanelFavorites);
                FillFavorites();
            }

        }



        /// <summary>
        /// Method to create layer "button" (image)
        /// </summary>
        /// <param name="name">Layer name (np: Treko; 2D)</param>
        /// <param name="favorite">True if layer in favorites list</param>
        /// <returns></returns>
        private System.Windows.Controls.Button CreateLayerButton(string name, bool favorite)
        {
            System.Windows.Controls.Button result = new System.Windows.Controls.Button();
            //result.Width = result.Height = 100;
            result.Click += NewLayer_Click;
            result.HorizontalAlignment = HorizontalAlignment.Center;
            result.Margin = new Thickness(3);
            result.Name = $"btn{name}";
            result.ToolTip = name;
            result.BorderThickness = new Thickness(0);

            result.VerticalAlignment = VerticalAlignment.Stretch;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;

            //image source
            string path = FileOperations.FileStructure.GetLayersResourcesFolderPath() + $"{name}.png";
            if (FileOperations.FileAccess.CheckFileExists(path))
            {
                //load image
                ImageSource imageSource = ImageProcessing.ImageProcessingUI.LoadImageSourceFromFile(path);
                if (imageSource != null)
                {
                    System.Windows.Controls.Image btnImg = new System.Windows.Controls.Image();
                    btnImg.Source = imageSource;
                    result.Content = btnImg;
                }
            }
            ContextMenu cm = new ContextMenu();
            cm.StaysOpen = true;
            MenuItem newlayer = LayerPanelOperations.CreateContextMenuItem($"New {name}", NewLayer_Click);
            cm.Items.Add(newlayer);
            MenuItem mi = LayerPanelOperations.AddLayerFavMenuItem(favorite, AddToFavorites, RemoveFromFavorites);
            cm.Items.Add(mi);
            result.ContextMenu = cm;

            return result;
        }


        #region Favorites User Actions
        private void AddToFavorites(object sender, RoutedEventArgs e)
        {
            string name = LayerPanelOperations.GetLayerNameFromControlContextMenu(sender);

            if (!string.IsNullOrWhiteSpace(name))
            {
                LayerPanelOperations.CreateFavorites(LayerPanelElements, LayerPanelFavorites);
                if (LayerPanelFavorites == null)
                {
                    LayerPanelFavorites = new List<string>();
                    //CreateFavorites
                    FillFavorites();
                }

                if (!LayerPanelFavorites.Contains(name))
                {
                    LayerPanelFavorites.Add(name);
                }

                LayerPanelOperations.MoveLayerFavorites<UIElement>(LayerPanelElements, name, LayerPanelFavorites.Count - 1);

                LayerPanelOperations.ChangeFavoriteHeader<UIElement>(LayerPanelElements, name, true, AddToFavorites, RemoveFromFavorites);

                LayerPanelOperations.SaveFavoriteLayers(LayerPanelFavorites);
                /*Image i = CreateLayerButton(name, true);
                if (!LayerPanelFavorites.Contains(GetLayerNameFromElement(i.Name)))
                {
                    LayerPanelFavorites.Add(GetLayerNameFromElement(i.Name));
                }*/
            }
        }

        private void RemoveFromFavorites(object sender, RoutedEventArgs e)
        {
            string name = LayerPanelOperations.GetLayerNameFromControlContextMenu(sender);



            if (!string.IsNullOrWhiteSpace(name))
            {
                if (LayerPanelFavorites == null)
                {
                    LayerPanelFavorites = new List<string>();
                    //CreateFavorites
                    FillFavorites();
                }

                int favoritesCount = LayerPanelFavorites.Count - 1 + 1; //1 - separator

                int layerIndex = LayerPanelOperations.FindUIElementIndex<UIElement>(LayerPanelLayers, name);

                for (int i = layerIndex; i > 0; i--)
                {
                    int elementIndex = LayerPanelOperations.FindUIElementIndex<UIElement>(
                        LayerPanelLayers, LayerPanelOperations.GetLayerNameFromControlContextMenu(LayerPanelLayers[i])
                        );
                    if (elementIndex > favoritesCount)
                    {
                        layerIndex = favoritesCount + i;
                        break;
                    }
                }

                if (layerIndex == 0)
                {
                    layerIndex = favoritesCount - 1;
                }

                LayerPanelOperations.MoveLayerFavorites<UIElement>(LayerPanelElements, name, layerIndex);

                LayerPanelOperations.ChangeFavoriteHeader<UIElement>(LayerPanelElements, name, false, AddToFavorites, RemoveFromFavorites);

                if (LayerPanelFavorites.Contains(name))
                {
                    LayerPanelFavorites.Remove(name);
                }

                if (LayerPanelFavorites != null)
                {
                    if (LayerPanelFavorites.Count < 1)
                    {
                        LayerPanelOperations.RemoveFavorites(LayerPanelElements);
                    }
                }
                else
                {
                    LayerPanelOperations.RemoveFavorites(LayerPanelElements);
                }
                LayerPanelOperations.SaveFavoriteLayers(LayerPanelFavorites);
            }
        }
        #endregion


        private void FillFavorites()
        {
            int favIndex = 0;
            foreach (string name in LayerPanelFavorites)
            {
                LayerPanelOperations.MoveLayerFavorites<UIElement>(LayerPanelElements, name, favIndex);
                LayerPanelOperations.ChangeFavoriteHeader<UIElement>(LayerPanelElements, name, true, AddToFavorites, RemoveFromFavorites);
                favIndex++;
            }
        }
        #endregion


        #region status bar
        /// <summary>
        /// StatusBar content for selected project
        /// </summary>
        private void SetStatusBarContent()
        {
            ProjectClassLib.Project project = null;
            if (Projects != null)
            {
                if (Projects.Count > 0)
                {
                    project = !SelectedProject.HasValue ? null : SelectedProject.Value >= Projects.Count ? null : Projects[SelectedProject.Value];

                    //project = SelectedProject.GetValueOrDefault(0) < Projects.Count ? Projects[SelectedProject.GetValueOrDefault(0)] : null;
                }
            }

            SetStatusBarContent(project);
        }

        private void SetStatusBarContent(ProjectClassLib.Project project)
        {
            if (StatusBar == null)
            {
                StatusBar = new StatusBar();
            }
            StatusBar.GetProjectInfo(project);
        }
        #endregion

        

        

        public void ShowSelectedLayerPreview()
        {
            String message = "";
            string m1 = Environment.NewLine + "No projects available";
            string m2 = Environment.NewLine + "No layers available in selected project";
            if (Projects == null)
            {
                message = m1;
            }
            else
            {
                if (Projects.Count < 1)
                {
                    message = m1;
                }
                else
                {
                    if (!SelectedProject.HasValue)
                    {
                        message = Environment.NewLine + "No project selected";
                    }
                    else
                    {
                        if (Projects[SelectedProject.GetValueOrDefault(0)].Layers == null)
                        {
                            message = m2;
                        }
                        else
                        {
                            if (Projects[SelectedProject.GetValueOrDefault(0)].Layers.Count < 1)
                            {
                                message = m2;
                            }
                            else
                            {
                                int selectedLayer = -1;//Projects[SelectedProject.GetValueOrDefault(0)].LayerSelected;
                                //Projects[SelectedProject.GetValueOrDefault(0)].LayerSelected = selectedLayer;
                                if (selectedLayer < 0)
                                {
                                    message = Environment.NewLine + "No layer selected";
                                }
                                else
                                {
                                    Layers.Layer l = Projects[SelectedProject.GetValueOrDefault(0)].Layers[selectedLayer];
                                    message = Environment.NewLine + $"Layer{l.Name}; tn:{l.TechnologyName}; order:{l.Order}; id:{l.Id}; enabled:{l.Enabled}";

                                    PropertyInfo anglesInfo = l.GetType().GetProperty("AnglesPath");
                                    if (anglesInfo != null)
                                    {
                                        string anglesPath = (string)anglesInfo.GetValue(l);

                                        message += Environment.NewLine + $"Angles exist; Path:{l.ImagePath};";
                                    }

                                    message += Environment.NewLine + $"Image path{l.ImagePath};";
                                    message += Environment.NewLine + $"Mask path{l.MaskPath};";
                                    message += Environment.NewLine + $"ProjectId:{l.ProjectID}";
                                }
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Preview!" + message);
        }

        

        #region Binding
        private void SetOneWayBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty)
        {
            SetBinding(source, sourcePropertyName, targetObject, targetProperty, BindingMode.OneWay);
        }

        private void SetTwoWayBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty)
        {
            SetBinding(source, sourcePropertyName, targetObject, targetProperty, BindingMode.TwoWay);
        }

        private void SetBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty, BindingMode mode)
        {
            Binding b = new Binding();
            b.Source = source;
            b.Path = new PropertyPath(sourcePropertyName);
            b.Mode = mode;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(targetObject, targetProperty, b);
        }
        #endregion
        #region LayerProperties
        private void BindLayerObjToPropertyControl(ProjectClassLib.Project project, Layers.Layer layer)
        {
            if (layer != null)
            {
                if (lpcProps == null) lpcProps = new ControlLibrary.LayerPropertiesControl(); // invoke
                lpcProps.DataContext = layer;
                //SetOneWayBinding(layer, nameof(layer), (ViewModel)this, ViewModel.LayerSelectedProperty);
                //SetOneWayBinding(layer, nameof(layer.Name), (ViewModel)this, ViewModel.LayerSelectedNameProperty);
                //var a = LayerSelectedName;
                //var b = LayerSelected.Name;

                if (layer.ColorProfile == null)
                {
                    layer.ColorProfile = new ColorProfile.ColorProfile();
                    layer.ColorProfile.ArcWidth = 12 * 1000; //nm
                }

                if (cpVM == null) cpVM = new ColorProfile.ColorProfileVM();
                //lpcProps.VM = cpVM;
                //Set TwoWay Binding
                cpVM.LoadByColorProfile(layer.ColorProfile);
                SetTwoWayBinding(layer, nameof(layer.ColorProfile), cpVM, ColorProfile.ColorProfileVM.ColorProfileProperty);
                SetTwoWayBinding(layer.ColorProfile, nameof(layer.ColorProfile.ArcWidth), cpVM, ColorProfile.ColorProfileVM.ArcWidthProperty);
                layer.ColorProfile.ArcWidth = 12000;
                //SetTwoWayBinding(layer.ColorProfile, nameof(layer.ColorProfile.ArcWidth), cpVM, ColorProfile.ColorProfileVM.ArcWidthProperty);

                //var a = cpVM.ColorProfile;

                //cpVM.ArcWidthLinearUnit = DataTypes.LinearUnit.nm;
                //cpVM.ArcWidthDimnsionUnit = DataTypes.ImageDimnsionUnit.pix;
                /*cpVM.Loading = true;
                cpVM.ArcWidth = 4.32m * layer.Order + 1;
                cpVM.Loading = false;*/
                //layer.ColorProfile.ArcWidth = (4.32m * layer.Order + 1) * 1000;
                lpcProps.VM = cpVM;
                


                //thumbnails
                //SetOneWayBinding(layer.Thumbnail, nameof(layer.Thumbnail.ImageSourceValue), this, ViewModel.LayerSelectedPreviewProperty);
                //SetOneWayBinding(layer.MaskThumbnail, nameof(layer.MaskThumbnail.ImageSourceValue), this, ViewModel.LayerSelectedMaskProperty);
            }
            //t.Name
            /*var b1 = sender.GetType().GetProperty("Name");
            var c = b1.GetValue(sender);*/


            //SetOneWayBinding(t, nameof(t.Id), lpcProps, ControlLibrary.LayerPropertiesControl.LayerNameProperty);
            //SetOneWayBinding(t, nameof(t.Name), lpcProps, ControlLibrary.LayerPropertiesControl.LayerNameProperty);
            //SetOneWayBinding(t, nameof(t.Order), lpcProps, ControlLibrary.LayerControl.LayerOrderProperty);
            /*Binding b = new Binding();
            b.Source = (Layers.Layer)sender;
            b.Path = new PropertyPath("Name");
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(lpcProps, ControlLibrary.LayerPropertiesControl.LayerName_DP, b);
            lpcProps.InvalidateProperty(ControlLibrary.LayerPropertiesControl.LayerName_DP);*/
        }
        #endregion

        #region Project
        #region Actions
        /// <summary>
        /// Method to call new project dialog window
        /// </summary>
        /// <param name="owner"></param>
        public void NewProject(Window owner)
        {
            //Checking project count and limiter
            bool outOfLimit = IsNewProjectOutOfLimit();
            bool save = false;

            bool cancel = false;

            //SelectedProject
            //Select the last
            int indexSelected = Projects.Count > 0 ? Projects.Count - 1 : 0;
            ProjectClassLib.Project selProject = null;
            if (SelectedProject.HasValue)
            {
                indexSelected = SelectedProject.Value;
                selProject = Projects[indexSelected];
            }
            bool noDialog = false;
            MessageBoxResult dr = MessageBoxResult.Cancel;
            if (outOfLimit)
            {
                dr = MessageBox.Show("Projects count max. Do you want save current project", "Croject collision", MessageBoxButton.YesNoCancel);
            }
            else
            {
                noDialog = true;
            }

            if (!noDialog)
            {
                switch (dr)
                {
                    //Save
                    case MessageBoxResult.Yes:
                        save = true;
                        break;
                    //No save
                    case MessageBoxResult.No:
                        save = false;
                        break;
                    //Cancel
                    case MessageBoxResult.Cancel:
                        cancel = true;
                        break;
                }
            }

            if (outOfLimit)
            {

                if (save)
                {
                    SaveProject(selProject, false);
                }
            }

            ShowProjectDialog(owner, null, false);
        }

        /// <summary>
        /// Method to show project dialog
        /// </summary>
        /// <param name="owner"></param>
        public void ShowProjectDialog(Window owner, ProjectClassLib.Project project, bool editMode)
        {
            ControlLibrary.ProjectControl pc = new ControlLibrary.ProjectControl();
            NewProjectWindow pcw = new NewProjectWindow(editMode);
            pcw.Height = 500;
            pcw.Width = 400;
            pcw.Owner = owner;
            pcw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            pcw.OnApplyProject += AddProject;
            //pcw.OnSaveProject += SaveProject;
            pcw.OnDeleteProject += DeleteProject;
            if (project == null && editMode)
            {
                editMode = false;
            }

            //edit current existed project
            if (editMode)
            {
                pcw.LoadControlByProject(project);
            }
            //new project
            else
            {
                pcw.LoadProjectTemplate();
            }
            //pcw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            /*SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            pcw.Background = brush;*/
            //pcw.Content = pc;
            pcw.ShowDialog();
        }

        public void OpenProjectFromFile(Window owner)
        {
            ControlLibrary.DialogService.FileDialogService FDS = new ControlLibrary.DialogService.FileDialogService();
            if (FDS.OpenFileDialog())
            {
                AddProjectToListFromFile(FDS.FilePath);
            }
        }

        public void AddProjectToListFromFile(string filePath)
        {
            ProjectClassLib.Project project = null;
            try
            {
                project = ProjectClassLib.ProjectLogic.LoadProjectFromFile(filePath);
                //If open the same project
                ProjectClassLib.Project sameProject = FindTheSameProjects(project);
                //Checking project count and limiter
                bool outOfLimit = IsNewProjectOutOfLimit();
                bool save = false;

                bool cancel = false;

                //SelectedProject
                //Select the last
                int indexSelected = Projects.Count > 0 ? Projects.Count - 1 : 0;
                ProjectClassLib.Project selProject = null;
                if (SelectedProject.HasValue)
                {
                    indexSelected = SelectedProject.Value;
                    selProject = Projects[indexSelected];
                }
                bool noDialog = false;
                MessageBoxResult dr = MessageBoxResult.Cancel;
                if (sameProject != null)
                {
                    dr = MessageBox.Show("Same project already exists. Do you want to save it?", "Croject collision", MessageBoxButton.YesNoCancel);
                }
                else
                {
                    if (outOfLimit)
                    {
                        dr = MessageBox.Show("Projects count max. Do you want save current project", "Croject collision", MessageBoxButton.YesNoCancel);
                    }
                    else
                    {
                        noDialog = true;
                    }
                }

                if (!noDialog)
                {
                    switch (dr)
                    {
                        //Save
                        case MessageBoxResult.Yes:
                            save = true;
                            break;
                        //No save
                        case MessageBoxResult.No:
                            save = false;
                            break;
                        //Cancel
                        case MessageBoxResult.Cancel:
                            cancel = true;
                            break;
                    }
                }

                if (!cancel)
                {
                    if (sameProject != null)
                    {
                        int index = Projects.IndexOf(sameProject);
                        if (save)
                        {
                            SaveProject(sameProject, false);
                            //Reload
                            project = ProjectClassLib.ProjectLogic.LoadProjectFromFile(filePath);

                        }

                        //remove
                        //insert
                        Projects.RemoveAt(index);
                        Projects.Insert(index, project);
                        SelectProject((byte)index, true);
                    }
                    else
                    {
                        if (outOfLimit)
                        {

                            if (save)
                            {
                                SaveProject(selProject, false);
                            }

                            //remove
                            //insert
                            Projects.RemoveAt(indexSelected);
                            Projects.Insert(indexSelected, project);
                            SelectProject((byte)indexSelected, true);
                        }
                        else
                        {

                            //add
                            AddProject(project);
                            //SelectProject(SelectedProject.Value, false);
                            SelectProject((byte)(Projects.Count() - 1), false);
                        }
                    }

                }


            }
            catch (Exception e)
            {
                Projects.Remove(project);
            }
        }

        public void EditCurrentProject(Window owner)
        {
            ProjectClassLib.Project project = GetCurrentProject();

            ShowProjectDialog(owner, project, true);

            /*if (project != null)
            {
                
            }*/
        }


        private ProjectClassLib.Project CreateNewProject(ProjectClassLib.Project project)
        {
            ProjectClassLib.Project result = null;
            try
            {
                if (Projects == null)
                {
                    Projects = new List<ProjectClassLib.Project>();
                }

                bool createNewProject = false;
                bool replaceCurrentProject = false;
                bool saveCurrentProject = false;

                byte lim = 1;
                #region Checking project count and limiter
                if (lim < 1)
                {
                    lim = 1;
                }

                if (Projects.Count == 0)
                {
                    createNewProject = true;
                }
                else
                {
                    if (Projects.Count >= lim)
                    {
                        replaceCurrentProject = true;
                        //Ask about saving

                        //Messagebox: ?
                        MessageBoxResult dr = MessageBox.Show("Projects count max. Do you want save current project", "Croject collision", MessageBoxButton.YesNoCancel);
                    }
                    else
                    {
                        //Find the same;
                        //Messagebox: Same project found in list. Do you want save current project?
                    }
                }
                #endregion

                if (createNewProject | replaceCurrentProject)
                {
                    //Replace current with saving
                    if (saveCurrentProject)
                    {
                        //SaveProject(currentProject, false);
                    }
                    //Create new / Replace current without saving

                    //New
                    if (createNewProject)
                    {
                        AddProject(project);
                        SelectProject(SelectedProject.Value, false);
                    }
                    //Replace
                    else
                    {
                        Projects.RemoveAt(SelectedProject.Value);
                        Projects.Insert(SelectedProject.Value, result);
                        SelectProject(SelectedProject.Value, true);
                    }
                }
                //Cancel (!newReplace && !saveCurrent)
                else
                {
                    result = null;
                }

                /*
                bool projectExistsCurrent = false;
                if (Projects != null)
                {
                    result = Projects.Find(x => x.CreationDate.Equals(project.CreationDate) && x.LastUpdatedDate.Equals(project.LastUpdatedDate) &&
                    x.Name.Equals(project.Name) && x.id.Equals(project.id) && x.FullPath.Equals(project.FullPath) && (x.ResultFullPath == project.ResultFullPath));

                    if (SelectedProject.HasValue)
                    {
                        if (Projects.IndexOf(p).Equals(SelectedProject.Value))
                        {
                            projectExistsCurrent = true;
                        }
                    }
                }*/



                /*if (p != null)
                {
                    if (projectExistsCurrent)
                    {
                        MessageBoxResult dr = MessageBox.Show("Same project already exists. Do you want to save it?", "Croject collision", MessageBoxButton.YesNoCancel);
                        //Save and replace
                        if (dr == MessageBoxResult.Yes)
                        {
                            SaveProject(p, false);
                        }
                        //Cancel
                        else if (dr == MessageBoxResult.Cancel)
                        {
                            return;
                        }

                        Projects.RemoveAt(SelectedProject.Value);
                        Projects.Insert(SelectedProject.Value, project);
                        SelectProject(SelectedProject.Value, true);
                    }
                }
                else
                {
                    AddProject(project);
                    SelectProject(SelectedProject.Value, false);
                }*/
                /*Projects.Add(project);
                SelectedProject = (byte)(Projects.Count() - 1);
                SetStatusBarContent();*/
            }
            catch (Exception e)
            {
                Projects.Remove(project);
            }
            return result;
        }

        private ProjectClassLib.Project FindTheSameProjects(ProjectClassLib.Project project)
        {
            ProjectClassLib.Project foundProject = null;
            if (Projects != null)
            {
                if (Projects != null)
                {
                    foundProject = Projects.Where(x => x.CreationDate.Equals(project.CreationDate) && x.LastUpdatedDate.Equals(project.LastUpdatedDate) &&
                    x.Name.Equals(project.Name) && x.id.Equals(project.id) && x.FullPath.Equals(project.FullPath) && (x.ResultFullPath == project.ResultFullPath)).FirstOrDefault();
                }
            }
            return foundProject;
        }

        private bool IsNewProjectOutOfLimit()
        {
            bool result = false;

            byte lim = 1;

            if (lim < 1)
            {
                lim = 1;
            }

            if (Projects.Count >= lim)
            {
                result = true;
            }

            return result;
        }

        private void SelectProject(byte index, bool reselect)
        {
            if (Projects == null)
            {
                throw new Exception("No projects available");
            }
            if (Projects.Count < 1)
            {
                throw new Exception("No projects available");
            }

            bool select = false;
            if (!SelectedProject.HasValue)
            {
                select = true;
            }

            if (!select)
            {
                if (SelectedProject.Value < (byte)Projects.Count())
                {
                    if (!SelectedProject.Value.Equals(index))
                    {
                        select = true;
                    }
                }
                else
                {
                    throw new Exception("Project index in collection out of range");
                }
            }

            if (select || reselect)
            {
                if (select)
                {
                    SelectedProject = index;
                }
                else
                {
                    //Add layers
                    if (CheckLayerControlsNotEmpty())
                    {
                        ClearLayers();
                    }
                    //Unselect layer properties
                    //asd
                }

                CreateLayersFromProject();
                //Change title may be
                SetStatusBarContent();

            }
        }

        private bool DeleteProject(ulong projectId)
        {
            bool success = false;

            #region UI
            UnselectLayer();
            ClearLayers();
            #endregion

            return success;
        }
        #endregion

        #region Save/Load
        public void SaveProjectAs(Window owner)
        {
            //Path
            ProjectClassLib.Project project = Projects[SelectedProject.GetValueOrDefault(0)];
            if (project != null)
            {
                SaveProject(project, true);
            }
        }

        public void SaveProject(Window owner)
        {
            ProjectClassLib.Project project = Projects[SelectedProject.GetValueOrDefault(0)];
            if (project != null)
            {
                SaveProject(project, false);
            }
        }

        private bool SaveProject(ProjectClassLib.Project project, bool createNew)
        {
            bool success = true;
            try
            {
                if (string.IsNullOrWhiteSpace(project.OpenedFilePath))
                {
                    createNew = true;
                }

                if (createNew)
                {
                    project.SetCreationDate(DateTime.UtcNow);
                    project.LastUpdatedDate = project.CreationDate;
                }
                else
                {
                    project.LastUpdatedDate = DateTime.Now;
                }

                ProjectClassLib.ProjectLogic.SaveProject(project, project.FullPath, true);
                //AddProjectToListFromFile(project.FullPath);
                project.ProjectSaved = true;
                if (project.OpenedFilePath == null)
                {
                    project.SetFilePath(project.FullPath);
                }
                else if (project.OpenedFilePath.Equals(project.FullPath))
                {
                    project.SetFilePath(project.FullPath);
                }

                //SelectProject(SelectedProject.GetValueOrDefault(0), true);
                /*Projects.Add(project);
                SelectedProject = (byte)(Projects.Count() - 1);*/
            }
            catch (Exception e)
            {
                success = false;
                MessageBox.Show(e.Message);
            }

            //FileOperations.SavindLoading.SaveJSON(project, project.FullPath);
            //CreateFolders
            /*string layerDir = FileOperations.FileStructure.GetLayersFolder(project.FullPath);
            if (!FileOperations.FileAccess.CheckDirectoryExists(layerDir))
            {
                //FileOperations.FileAccess.CreateDirectory(layerDir);
            }*/

            return success;
        }
        #endregion

        private ProjectClassLib.Project GetCurrentProject()
        {
            ProjectClassLib.Project project = null;

            try
            {
                project = Projects[SelectedProject.GetValueOrDefault(0)];
            }
            catch (Exception e)
            {

            }

            return project;
        }

        public bool AddProject(ProjectClassLib.Project project)
        {
            ProjectClassLib.Project proj = null;
            if (Projects == null)
            {
                Projects = new List<ProjectClassLib.Project>();
            }
            try
            {
                //Deep copy
                proj = project.Clone();
                //Checking project count and limiter
                bool outOfLimit = IsNewProjectOutOfLimit();
                bool save = false;

                bool cancel = false;

                //SelectedProject
                //Select the last
                int indexSelected = Projects.Count > 0 ? Projects.Count - 1 : 0;
                ProjectClassLib.Project selProject = null;
                if (SelectedProject.HasValue)
                {
                    indexSelected = SelectedProject.Value;
                    selProject = Projects[indexSelected];
                }

                if (outOfLimit)
                {
                    //remove
                    //insert
                    Projects.RemoveAt(indexSelected);
                    Projects.Insert(indexSelected, project);
                    SelectProject((byte)indexSelected, true);
                }
                else
                {
                    Projects.Add(proj);
                    SelectProject((byte)(Projects.Count() - 1), false);
                }

                
            }
            catch (Exception e)
            {
                Projects.Remove(proj);
                return false;
            }
            return true;
        }
        #endregion


        #region Layers
        #region UI
        /// <summary>
        /// Method to create layer controls from selected project
        /// </summary>
        private void CreateLayersFromProject()
        {
            UnselectLayer();
            ProjectClassLib.Project project = GetCurrentProject();

            if (project != null)
            {
                if (project.Layers != null)
                {
                    if (project.Layers.Count > 0)
                    {
                        if (ProjectLayers == null)
                        {
                            ProjectLayers = new ObservableCollection<ControlLibrary.LayerControl>();

                            foreach (Layers.Layer layer in project.Layers)
                            {
                                //Control c = new Control();
                                AddLayerControl(layer, project.id, false);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to add layer to project and UI
        /// </summary>
        /// <param name="newLayer"></param>
        /// <returns></returns>
        private bool AddLayerControl(Layers.Layer layer, ulong projectID, bool selectLayer)
        {
            bool success = false;

            if (ProjectLayers == null)
            {
                ProjectLayers = new ObservableCollection<ControlLibrary.LayerControl>();
            }

            ControlLibrary.LayerControl c = new ControlLibrary.LayerControl(layer.Enabled, layer.OpticalSchema, layer.TechnologyName, projectID);
            #region binding properties to layer control
            SetOneWayBinding(layer, nameof(layer.Id), c, ControlLibrary.LayerControl.LayerIdProperty);
            SetOneWayBinding(layer, nameof(layer.Name), c, ControlLibrary.LayerControl.LayerNameProperty);

            //thumbnails
            SetOneWayBinding(layer.Thumbnail, nameof(layer.Thumbnail.ImageSourceValue), c, ControlLibrary.LayerControl.LayerPreviewProperty);
            SetOneWayBinding(layer.MaskThumbnail, nameof(layer.MaskThumbnail.ImageSourceValue), c, ControlLibrary.LayerControl.LayerMaskProperty);

            SetTwoWayBinding(layer, nameof(layer.Order), c, ControlLibrary.LayerControl.LayerOrderProperty);
            SetTwoWayBinding(layer, nameof(layer.Enabled), c, ControlLibrary.LayerControl.LayerEnabledProperty);
            SetTwoWayBinding(layer, nameof(layer.Selected), c, ControlLibrary.LayerControl.LayerSelectedProperty);
            #endregion
            //BindLayerObjToPropertyControl(curProject, newLayer);

            c.HorizontalAlignment = HorizontalAlignment.Stretch;
            c.VerticalAlignment = VerticalAlignment.Stretch;
            c.Margin = new Thickness(0, 2, 0, 2);
            c.OnLayerUpClicked += LayerUp;
            c.OnLayerDownClicked += LayerDown;
            c.OnDeleteLayer += DeleteLayer;
            c.OnLayerControlClicked += SelectLayer;
            //c.LayerEnabled = 
            ProjectLayers.Add(c);


            OnChangeLayersDirection?.Invoke(LayersSortDirection);

            if (selectLayer)
            {
                SelectLayer(layer);
            }

            return success = true;
        }

        /// <summary>
        /// Method to clear all layer controls
        /// </summary>
        private void ClearLayers()
        {
            if (ProjectLayers != null)
            {
                ProjectLayers.Clear();
                ProjectLayers = null;
            }
        }
        #endregion

        #region Actions
        private void NewLayer_Click(object sender, RoutedEventArgs e)
        {
            ProjectClassLib.Project curProject = GetCurrentProject();
            if (curProject == null) return;
            if (Owner == null) return;

            ControlLibrary.ProjectControl pc = new ControlLibrary.ProjectControl();
            NewLayerWindow nlWindow = new NewLayerWindow(LayerPanelOperations.GetLayerNameFromControl(sender), curProject);
            nlWindow.Height = 500;
            nlWindow.Width = 400;
            nlWindow.Owner = Owner;
            nlWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            nlWindow.OnCreateLayer += AddNewLayer;
            nlWindow.OnDeleteProject += DeleteProject;
            //pcw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            /*SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(48, 48, 48));
            pcw.Background = brush;*/
            //pcw.Content = pc;
            nlWindow.ShowDialog();
            return;

            /*string technologyName = LayerPanelOperations.GetLayerNameFromControl(sender);

            Layers.Layer l = Layers.Layer.NewLayer(technologyName);

            l.Id = ProjectClassLib.ProjectLogic.GetNewLayerID(curProject);

            #region test layerFill
            l.ImagePath = @"E:\New project 1\Layers\Layer 1\SourceImages\Star3.bmp";
            l.MaskPath = @"E:\New project 1\Layers\Layer 1\Mask\Holog-30x30_Mask_Face.bmp";
            //l.Image = ImageProcessing.ImageProcessing.LoadMatFromFile(l.ImagePath);
            //l.Mask = ImageProcessing.ImageProcessing.LoadMatFromFile(l.MaskPath);

            #endregion

            if (curProject.Layers == null) curProject.Layers = new List<Layers.Layer>();

            bool ena = true;
            /*ListSortDirection dir = ListSortDirection.Descending;
            if (Projects[SelectedProject.GetValueOrDefault(0)].Layers.Count > 1)
            {
                ena = !Projects[SelectedProject.GetValueOrDefault(0)].Layers[Projects[SelectedProject.GetValueOrDefault(0)].Layers.Count - 1].Enabled;
                int s = Projects[SelectedProject.GetValueOrDefault(0)].Layers.Count / 5;
                int ss = Projects[SelectedProject.GetValueOrDefault(0)].Layers.Count % 5;
                if (ss == 0)
                {
                    dir = s % 2 == 0 ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    OnChangeLayersDirection?.Invoke(dir);
                    l.ImagePath = @"E:\New project 1\Layers\Layer 4\SourceImages\Basket_03.png";
                    l.Image = ImageProcessing.ImageProcessing.LoadMatFromFile(l.ImagePath);
                }
            }*/


            //curProject.Layers.Add(l);

            /*l.Order = curProject.Layers.Count - 1;

            l.Image = ImageProcessing.ImageProcessing.LoadMatFromFile(l.ImagePath);
            l.Mask = ImageProcessing.ImageProcessing.LoadMatFromFile(l.MaskPath);

            //ConstructorInfo ctor = 
            /*l.Name = "Layer";

            PropertyInfo anglesInfo = l.GetType().GetProperty("AnglesPath");
            if (anglesInfo != null)
            {
                string anglesPath = @"E:\New project 1\Layers\Layer 1\AnglesFile\ang.ini";
                anglesInfo.SetValue(l, anglesPath);
            }

            PropertyInfo stepInfo = l.GetType().GetProperty("Step");
            if (stepInfo != null)
            {
                decimal step = 0.625m;
                stepInfo.SetValue(l, step);
            }

            PropertyInfo radiusInfo = l.GetType().GetProperty("ArcRadius");
            if (radiusInfo != null)
            {
                DataContainers.Resolution<decimal> arcRadius = new DataContainers.Resolution<decimal>(20, 0.2m);
                radiusInfo.SetValue(l, arcRadius);
            }*/

            /*l.Enabled = ena;

            l.Order = curProject.Layers.Count - 1;
            l.Name += $" {curProject.Layers.Count()} - {l.GetType().Name}";*/

            //AddNewLayer(l);
            //Select layer
            //SelectLayer(l);
            //curProject.LayerSelected = l.Order;

        }

        /// <summary>
        /// Method to select layer from layer control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectLayer(object sender, RoutedEventArgs e)
        {
            ProjectClassLib.Project curProject = GetCurrentProject();

            #region check project
            if (curProject == null)
            {
                return;
            }
            else
            {
                if (curProject.Layers == null)
                {
                    return;
                }
                else if (curProject.Layers.Count < 1)
                {
                    return;
                }
            }
            #endregion

            if (sender.GetType().Equals(typeof(ControlLibrary.LayerControl)))
            {
                PropertyInfo piLayerEnabled = sender.GetType().GetProperty("LayerEnabled");
                PropertyInfo piLayerId = sender.GetType().GetProperty("LayerId");
                int senderID = (int)piLayerId.GetValue(sender);
                Layers.Layer l = curProject.Layers.Find(x => x.Id.Equals(senderID));

                if (l == null) return;

                SelectLayer(l);
            }
        }

        /// <summary>
        /// Method to select some layer
        /// </summary>
        /// <param name="layer"></param>
        private void SelectLayer(Layers.Layer layer)
        {
            if (layer == null)
            {
                return;
            }

            ProjectClassLib.Project curProject = GetCurrentProject();

            #region check project
            if (curProject == null)
            {
                return;
            }
            else
            {
                if (curProject.Layers == null)
                {
                    return;
                }
                else if (curProject.Layers.Count < 1)
                {
                    return;
                }
            }
            #endregion

            //check properties control
            if (lpcProps != null)
            {
                if (lpcProps.DataContext != null)
                {
                    Layers.Layer propLayer = null;
                    if (Layers.Layer.IsLayer(lpcProps.DataContext.GetType()) || lpcProps.DataContext.GetType().Equals(typeof(Layers.Layer)))
                    {
                        //var a = lpcProps.DataContext;
                        PropertyInfo pi_propLayerId = lpcProps.DataContext.GetType().GetProperty("Id");

                        int propID = (int)pi_propLayerId.GetValue(lpcProps.DataContext);
                        propLayer = curProject.Layers.Find(x => x.Id.Equals(propID));
                        if (propLayer == null) return;

                        if (!layer.Id.Equals(propID))
                        {

                            //Unselect layer
                            propLayer.Selected = false;

                            //Select layer
                            layer.Selected = true;

                            lpcProps.Empty = false;
                            //curProject.LayerSelected = layer.Id;
                            #region ShowLayerProperties
                            //BindLayerObjToPropertyControl(curProject, layer);
                            LayerProperteisControlOperations.BindLayerObjToPropertyControl(layer, lpcProps);
                            #endregion
                        }
                    }
                    else
                    {
                        for (int i = 0; i < curProject.Layers.Count; i++)
                        {
                            Layers.Layer l = curProject.Layers[i];
                            if (l.Id != layer.Id)
                            {
                                l.Selected = false;
                            }
                            else
                            {
                                l.Selected = true;
                                lpcProps.Empty = false;
                                #region ShowLayerProperties
                                LayerProperteisControlOperations.BindLayerObjToPropertyControl(layer, lpcProps);
                                //BindLayerObjToPropertyControl(curProject, layer);
                                #endregion
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to unselect selected layer
        /// </summary>
        /// <param name="layer"></param>
        private void UnselectLayer()
        {
            LayerProperteisControlOperations.ClearBindingsLayerPropertyControl(lpcProps);

            lpcProps.Empty = true;

            try
            {
                ProjectClassLib.Project curProject = GetCurrentProject();

                #region check project
                if (curProject == null)
                {
                    return;
                }
                else
                {
                    if (curProject.Layers == null)
                    {
                        return;
                    }
                    else if (curProject.Layers.Count < 1)
                    {
                        return;
                    }
                }
                #endregion

                for (int i = 0; i < curProject.Layers.Count; i++)
                {
                    Layers.Layer l = curProject.Layers[i];
                    l.Selected = false;
                }
            }
            catch (Exception e)
            {

            }
        }

        #region Layer order changing
        private void LayerUp(int layerId, ulong projectId)
        {
            LayerUp(layerId, projectId, 1);
        }

        private void LayerUp(int layerId, ulong projectId, uint step)
        {
            if (step < 0) return;

            ProjectClassLib.Project project = Projects.Find(x => x.id.Equals(projectId));

            if (ProjectClassLib.ProjectLogic.CheckProjectLayersExists(project) && CheckLayerControlsNotEmpty())
            {
                Layers.Layer layer = project.Layers.Find(x => x.Id.Equals(layerId));
                if (layer != null)
                {
                    if (layer.Order + step < project.Layers.Count)
                    {
                        MessageBox.Show("Up: " + layer.Name);
                        Layers.Layer upperLayer = project.Layers.Find(x => x.Order == (int)(layer.Order + step));
                        layer.Order += (int)step;
                        upperLayer.Order -= (int)step;
                        OnChangeLayersDirection?.Invoke(LayersSortDirection);
                    }
                }
            }
        }

        private void LayerDown(int layerId, ulong projectId)
        {
            LayerDown(layerId, projectId, 1);
        }

        private void LayerDown(int layerId, ulong projectId, uint step)
        {
            if (step < 0) return;

            ProjectClassLib.Project project = Projects.Find(x => x.id.Equals(projectId));

            if (ProjectClassLib.ProjectLogic.CheckProjectLayersExists(project) && CheckLayerControlsNotEmpty())
            {
                Layers.Layer layer = project.Layers.Find(x => x.Id.Equals(layerId));
                if (layer != null)
                {
                    if (layer.Order - step >= 0)
                    {
                        MessageBox.Show("Down: " + layer.Name);
                        Layers.Layer lowerLayer = project.Layers.Find(x => x.Order == layer.Order - step);
                        //var a = lowerLayer.Order;
                        //lowerLayer.Order = -1000;
                        layer.Order -= (int)step;
                        //lowerLayer.Order = a++;
                        lowerLayer.Order += (int)step;
                        OnChangeLayersDirection?.Invoke(LayersSortDirection);
                    }
                }
            }
        }

        #endregion

        private bool DeleteLayer(int layerId, ulong projectId)
        {
            bool success = false;
            ProjectClassLib.Project project = Projects.Find(x => x.id.Equals(projectId));
            if (project != null)
            {
                if (project.Layers != null)
                {
                    Layers.Layer layer = project.Layers.Find(x => x.Id == layerId);
                    if (layer != null)
                    {
                        ControlLibrary.LayerControl lc = ProjectLayers.Where(x => x.LayerId.Equals(layerId)).FirstOrDefault();
                        if (lc != null)
                        {
                            int deletedOrder = layer.Order;
                            project.Layers.Remove(layer);
                            ProjectLayers.Remove(lc);

                            //UpdateOrders
                            if (project.Layers.Count > 0)
                            {
                                UpdateLayersOrders(project);
                            }
                            else
                            {
                                UnselectLayer();
                                //lpcProps = null; //empty panel
                            }
                            success = true;
                        }


                    }
                }
            }
            return success;
        }
        #endregion

        #region Logic
        /// <summary>
        /// Method to add new layer to project and UI
        /// </summary>
        /// <param name="newLayer"></param>
        /// <returns></returns>
        private bool AddNewLayer(Layers.Layer newLayer)
        {
            bool success = false;
            ProjectClassLib.Project curProject = GetCurrentProject();
            if (curProject == null) return success;

            if (curProject.Layers == null)
            {
                curProject.Layers = new List<Layers.Layer>();
            }
            curProject.Layers.Add(newLayer);

            success = AddLayerControl(newLayer, curProject.id, true);
            SetStatusBarContent(curProject);
            return success;
        }

        /// <summary>
        /// Method to update payers orders when delete 1 layer
        /// </summary>
        /// <param name="project"></param>
        private void UpdateLayersOrders(ProjectClassLib.Project project)
        {
            if (project.Layers == null) return;
            if (project.Layers.Count <= 0) return;

            for (int i = 0; i < project.Layers.Count; i++)
            {
                project.Layers.OrderBy(x => x.Order);
                Layers.Layer l = project.Layers[i];
                if (l.Order != i)
                {
                    l.Order = i;
                }
            }
            //OnChangeLayersDirection?.Invoke(LayersSortDirection);
            /*foreach (Control ie in ProjectLayers)
            {
                var a = ((ControlLibrary.LayerControl)ie).LayerOrder;
            }*/
        }
        #endregion

        private bool CheckLayerControlsNotEmpty()
        {
            bool success = true;

            if (ProjectLayers == null)
            {
                success = false;
            }
            else if (ProjectLayers.Count <= 0)
            {
                success = false;
            }

            return success;
        }
        #endregion

















        public void Exit(Window owner)
        {
            //Path
            owner.Close();
        }

        public void CallOptions(Window Owner)
        {

        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class StatusBar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool nullProject = false;

        public string Status
        {
            get
            {
                string status = null;
                status = "No project selected";
                if (nullProject)
                {
                    return status;
                }
                try
                {
                    status = $"Hologram size: {HologramSize.Width}x{HologramSize.Height};";
                    status += new string(' ', 5) + $"Frame resolution: {FrameResolution.X}x{FrameResolution.Y};";
                    status += new string(' ', 5) + $"Layer count: {Convert.ToString(LayersCount, System.Globalization.NumberFormatInfo.InvariantInfo)};";
                    status += new string(' ', 5) + $"Mask resolution: {MaskResolution.X}x{MaskResolution.Y};";
                    status += new string(' ', 5) + $"Gray range: {GrayRange.Low}x{GrayRange.High};";
                    status += new string(' ', 5) + $"UnitSize: {UnitSize};";
                    status += new string(' ', 5) + $"DPI: {DPI};";
                    status += new string(' ', 5) + $"Out path: {OutPath};";
                }
                catch (Exception e)
                {
                    status = "No project selected";
                }
                return status;
            }
            private set
            {
                OnPropertyChanged("Status");
            }
        }

        private DataContainers.LinearDimension<decimal> hologramSize;
        internal DataContainers.LinearDimension<decimal> HologramSize
        {
            get
            {
                return hologramSize;
            }
            set
            {
                hologramSize = value;
                //OnPropertyChanged("HologramSizeText");
                OnPropertyChanged("Status");
            }
        }

        private DataContainers.Resolution frameResolution;
        internal DataContainers.Resolution FrameResolution
        {
            get
            {
                return frameResolution;
            }
            set
            {
                frameResolution = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        private int layersCount;
        internal int LayersCount
        {
            get
            {
                return layersCount;
            }
            set
            {
                try
                {
                    layersCount = value;//Convert.ToInt32(value, System.Globalization.NumberFormatInfo.InvariantInfo);
                    //OnPropertyChanged("LayersCountText");
                    OnPropertyChanged("Status");
                }
                catch
                {

                }
            }
        }

        private DataContainers.Resolution<decimal> maskResolution;
        internal DataContainers.Resolution<decimal> MaskResolution
        {
            get
            {
                return maskResolution;
            }
            set
            {
                maskResolution = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        private Ranges.GreyRange grayRange;

        internal Ranges.GreyRange GrayRange
        {
            get
            {
                return grayRange;
            }
            set
            {
                grayRange = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        private decimal unitSize;
        internal decimal UnitSize
        {
            get
            {
                return unitSize;
            }
            set
            {
                unitSize = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        private decimal dpi;
        internal decimal DPI
        {
            get
            {
                return dpi;
            }
            set
            {
                dpi = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        private string outPath;
        internal string OutPath
        {
            get
            {
                return outPath;
            }
            set
            {
                outPath = value;
                //OnPropertyChanged("HologramResolutionText");
                OnPropertyChanged("Status");
            }
        }

        internal void GetProjectInfo(ProjectClassLib.Project project)
        {
            if (project == null)
            {
                nullProject = true;
            }
            else
            {
                nullProject = false;

                /*int lCount = -1;

                if (project.Layers != null)
                {
                    if (project.Layers.Count > 0)
                    {
                        lCount = project.Layers.Count;
                    }
                }*/


                hologramSize = project.HologramDimension;
                frameResolution = project.FrameResolution;
                maskResolution = new DataContainers.Resolution<decimal>(20000, 1500);
                grayRange = project.GreyRange;
                unitSize = project.UnitSize;
                dpi = 660.8m;
                outPath = project.ResultFullPath;

                //layersCount = lCount > 0 ? lCount : 0;
                layersCount = 0;
                if (project.Layers != null)
                {
                    if (project.Layers.Count > 0)
                    {
                        layersCount = project.Layers.Count;
                    }
                }

                OnPropertyChanged("Status");
            }
        }

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
