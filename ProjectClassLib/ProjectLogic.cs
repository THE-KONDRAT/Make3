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

        public static uint GetNewLayerOrder(Project project)
        {
            uint order = 0;
            //order = project == null ? 0 : project.Layers == null ? 0 : (uint)project.Layers.Count;
            if (project != null)
            {
                order = project.Layers == null ? 0 : (uint)project.Layers.Count;
            }
            return order;
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

        #region Load/Save Project
        #region Save
        public static void SaveProject(ProjectClassLib.Project project, string filePath, bool newProjectDirectory)
        {
            //check path

            //Create folders
            if (!CheckProjectFolders(project, newProjectDirectory))
            {
                return;
            }

            //Save project file
            FileOperations.SavindLoading.SaveJSON(project, filePath);

            //Save layers

        }
        #endregion

        #region Load (async)
        public static ProjectClassLib.Project LoadProjectFromFile(string filePath)
        {
            ProjectClassLib.Project project = null;
            if (FileOperations.FileAccess.CheckFileExists(filePath))
            {
                project = FileOperations.SavindLoading.LoadJSON<ProjectClassLib.Project>(filePath);
            }

            //Load layer objects

            return project;
        }
        #endregion
        #endregion

        #region Directory operations
        private static bool CheckProjectFolders(ProjectClassLib.Project project, bool newProjectDirectory)
        {
            bool success = true;

            if (string.IsNullOrWhiteSpace(project.FullPath))
            {
                return success = false;
            }

            //Project folder
            if (newProjectDirectory)
            {
                string projDir = FileOperations.FileAccess.GetDirectoryPath(project.FullPath);

                if (!FileOperations.FileAccess.CheckDirectoryExists(projDir))
                {
                    return success = false;
                }

                if (!FileOperations.FileAccess.CheckDirectoryExists(projDir))
                {
                    success = FileOperations.FileAccess.CreateDirectory(projDir);
                }
            }

            //Layers folder
            if (FileOperations.FileStructure.GetLayersFolder(project.FullPath) != null)
            {
                if (!FileOperations.FileAccess.CheckDirectoryExists(FileOperations.FileStructure.GetLayersFolder(project.FullPath)))
                {
                    success = FileOperations.FileAccess.CreateDirectory(FileOperations.FileStructure.GetLayersFolder(project.FullPath));
                }
            }

            //Preview folder
            /*if (FileOperations.FileStructure.GetProjectDirectoryFullPath(project.FullPath) != null)
            {
                if (!FileOperations.FileAccess.CheckDirectoryExists(FileOperations.FileStructure.GetLayersFolder(project.FullPath)))
                {
                    FileOperations.FileAccess.CreateDirectory(FileOperations.FileStructure.GetLayersFolder(project.FullPath));
                }
            }*/

            return success;
        }
        #endregion
    }
}
