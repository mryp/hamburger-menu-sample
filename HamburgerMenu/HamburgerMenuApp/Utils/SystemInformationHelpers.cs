using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace HamburgerMenuApp.Utils
{
    /// <summary>
    /// システム設定情報便利操作クラス
    /// </summary>
    class SystemInformationHelpers
    {
        /// <summary>
        /// 動作デバイスがXBOXかどうか
        /// </summary>
        private static bool _isXbox = (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox");

        /// <summary>
        /// 10フィートUIを表示するかどうか
        /// </summary>
        public static bool IsTenFootExperience => _isXbox;

        /// <summary>
        /// スマートフォンのディスプレイサイズかどうか
        /// </summary>
        /// <returns></returns>
        public static bool IsSmartPhoneDisplaySize
        {
            get
            {
                //ディスプレイインチサイズからスマートフォンかどうかチェックする
                double? diagonal = DisplayInformation.GetForCurrentView().DiagonalSizeInInches;
                if (diagonal < 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
