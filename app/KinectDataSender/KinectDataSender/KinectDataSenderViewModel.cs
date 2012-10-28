using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System;
using System.Collections.ObjectModel;

namespace KinectDataSender
{
    /// <summary>
    /// メインウインドウ ViewModel
    /// </summary>
    public class KinectDataSenderViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private KinectManager _kinectManager;
        private CameraOptions _cameraOptions;
        private JointsOption _jointsOption;
        private BlenderOptions _blenderOptions;
        private OriginPositionAutoSetter _originPositionAutoSetter;
        private KinectDataManager _kinectDataManager;

        private string _statusBarMessage;

        private ICommand _startOrStopKinectCommand; // Kinect スタートストップコマンド
        private ICommand _applyKinectElevationAngleCommand; // Kinect カメラ角度設定コマンド
        private ICommand _setOriginPositionCommand; // 原点座標設定コマンド

        #region

        /// <summary>
        /// 頭名
        /// </summary>
        public string HeadName
        {
            get { return _jointsOption.HeadName; }
            set
            {
                if (_jointsOption.HeadName == value)
                {
                    return;
                }
                _jointsOption.HeadName = value;
                OnPropertyChanged("HeadName");
            }
        }
        /// <summary>
        /// 肩中央名
        /// </summary>
        public string ShoulderCenterName
        {
            get { return _jointsOption.ShoulderCenterName; }
            set
            {
                if (_jointsOption.ShoulderCenterName == value)
                {
                    return;
                }
                _jointsOption.ShoulderCenterName = value;
                OnPropertyChanged("ShoulderCenterName");
            }
        }
        /// <summary>
        /// 右肩名
        /// </summary>
        public string ShoulderRightName
        {
            get { return _jointsOption.ShoulderRightName; }
            set
            {
                if (_jointsOption.ShoulderRightName == value)
                {
                    return;
                }
                _jointsOption.ShoulderRightName = value;
                OnPropertyChanged("ShoulderRightName");
            }
        }
        /// <summary>
        /// 右肘名
        /// </summary>
        public string ElbowRightName
        {
            get { return _jointsOption.ElbowRightName; }
            set
            {
                if (_jointsOption.ElbowRightName == value)
                {
                    return;
                }
                _jointsOption.ElbowRightName = value;
                OnPropertyChanged("ElbowRightName");
            }
        }
        /// <summary>
        /// 右手首名
        /// </summary>
        public string WristRightName
        {
            get { return _jointsOption.WristRightName; }
            set
            {
                if (_jointsOption.WristRightName == value)
                {
                    return;
                }
                _jointsOption.WristRightName = value;
                OnPropertyChanged("WristRightName");
            }
        }
        /// <summary>
        /// 右手のひら名
        /// </summary>
        public string HandRightName
        {
            get { return _jointsOption.HandRightName; }
            set
            {
                if (_jointsOption.HandRightName == value)
                {
                    return;
                }
                _jointsOption.HandRightName = value;
                OnPropertyChanged("HandRightName");
            }
        }
        /// <summary>
        /// 左肩名
        /// </summary>
        public string ShoulderLeftName
        {
            get { return _jointsOption.ShoulderLeftName; }
            set
            {
                if (_jointsOption.ShoulderLeftName == value)
                {
                    return;
                }
                _jointsOption.ShoulderLeftName = value;
                OnPropertyChanged("ShoulderLeftName");
            }
        }
        /// <summary>
        /// 左肘名
        /// </summary>
        public string ElbowLeftName
        {
            get { return _jointsOption.ElbowLeftName; }
            set
            {
                if (_jointsOption.ElbowLeftName == value)
                {
                    return;
                }
                _jointsOption.ElbowLeftName = value;
                OnPropertyChanged("ElbowLeftName");
            }
        }
        /// <summary>
        /// 左手首名
        /// </summary>
        public string WristLeftName
        {
            get { return _jointsOption.WristLeftName; }
            set
            {
                if (_jointsOption.WristLeftName == value)
                {
                    return;
                }
                _jointsOption.WristLeftName = value;
                OnPropertyChanged("WristLeftName");
            }
        }
        /// <summary>
        /// 左手のひら名
        /// </summary>
        public string HandLeftName
        {
            get { return _jointsOption.HandLeftName; }
            set
            {
                if (_jointsOption.HandLeftName == value)
                {
                    return;
                }
                _jointsOption.HandLeftName = value;
                OnPropertyChanged("HandLeftName");
            }
        }
        /// <summary>
        /// 鳩尾（背骨）名
        /// </summary>
        public string SpineName
        {
            get { return _jointsOption.SpineName; }
            set
            {
                if (_jointsOption.SpineName == value)
                {
                    return;
                }
                _jointsOption.SpineName = value;
                OnPropertyChanged("SpineName");
            }
        }
        /// <summary>
        /// おしり中心名
        /// </summary>
        public string HipCenterName
        {
            get { return _jointsOption.HipCenterName; }
            set
            {
                if (_jointsOption.HipCenterName == value)
                {
                    return;
                }
                _jointsOption.HipCenterName = value;
                OnPropertyChanged("HipCenterName");
            }
        }
        /// <summary>
        /// おしりの右名
        /// </summary>
        public string HipRightName
        {
            get { return _jointsOption.HipRightName; }
            set
            {
                if (_jointsOption.HipRightName == value)
                {
                    return;
                }
                _jointsOption.HipRightName = value;
                OnPropertyChanged("HipRightName");
            }
        }
        /// <summary>
        /// 右膝名
        /// </summary>
        public string KneeRightName
        {
            get { return _jointsOption.KneeRightName; }
            set
            {
                if (_jointsOption.KneeRightName == value)
                {
                    return;
                }
                _jointsOption.KneeRightName = value;
                OnPropertyChanged("KneeRightName");
            }
        }
        /// <summary>
        /// 右足首名
        /// </summary>
        public string AnkleRightName
        {
            get { return _jointsOption.AnkleRightName; }
            set
            {
                if (_jointsOption.AnkleRightName == value)
                {
                    return;
                }
                _jointsOption.AnkleRightName = value;
                OnPropertyChanged("AnkleRightName");
            }
        }
        /// <summary>
        /// 右足名
        /// </summary>
        public string FootRightName
        {
            get { return _jointsOption.FootRightName; }
            set
            {
                if (_jointsOption.FootRightName == value)
                {
                    return;
                }
                _jointsOption.FootRightName = value;
                OnPropertyChanged("FootRightName");
            }
        }
        /// <summary>
        /// おしりの左名
        /// </summary>
        public string HipLeftName
        {
            get { return _jointsOption.HipLeftName; }
            set
            {
                if (_jointsOption.HipLeftName == value)
                {
                    return;
                }
                _jointsOption.HipLeftName = value;
                OnPropertyChanged("HipLeftName");
            }
        }
        /// <summary>
        /// 左膝名
        /// </summary>
        public string KneeLeftName
        {
            get { return _jointsOption.KneeLeftName; }
            set
            {
                if (_jointsOption.KneeLeftName == value)
                {
                    return;
                }
                _jointsOption.KneeLeftName = value;
                OnPropertyChanged("KneeLeftName");
            }
        }
        /// <summary>
        /// 左足首名
        /// </summary>
        public string AnkleLeftName
        {
            get { return _jointsOption.AnkleLeftName; }
            set
            {
                if (_jointsOption.AnkleLeftName == value)
                {
                    return;
                }
                _jointsOption.AnkleLeftName = value;
                OnPropertyChanged("AnkleLeftName");
            }
        }
        /// <summary>
        /// 左足名
        /// </summary>
        public string FootLeftName
        {
            get { return _jointsOption.FootLeftName; }
            set
            {
                if (_jointsOption.FootLeftName == value)
                {
                    return;
                }
                _jointsOption.FootLeftName = value;
                OnPropertyChanged("FootLeftName");
            }
        }

