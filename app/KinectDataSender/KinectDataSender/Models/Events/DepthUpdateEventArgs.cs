using System;
using Microsoft.Kinect;

namespace KinectDataSender.Models.Events
{
    public class DepthUpdateEventArgs : EventArgs
    {
        private KinectSensor _kinect;
        private DepthImageFrame _depthFrame;

        /// <summary>
        /// Kinect センサー
        /// </summary>
        public KinectSensor Kinect
        {
            get { return _kinect;  }
            set { _kinect = value; }
        }

        /// <summary>
        /// 距離カメラのフレームデータ
        /// </summary>
        public DepthImageFrame DepthFrame
        {
            get { return _depthFrame;  }
            set { _depthFrame = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DepthUpdateEventArgs()
        {
            _kinect     = null;
            _depthFrame = null;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~DepthUpdateEventArgs()
        {
        }
    }
}
