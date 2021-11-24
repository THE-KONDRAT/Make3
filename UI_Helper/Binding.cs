using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UI_Helper
{
    public static class Binding
    {
        #region Binding
        public static void SetOneWayBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty)
        {
            SetBinding(source, sourcePropertyName, targetObject, targetProperty, System.Windows.Data.BindingMode.OneWay);
        }

        public static void SetTwoWayBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty)
        {
            SetBinding(source, sourcePropertyName, targetObject, targetProperty, System.Windows.Data.BindingMode.TwoWay);
        }

        public static void SetBinding(object source, string sourcePropertyName, DependencyObject targetObject, DependencyProperty targetProperty, System.Windows.Data.BindingMode mode)
        {
            System.Windows.Data.Binding b = new System.Windows.Data.Binding();
            b.Source = source;
            b.Path = new PropertyPath(sourcePropertyName);
            b.Mode = mode;
            b.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged;
            System.Windows.Data.BindingOperations.SetBinding(targetObject, targetProperty, b);
        }
        #endregion
    }
}