        /// <summary>
        /// 頭を利用するなら true
        /// </summary>
        public bool HeadEnable
        {
            get { return _jointsOption.HeadEnable; }
            set
            {
                if (_jointsOption.HeadEnable == value)
                {
                    return;
                }
                _jointsOption.HeadEnable = value;
                OnPropertyChanged("HeadEnable");
            }
        }
        /// <summary>
        /// 肩中央を利用するなら true
        /// </summary>
        public bool ShoulderCenterEnable
        {
            get { return _jointsOption.ShoulderCenterEnable; }
            set
            {
                if (_jointsOption.ShoulderCenterEnable == value)
                {
                    return;
                }
                _jointsOption.ShoulderCenterEnable = value;
                OnPropertyChanged("ShoulderCenterEnable");
            }
        }
        /// <summary>
        /// 右肩を利用するなら true
        /// </summary>
        public bool ShoulderRightEnable
        {
            get { return _jointsOption.ShoulderRightEnable; }
            set
            {
                if (_jointsOption.ShoulderRightEnable == value)
                {
                    return;
                }
                _jointsOption.ShoulderRightEnable = value;
                OnPropertyChanged("ShoulderRightEnable");
            }
        }
        /// <summary>
        /// 右肘を利用するなら true
        /// </summary>
        public bool ElbowRightEnable
        {
            get { return _jointsOption.ElbowRightEnable; }
            set
            {
                if (_jointsOption.ElbowRightEnable == value)
                {
                    return;
                }
                _jointsOption.ElbowRightEnable = value;
                OnPropertyChanged("ElbowRightEnable");
            }
        }
        /// <summary>
        /// 右手首を利用するなら true
        /// </summary>
        public bool WristRightEnable
        {
            get { return _jointsOption.WristRightEnable; }
            set
            {
                if (_jointsOption.WristRightEnable == value)
                {
                    return;
                }
                _jointsOption.WristRightEnable = value;
                OnPropertyChanged("WristRightEnable");
            }
        }
        /// <summary>
        /// 右手のひらを利用するなら true
        /// </summary>
        public bool HandRightEnable
        {
            get { return _jointsOption.HandRightEnable; }
            set
            {
                if (_jointsOption.HandRightEnable == value)
                {
                    return;
                }
                _jointsOption.HandRightEnable = value;
                OnPropertyChanged("HandRightEnable");
            }
        }
        /// <summary>
        /// 左肩を利用するなら true
        /// </summary>
        public bool ShoulderLeftEnable
        {
            get { return _jointsOption.ShoulderLeftEnable; }
            set
            {
                if (_jointsOption.ShoulderLeftEnable == value)
                {
                    return;
                }
                _jointsOption.ShoulderLeftEnable = value;
                OnPropertyChanged("ShoulderLeftEnable");
            }
        }
        /// <summary>
        /// 左肘を利用するなら true
        /// </summary>
        public bool ElbowLeftEnable
        {
            get { return _jointsOption.ElbowLeftEnable; }
            set
            {
                if (_jointsOption.ElbowLeftEnable == value)
                {
                    return;
                }
                _jointsOption.ElbowLeftEnable = value;
                OnPropertyChanged("ElbowLeftEnable");
            }
        }
        /// <summary>
        /// 左手首を利用するなら true
        /// </summary>
        public bool WristLeftEnable
        {
            get { return _jointsOption.WristLeftEnable; }
            set
            {
                if (_jointsOption.WristLeftEnable == value)
                {
                    return;
                }
                _jointsOption.WristLeftEnable = value;
                OnPropertyChanged("WristLeftEnable");
            }
        }
        /// <summary>
        /// 左手のひらを利用するなら true
        /// </summary>
        public bool HandLeftEnable
        {
            get { return _jointsOption.HandLeftEnable; }
            set
            {
                if (_jointsOption.HandLeftEnable == value)
                {
                    return;
                }
                _jointsOption.HandLeftEnable = value;
                OnPropertyChanged("HandLeftEnable");
            }
        }
        /// <summary>
        /// 鳩尾（背骨）を利用するなら true
        /// </summary>
        public bool SpineEnable
        {
            get { return _jointsOption.SpineEnable; }
            set
            {
                if (_jointsOption.SpineEnable == value)
                {
                    return;
                }
                _jointsOption.SpineEnable = value;
                OnPropertyChanged("SpineEnable");
            }
        }
        /// <summary>
        /// おしり中心を利用するなら true
        /// </summary>
        public bool HipCenterEnable
        {
            get { return _jointsOption.HipCenterEnable; }
            set
            {
                if (_jointsOption.HipCenterEnable == value)
                {
                    return;
                }
                _jointsOption.HipCenterEnable = value;
                OnPropertyChanged("HipCenterEnable");
            }
        }
        /// <summary>
        /// おしりの右を利用するなら true
        /// </summary>
        public bool HipRightEnable
        {
            get { return _jointsOption.HipRightEnable; }
            set
            {
                if (_jointsOption.HipRightEnable == value)
                {
                    return;
                }
                _jointsOption.HipRightEnable = value;
                OnPropertyChanged("HipRightEnable");
            }
        }
        /// <summary>
        /// 右膝を利用するなら true
        /// </summary>
        public bool KneeRightEnable
        {
            get { return _jointsOption.KneeRightEnable; }
            set
            {
                if (_jointsOption.KneeRightEnable == value)
                {
                    return;
                }
                _jointsOption.KneeRightEnable = value;
                OnPropertyChanged("KneeRightEnable");
            }
        }
        /// <summary>
        /// 右足首を利用するなら true
        /// </summary>
        public bool AnkleRightEnable
        {
            get { return _jointsOption.AnkleRightEnable; }
            set
            {
                if (_jointsOption.AnkleRightEnable == value)
                {
                    return;
                }
                _jointsOption.AnkleRightEnable = value;
                OnPropertyChanged("AnkleRightEnable");
            }
        }
        /// <summary>
        /// 右足を利用するなら true
        /// </summary>
        public bool FootRightEnable
        {
            get { return _jointsOption.FootRightEnable; }
            set
            {
                if (_jointsOption.FootRightEnable == value)
                {
                    return;
                }
                _jointsOption.FootRightEnable = value;
                OnPropertyChanged("FootRightEnable");
            }
        }
        /// <summary>
        /// おしりの左を利用するなら true
        /// </summary>
        public bool HipLeftEnable
        {
            get { return _jointsOption.HipLeftEnable; }
            set
            {
                if (_jointsOption.HipLeftEnable == value)
                {
                    return;
                }
                _jointsOption.HipLeftEnable = value;
                OnPropertyChanged("HipLeftEnable");
            }
        }
        /// <summary>
        /// 左膝を利用するなら true
        /// </summary>
        public bool KneeLeftEnable
        {
            get { return _jointsOption.KneeLeftEnable; }
            set
            {
                if (_jointsOption.KneeLeftEnable == value)
                {
                    return;
                }
                _jointsOption.KneeLeftEnable = value;
                OnPropertyChanged("KneeLeftEnable");
            }
        }
        /// <summary>
        /// 左足首を利用するなら true
        /// </summary>
        public bool AnkleLeftEnable
        {
            get { return _jointsOption.AnkleLeftEnable; }
            set
            {
                if (_jointsOption.AnkleLeftEnable == value)
                {
                    return;
                }
                _jointsOption.AnkleLeftEnable = value;
                OnPropertyChanged("AnkleLeftEnable");
            }
        }
        /// <summary>
        /// 左足を利用するなら true
        /// </summary>
        public bool FootLeftEnable
        {
            get { return _jointsOption.FootLeftEnable; }
            set
            {
                if (_jointsOption.FootLeftEnable == value)
                {
                    return;
                }
                _jointsOption.FootLeftEnable = value;
                OnPropertyChanged("FootLeftEnable");
            }
        }

