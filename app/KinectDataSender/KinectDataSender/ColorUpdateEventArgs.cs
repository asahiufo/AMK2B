using System;
using Microsoft.Kinect;

namespace KinectDataSender
{
    public class ColorUpdateEventArgs : EventArgs
    {
        private KinectSensor _kinect;
        private ColorImageFrame _colorFrame;

        /// <summary>
        /// Kinect センサー
        /// </summary>
        public KinectSensor Kinect
        {
            get { return _kinect;  }
            set { _kinect = value; }
        }

        /// <summary>
        /// RGB カメラのフレームデータ
        /// </summary>
        public ColorImageFrame ColorFrame
        {
            get { return _colorFrame;  }
            set { _colorFrame = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ColorUpdateEventArgs()
        {
            _kinect     = null;
            _colorFrame = null;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ColorUpdateEventArgs()
        {
        }
    }
}
