using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectDataSender
{
    /// <summary>
    /// Kinectマネージャー
    /// </summary>
    class KinectManager
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KinectManager()
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // Kinectが接続されていない場合
            if (KinectSensor.KinectSensors.Count == 0)
            {
                throw new Exception("Kinectを接続して下さい。");
            }
        }
    }
}