        #endregion

        /// <summary>
        /// サイズ比率
        /// </summary>
        public double SizeProportion
        {
            get { return _blenderOptions.SizeProportion; }
            set
            {
                if (_blenderOptions.SizeProportion == value)
                {
                    return;
                }
                _blenderOptions.SizeProportion = value;
                OnPropertyChanged("SizeProportion");
            }
        }

        /// <summary>
        /// 中心 x 座標
        /// </summary>
        public double CenterX
        {
            get { return _blenderOptions.CenterX; }
            set
            {
                if (_blenderOptions.CenterX == value)
                {
                    return;
                }
                _blenderOptions.CenterX = value;
                OnPropertyChanged("CenterX");
            }
        }
        /// <summary>
        /// 中心 y 座標
        /// </summary>
        public double CenterY
        {
            get { return _blenderOptions.CenterY; }
            set
            {
                if (_blenderOptions.CenterY == value)
                {
                    return;
                }
                _blenderOptions.CenterY = value;
                OnPropertyChanged("CenterY");
            }
        }
        /// <summary>
        /// 中心 z 座標
        /// </summary>
        public double CenterZ
        {
            get { return _blenderOptions.CenterZ; }
            set
            {
                if (_blenderOptions.CenterZ == value)
                {
                    return;
                }
                _blenderOptions.CenterZ = value;
                OnPropertyChanged("CenterZ");
            }
        }
        /// <summary>
        /// 向い合っている想定で座標を適用するなら true
        /// </summary>
        public bool Mirror
        {
            get { return _blenderOptions.Mirror; }
            set
            {
                if (_blenderOptions.Mirror == value)
                {
                    return;
                }
                _blenderOptions.Mirror = value;
                OnPropertyChanged("Mirror");
            }
        }

