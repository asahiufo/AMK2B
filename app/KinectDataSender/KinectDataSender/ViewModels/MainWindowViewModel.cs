using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using KinectDataSender.Models;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using KinectDataSender.Models.Events;

namespace KinectDataSender.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        private KinectManager _kinectManager;
        private CameraOptions _cameraOptions;
        private JointsOption _jointsOption;
        private BlenderOptions _blenderOptions;
        private OriginPositionAutoSetter _originPositionAutoSetter;
        private KinectDataManager _kinectDataManager;
        private string _statusBarMessage;

        #region ツールバー用変更通知プロパティ
        /// <summary>
        /// Kinect スタートまたはストップボタンラベル
        /// </summary>
        public string KinectStartOrStopButtonLabel
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
        #endregion

        #region ステータスバー用変更通知プロパティ
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
                RaisePropertyChanged("StatusBarMessage");
            }
        }
        #endregion

        #region カメラ画像表示エリア用変更通知プロパティ
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
        #endregion

        #region 全体設定用変更通知プロパティ
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
                RaisePropertyChanged("SizeProportion");
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
                RaisePropertyChanged("CenterX");
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
                RaisePropertyChanged("CenterY");
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
                RaisePropertyChanged("CenterZ");
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
                RaisePropertyChanged("Mirror");
            }
        }
        #endregion

        #region カメラ設定用変更通知プロパティ
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
                RaisePropertyChanged("KinectElevationAngle");
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
                RaisePropertyChanged("ColorDrawEnable");
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
                RaisePropertyChanged("DepthDrawEnable");
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
                RaisePropertyChanged("SkeletonDrawEnable");
            }
        }
        #endregion

        #region 詳細設定用変更通知プロパティ
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
                RaisePropertyChanged("HeadName");
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
                RaisePropertyChanged("ShoulderCenterName");
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
                RaisePropertyChanged("ShoulderRightName");
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
                RaisePropertyChanged("ElbowRightName");
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
                RaisePropertyChanged("WristRightName");
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
                RaisePropertyChanged("HandRightName");
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
                RaisePropertyChanged("ShoulderLeftName");
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
                RaisePropertyChanged("ElbowLeftName");
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
                RaisePropertyChanged("WristLeftName");
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
                RaisePropertyChanged("HandLeftName");
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
                RaisePropertyChanged("SpineName");
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
                RaisePropertyChanged("HipCenterName");
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
                RaisePropertyChanged("HipRightName");
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
                RaisePropertyChanged("KneeRightName");
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
                RaisePropertyChanged("AnkleRightName");
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
                RaisePropertyChanged("FootRightName");
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
                RaisePropertyChanged("HipLeftName");
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
                RaisePropertyChanged("KneeLeftName");
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
                RaisePropertyChanged("AnkleLeftName");
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
                RaisePropertyChanged("FootLeftName");
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
                RaisePropertyChanged("HeadEnable");
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
                RaisePropertyChanged("ShoulderCenterEnable");
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
                RaisePropertyChanged("ShoulderRightEnable");
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
                RaisePropertyChanged("ElbowRightEnable");
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
                RaisePropertyChanged("WristRightEnable");
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
                RaisePropertyChanged("HandRightEnable");
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
                RaisePropertyChanged("ShoulderLeftEnable");
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
                RaisePropertyChanged("ElbowLeftEnable");
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
                RaisePropertyChanged("WristLeftEnable");
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
                RaisePropertyChanged("HandLeftEnable");
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
                RaisePropertyChanged("SpineEnable");
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
                RaisePropertyChanged("HipCenterEnable");
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
                RaisePropertyChanged("HipRightEnable");
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
                RaisePropertyChanged("KneeRightEnable");
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
                RaisePropertyChanged("AnkleRightEnable");
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
                RaisePropertyChanged("FootRightEnable");
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
                RaisePropertyChanged("HipLeftEnable");
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
                RaisePropertyChanged("KneeLeftEnable");
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
                RaisePropertyChanged("AnkleLeftEnable");
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
                RaisePropertyChanged("FootLeftEnable");
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            _kinectManager = new KinectManager();
            _cameraOptions = new CameraOptions();
            _jointsOption = new JointsOption();
            _blenderOptions = new BlenderOptions();
            _originPositionAutoSetter = new OriginPositionAutoSetter(_jointsOption);
            _kinectDataManager = new KinectDataManager(_cameraOptions, _blenderOptions, _jointsOption);

            _statusBarMessage = "";
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MainWindowViewModel()
        {
            if (_kinectManager.Started)
            {
                _kinectManager.SkeletonUpdate -= _kinectManager_SkeletonUpdate;
                _kinectManager.DepthUpdate -= _kinectManager_DepthUpdate;
                _kinectManager.ColorUpdate -= _kinectManager_ColorUpdate;
                _kinectDataManager.RemoveEventListenerTo(_kinectManager);
                _originPositionAutoSetter.RemoveEventListenerTo(_kinectManager);

                _kinectManager.Stop();
                _kinectManager.Terminate();
            }
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            System.Console.WriteLine("Initialize");
        }

        /// <summary>
        /// RGB カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_ColorUpdate(object sender, ColorUpdateEventArgs e)
        {
            RaisePropertyChanged("ColorSource");
        }
        /// <summary>
        /// 深度カメラのデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_DepthUpdate(object sender, DepthUpdateEventArgs e)
        {
            RaisePropertyChanged("DepthSource");
        }
        /// <summary>
        /// スケルトンデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _kinectManager_SkeletonUpdate(object sender, SkeletonUpdateEventArgs e)
        {
            RaisePropertyChanged("JointDrawPositions");
        }

        /// <summary>
        /// 原点座標自動設定更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void _originPositionAutoSetter_Update(object sender, EventArgs e)
        {
            RaisePropertyChanged("OriginPositionAutoSetInfo");
            // TODO: 自動設定ボタンのセンシティビティが自動で元に戻らない。（画面をクリックしたら戻る。）
        }

        /// <summary>
        /// パラメータファイル保存
        /// </summary>
        public void SaveParamFile()
        {
            System.Console.WriteLine("SaveParamFile");
        }

        /// <summary>
        /// パラメータファイル読込
        /// </summary>
        public void LoadParamFile()
        {
            System.Console.WriteLine("LoadParamFile");
        }

        /// <summary>
        /// Kinect スタートまたはストップ
        /// </summary>
        public void StartOrStopKinect()
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

                    _originPositionAutoSetter.AddEventListenerTo(_kinectManager);
                    _originPositionAutoSetter.Update += new EventHandler<EventArgs>(_originPositionAutoSetter_Update);

                    _kinectDataManager.AddEventListenerTo(_kinectManager);
                    _kinectManager.ColorUpdate += new EventHandler<ColorUpdateEventArgs>(_kinectManager_ColorUpdate);
                    _kinectManager.DepthUpdate += new EventHandler<DepthUpdateEventArgs>(_kinectManager_DepthUpdate);
                    _kinectManager.SkeletonUpdate += new EventHandler<SkeletonUpdateEventArgs>(_kinectManager_SkeletonUpdate);

                    StatusBarMessage = "実行中";
                }
                else
                {
                    _kinectManager.SkeletonUpdate -= _kinectManager_SkeletonUpdate;
                    _kinectManager.DepthUpdate -= _kinectManager_DepthUpdate;
                    _kinectManager.ColorUpdate -= _kinectManager_ColorUpdate;
                    _kinectDataManager.RemoveEventListenerTo(_kinectManager);
                    _originPositionAutoSetter.RemoveEventListenerTo(_kinectManager);

                    _kinectManager.Stop();
                    _kinectManager.Terminate();
                    StatusBarMessage = "";
                }
            }
            catch (Exception e)
            {
                StatusBarMessage = e.Message;
            }
            RaisePropertyChanged("KinectStartOrStopButtonLabel");
            RaisePropertyChanged("OriginPositionAutoSetInfo");
        }

        /// <summary>
        /// 原点座標設定
        /// </summary>
        public void SetOriginPosition()
        {
            _originPositionAutoSetter.StartAutoSetting();
        }

        /// <summary>
        /// Kinect カメラ角度適用
        /// </summary>
        public void ApplyKinectElevationAngle()
        {
            _kinectManager.SetElevationAngle(_cameraOptions.ElevationAngle);
        }
    }
}
