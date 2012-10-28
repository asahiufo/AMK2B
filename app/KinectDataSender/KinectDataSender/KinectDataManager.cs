using System;
using System.Net;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace KinectDataSender
{
    /// <summary>
    /// Kinect データを見える形へ変換するクラス
    /// </summary>
    public class KinectDataManager
    {
        private static readonly int Bgr32BytesPerPixel = PixelFormats.Bgr32.BitsPerPixel / 8;

        private bool _addedEventListener;

        private BitmapSource _colorSource; // RGB カメラの画像データ
        private BitmapSource _depthSource; // 深度カメラの画像データ
        private ObservableCollection<JointDrawPosition> _jointDrawPositions; // ジョイント描画位置リスト

        private CameraOptions _cameraOptions;
        private BlenderOptions _blenderOptions;
        private JointsOption _jointsOption;
        private SkeletonDataSender _skeletonDataSender;

        /// <summary>
        /// RGB カメラの画像データ
        /// </summary>
        public BitmapSource ColorSource
        {
            get { return _colorSource; }
            set { _colorSource = value; }
        }

        /// <summary>
        /// 深度カメラの画像データ
        /// </summary>
        public BitmapSource DepthSource
        {
            get { return _depthSource; }
            set { _depthSource = value; }
        }

        /// <summary>
        /// ジョイント描画位置リスト
        /// </summary>
        public ObservableCollection<JointDrawPosition> JointDrawPositions
        {
            get { return _jointDrawPositions;  }
            set { _jointDrawPositions = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cameraOptions">カメラ設定</param>
        /// <param name="blenderOptions">Blender 側へ反映する際のオプション</param>
        /// <param name="jointsOption">Joint 単位の設定</param>
        public KinectDataManager(CameraOptions cameraOptions, BlenderOptions blenderOptions, JointsOption jointsOption)
        {
            _addedEventListener = false;

            _colorSource = null;
            _depthSource = null;
            _jointDrawPositions = null;

            _cameraOptions = cameraOptions;
            _blenderOptions = blenderOptions;
            _jointsOption = jointsOption;
            _skeletonDataSender = new SkeletonDataSender(IPAddress.Loopback, 38040);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~KinectDataManager()
        {
            _colorSource = null;
            _depthSource = null;
        }

        /// <summary>
        /// Kinect からデータを受け取るためのイベントリスナー登録
        /// </summary>
        /// <param name="kinectManager">イベントリスナーを登録する KinectManager</param>
        public void AddEventListenerTo(KinectManager kinectManager)
        {
            if (_addedEventListener)
            {
                throw new InvalidOperationException("既にイベントリスナーが登録済みです。");
            }
            kinectManager.ColorUpdate += new EventHandler<ColorUpdateEventArgs>(kinectManager_ColorUpdate);
            kinectManager.DepthUpdate += new EventHandler<DepthUpdateEventArgs>(kinectManager_DepthUpdate);
            kinectManager.SkeletonUpdate += new EventHandler<SkeletonUpdateEventArgs>(kinectManager_SkeletonUpdate);
            _addedEventListener = true;
        }

        /// <summary>
        /// Kinect からデータを受け取るためのイベントリスナー削除
        /// </summary>
        /// <param name="kinectManager">イベントリスナーを削除する KinectManager</param>
        public void RemoveEventListenerTo(KinectManager kinectManager)
        {
            if (!_addedEventListener)
            {
                throw new InvalidOperationException("イベントリスナーが登録されていません。");
            }
            kinectManager.SkeletonUpdate -= kinectManager_SkeletonUpdate;
            kinectManager.DepthUpdate -= kinectManager_DepthUpdate;
            kinectManager.ColorUpdate -= kinectManager_ColorUpdate;
            _addedEventListener = false;
        }

        /// <summary>
        /// RGB カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        void kinectManager_ColorUpdate(object sender, ColorUpdateEventArgs e)
        {
            _colorSource = null;

            if (!_cameraOptions.ColorDrawEnable)
            {
                return;
            }

            ColorImageFrame colorFrame = e.ColorFrame;

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
        }
        /// <summary>
        /// 深度カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        void kinectManager_DepthUpdate(object sender, DepthUpdateEventArgs e)
        {
            _depthSource = null;

            if (!_cameraOptions.DepthDrawEnable)
            {
                return;
            }

            DepthImageFrame depthFrame = e.DepthFrame;

            _depthSource = BitmapSource.Create(
                depthFrame.Width, depthFrame.Height,
                96, 96,
                PixelFormats.Bgr32,
                null,
                _ConvertDepthColor(e.Kinect, depthFrame),
                depthFrame.Width * Bgr32BytesPerPixel
            );
        }
        /// <summary>
        /// スケルトンデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        void kinectManager_SkeletonUpdate(object sender, SkeletonUpdateEventArgs e)
        {
            if (!_cameraOptions.SkeletonDrawEnable)
            {
                return;
            }

            KinectSensor kinect = e.Kinect;
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeletons);

            IList<JointDrawPosition> jointDrawPositionList = new List<JointDrawPosition>();

            uint userNo = 1;
            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                {
                    continue;
                }

                // 描画
                foreach (Joint joint in skeleton.Joints)
                {
                    if (joint.TrackingState == JointTrackingState.NotTracked)
                    {
                        continue;
                    }

                    // スケルトンの座標を RGB カメラの座標に変換する
                    ColorImagePoint point = kinect.MapSkeletonPointToColor(skeleton.Position, kinect.ColorStream.Format);
                    // 座標を画面のサイズに変換する
                    // TODO: ダメなハードコーディング
                    point.X = (int)((point.X * 320) / kinect.ColorStream.FrameWidth);
                    point.Y = (int)((point.Y * 240) / kinect.ColorStream.FrameHeight);

                    JointDrawPosition jointDrawPosition = new JointDrawPosition();
                    jointDrawPosition.X = point.X;
                    jointDrawPosition.Y = point.Y;
                    jointDrawPositionList.Add(jointDrawPosition);
                }

                // 送信
                _skeletonDataSender.Send(skeleton, userNo, _blenderOptions, _jointsOption);

                userNo++;
            }

            _jointDrawPositions = new ObservableCollection<JointDrawPosition>(jointDrawPositionList);
        }

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
    }
}