        #region

        /// <summary>
        /// Kinect のカメラ角度
        /// </summary>
        public int KinectElevationAngle
        {
            get { return _cameraOptions.ElevationAngle; }
            set
            {
                if (_cameraOptions.ElevationAngle == value)
                {
                    return;
                }
                _cameraOptions.ElevationAngle = value;
                OnPropertyChanged("KinectElevationAngle");
            }
        }
        /// <summary>
        /// RGB カメラのデータを描画するなら true
        /// </summary>
        public bool ColorDrawEnable
        {
            get { return _cameraOptions.ColorDrawEnable; }
            set
            {
                if (_cameraOptions.ColorDrawEnable == value)
                {
                    return;
                }
                _cameraOptions.ColorDrawEnable = value;
                OnPropertyChanged("ColorDrawEnable");
            }
        }
        /// <summary>
        /// 深度カメラのデータを描画するなら true
        /// </summary>
        public bool DepthDrawEnable
        {
            get { return _cameraOptions.DepthDrawEnable; }
            set
            {
                if (_cameraOptions.DepthDrawEnable == value)
                {
                    return;
                }
                _cameraOptions.DepthDrawEnable = value;
                OnPropertyChanged("DepthDrawEnable");
            }
        }
        /// <summary>
        /// スケルトンデータを描画するなら true
        /// </summary>
        public bool SkeletonDrawEnable
        {
            get { return _cameraOptions.SkeletonDrawEnable; }
            set
            {
                if (_cameraOptions.SkeletonDrawEnable == value)
                {
                    return;
                }
                _cameraOptions.SkeletonDrawEnable = value;
                OnPropertyChanged("SkeletonDrawEnable");
            }
        }

