using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    class LayerProperteisControlOperations
    {
        public static void BindLayerObjToPropertyControl(Layers.Layer layer, ControlLibrary.LayerPropertiesControl control)
        {
            if (layer != null)
            {
                if (control == null) control = new ControlLibrary.LayerPropertiesControl(); // invoke
                control.DataContext = layer;

                if (layer.ColorProfile != null)
                {
                    if (control.VM == null)
                    {
                        control.VM = new ColorProfile.ColorProfileVM();
                    }
                    /*System.Windows.Data.BindingOperations.ClearBinding(control.VM, ColorProfile.ColorProfileVM.ColorProfileProperty);
                    System.Windows.Data.BindingOperations.ClearBinding(control.VM, ColorProfile.ColorProfileVM.ArcWidthProperty);*/
                    ClearBindingsLayerPropertyControl(control);
                    control.VM.LoadByColorProfile(layer.ColorProfile);
                    UI_Helper.Binding.SetTwoWayBinding(layer, nameof(layer.ColorProfile), control.VM, ColorProfile.ColorProfileVM.ColorProfileProperty);
                    UI_Helper.Binding.SetTwoWayBinding(layer.ColorProfile, nameof(layer.ColorProfile.ArcWidth), control.VM, ColorProfile.ColorProfileVM.ArcWidthProperty);
                }
            }
            
        }

        public static void ClearBindingsLayerPropertyControl(ControlLibrary.LayerPropertiesControl control)
        {
            if (control == null) return;
            if (control.VM == null) return;
            System.Windows.Data.BindingOperations.ClearBinding(control.VM, ColorProfile.ColorProfileVM.ColorProfileProperty);
            System.Windows.Data.BindingOperations.ClearBinding(control.VM, ColorProfile.ColorProfileVM.ArcWidthProperty);
        }

    }
}
