using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace KinectDataSender
{
    /// <summary>
    /// Kinectマネージャー
    /// </summary>
    class KinectManager
    {
        private static readonly int Bgr32BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;

        public event EventHandler<ColorUpdateEventArgs>    ColorUpdate;
        public event EventHandler<DepthUpdateEventArgs>    DepthUpdate;
        public event EventHandler<SkeletonUpdateEventArgs> SkeletonUpdate;

        private uint _kinectNo;       // 利用する Kinect の番号
        private KinectSensor _kinect; // 利用する Kinect の KinectSensor

        private bool _drawEnable; // 描画情報を更新するなら true

        // TODO:
        //private BitmapSource _colorSource; // RGB カメラの画像データ
        //private BitmapSource _depthSource; // 深度の画像データ

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
            _kinect.DepthStream.Enable();
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
            _kinect.Dispose();
            _kinect.SkeletonStream.Disable();
            _kinect.DepthStream.Disable();
            _kinect.ColorStream.Disable();
        }

        /// <summary>
        /// RGB カメラフレーム更新イベント
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
                if (colorFrame != null)
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

                // TODO:
                /*
                byte[] colorPixel = new byte[colorFrame.PixelDataLength];
                colorFrame.CopyPixelDataTo(colorPixel);
                _colorSource = BitmapSource.Create(
                    colorFrame.Width, colorFrame.Height,
                    96, 96,
                    PixelFormats.Bgr32,
                    null,
                    colorPixel,
                    colorFrame.Width * colorFrame.BytesPerPixel
                );
                */
            }
        }

        /// <summary>
        /// 深度カメラフレーム更新イベント
        /// </summary>
        /// <param name="sender">Kinect センサー</param>
        /// <param name="e">イベント</param>
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

                // TODO:
                /*
                _depthSource = BitmapSource.Create(
                    depthFrame.Width, depthFrame.Height,
                    96, 96,
                    PixelFormats.Bgr32,
                    null,
                    _ConvertDepthColor(kinect, depthFrame),
                    depthFrame.Width * Bgr32BytesPerPixel
                );
                */
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

                /*
                // TODO: スケルトンデータ送信

                // スケルトン情報描画
                if (!_drawEnable)
                {
                    return;
                }
                _DrawSkeleton(kinect, skeletonFrame);
                */
            }
        }

        /*
        /// <summary>
        /// 距離データの画像変換
        /// </summary>
        /// <param name="kinect">Kinect センサー</param>
        /// <param name="depthFrame">深度フレームデータ</param>
        /// <returns>距離データの画像データ</returns>
        private byte[] _ConvertDepthColor(KinectSensor kinect, DepthImageFrame depthFrame)
        {
            ColorImageStream colorStream = kinect.ColorStream;
            DepthImageStream depthStream = kinect.DepthStream;

            // 距離カメラのピクセル毎のデータを取得する
            short[] depthPixel = new short[depthFrame.PixelDataLength];
            depthFrame.CopyPixelDataTo(depthPixel);

            // 距離カメラの座標に対する RGB カメラ座標を取得する（座標合わせ）
            ColorImagePoint[] colorPoint = new ColorImagePoint[depthFrame.PixelDataLength];
            kinect.MapDepthFrameToColorFrame(depthStream.Format, depthPixel, colorStream.Format, colorPoint);

            byte[] depthColor = new byte[depthFrame.PixelDataLength * Bgr32BytesPerPixel];
            int pxLen = depthPixel.Length;
            for (int i = 0; i < pxLen; i++)
            {
                int distance = depthPixel[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // 変換した結果がフレームサイズを超えないよう、小さい方を採用
                int x = Math.Min(colorPoint[i].X, colorStream.FrameWidth - 1);
                int y = Math.Min(colorPoint[i].Y, colorStream.FrameHeight - 1);
                int colorIndex = ((y * depthFrame.Width) + x) * Bgr32BytesPerPixel;

                // サポート外 0-40cm
                if (distance == depthStream.UnknownDepth)
                {
                    depthColor[colorIndex] = 0;
                    depthColor[colorIndex + 1] = 0;
                    depthColor[colorIndex + 2] = 255;
                }
                // 近すぎ 40cm-80cm（Default）
                else if (distance == depthStream.TooNearDepth)
                {
                    depthColor[colorIndex] = 0;
                    depthColor[colorIndex + 1] = 255;
                    depthColor[colorIndex + 2] = 0;
                }
                // 遠すぎ 3m（Near）, 4m（Default）-8m
                else if (distance == depthStream.TooFarDepth)
                {
                    depthColor[colorIndex] = 255;
                    depthColor[colorIndex + 1] = 0;
                    depthColor[colorIndex + 2] = 0;
                }
                // 有効な距離データ
                else
                {
                    depthColor[colorIndex] = 0;
                    depthColor[colorIndex + 1] = 255;
                    depthColor[colorIndex + 2] = 255;
                }
            }

            return depthColor;
        }

        /// <summary>
        /// スケルトンデータの画像変換
        /// </summary>
        /// <param name="kinect"></param>
        /// <param name="skeletonFrame"></param>
        private void _DrawSkeleton(KinectSensor kinect, SkeletonFrame skeletonFrame)
        {
            // TODO: どうやって View へ連携するか
        }
        */
    }
}