        #endregion

        /// <summary>
        /// ステータスバーメッセージ
        /// </summary>
        public string StatusBarMessage
        {
            get { return _statusBarMessage; }
            set
            {
                if (_statusBarMessage == value)
                {
                    return;
                }
                _statusBarMessage = value;
                OnPropertyChanged("StatusBarMessage");
            }
        }

        /// <summary>
        /// 原点座標自動設定情報
        /// </summary>
        public string OriginPositionAutoSetInfo
        {
            get
            {
                if (!_kinectManager.Started)
                {
                    return "";
                }
                if (_originPositionAutoSetter.Status == OriginPositionAutoSetter.OriginPositionAutoSetterStatus.NOT_STARTING)
                {
                    if (!_originPositionAutoSetter.AlreadySet)
                    {
                        return "設定されていません。";
                    }
                    else
                    {
                        return "前回設定時間: " + _originPositionAutoSetter.LastSetTime.ToLongTimeString();
                    }
                }
                else if (_originPositionAutoSetter.Status == OriginPositionAutoSetter.OriginPositionAutoSetterStatus.STARTING)
                {
                    return _originPositionAutoSetter.RemainingTime.ToString() + " 秒後に実行します。";
                }
                return "実行します。";
            }
        }

        /// <summary>
        /// RGB カメラの画像データ
        /// </summary>
        public BitmapSource ColorSource
        {
            get { return _kinectDataManager.ColorSource; }
        }
        /// <summary>
        /// 深度カメラの画像データ
        /// </summary>
        public BitmapSource DepthSource
        {
            get { return _kinectDataManager.DepthSource; }
        }
        /// <summary>
        /// ジョイント描画位置リスト
        /// </summary>
        public ObservableCollection<JointDrawPosition> JointDrawPositions
        {
            get { return _kinectDataManager.JointDrawPositions; }
        }

        /// <summary>
        /// Kinect スタートストップメニューラベル
        /// </summary>
        public string KinectStartOrStopMenuLabel
        {
            get
            {
                if (!_kinectManager.Started)
                {
                    return "Kinect Start";
                }
                else
                {
                    return "Kinect Stop";
                }
            }
        }

        /// <summary>
        /// Kinect スタートストップコマンド
        /// </summary>
        public ICommand StarOrStoptKinectCommand
        {
            get { return _startOrStopKinectCommand; }
        }

