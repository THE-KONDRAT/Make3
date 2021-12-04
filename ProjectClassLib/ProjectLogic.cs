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
        public static void SaveProject(ProjectClassLib.Project project, string filePath, bool createDirs)
        {
            //check path

            //Create folders
            bool folders = CheckProjectFolders(project, createDirs);
            if (!folders)
            {
                if (!createDirs)
                {
                    return;
                }
            }
            /*else
            {
                
            }*/

            //Save project file
            FileOperations.SavindLoading.SaveJSON(project, filePath);

            //Save layers
            if (project.Layers != null)
            {
                foreach (Layers.Layer l in project.Layers)
                {
                    var lDir = FileOperations.FileStructure.GetLayerFolder(project.FullPath, l.RelativePath);
                    l.SaveLayerData(l.GetType(), lDir);
                }
            }
        }
        #endregion

        #region Load (async)
        public static ProjectClassLib.Project LoadProjectFromFile(string filePath)
        {
            ProjectClassLib.Project project = null;
            if (FileOperations.FileAccess.CheckFileExists(filePath))
            {
                project = FileOperations.SavindLoading.LoadJSON<ProjectClassLib.Project>(filePath);

                Dictionary<string, object>  dic = FileOperations.SavindLoading.LoadJSON<Dictionary<string, object>>(filePath);
                DateTime creationDate = (DateTime)dic["CreationDate"];
                project.SetCreationDate(creationDate);
                project.SetFilePath(filePath);
            }

            //Load layer objects

            return project;
        }
        #endregion
        #endregion

        #region Directory operations
        private static bool CheckProjectFolders(ProjectClassLib.Project project, bool createDirs)
        {
            bool success = true;

            if (string.IsNullOrWhiteSpace(project.FullPath))
            {
                return success = false;
            }

            //Project folder
            string projDir = FileOperations.FileAccess.GetDirectoryPath(project.FullPath, FileOperations.FileAccess.PathType.File);

            if (!FileOperations.FileAccess.CheckDirectoryExists(projDir))
            {
                if (!createDirs)
                {
                    success = false;
                    throw new Exception("Can't create project directory");
                }
                else
                {
                    success = FileOperations.FileAccess.CreateDirectory(projDir);
                }
            }

            //Layers folder
            if (FileOperations.FileStructure.GetLayersFolder(project.FullPath, FileOperations.FileAccess.PathType.File) != null)
            {
                if (!FileOperations.FileAccess.CheckDirectoryExists(FileOperations.FileStructure.GetLayersFolder(project.FullPath, FileOperations.FileAccess.PathType.File)))
                {
                    if (!createDirs)
                    {
                        success = false;
                        throw new Exception("Can't create layers directory");
                    }
                    else
                    {
                        success = FileOperations.FileAccess.CreateDirectory(FileOperations.FileStructure.GetLayersFolder(project.FullPath, FileOperations.FileAccess.PathType.File));
                    }
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
