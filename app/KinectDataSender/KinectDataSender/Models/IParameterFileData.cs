using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectDataSender.Models
{
    /// <summary>
    /// パラメータファイルデータ
    /// </summary>
    public interface IParameterFileData
    {
        /// <summary>
        /// サイズ比率
        /// </summary>
        double SizeProportion { get; set; }
        /// <summary>
        /// 中心 x 座標
        /// </summary>
        double CenterX { get; set; }
        /// <summary>
        /// 中心 y 座標
        /// </summary>
        double CenterY { get; set; }
        /// <summary>
        /// 中心 z 座標
        /// </summary>
        double CenterZ { get; set; }
        /// <summary>
        /// 向い合っている想定で座標を適用するなら true
        /// </summary>
        bool Mirror { get; set; }

        /// <summary>
        /// Kinect のカメラ角度
        /// </summary>
        int KinectElevationAngle { get; set; }
        /// <summary>
        /// RGB カメラのデータを描画するなら true
        /// </summary>
        bool ColorDrawEnable { get; set; }
        /// <summary>
        /// 深度カメラのデータを描画するなら true
        /// </summary>
        bool DepthDrawEnable { get; set; }
        /// <summary>
        /// スケルトンデータを描画するなら true
        /// </summary>
        bool SkeletonDrawEnable { get; set; }

        /// <summary>
        /// 頭名
        /// </summary>
        string HeadName { get; set; }
        /// <summary>
        /// 肩中央名
        /// </summary>
        string ShoulderCenterName { get; set; }
        /// <summary>
        /// 右肩名
        /// </summary>
        string ShoulderRightName { get; set; }
        /// <summary>
        /// 右肘名
        /// </summary>
        string ElbowRightName { get; set; }
        /// <summary>
        /// 右手首名
        /// </summary>
        string WristRightName { get; set; }
        /// <summary>
        /// 右手のひら名
        /// </summary>
        string HandRightName { get; set; }
        /// <summary>
        /// 左肩名
        /// </summary>
        string ShoulderLeftName { get; set; }
        /// <summary>
        /// 左肘名
        /// </summary>
        string ElbowLeftName { get; set; }
        /// <summary>
        /// 左手首名
        /// </summary>
        string WristLeftName { get; set; }
        /// <summary>
        /// 左手のひら名
        /// </summary>
        string HandLeftName { get; set; }
        /// <summary>
        /// 鳩尾（背骨）名
        /// </summary>
        string SpineName { get; set; }
        /// <summary>
        /// おしり中心名
        /// </summary>
        string HipCenterName { get; set; }
        /// <summary>
        /// おしりの右名
        /// </summary>
        string HipRightName { get; set; }
        /// <summary>
        /// 右膝名
        /// </summary>
        string KneeRightName { get; set; }
        /// <summary>
        /// 右足首名
        /// </summary>
        string AnkleRightName { get; set; }
        /// <summary>
        /// 右足名
        /// </summary>
        string FootRightName { get; set; }
        /// <summary>
        /// おしりの左名
        /// </summary>
        string HipLeftName { get; set; }
        /// <summary>
        /// 左膝名
        /// </summary>
        string KneeLeftName { get; set; }
        /// <summary>
        /// 左足首名
        /// </summary>
        string AnkleLeftName { get; set; }
        /// <summary>
        /// 左足名
        /// </summary>
        string FootLeftName { get; set; }

        /// <summary>
        /// 頭を利用するなら true
        /// </summary>
        bool HeadEnable { get; set; }
        /// <summary>
        /// 肩中央を利用するなら true
        /// </summary>
        bool ShoulderCenterEnable { get; set; }
        /// <summary>
        /// 右肩を利用するなら true
        /// </summary>
        bool ShoulderRightEnable { get; set; }
        /// <summary>
        /// 右肘を利用するなら true
        /// </summary>
        bool ElbowRightEnable { get; set; }
        /// <summary>
        /// 右手首を利用するなら true
        /// </summary>
        bool WristRightEnable { get; set; }
        /// <summary>
        /// 右手のひらを利用するなら true
        /// </summary>
        bool HandRightEnable { get; set; }
        /// <summary>
        /// 左肩を利用するなら true
        /// </summary>
        bool ShoulderLeftEnable { get; set; }
        /// <summary>
        /// 左肘を利用するなら true
        /// </summary>
        bool ElbowLeftEnable { get; set; }
        /// <summary>
        /// 左手首を利用するなら true
        /// </summary>
        bool WristLeftEnable { get; set; }
        /// <summary>
        /// 左手のひらを利用するなら true
        /// </summary>
        bool HandLeftEnable { get; set; }
        /// <summary>
        /// 鳩尾（背骨）を利用するなら true
        /// </summary>
        bool SpineEnable { get; set; }
        /// <summary>
        /// おしり中心を利用するなら true
        /// </summary>
        bool HipCenterEnable { get; set; }
        /// <summary>
        /// おしりの右を利用するなら true
        /// </summary>
        bool HipRightEnable { get; set; }
        /// <summary>
        /// 右膝を利用するなら true
        /// </summary>
        bool KneeRightEnable { get; set; }
        /// <summary>
        /// 右足首を利用するなら true
        /// </summary>
        bool AnkleRightEnable { get; set; }
        /// <summary>
        /// 右足を利用するなら true
        /// </summary>
        bool FootRightEnable { get; set; }
        /// <summary>
        /// おしりの左を利用するなら true
        /// </summary>
        bool HipLeftEnable { get; set; }
        /// <summary>
        /// 左膝を利用するなら true
        /// </summary>
        bool KneeLeftEnable { get; set; }
        /// <summary>
        /// 左足首を利用するなら true
        /// </summary>
        bool AnkleLeftEnable { get; set; }
        /// <summary>
        /// 左足を利用するなら true
        /// </summary>
        bool FootLeftEnable { get; set; }
    }
}