        /// <summary>
        /// Kinect カメラ適用コマンド
        /// </summary>
        public ICommand ApplyKinectElevationAngleCommand
        {
            get { return _applyKinectElevationAngleCommand; }
        }

        /// <summary>
        /// 原点座標設定コマンド
        /// </summary>
        public ICommand SetOriginPositionCommand
        {
            get { return _setOriginPositionCommand; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KinectDataSenderViewModel()
        {
            _kinectManager = new KinectManager();
            _cameraOptions = new CameraOptions();
            _jointsOption = new JointsOption();
            _blenderOptions = new BlenderOptions();
            _originPositionAutoSetter = new OriginPositionAutoSetter(_jointsOption);
            _kinectDataManager = new KinectDataManager(_cameraOptions, _blenderOptions, _jointsOption);

            _statusBarMessage = "";

            _startOrStopKinectCommand = new DelegateCommand(
                new Action<object>(_StartOrStopKinect),
                new Func<object, bool>(_CanStartOrStopKinect)
            );
            _applyKinectElevationAngleCommand = new DelegateCommand(
                new Action<object>(_ApplyKinectElevationAngle),
                new Func<object, bool>(_CanApplyKinectElevationAngle)
            );
            _setOriginPositionCommand = new DelegateCommand(
                new Action<object>(_SetOriginPosition),
                new Func<object, bool>(_CanSetOriginPosition)
            );

            _originPositionAutoSetter.AddEventListenerTo(_kinectManager);
            _originPositionAutoSetter.Update += new EventHandler<EventArgs>(_originPositionAutoSetter_Update);

            _kinectDataManager.AddEventListenerTo(_kinectManager);
            _kinectManager.ColorUpdate += new EventHandler<ColorUpdateEventArgs>(_kinectManager_ColorUpdate);
            _kinectManager.DepthUpdate += new EventHandler<DepthUpdateEventArgs>(_kinectManager_DepthUpdate);
            _kinectManager.SkeletonUpdate += new EventHandler<SkeletonUpdateEventArgs>(_kinectManager_SkeletonUpdate);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~KinectDataSenderViewModel()
        {
            _kinectManager.SkeletonUpdate -= _kinectManager_SkeletonUpdate;
            _kinectManager.DepthUpdate -= _kinectManager_DepthUpdate;
            _kinectManager.ColorUpdate -= _kinectManager_ColorUpdate;
            _kinectDataManager.RemoveEventListenerTo(_kinectManager);
            _originPositionAutoSetter.RemoveEventListenerTo(_kinectManager);
        }

        /// <summary>
        /// RGB カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_ColorUpdate(object sender, ColorUpdateEventArgs e)
        {
            OnPropertyChanged("ColorSource");
        }
        /// <summary>
        /// 深度カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_DepthUpdate(object sender, DepthUpdateEventArgs e)
        {
            OnPropertyChanged("DepthSource");
        }
        /// <summary>
        /// スケルトンデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_SkeletonUpdate(object sender, SkeletonUpdateEventArgs e)
        {
            //OnPropertyChanged("JointDrawPositions");
        }

        /// <summary>
        /// 原点座標自動設定更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _originPositionAutoSetter_Update(object sender, EventArgs e)
        {
            OnPropertyChanged("OriginPositionAutoSetInfo");
            // TODO: 自動設定ボタンのセンシティビティが自動で元に戻らない。（画面をクリックしたら戻る。）
        }

        /// <summary>
        /// プロパティの変更通知イベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティの変更通知イベント送信
        /// </summary>
        /// <param name="columnName">項目名</param>
        protected void OnPropertyChanged(string columnName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(columnName));
        }

        /// <summary>
        /// エラー処理
        /// </summary>
        public string Error
        {
            get { return null; }
        }

        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <param name="columnName">項目名</param>
        /// <returns>エラーメッセージ</returns>
        public string this[string columnName]
        {
            get
            {
                try
                {
                    if (columnName == "HeadName")
                    {
                        if (HeadEnable && string.IsNullOrEmpty(HeadName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "ShoulderCenterName")
                    {
                        if (ShoulderCenterEnable && string.IsNullOrEmpty(ShoulderCenterName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "ShoulderRightName")
                    {
                        if (ShoulderRightEnable && string.IsNullOrEmpty(ShoulderRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "ElbowRightName")
                    {
                        if (ElbowRightEnable && string.IsNullOrEmpty(ElbowRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "WristRightName")
                    {
                        if (WristRightEnable && string.IsNullOrEmpty(WristRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "HandRightName")
                    {
                        if (HandRightEnable && string.IsNullOrEmpty(HandRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "ShoulderLeftName")
                    {
                        if (ShoulderLeftEnable && string.IsNullOrEmpty(ShoulderLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "ElbowLeftName")
                    {
                        if (ElbowLeftEnable && string.IsNullOrEmpty(ElbowLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "WristLeftName")
                    {
                        if (WristLeftEnable && string.IsNullOrEmpty(WristLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "HandLeftName")
                    {
                        if (HandLeftEnable && string.IsNullOrEmpty(HandLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "SpineName")
                    {
                        if (SpineEnable && string.IsNullOrEmpty(SpineName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "HipCenterName")
                    {
                        if (HipCenterEnable && string.IsNullOrEmpty(HipCenterName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "HipRightName")
                    {
                        if (HipRightEnable && string.IsNullOrEmpty(HipRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "KneeRightName")
                    {
                        if (KneeRightEnable && string.IsNullOrEmpty(KneeRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "AnkleRightName")
                    {
                        if (AnkleRightEnable && string.IsNullOrEmpty(AnkleRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "FootRightName")
                    {
                        if (FootRightEnable && string.IsNullOrEmpty(FootRightName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "HipLeftName")
                    {
                        if (HipLeftEnable && string.IsNullOrEmpty(HipLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "KneeLeftName")
                    {
                        if (KneeLeftEnable && string.IsNullOrEmpty(KneeLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "AnkleLeftName")
                    {
                        if (AnkleLeftEnable && string.IsNullOrEmpty(AnkleLeftName))
                        {
                            return "*";
                        }
                    }
                    if (columnName == "FootLeftName")
                    {
                        if (FootLeftEnable && string.IsNullOrEmpty(FootLeftName))
                        {
                            return "*";
                        }
                    }
                    return null;
                }
                finally
                {
                    // CanExecuteChanged イベントの発行
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// Kinect スタートまたはストップ
        /// </summary>
        /// <param name="param">パラメータ</param>
        private void _StartOrStopKinect(object param)
        {
            try
            {
                if (!_kinectManager.Started)
                {
                    _kinectManager.Initialize();
                    try
                    {
                        _kinectManager.Start();
                    }
                    catch (Exception)
                    {
                        _kinectManager.Terminate();
                        throw;
                    }
                    StatusBarMessage = "実行中";
                }
                else
                {
                    _kinectManager.Stop();
                    _kinectManager.Terminate();
                    StatusBarMessage = "";
                }
            }
            catch (Exception e)
            {
                StatusBarMessage = e.Message;
            }
            OnPropertyChanged("KinectStartOrStopMenuLabel");
            OnPropertyChanged("OriginPositionAutoSetInfo");
        }
        /// <summary>
        /// Kinect スタートまたはストップ実行可能判定
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>実行可能なら true</returns>
        private bool _CanStartOrStopKinect(object param)
        {
            return true;
        }

        /// <summary>
        /// Kinect カメラ角度適用
        /// </summary>
        /// <param name="param">パラメータ</param>
        private void _ApplyKinectElevationAngle(object param)
        {
            _kinectManager.SetElevationAngle(_cameraOptions.ElevationAngle);
        }
        /// <summary>
        ///  Kinect カメラ角度適用実行可能判定
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>実行可能なら true</returns>
        private bool _CanApplyKinectElevationAngle(object param)
        {
            return _kinectManager.Started;
        }

        /// <summary>
        /// 原点座標設定
        /// </summary>
        /// <param name="param">パラメータ</param>
        private void _SetOriginPosition(object param)
        {
            _originPositionAutoSetter.StartAutoSetting();
        }
        /// <summary>
        ///  原点座標設定実行可能判定
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns>実行可能なら true</returns>
        private bool _CanSetOriginPosition(object param)
        {
            return (
                _kinectManager.Started && 
                _originPositionAutoSetter.Status == OriginPositionAutoSetter.OriginPositionAutoSetterStatus.NOT_STARTING
            );
        }
    }
}
