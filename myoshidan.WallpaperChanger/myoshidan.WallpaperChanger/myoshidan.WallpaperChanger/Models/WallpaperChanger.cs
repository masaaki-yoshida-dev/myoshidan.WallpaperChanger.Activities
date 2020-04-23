using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace myoshidan.WallpaperChanger.Models
{
    /// <summary>
    /// ChangeWallpaper
    /// </summary>
    public class WallpaperChanger
    {
        #region Constructor
        /// <summary>
        /// ChangeWallpaper
        /// </summary>
        public WallpaperChanger()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// SetWallPaper
        /// </summary>
        /// <param name="filePath"></param>
        public void SetWallPaper(string filePath)
        {
            using (var regKeyDesktop = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
            {
                //中央に表示
                regKeyDesktop.SetValue("TileWallpaper", "0");
                regKeyDesktop.SetValue("WallpaperStyle", "0");
            }

            SystemParametersInfo(0x0014, 0, filePath, 0);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// SystemParametersInfo
        /// </summary>
        /// <param name="uAction"></param>
        /// <param name="uParam"></param>
        /// <param name="lpvParam"></param>
        /// <param name="fuWinIni"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        #endregion
    }
}