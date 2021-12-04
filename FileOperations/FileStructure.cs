using System;
using System.IO;
using static FileOperations.FileAccess;

namespace FileOperations
{
    public static class FileStructure
    {
        private static string productName = AppDomain.CurrentDomain.FriendlyName;
        private static string data = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{productName}\\";
        private static string dataP = AppDomain.CurrentDomain.BaseDirectory;
        #region modificators (file/directory names)
        private static string dSettings = "Settings";
        #region Settings
        private static string fFavoriteLayers = "FavoriteLayers";
        private static string eFavoriteLayers = "json";
        #endregion

        private static string eLicenseRequest = "licReq";
        private static string eLicenseInput = "cen";

        private static string dLicense = "License";
        private static string fLicense = "license";
        private static string eLicense = "lic";
        private static string fLicensePublicKey = "license_publicKey";
        private static string eLicensePublicKey = "xml";
        private static string fLicenseUser = "cl";
        private static string eLicenseUser = "un";
        private static string fLicenseCreationTime = "ct";
        private static string eLicenseCreationTime = "l";
        private static string fLicenseLastStart = "ls";
        private static string eLicenseLastStart = "tim";

        #region resources
        private static string dResources = "Resources";
        private static string dMedia = "Media";
        private static string dLayers = "Layers";
        #endregion

        #region Project
        private static string eProject = "ppf";
        private static string dProjectLayers = "Layers";
        #endregion
        #endregion

        public static string ExtLicenseInput
        {
            get
            {
                return eLicenseInput;
            }
        }

        public static string GetDataFolderPath(bool portable)
        {
            if (!portable)
            {
                return data;
            }
            else
            {
                return dataP;
            }
        }

        public static string GetSettingsFolderPath(bool portable)
        {
            return GetDataFolderPath(portable) + dSettings + Path.DirectorySeparatorChar;
        }

        #region License
        /// <summary>
        /// Method to get folder contains license file
        /// </summary>
        /// <param name="portable"></param>
        /// <returns></returns>
        public static string GetLicenseFolderPath(bool portable)
        {
            return GetDataFolderPath(portable) + dLicense + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Method tho get full path of license file
        /// </summary>
        /// <param name="portable"></param>
        /// <returns></returns>
        public static string GetLicensePath(bool portable)
        {
            return GetLicenseFolderPath(portable) + $"{fLicense}.{eLicense}";
        }

        public static string GetLicensePublicKeyPath(bool portable)
        {
            return GetLicenseFolderPath(portable) + $"{fLicensePublicKey}.{eLicensePublicKey}";
        }

        /// <summary>
        /// Method tho get full path of file with license creation time info
        /// </summary>
        /// <param name="portable"></param>
        /// <returns></returns>
        public static string GetLicenseCreationTimePath(bool portable)
        {
            return GetLicenseFolderPath(portable) + $"{fLicenseCreationTime}.{eLicenseCreationTime}";
        }
        /// <summary>
        /// Method tho get full path of license file with program last start time info
        /// </summary>
        /// <param name="portable"></param>
        /// <returns></returns>
        public static string GetLicenseLastStartPath(bool portable)
        {
            return GetLicenseFolderPath(portable) + $"{fLicenseLastStart}.{eLicenseLastStart}";
        }

        /// <summary>
        /// Method tho get full path of file with information of current user
        /// </summary>
        /// <param name="portable"></param>
        /// <returns></returns>
        public static string GetLicenseUserPath(bool portable)
        {
            return GetLicenseFolderPath(portable) + $"{fLicenseUser}.{eLicenseUser}";
        }
        #endregion

        #region Resources
        public static string GetResourcesFolderPath()
        {
            return dataP + dResources + Path.DirectorySeparatorChar;
        }

        public static string GetMediaResourcesFolderPath()
        {
            return GetResourcesFolderPath() + dMedia + Path.DirectorySeparatorChar;
        }

        public static string GetLayersResourcesFolderPath()
        {
            return GetMediaResourcesFolderPath() + dLayers + Path.DirectorySeparatorChar;
        }
        #endregion

        #region Settings & config
        public static string GetFavoriteLayersPath(bool portable)
        {
            return GetSettingsFolderPath(portable) + $"{fFavoriteLayers}.{eFavoriteLayers}";
        }
        #endregion

        #region Project
        public static string GetProjectDirectoryNotNamed(string projectFullPath)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(projectFullPath))
            {
                string name = Path.GetFileNameWithoutExtension(projectFullPath);
                string dir = GetProjectDirectory(projectFullPath);
                string[] fArray = dir.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
                if (fArray[fArray.Length - 1].Equals((name)))
                {
                    fArray[fArray.Length - 1] = null;
                    dir = string.Join(Path.DirectorySeparatorChar, fArray);
                }
                result = dir.Trim();
            }
            return result;
        }

        public static string GetProjectDirectory(string projectFullPath)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(projectFullPath))
            {
                result = FileAccess.GetDirectoryPathWithSeparator(Path.GetDirectoryName(projectFullPath));
            }
            return result;
        }

        public static string GetProjectDirectory(string projectDirectory, string projectName)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(projectDirectory) && !string.IsNullOrWhiteSpace(projectName))
            {
                result = $"{FileAccess.GetDirectoryPathWithSeparator(projectDirectory)}{projectName}{Path.DirectorySeparatorChar}";
            }
            return result;
        }

        public static string GetProjectFullPath(string projectDirectory, string projectName)
        {
            string result = GetProjectDirectory(projectDirectory, projectName);
            if (!string.IsNullOrWhiteSpace(result))
            {
                result += projectName + $".{eProject}";
            }
            return result;
        }

        public static string GetLayersFolder(string projectFullPath)
        {
            PathType pathType = FileAccess.GetPathType(projectFullPath);

            return GetLayersFolder(projectFullPath, pathType);
        }

        public static string GetLayersFolder(string projectFullPath, PathType pathType)
        {
            string projectFolder = FileAccess.GetDirectoryPath(projectFullPath, pathType);

            return $"{projectFolder}{Path.DirectorySeparatorChar}" + dProjectLayers + Path.DirectorySeparatorChar;
        }

        public static string GetLayerFolder(string projectFullPath, string layerRelativePath)
        {
            string layersFolder = GetLayersFolder(projectFullPath);

            return layersFolder + layerRelativePath + Path.DirectorySeparatorChar;
        }
        #endregion
    }
}
