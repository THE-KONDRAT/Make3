using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ColorProfile.Program_templates
{
    public class ProgramTemplates
    {
        /// <summary>
        /// Arc width 
        /// Unit: nm
        /// </summary>
        public decimal DefaultArcWidth { get; set; }

        public List<DataContainers.ColorProfileTemplate> Templates { get; set; }

        public ProgramTemplates()
        {
            DefaultArcWidth = 15000; //15000nm = 15um
            FillList();
        }

        public ProgramTemplates(decimal defaultArcWidth)
        {
            DefaultArcWidth = defaultArcWidth;
            FillList();
        }

        private void FillList()
        {
            if (Templates == null)
            {
                Templates = new List<DataContainers.ColorProfileTemplate>();
            }

            Templates.Clear();

            //
            var methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Where(
                x => x.ReturnType.Equals(typeof(DataContainers.ColorProfileTemplate)) && x.Name.Contains("t_"));


            foreach (MethodInfo mi in methods)
            {
                try
                {
                    var n = mi.Name;
                    DataContainers.ColorProfileTemplate ct = (DataContainers.ColorProfileTemplate)mi.Invoke(this, null);
                    if (ct != null)
                    {
                        ct.CreateThumbnail();
                        Templates.Add(ct);
                    }
                }
                catch(Exception e)
                {

                }
            }
        }

        #region Templates

        private DataContainers.ColorProfileTemplate t_Gauss()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 27));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 36));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 45));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 128));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 182));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 128));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 45));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 36));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 27));
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_InvGauss()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 129));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 133));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 100));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 88));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, -150));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 88));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 100));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 113));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 129));
            //"{X=0, Y=129},{X=32, Y=113},{X=64, Y=100},{X=96, Y=88},{X=128, Y=-150},{X=160, Y=88},{X=192, Y=100},{X=224, Y=113},{X=255, Y=129}";
            #endregion

            result.CheckAsymm();
            result.Symmetric = true;

            return result;
        }

        private DataContainers.ColorProfileTemplate t_Line()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 30));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 38));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 51));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 62));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 83));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 101));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 117));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 246));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 35));
            //"{X=0, Y=30},{X=32, Y=38},{X=64, Y=51},{X=96, Y=62},{X=128, Y=83},{X=160, Y=101},{X=192, Y=117},{X=224, Y=246},{X=255, Y=35}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_InvLine()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 38));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 73));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 105));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 156));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 203));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 270));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 186));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 504));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 28));
            //"{X=0, Y=38},{X=32, Y=73},{X=64, Y=105},{X=96, Y=156},{X=128, Y=203},{X=160, Y=270},{X=192, Y=186},{X=224, Y=504},{X=255, Y=28}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_AsimmVipSphere()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 27));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 171));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 109));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 110));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 124));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 103));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 87));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 63));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 27));
            //"{X=0, Y=27},{X=32, Y=171},{X=64, Y=109},{X=96, Y=110},{X=128, Y=124},{X=160, Y=103},{X=192, Y=87},{X=224, Y=63},{X=255, Y=27}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_AsimmInvertSphere()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 27));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 171));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 109));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 110));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 58));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 62));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 33));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 36));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 27));
            //"{X=0, Y=27},{X=32, Y=171},{X=64, Y=109},{X=96, Y=110},{X=128, Y=58},{X=160, Y=62},{X=192, Y=33},{X=224, Y=36},{X=255, Y=27}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_sin2()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 128));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, -303));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 700));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 86));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, -533));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 86));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 700));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, -303));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 128));
            //"{X=0, Y=128},{X=32, Y=-303},{X=64, Y=700},{X=96, Y=86},{X=128, Y=-533},{X=160, Y=86},{X=192, Y=700},{X=224, Y=-303},{X=255, Y=128}"
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_DoubleSin()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 28));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 454));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 384));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 383));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, -796));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 383));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 384));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 454));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 28));
            //"{X=0, Y=28},{X=32, Y=454},{X=64, Y=384},{X=96, Y=383},{X=128, Y=-796},{X=160, Y=383},{X=192, Y=384},{X=224, Y=454},{X=255, Y=28}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_DeltaDirac()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, -50));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, -31));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, -76));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, -235));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 467));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, -235));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, -76));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, -31));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, -50));
            //"{X=0, Y=-50},{X=32, Y=-31},{X=64, Y=-76},{X=96, Y=-235},{X=128, Y=467},{X=160, Y=-235},{X=192, Y=-76},{X=224, Y=-31},{X=255, Y=-50}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_Sphere()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 27));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 78));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 117));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 79));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 177));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 79));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 117));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 78));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 27));
            //"{X=0, Y=27},{X=32, Y=78},{X=64, Y=117},{X=96, Y=79},{X=128, Y=177},{X=160, Y=79},{X=192, Y=117},{X=224, Y=78},{X=255, Y=27}";
            #endregion

            result.CheckAsymm();

            return result;
        }

        private DataContainers.ColorProfileTemplate t_InvSpherre()
        {
            DataContainers.ColorProfileTemplate result = ProgramTemplate();
            result.Name = GetTemplateNameFromSignature(MethodInfo.GetCurrentMethod());
            if (string.IsNullOrWhiteSpace(result.Name))
            {
                return null;
            }

            #region Key points
            result.KeyPoints[0] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(0, 38));
            result.KeyPoints[1] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(32, 39));
            result.KeyPoints[2] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(64, 46));
            result.KeyPoints[3] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(96, 56));
            result.KeyPoints[4] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(128, 69));
            result.KeyPoints[5] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(160, 85));
            result.KeyPoints[6] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(192, 121));
            result.KeyPoints[7] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(224, 342));
            result.KeyPoints[8] = new DataContainers.ProfileKeyPoint(new DataContainers.PointDec(255, 28));
            //"{X=0, Y=38},{X=32, Y=39},{X=64, Y=46},{X=96, Y=56},{X=128, Y=69},{X=160, Y=85},{X=192, Y=121},{X=224, Y=342},{X=255, Y=28}";
            #endregion

            result.CheckAsymm();

            return result;
        }



        private DataContainers.ColorProfileTemplate ProgramTemplate()
        {
            DataContainers.ColorProfileTemplate result = new DataContainers.ColorProfileTemplate();
            result.ArcWidth = DefaultArcWidth;
            result.Created_By_User = "*%*Make3*%*";
            result.CreationTime = DateTime.ParseExact("13.06.2019 | 12:00:00", "dd.MM.yyyy | HH:mm:ss", CultureInfo.GetCultureInfo("en-US")); ;
            result.LastEditedTime = result.CreationTime;
            result.CustomTemplate = false;
            result.MinimalMake3Version = "0.8.1.26";
            
            return result;
        }
        #endregion


        private string GetTemplateNameFromSignature(MethodBase method)
        {
            string name = null;
            try
            {
                MethodInfo mi = this.GetType().GetMethod(method.Name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (mi != null)
                {
                    if (mi.ReturnType.Equals(typeof(DataContainers.ColorProfileTemplate)))
                    {
                        name = method.Name.Replace("t_", "");
                    }
                }
            }
            catch (Exception e)
            {

            }

            return name;
        }


    }
}
