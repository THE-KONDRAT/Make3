using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectClassLib
{
    public static class ProjectLogic
    {
        public static int GetNewLayerID(Project project)
        {
            int id = -1;

            if (project.Layers != null)
            {
                if (project.Layers.Count > 0)
                {
                    foreach (Layers.Layer layer in project.Layers)
                    {
                        if (layer.Id <= id)
                        {
                            id = layer.Id - 1;
                        }
                    }
                }
            }
            return id;
        }

        public static bool CheckProjectLayersExists(ProjectClassLib.Project project)
        {
            bool success = true;

            if (project == null)
            {
                success = false;
            }
            else
            {
                if (project.Layers == null)
                {
                    success = false;
                }
                else if (project.Layers.Count <= 0)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
