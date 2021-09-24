using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace UI_Helper
{
    public static class UI_Helper
    {
        /// <summary>
        /// Method to get technology name of control when it calls thrue the context menu
        /// </summary>
        /// <param name="inputobject"></param>
        /// <returns></returns>
        public static string GetControlNameFromContextMenu(object inputobject)
        {
            string name = null;
            if (inputobject.GetType() == typeof(MenuItem))
            {
                MenuItem mi = (MenuItem)inputobject;
                if (mi.Parent.GetType() == typeof(ContextMenu))
                {
                    ContextMenu cm = (ContextMenu)mi.Parent;
                    name = GetControlName(cm.PlacementTarget);
                    /*if (cm.PlacementTarget.GetType().GetProperty("Name") != null)
                    {
                        var obj = cm.PlacementTarget;
                        name = GetLayerNameFromElement((string)obj.GetType().GetProperty("Name").GetValue(obj));
                    }*/
                }
            }
            return name;
        }

        /// <summary>
        /// Method to get technology name of control
        /// </summary>
        /// <param name="inputobject"></param>
        /// <returns></returns>
        public static string GetControlName(object inputobject)
        {
            string name = null;
            if (inputobject.GetType().GetProperty("Name") != null)
            {
                name = (string)inputobject.GetType().GetProperty("Name").GetValue(inputobject);
            }
            return name;
        }
    }
}
