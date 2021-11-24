using System;
using System.IO;
using System.Linq;

namespace FileOperations
{
    public static class FileAccess
    {
        #region Logic
        #region Creation
        public static bool CreateFile(string path)
        {
            bool result = false;
            try
            {
                File.Create(path);
            }
            catch(Exception e)
            {
                result = false;
            }
            return result;
        }

        public static bool CreateDirectory(string path)
        {
            bool result = false;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        #endregion

        public static bool CheckFileExists(string path)
        {
            bool result = false;
            if (File.Exists(path))
            {
                result = true;
            }
            return result;
        }

        public static bool CheckDirectoryExists(string path)
        {
            bool result = false;
            if (Directory.Exists(path))
            {
                result = true;
            }
            return result;
        }

        /*public static bool ValidateFileName(string fileName)
        {
            bool result = false;
            if (Directory.Exists(path))
            {
                result = true;
            }
            return result;
        }*/

        public static string GetDirectoryPath(string path)
        {
            FileAccess.PathType pathType = FileAccess.PathType.File;
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                pathType = FileAccess.PathType.Directory;
            }

            if (pathType.Equals(PathType.File))
            {
                return Path.GetDirectoryName(path);
            }
            else
            {
                return path;
            }
        }

        public static string GetDirectoryPathWithSeparator(string dir)
        {
            char dirSeparator = Path.DirectorySeparatorChar;
            string directory = dir + dirSeparator;
            directory = Path.GetDirectoryName(directory) + dirSeparator;
            if (dir.Equals(Path.GetPathRoot(dir)))
            {
                directory = dir;
                if (!directory.Contains(dirSeparator))
                {
                    directory += dirSeparator;
                }
            }

            directory = directory.Replace($"{dirSeparator}{dirSeparator}", $"{dirSeparator}");

            return directory;
        }

        public static bool ValidateFilePathSymbols(string filePath)
        {
            return ValidatePathSymbols(filePath, true);
        }

        public static bool ValidateDirectoryPathSymbols(string directoryPath)
        {
            return ValidatePathSymbols(directoryPath, false);
        }

        public static bool ValidatePathSymbols(string path, Boolean Name)
        {
            return !path.Intersect(ForbiddenChars(Name)).Any();
        }

        public static bool ValidatePathSymbols(string path, PathType pathType)
        {
            bool name = false;
            string pathToValidation = path;
            switch (pathType)
            {
                case PathType.Directory:
                    if (Path.IsPathRooted(path))
                    {
                        pathToValidation = path.Replace(Path.GetPathRoot(path), "");
                    }
                    name = false;
                    break;
                case PathType.File:
                    name = true;
                    break;
            }
            //bool b = pathToValidation.Intersect(ForbiddenChars(name)).Any();
            return !pathToValidation.Intersect(ForbiddenChars(name)).Any();
        }

        public static Char[] ForbiddenChars(bool name)
        {
            char[] result = new char[]
            {
                '?',
                '/',
                ':',
                '"',
                '*',
                '>',
                '<',
                '|'
            };

            if (name)
            {
                Array.Resize(ref result, result.Length + 1);
                result[result.Length - 1] = '\\';
            }
            return result;
        }
        public enum PathType
        {
            File,
            Directory,
        };
        #endregion
    }
}
