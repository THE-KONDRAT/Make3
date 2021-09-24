using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ViewModels
{
    public static class LayerPanelOperations
    {
        /// <summary>
        /// Regex which finds name of layer objects (np: btnTreko => Treko)
        /// </summary>
        private static Regex regexLayers = new Regex("[0-9A-Z]+[0-9a-zA-Z]+");

        /// <summary>
        /// Method to get Layer name from element name
        /// np: btnTreko -> Treko; img2D -> 2D
        /// </summary>
        /// <param name="elementName">Name of UIElement (np: imgTreko3D)</param>
        /// <returns></returns>
        private static string GetLayerNameFromElement(string elementName)
        {
            string result = null;
            Match match = regexLayers.Match(elementName);
            while (match.Success)
            {
                string sMatch = match.Groups[0].Value;
                result = sMatch;
                match = match.NextMatch();
            }
            return result;
        }

        /// <summary>
        /// Method to get technology name of control when it calls thrue the context menu
        /// </summary>
        /// <param name="inputobject"></param>
        /// <returns></returns>
        internal static string GetLayerNameFromControlContextMenu(object inputobject)
        {
            string name = UI_Helper.UI_Helper.GetControlNameFromContextMenu(inputobject);
            //now get layername
            name = GetLayerNameFromElement(name);
            return name;
        }

        /// <summary>
        /// Method to get technology name of control
        /// </summary>
        /// <param name="inputobject"></param>
        /// <returns></returns>
        internal static string GetLayerNameFromControl(object inputobject)
        {
            string name = null;
            if (inputobject.GetType().GetProperty("Name") != null)
            {
                name = GetLayerNameFromElement(UI_Helper.UI_Helper.GetControlName(inputobject));
            }
            return name;
        }

        /// <summary>
        /// Method to move some layer in collection by name 
        /// </summary>
        /// <param name="name">UIElement name</param>
        /// <param name="newIndex">New index of element (+1 due label "Favorites" always have index 0)</param>
        internal static void MoveLayerFavorites<T>(Collection<T> collection, string name, int newIndex)
        {
            newIndex = newIndex + 1; //0 - label;
            int layerIndex = LayerPanelOperations.FindUIElementIndex<T>(collection, name);
            LayerPanelOperations.MoveLayerInList<T>(collection, layerIndex, newIndex);
        }

        /// <summary>
        /// Method to move Layer in layerList ftome old index to new
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        private static void MoveLayerInList<T>(Collection<T> collection, int oldIndex, int newIndex)
        {
            if (newIndex > oldIndex - 1) newIndex++;
            var item = collection[oldIndex];
            collection.RemoveAt(oldIndex);
            collection.Insert(newIndex, item);
        }

        /// <summary>
        /// Method to create list of favorite layers
        /// also it creates label "Favorites" and separator
        /// </summary>
        /// <param name="elementsCollection"></param>
        /// <param name="favoritesList"></param>
        internal static void CreateFavorites(Collection<UIElement> elementsCollection, List<string> favoritesList)
        {
            UIElement el = elementsCollection.Where(x => x.GetType() == typeof(Separator)).FirstOrDefault();
            if (el == null)
            {
                if (favoritesList == null)
                {
                    favoritesList = new List<string>();
                }

                if (favoritesList.Find(x => x.GetType() == typeof(Label)) == null)
                {
                    Label lbFavorites = new Label();
                    lbFavorites.Content = "Favorites";
                    lbFavorites.FontSize = 10;
                    lbFavorites.HorizontalAlignment = HorizontalAlignment.Stretch;
                    lbFavorites.HorizontalContentAlignment = HorizontalAlignment.Center;
                    elementsCollection.Insert(0, lbFavorites);
                }

                elementsCollection.Insert(1, new Separator());
            }
            //FillFavorites();
        }

        /// <summary>
        /// Method to remove label "Favorites" and separator
        /// from Layer Panel
        /// </summary>
        /// <param name="elementsCollection"></param>
        internal static void RemoveFavorites(Collection<UIElement> elementsCollection)
        {
            UIElement sep = elementsCollection.Where(x => x.GetType() == typeof(Separator)).FirstOrDefault();
            if (sep != null)
            {
                elementsCollection.Remove(sep);
            }
            UIElement lab = elementsCollection.Where(x => x.GetType() == typeof(Label)).FirstOrDefault();
            if (lab != null)
            {
                elementsCollection.Remove(lab);
            }
        }

        internal static int FindUIElementIndex<T>(Collection<T> collection, string name)
        {
            //Predicate<string> namePr = delegate (string x) { return GetLayerNameFromElement((string)x.GetType().GetProperty("Name").GetValue(x)) == name; };
            T item = collection.Where(x => GetLayerNameFromElement((string)x.GetType().GetProperty("Name").GetValue(x)) == name).FirstOrDefault();
            return item == null ? -1 : collection.IndexOf(item);
        }

        internal static int FindUIElementIndex<T>(List<T> list, string name)
        {
            T item = list.Where(x => GetLayerNameFromElement((string)x.GetType().GetProperty("Name").GetValue(x)) == name).FirstOrDefault();
            return item == null ? -1 : list.IndexOf(item);
        }

        #region Context Menu

        /// <summary>
        /// Method to create menu item in context menu,
        /// that creates only favorite operation (add/remove)
        /// </summary>
        /// <param name="favorite">True if layer in favorites list</param>
        /// <param name="handlerFav">RoutedEventHandler which adds to favorites</param>
        /// <param name="handlerRemoveFav">RoutedEventHandler which removes from favorites</param>
        /// <returns></returns>
        internal static MenuItem AddLayerFavMenuItem(bool favorite, RoutedEventHandler handlerFav, RoutedEventHandler handlerRemoveFav)
        {
            RoutedEventHandler handler = handlerFav;
            if (!favorite)
            {
                handler = handlerRemoveFav;
            }

            MenuItem mi = CreateContextMenuItem(GetContextMenuHeader(favorite), handlerFav);

            return mi;
        }

        /// <summary>
        /// Method to create menu item with text and RoutedEventHandler
        /// </summary>
        /// <param name="txtHeader">Element text</param>
        /// <param name="handler"></param>
        /// <returns></returns>
        internal static MenuItem CreateContextMenuItem(string txtHeader, RoutedEventHandler handler)
        {
            MenuItem mi = new MenuItem();
            if (txtHeader != null)
            {
                mi.Header = txtHeader;
            }
            if (handler != null)
            {
                mi.Click += handler;
            }

            return mi;
        }

        /// <summary>
        /// Method to change favorite menuitem header and RoutedEventHandler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="name">Layer name (np: Treko; 2D)</param>
        /// <param name="favorite">True if layer in favorites list</param>
        /// <param name="handlerFav">RoutedEventHandler which adds to favorites</param>
        /// <param name="handlerRemoveFav">RoutedEventHandler which removes from favorites</param>
        internal static void ChangeFavoriteHeader<T>(Collection<T> collection, string name, bool favorite, RoutedEventHandler handlerFav, RoutedEventHandler handlerRemoveFav)
        {
            T el = collection[FindUIElementIndex<T>(collection, name)];
            if (el.GetType().GetProperty("ContextMenu").GetValue(el) != null)
            {
                ContextMenu cm = (ContextMenu)el.GetType().GetProperty("ContextMenu").GetValue(el);
                MenuItem mi = cm.Items.OfType<MenuItem>().Where(x => x.Header.Equals(GetContextMenuHeader(!favorite))).FirstOrDefault();
                if (favorite)
                {
                    mi.Click -= handlerFav;
                    mi.Click += handlerRemoveFav;
                }
                else
                {
                    mi.Click -= handlerRemoveFav;
                    mi.Click += handlerFav;
                }
                mi.Header = GetContextMenuHeader(favorite);
            }
        }

        /// <summary>
        /// Method to get text to favorite menu item
        /// </summary>
        /// <param name="favorite">True if layer in favorites list</param>
        /// <returns></returns>
        private static string GetContextMenuHeader(bool favorite)
        {
            if (!favorite)
            {
                return "Add to favorites";
            }
            else
            {
                return "Remove from favorites";
            }
        }
        #endregion

        /*internal static ImageSource LoadImageSourceFromFile(string fileName)
        {
            Bitmap bmp = ImageProcessing.Basic.LoadBMPFromFile(fileName);

            //ImageSource imageSource = null;

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }*/

        #region Load/Save
        private static string tempPath = "E:\\New project 1\\Layers\\Layer 1\\SourceImages\\testFav.json";
        internal static List<string> LoadFavoriteLayers()
        {
            bool portable = false;
            List<string> favoritesList = null;
            string path = FileOperations.FileStructure.GetFavoriteLayersPath(portable);
            path = tempPath;
            if (FileOperations.FileAccess.CheckFileExists(path))
            {
                favoritesList = FileOperations.SavindLoading.LoadJSON<List<string>>(path);
            }
            return favoritesList;
        }

        internal static void SaveFavoriteLayers(List<string> favoritesList)
        {
            bool portable = false;
            List<string> layers = null;

            if (favoritesList != null)
            {
                if (favoritesList.Count > 0)
                {
                    layers = new List<string>();
                }
            }

            if (layers != null)
            {
                /*foreach (UIElement el in LayerPanelFavorites)
                {
                    if (el.GetType() == typeof(Image))
                    {
                        string name = GetLayerNameFromElement(((Image)el).Name);
                        //string name = el.GetValue("Name"); //$"img{name}"
                        layers.Add(name);
                    }
                }*/

                string path = FileOperations.FileStructure.GetFavoriteLayersPath(portable);
                path = tempPath;
                /*if (FileOperations.FileAccess.CheckFileExists(path))
                {
                    FileOperations.SavindLoading.SaveJSON(favoritesList, path);
                }*/

                FileOperations.SavindLoading.SaveJSON(favoritesList/*layers*/, path);
            }
        }

        internal static List<string> GetAllLayers()
        {
            List<string> result = null;

            var layers = typeof(Layers.Layer).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Layers.Layer)));

            foreach (Type t in layers)
            {
                if (result == null)
                {
                    result = new List<string>();
                }
                if (!t.Name.Contains("_base"))
                {
                    result.Add(t.Name);
                }
            }

            return result;
        }

        internal static Type GetLayerTypeByTechnologyName(string technologyName)
        {
            Type result = null;

            //var layers = typeof(Layers.Layer).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Layers.Layer)));

            try
            {
                result = typeof(Layers.Layer).Assembly.GetTypes().Single(x => x.IsSubclassOf(typeof(Layers.Layer)) && x.Name.Equals(technologyName));
            }
            catch (Exception e)
            {

            }
            //var td = layers.Single(x => x.Name.Equals(technologyName));
            //result = typeof(layers.w)

            /*foreach (Type t in layers)
            {
                if (!t.Name.Contains("_base") && t.Name.Equals(technologyName))
                {
                    result = t;
                }
            }*/

            return result;
        }
        #endregion
    }
}
