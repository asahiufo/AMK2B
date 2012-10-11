using System;
using System.Windows.Media;
using Microsoft.Kinect;

namespace KinectDataSender
{
    /// <summary>
    /// Kinectマネージャー
    /// </summary>
    public class KinectManager
    {
        /// <summary>
        /// RGB カメラのデータ更新イベント
        /// </summary>
        public event EventHandler<ColorUpdateEventArgs> ColorUpdate;
        /// <summary>
        /// 深度カメラのデータ更新イベント
        /// </summary>
        public event EventHandler<DepthUpdateEventArgs> DepthUpdate;
        /// <summary>
        /// スケルトンデータ更新イベント
        /// </summary>
        public event EventHandler<SkeletonUpdateEventArgs> SkeletonUpdate;

        private uint _kinectNo;       // 利用する Kinect の番号
        private KinectSensor _kinect; // 利用する Kinect の KinectSensor

        private bool _drawEnable; // 描画情報を更新するなら true

        /// <summary>
        /// 描画情報を更新するなら true
        /// </summary>
        public bool DrawEnable
        {
            get { return _drawEnable;  }
            set { _drawEnable = value; }
        }

        /// <summary>
        /// 利用する番号の Kinect が接続されているなら true
        /// </summary>
        public bool Connected
        {
            get { return (KinectSensor.KinectSensors.Count >= _kinectNo); }
        }

        /// <summary>
        /// 初期化済みである場合 true
        /// </summary>
        public bool Initialized
        {
            get { return (Connected && _kinect != null); }
        }

        /// <summary>
        /// 開始済みである場合 true
        /// </summary>
        public bool Started
        {
            get { return (Initialized && _kinect.IsRunning); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="kinectNo">このクラスで利用する Kinect の番号</param>
        public KinectManager(uint kinectNo = 1)
        {
            _kinectNo = kinectNo;

            _drawEnable = true;

            ColorUpdate    = null;
            DepthUpdate    = null;
            SkeletonUpdate = null;

            _kinect      = null;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~KinectManager()
        {
            if (Started)
            {
                Stop();
            }
            if (Initialized)
            {
                Terminate();
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            if (_kinect != null)
            {
                throw new InvalidOperationException("すでに初期化済みです。");
            }
            if (!Connected)
            {
                throw new InvalidOperationException("Kinect が接続されていません。");
            }

            _kinect = KinectSensor.KinectSensors[(int)_kinectNo - 1];

            _kinect.ColorFrameReady    += new EventHandler<ColorImageFrameReadyEventArgs>(_kinect_ColorFrameReady);
            _kinect.DepthFrameReady    += new EventHandler<DepthImageFrameReadyEventArgs>(_kinect_DepthFrameReady);
            _kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(_kinect_SkeletonFrameReady);
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Terminate()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("初期化されていません。");
            }
            _kinect.SkeletonFrameReady -= _kinect_SkeletonFrameReady;
            _kinect.DepthFrameReady    -= _kinect_DepthFrameReady;
            _kinect.ColorFrameReady    -= _kinect_ColorFrameReady;
            _kinect = null;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            if (!Initialized)
            {
                throw new InvalidOperationException("初期化されていません。");
            }
            _kinect.ColorStream.Enable();
            // TODO: _kinect.DepthStream.Enable();
            _kinect.SkeletonStream.Enable();

            _kinect.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (!Started)
            {
                throw new InvalidOperationException("開始されていません。");
            }

            _kinect.Stop();
            // TODO: _kinect.Dispose();
            _kinect.SkeletonStream.Disable();
            // TODO: _kinect.DepthStream.Disable();
            _kinect.ColorStream.Disable();
        }

        /// <summary>
        /// RGB カメラフレーム更新イベントハンドラ
        /// </summary>
        /// <param name="sender">Kinect センサー</param>
        /// <param name="e">イベント</param>
        void _kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (!_drawEnable)
            {
                return;
            }

            KinectSensor kinect = sender as KinectSensor;
            if (kinect == null)
            {
                return;
            }

            // RGB カメラのフレームデータを取得する
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null)
                {
                    return;
                }

                EventHandler<ColorUpdateEventArgs> eventHandler = ColorUpdate;
                if (eventHandler != null)
                {
                    ColorUpdateEventArgs args = new ColorUpdateEventArgs();
                    args.Kinect     = kinect;
                    args.ColorFrame = colorFrame;
                    eventHandler(this, args);
                }
            }
        }

        /// <summary>
        /// 深度カメラフレーム更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (!_drawEnable)
            {
                return;
            }

            KinectSensor kinect = sender as KinectSensor;
            if (kinect == null)
            {
                return;
            }

            // 距離カメラのフレームデータを取得する
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame == null)
                {
                    return;
                }

                EventHandler<DepthUpdateEventArgs> eventHandler = DepthUpdate;
                if (eventHandler != null)
                {
                    DepthUpdateEventArgs args = new DepthUpdateEventArgs();
                    args.Kinect     = kinect;
                    args.DepthFrame = depthFrame;
                    eventHandler(this, args);
                }
            }
        }

        /// <summary>
        /// スケルトンフレーム更新イベント
        /// </summary>
        /// <param name="sender">Kinect センサー</param>
        /// <param name="e">イベント</param>
        private void _kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            KinectSensor kinect = sender as KinectSensor;
            if (kinect == null)
            {
                return;
            }

            // スケルトンのフレームを取得する
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                {
                    return;
                }

                EventHandler<SkeletonUpdateEventArgs> eventHandler = SkeletonUpdate;
                if (eventHandler != null)
                {
                    SkeletonUpdateEventArgs args = new SkeletonUpdateEventArgs();
                    args.Kinect        = kinect;
                    args.SkeletonFrame = skeletonFrame;
                    eventHandler(this, args);
                }
            }
        }
    }
}
