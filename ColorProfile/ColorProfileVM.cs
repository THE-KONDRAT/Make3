using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ColorProfile
{
    public class ColorProfileVM : DependencyObject, INotifyPropertyChanged
    {
        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Loading = false;

        #region get/set
        private byte accuracy = 3; // число знаков после запятой

        //Physical dimension unit
        public static readonly DependencyProperty ArcWidthLinearUnitProperty =
            DependencyProperty.Register(
                "ArcWidthLinearUnit",
                typeof(DataTypes.LinearUnit),
                typeof(ColorProfileVM));
        public DataTypes.LinearUnit ArcWidthLinearUnit
        {
            get { return (DataTypes.LinearUnit)GetValue(ArcWidthLinearUnitProperty); }
            set { SetValue(ArcWidthLinearUnitProperty, value); }
        }

        public static readonly DependencyProperty ArcWidthDimnsionUnitProperty =
            DependencyProperty.Register(
                "ArcWidthDimnsionUnit",
                typeof(DataTypes.ImageDimnsionUnit),
                typeof(ColorProfileVM));
        public DataTypes.ImageDimnsionUnit ArcWidthDimnsionUnit
        {
            get { return (DataTypes.ImageDimnsionUnit)GetValue(ArcWidthDimnsionUnitProperty); }
            set
            {
                SetValue(ArcWidthDimnsionUnitProperty, value);
                SetDimensionUnits(value);
            }
        }
        /*private Layers.Additional_containers.ColorProfile colorProfile;
        public Layers.Additional_containers.ColorProfile ColorProfile
        {
            get { return colorProfile; }
            set
            {
                colorProfile = value;
                OnPropertyChanged("ColorProfile");
            }
        }*/

        public static readonly DependencyProperty ArcWidthProperty = DependencyProperty.Register(
            "ArcWidth", typeof(decimal), typeof(ColorProfileVM),
            new FrameworkPropertyMetadata(
                1.00m, new PropertyChangedCallback(OnArcWidthPropertyChanged)
                )
            );
        public decimal ArcWidth
        {
            get { return (decimal)GetValue(ArcWidthProperty); }
            set
            {
                SetValue(ArcWidthProperty, value);
                OnPropertyChanged("ArcWidth");
            }
        }
        //public decimal ArcWidth2 { get; set; }

        /// <summary>
        /// Arc width.
        /// unit: um
        /// </summary>
        /*private decimal arcWidth;
        public decimal ArcWidth
        {
            get { return arcWidth; }
            set
            {
                arcWidth = value;
                //Validation?
                OnPropertyChanged("ArcWidth");
            }
        }*/

        private decimal mainArcWidth;
        public decimal MainArcWidth
        {
            get { return mainArcWidth; }
            set
            {
                mainArcWidth = value;
                OnPropertyChanged("MainArcWidth");
            }
        }

        private string mainArcWidthUnit;
        public string MainArcWidthUnit
        {
            get { return mainArcWidthUnit; }
            set
            {
                mainArcWidthUnit = value;
                OnPropertyChanged("MainArcWidthUnit");
            }
        }

        private decimal alternateArcWidth;
        public decimal AlternateArcWidth
        {
            get { return alternateArcWidth; }
            set
            {
                alternateArcWidth = value;
                OnPropertyChanged("AlternateArcWidth");
            }
        }

        private string alternateArcWidthUnit;
        public string AlternateArcWidthUnit
        {
            get { return alternateArcWidthUnit; }
            set
            {
                alternateArcWidthUnit = value;
                OnPropertyChanged("AlternateArcWidthUnit");
            }
        }


        #region Profile filters
        public ObservableCollection<Filter> Filters { get; private set; }

        private Filter selectedFilter;
        public Filter SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                FillTemplates();
                OnPropertyChanged("SelectedFilter");
            }
        }
        //private byte selectedFilterID = 0;
        #endregion

        #region Profile templates
        public ObservableCollection<DataContainers.ColorProfileTemplate> ProgramTemplates { get; private set; }
        public ObservableCollection<DataContainers.ColorProfileTemplate> UserTemplates { get; private set; }

        public ObservableCollection<DataContainers.ColorProfileTemplate> Templates { get; private set; }

        private DataContainers.ColorProfileTemplate selectedTemplate;
        public DataContainers.ColorProfileTemplate SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                //not binding
                //OnSetTemplate?.Invoke();
                SetTemplateToColorProfile();
                OnPropertyChanged("SelectedTemplate");
            }
        }
        //private byte selectedFilterID = 0;
        #endregion

        public static readonly DependencyProperty ColorProfileProperty =
            DependencyProperty.Register(
                "ColorProfile",
                typeof(ColorProfile),
                typeof(ColorProfileVM));

        public ColorProfile ColorProfile
        {
            get { return (ColorProfile)GetValue(ColorProfileProperty); }
            set
            {
                SetValue(ColorProfileProperty, value);
                //Set Template or custom
                //SetDimensionUnits(value);
                OnPropertyChanged("ColorProfile");
            }
        }

        private System.Windows.Media.ImageSource cp_thumbnail;
        public System.Windows.Media.ImageSource CpThumbnail
        {
            get { return cp_thumbnail; }
            set
            {
                cp_thumbnail = value;
                OnPropertyChanged("CpThumbnail");
            }
        }
        #endregion

        public ColorProfileVM()
        {
            #region Filters
            FillFilters();
            #endregion

            #region Templates
            FillTemplates();
            #endregion

            ArcWidthLinearUnit = DataTypes.LinearUnit.um;
            ArcWidthDimnsionUnit = DataTypes.ImageDimnsionUnit.si;
        }

        #region UI
        private void SetDimensionUnits(DataTypes.ImageDimnsionUnit unitType)
        {
            if (ArcWidthLinearUnit == null)
            {
                ArcWidthLinearUnit = DataTypes.LinearUnit.um;
            }

            if (unitType.Equals(DataTypes.ImageDimnsionUnit.si))
            {
                MainArcWidthUnit = ArcWidthLinearUnit.ToString();
                AlternateArcWidthUnit = DataTypes.ImageDimnsionUnit.pix.ToString();
            }
            else
            {
                MainArcWidthUnit = DataTypes.ImageDimnsionUnit.pix.ToString();
                AlternateArcWidthUnit = ArcWidthLinearUnit.ToString();
            }
        }
        
        private void FillFilters()
        {
            if (Filters == null)
            {
                Filters = new ObservableCollection<Filter>();
            }

            Filters.Clear();

            Filters.Add(new Filter());
            Filters.Add(new Filter(1, "Symmetrical"));
            Filters.Add(new Filter(2, "Asymmetrical"));

            //load selected filter id
            byte loadedID = 0;
            byte maxID = Filters.Max(x => x.Id);
            if (loadedID > maxID)
            {
                loadedID = 0;
                //selectedFilterID = 0;
            }
            /*else
            {
                selectedFilterID = loadedID;
            }*/
            try
            {
                SelectedFilter = Filters.Single(x => x.Id.Equals(loadedID));
            }
            catch
            {

            }
        }

        private void FillTemplates()
        {
            DataContainers.ColorProfileTemplate st = SelectedTemplate;

            if (Templates == null)
            {
                Templates = new ObservableCollection<DataContainers.ColorProfileTemplate>();
            }
            Templates.Clear();
            List<DataContainers.ColorProfileTemplate> templates = new List<DataContainers.ColorProfileTemplate>();
            Program_templates.ProgramTemplates t = new Program_templates.ProgramTemplates();
            templates = t.Templates;
            ProgramTemplates = new ObservableCollection<DataContainers.ColorProfileTemplate>(templates);

            UserTemplates = new ObservableCollection<DataContainers.ColorProfileTemplate>();

            if (ProgramTemplates != null)
            {
                if (ProgramTemplates.Count > 0)
                {
                    AddTemplates(SelectedFilter.Id, ProgramTemplates);
                    /*foreach (var p in Templates.Union(ProgramTemplates))
                    {
                        Templates.Add(p);
                    }*/
                }
            }

            if (UserTemplates != null)
            {
                if (UserTemplates.Count > 0)
                {
                    AddTemplates(SelectedFilter.Id, UserTemplates);
                    /*foreach (var p in Templates.Union(UserTemplates))
                    {
                        Templates.Add(p);
                    }*/
                }
            }
            

            /*Templates.Clear();

            DataContainers.ColorProfileTemplate a = new DataContainers.ColorProfileTemplate();
            a.Name = "1st";
            a.MinimalMake3Version = "tr3";
            DataContainers.ColorProfileTemplate b = new DataContainers.ColorProfileTemplate();
            b.Name = "2nd";
            b.MinimalMake3Version = "er4";
            DataContainers.ColorProfileTemplate c = new DataContainers.ColorProfileTemplate();
            c.Name = "3rd";
            c.MinimalMake3Version = "te1";
            Templates.Add(a);
            Templates.Add(b);
            Templates.Add(c);*/

            //load selected filter id
            int loadedID = 0;
            int maxID = Templates.Count - 1;
            if (loadedID > maxID)
            {
                loadedID = 0;
                //selectedFilterID = 0;
            }
            /*else
            {
                selectedFilterID = loadedID;
            }*/
            try
            {
                if (st != null)
                {
                    //DataContainers.ColorProfileTemplate ct = Templates.Where(x => x.Name.Equals(st.Name) && x.KeyPoints.Equals(st.KeyPoints)).FirstOrDefault();
                    DataContainers.ColorProfileTemplate ct = Templates.Where(x => x.Equals(st)).FirstOrDefault();
                    int index = Templates.IndexOf(ct);
                    //index = 
                    if (index >= 0)
                    {
                        SelectedTemplate = Templates[index];
                    }
                    else
                    {
                        SelectedTemplate = Templates[0];
                    }
                }
                else
                {
                    SelectedTemplate = Templates[0];
                }

                if (selectedTemplate.Thumbnail == null)
                {
                    SelectedTemplate.CreateThumbnail();
                }
                CpThumbnail = selectedTemplate.Thumbnail.Source;
            }
            catch (Exception e)
            {

            }
        }

        private void AddTemplates(byte filterID, ObservableCollection<DataContainers.ColorProfileTemplate> collection)
        {
            try
            {
                foreach (var p in Templates.Union(collection))
                {
                    if (Templates.IndexOf(p) < 0)
                    {
                        switch (filterID)
                        {
                            //All
                            case 0:
                                Templates.Add(p);
                                break;
                            //Symm
                            case 1:
                                if (p.Symmetric)
                                {
                                    Templates.Add(p);
                                }
                                break;
                            //Asymm
                            case 2:
                                if (!p.Symmetric)
                                {
                                    Templates.Add(p);
                                }
                                break;
                            //All
                            default:
                                Templates.Add(p);
                                break;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public static List<DataContainers.ColorProfileTemplate> GetDefaultTemplates()
        {
            Program_templates.ProgramTemplates t = new Program_templates.ProgramTemplates();
            return t.Templates;
        }
        #endregion

        #region Logic
        public void LoadByColorProfile(ColorProfile profile)
        {
            Loading = true;
            bool load_templates = false;
            if (Templates != null)
            {
                if (Templates.Count <= 0)
                {
                    load_templates = true;
                }
            }
            else
            {
                load_templates = true;
            }

            if (load_templates)
            {
                FillTemplates();
            }

            if (profile.Template)
            {
                DataContainers.ColorProfileTemplate t = null;

                try
                {
                    t = Templates.Single(x => x.Name.Equals(profile.Name));
                }
                catch (Exception e)
                {

                }

                if (t != null)
                {
                    //ColorProfile = profile;
                    ColorProfile = new ColorProfile();
                    SelectedTemplate = t;
                    //ArcWidth = profile.ArcWidth;
                    //SelectedTemplate.ArcWidth = profile.ArcWidth;
                }
                else
                {
                    //load default?
                }
                if (selectedTemplate.Thumbnail == null)
                {
                    SelectedTemplate.CreateThumbnail();
                }
                CpThumbnail = selectedTemplate.Thumbnail.Source;
            }
            Loading = false;
        }

        private void SetTemplateToColorProfile()
        {
            if (Loading) return;
            if (selectedTemplate != null)
            {
                if (ColorProfile != null)
                {
                    int layerID = ColorProfile.LayerID;
                    ColorProfile cp = new ColorProfile();
                    cp.ArcWidth = ArcWidth;
                    cp.id = layerID;
                    cp.Name = selectedTemplate.Name;
                    cp.Template = true;
                    //if not template - false;
                    cp.KeyPointsToTraceProfile(selectedTemplate.KeyPoints);
                    ColorProfile = cp;
                    //selectedTemplate.Thumbnail = crTh();
                    if (selectedTemplate.Thumbnail == null)
                    {
                        SelectedTemplate.CreateThumbnail();
                    }
                    CpThumbnail = selectedTemplate.Thumbnail.Source;
                }
            }
        }

        private System.Windows.Controls.Image crTh()
        {
            System.Windows.Controls.Image img = null;
            //SelectedTemplate.KeyPoints;
            SelectedTemplate.CreateThumbnail();
            img.Source = SelectedTemplate.Thumbnail.Source;
            return img;
        }

        #region Arc width
        private static void OnArcWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.PropertyType.Equals(typeof(decimal)))
            {
                ((ColorProfileVM)sender).UpdateValues(/*(bool)e.NewValue*/);
            }
        }

        private void CalcAlternateWidth()
        {
            AlternateArcWidth = MainArcWidth * 10;
        }

        private void SetArcWidth()
        {
            if (ArcWidthDimnsionUnit.Equals(DataTypes.ImageDimnsionUnit.si))
            {
                ArcWidth = ConvertLinearValueToNM(MainArcWidth);
            }
            else if (ArcWidthDimnsionUnit.Equals(DataTypes.ImageDimnsionUnit.pix))
            {
                //main pix alternate si
                ArcWidth = ConvertLinearValueToNM(AlternateArcWidth);
            }

            if (ColorProfile != null)
            {
                ColorProfile.ArcWidth = ArcWidth;
            }
        }

        public void UpdateValues()
        {
            Loading = true;
            SetVisibleValues(ArcWidth);
            Loading = false;
        }

        private void SetVisibleValues(decimal width)
        {
            System.Globalization.NumberFormatInfo ni = System.Globalization.NumberFormatInfo.InvariantInfo;
            if (ArcWidthDimnsionUnit.Equals(DataTypes.ImageDimnsionUnit.si))
            {
                //main si alternate pix
                MainArcWidth = Convert.ToDecimal(MathLogic.Math.ConvertUnits(
                Convert.ToDouble(ArcWidth, ni), DataTypes.LinearUnit.nm, ArcWidthLinearUnit), ni);
                //MainArcWidth = ConvertNMtoLinearValue(ArcWidth);

                AlternateArcWidth = Math.Round(ConvertNMtoPix(ArcWidth)
                    , accuracy, MidpointRounding.AwayFromZero);
            }
            else if (ArcWidthDimnsionUnit.Equals(DataTypes.ImageDimnsionUnit.pix))
            {
                //main pix alternate si
                MainArcWidth = ConvertNMtoPix(ArcWidth);

                AlternateArcWidth = Math.Round(Convert.ToDecimal(MathLogic.Math.ConvertUnits(
                Convert.ToDouble(ArcWidth, ni), DataTypes.LinearUnit.nm, ArcWidthLinearUnit), ni), 
                    accuracy, MidpointRounding.AwayFromZero);
            }
        }

        private decimal ConvertNMtoLinearValue(decimal value)
        {
            decimal result = value;

            System.Globalization.NumberFormatInfo ni = System.Globalization.NumberFormatInfo.InvariantInfo;

            result = Convert.ToDecimal(MathLogic.Math.ConvertUnits(
                Convert.ToDouble(value, ni), DataTypes.LinearUnit.nm, ArcWidthLinearUnit), ni);
            /*var a = MathLogic.Math.ConvertUnits(15, DataTypes.LinearUnit.inch, DataTypes.LinearUnit.nm);
            var b = MathLogic.Math.ConvertUnits(15, DataTypes.LinearUnit.inch, DataTypes.LinearUnit.um);
            var c = MathLogic.Math.ConvertUnits(1, DataTypes.LinearUnit.m, DataTypes.LinearUnit.nm);
            var d = MathLogic.Math.ConvertUnits(3, DataTypes.LinearUnit.sm, DataTypes.LinearUnit.nm);
            var e = MathLogic.Math.ConvertUnits(8, DataTypes.LinearUnit.mm, DataTypes.LinearUnit.nm);
            var f = MathLogic.Math.ConvertUnits(120, DataTypes.LinearUnit.um, DataTypes.LinearUnit.nm);
            var g = MathLogic.Math.ConvertUnits(120, DataTypes.LinearUnit.m, DataTypes.LinearUnit.inch);

            switch (ArcWidthLinearUnit)
            {
                case DataTypes.LinearUnit.nm:
                    result = value;
                    break;
                case DataTypes.LinearUnit.um:
                    result = value / 1000m;
                    break;

            }*/

            return result;
        }

        private decimal ConvertLinearValueToNM(decimal value)
        {
            decimal result = value;

            switch (ArcWidthLinearUnit)
            {
                case DataTypes.LinearUnit.nm:
                    result = value;
                    break;
                case DataTypes.LinearUnit.um:
                    result = value * 1000m;
                    break;

            }

            return result;
        }

        private static decimal coeff = (decimal)Math.Sqrt(3.14 * 10);

        private decimal ConvertNMtoPix(decimal widthNM)
        {
            return widthNM * coeff;
        }

        private decimal ConvertPixToNM(decimal widthPix)
        {
            return widthPix / coeff;
        }
        #endregion
        #endregion

        private void OnPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                if (property.Equals("MainArcWidth") || property.Equals("MainArcWidthUnit") || property.Equals("AlternateArcWidthUnit"))
                {
                    CalcAlternateWidth();
                    if ((!property.Equals("AlternateArcWidthUnit")) && !Loading)
                    {
                        SetArcWidth();
                    }
                }
                else if (property.Equals("ArcWidth") && Loading)
                {
                    SetVisibleValues(ArcWidth);
                }
            }
        }

        public class Filter
        {
            public byte Id { get; set; }
            public string Name { get; set; }

            public Filter()
            {
                Id = 0;
                Name = "All";
            }

            internal Filter(byte id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }

}