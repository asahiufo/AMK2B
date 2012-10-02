using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;

namespace KinectDataSender
{
    /// <summary>
    /// メインウインドウ ViewModel
    /// </summary>
    public class KinectDataSenderViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private BlenderJoints _blenderJoints;

        /// <summary>
        /// 頭名
        /// </summary>
        public string HeadName
        {
            get { return _blenderJoints.HeadName; }
            set
            {
                if (_blenderJoints.HeadName == value)
                {
                    return;
                }
                _blenderJoints.HeadName = value;
                OnPropertyChanged("HeadName");
            }
        }
        /// <summary>
        /// 肩中央名
        /// </summary>
        public string ShoulderCenterName
        {
            get { return _blenderJoints.ShoulderCenterName; }
            set
            {
                if (_blenderJoints.ShoulderCenterName == value)
                {
                    return;
                }
                _blenderJoints.ShoulderCenterName = value;
                OnPropertyChanged("ShoulderCenterName");
            }
        }
        /// <summary>
        /// 右肩名
        /// </summary>
        public string ShoulderRightName
        {
            get { return _blenderJoints.ShoulderRightName; }
            set
            {
                if (_blenderJoints.ShoulderRightName == value)
                {
                    return;
                }
                _blenderJoints.ShoulderRightName = value;
                OnPropertyChanged("ShoulderRightName");
            }
        }
        /// <summary>
        /// 右肘名
        /// </summary>
        public string ElbowRightName
        {
            get { return _blenderJoints.ElbowRightName; }
            set
            {
                if (_blenderJoints.ElbowRightName == value)
                {
                    return;
                }
                _blenderJoints.ElbowRightName = value;
                OnPropertyChanged("ElbowRightName");
            }
        }
        /// <summary>
        /// 右手首名
        /// </summary>
        public string WristRightName
        {
            get { return _blenderJoints.WristRightName; }
            set
            {
                if (_blenderJoints.WristRightName == value)
                {
                    return;
                }
                _blenderJoints.WristRightName = value;
                OnPropertyChanged("WristRightName");
            }
        }
        /// <summary>
        /// 右手のひら名
        /// </summary>
        public string HandRightName
        {
            get { return _blenderJoints.HandRightName; }
            set
            {
                if (_blenderJoints.HandRightName == value)
                {
                    return;
                }
                _blenderJoints.HandRightName = value;
                OnPropertyChanged("HandRightName");
            }
        }
        /// <summary>
        /// 左肩名
        /// </summary>
        public string ShoulderLeftName
        {
            get { return _blenderJoints.ShoulderLeftName; }
            set
            {
                if (_blenderJoints.ShoulderLeftName == value)
                {
                    return;
                }
                _blenderJoints.ShoulderLeftName = value;
                OnPropertyChanged("ShoulderLeftName");
            }
        }
        /// <summary>
        /// 左肘名
        /// </summary>
        public string ElbowLeftName
        {
            get { return _blenderJoints.ElbowLeftName; }
            set
            {
                if (_blenderJoints.ElbowLeftName == value)
                {
                    return;
                }
                _blenderJoints.ElbowLeftName = value;
                OnPropertyChanged("ElbowLeftName");
            }
        }
        /// <summary>
        /// 左手首名
        /// </summary>
        public string WristLeftName
        {
            get { return _blenderJoints.WristLeftName; }
            set
            {
                if (_blenderJoints.WristLeftName == value)
                {
                    return;
                }
                _blenderJoints.WristLeftName = value;
                OnPropertyChanged("WristLeftName");
            }
        }
        /// <summary>
        /// 左手のひら名
        /// </summary>
        public string HandLeftName
        {
            get { return _blenderJoints.HandLeftName; }
            set
            {
                if (_blenderJoints.HandLeftName == value)
                {
                    return;
                }
                _blenderJoints.HandLeftName = value;
                OnPropertyChanged("HandLeftName");
            }
        }
        /// <summary>
        /// 鳩尾（背骨）名
        /// </summary>
        public string SpineName
        {
            get { return _blenderJoints.SpineName; }
            set
            {
                if (_blenderJoints.SpineName == value)
                {
                    return;
                }
                _blenderJoints.SpineName = value;
                OnPropertyChanged("SpineName");
            }
        }
        /// <summary>
        /// おしり中心名
        /// </summary>
        public string HipCenterName
        {
            get { return _blenderJoints.HipCenterName; }
            set
            {
                if (_blenderJoints.HipCenterName == value)
                {
                    return;
                }
                _blenderJoints.HipCenterName = value;
                OnPropertyChanged("HipCenterName");
            }
        }
        /// <summary>
        /// おしりの右名
        /// </summary>
        public string HipRightName
        {
            get { return _blenderJoints.HipRightName; }
            set
            {
                if (_blenderJoints.HipRightName == value)
                {
                    return;
                }
                _blenderJoints.HipRightName = value;
                OnPropertyChanged("HipRightName");
            }
        }
        /// <summary>
        /// 右膝名
        /// </summary>
        public string KneeRightName
        {
            get { return _blenderJoints.KneeRightName; }
            set
            {
                if (_blenderJoints.KneeRightName == value)
                {
                    return;
                }
                _blenderJoints.KneeRightName = value;
                OnPropertyChanged("KneeRightName");
            }
        }
        /// <summary>
        /// 右足首名
        /// </summary>
        public string AnkleRightName
        {
            get { return _blenderJoints.AnkleRightName; }
            set
            {
                if (_blenderJoints.AnkleRightName == value)
                {
                    return;
                }
                _blenderJoints.AnkleRightName = value;
                OnPropertyChanged("AnkleRightName");
            }
        }
        /// <summary>
        /// 右足名
        /// </summary>
        public string FootRightName
        {
            get { return _blenderJoints.FootRightName; }
            set
            {
                if (_blenderJoints.FootRightName == value)
                {
                    return;
                }
                _blenderJoints.FootRightName = value;
                OnPropertyChanged("FootRightName");
            }
        }
        /// <summary>
        /// おしりの左名
        /// </summary>
        public string HipLeftName
        {
            get { return _blenderJoints.HipLeftName; }
            set
            {
                if (_blenderJoints.HipLeftName == value)
                {
                    return;
                }
                _blenderJoints.HipLeftName = value;
                OnPropertyChanged("HipLeftName");
            }
        }
        /// <summary>
        /// 左膝名
        /// </summary>
        public string KneeLeftName
        {
            get { return _blenderJoints.KneeLeftName; }
            set
            {
                if (_blenderJoints.KneeLeftName == value)
                {
                    return;
                }
                _blenderJoints.KneeLeftName = value;
                OnPropertyChanged("KneeLeftName");
            }
        }
        /// <summary>
        /// 左足首名
        /// </summary>
        public string AnkleLeftName
        {
            get { return _blenderJoints.AnkleLeftName; }
            set
            {
                if (_blenderJoints.AnkleLeftName == value)
                {
                    return;
                }
                _blenderJoints.AnkleLeftName = value;
                OnPropertyChanged("AnkleLeftName");
            }
        }
        /// <summary>
        /// 左足名
        /// </summary>
        public string FootLeftName
        {
            get { return _blenderJoints.FootLeftName; }
            set
            {
                if (_blenderJoints.FootLeftName == value)
                {
                    return;
                }
                _blenderJoints.FootLeftName = value;
                OnPropertyChanged("FootLeftName");
            }
        }

        /// <summary>
        /// 頭を利用するなら true
        /// </summary>
        public bool HeadEnable
        {
            get { return _blenderJoints.HeadEnable; }
            set
            {
                if (_blenderJoints.HeadEnable == value)
                {
                    return;
                }
                _blenderJoints.HeadEnable = value;
                OnPropertyChanged("HeadEnable");
            }
        }
        /// <summary>
        /// 肩中央を利用するなら true
        /// </summary>
        public bool ShoulderCenterEnable
        {
            get { return _blenderJoints.ShoulderCenterEnable; }
            set
            {
                if (_blenderJoints.ShoulderCenterEnable == value)
                {
                    return;
                }
                _blenderJoints.ShoulderCenterEnable = value;
                OnPropertyChanged("ShoulderCenterEnable");
            }
        }
        /// <summary>
        /// 右肩を利用するなら true
        /// </summary>
        public bool ShoulderRightEnable
        {
            get { return _blenderJoints.ShoulderRightEnable; }
            set
            {
                if (_blenderJoints.ShoulderRightEnable == value)
                {
                    return;
                }
                _blenderJoints.ShoulderRightEnable = value;
                OnPropertyChanged("ShoulderRightEnable");
            }
        }
        /// <summary>
        /// 右肘を利用するなら true
        /// </summary>
        public bool ElbowRightEnable
        {
            get { return _blenderJoints.ElbowRightEnable; }
            set
            {
                if (_blenderJoints.ElbowRightEnable == value)
                {
                    return;
                }
                _blenderJoints.ElbowRightEnable = value;
                OnPropertyChanged("ElbowRightEnable");
            }
        }
        /// <summary>
        /// 右手首を利用するなら true
        /// </summary>
        public bool WristRightEnable
        {
            get { return _blenderJoints.WristRightEnable; }
            set
            {
                if (_blenderJoints.WristRightEnable == value)
                {
                    return;
                }
                _blenderJoints.WristRightEnable = value;
                OnPropertyChanged("WristRightEnable");
            }
        }
        /// <summary>
        /// 右手のひらを利用するなら true
        /// </summary>
        public bool HandRightEnable
        {
            get { return _blenderJoints.HandRightEnable; }
            set
            {
                if (_blenderJoints.HandRightEnable == value)
                {
                    return;
                }
                _blenderJoints.HandRightEnable = value;
                OnPropertyChanged("HandRightEnable");
            }
        }
        /// <summary>
        /// 左肩を利用するなら true
        /// </summary>
        public bool ShoulderLeftEnable
        {
            get { return _blenderJoints.ShoulderLeftEnable; }
            set
            {
                if (_blenderJoints.ShoulderLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.ShoulderLeftEnable = value;
                OnPropertyChanged("ShoulderLeftEnable");
            }
        }
        /// <summary>
        /// 左肘を利用するなら true
        /// </summary>
        public bool ElbowLeftEnable
        {
            get { return _blenderJoints.ElbowLeftEnable; }
            set
            {
                if (_blenderJoints.ElbowLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.ElbowLeftEnable = value;
                OnPropertyChanged("ElbowLeftEnable");
            }
        }
        /// <summary>
        /// 左手首を利用するなら true
        /// </summary>
        public bool WristLeftEnable
        {
            get { return _blenderJoints.WristLeftEnable; }
            set
            {
                if (_blenderJoints.WristLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.WristLeftEnable = value;
                OnPropertyChanged("WristLeftEnable");
            }
        }
        /// <summary>
        /// 左手のひらを利用するなら true
        /// </summary>
        public bool HandLeftEnable
        {
            get { return _blenderJoints.HandLeftEnable; }
            set
            {
                if (_blenderJoints.HandLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.HandLeftEnable = value;
                OnPropertyChanged("HandLeftEnable");
            }
        }
        /// <summary>
        /// 鳩尾（背骨）を利用するなら true
        /// </summary>
        public bool SpineEnable
        {
            get { return _blenderJoints.SpineEnable; }
            set
            {
                if (_blenderJoints.SpineEnable == value)
                {
                    return;
                }
                _blenderJoints.SpineEnable = value;
                OnPropertyChanged("SpineEnable");
            }
        }
        /// <summary>
        /// おしり中心を利用するなら true
        /// </summary>
        public bool HipCenterEnable
        {
            get { return _blenderJoints.HipCenterEnable; }
            set
            {
                if (_blenderJoints.HipCenterEnable == value)
                {
                    return;
                }
                _blenderJoints.HipCenterEnable = value;
                OnPropertyChanged("HipCenterEnable");
            }
        }
        /// <summary>
        /// おしりの右を利用するなら true
        /// </summary>
        public bool HipRightEnable
        {
            get { return _blenderJoints.HipRightEnable; }
            set
            {
                if (_blenderJoints.HipRightEnable == value)
                {
                    return;
                }
                _blenderJoints.HipRightEnable = value;
                OnPropertyChanged("HipRightEnable");
            }
        }
        /// <summary>
        /// 右膝を利用するなら true
        /// </summary>
        public bool KneeRightEnable
        {
            get { return _blenderJoints.KneeRightEnable; }
            set
            {
                if (_blenderJoints.KneeRightEnable == value)
                {
                    return;
                }
                _blenderJoints.KneeRightEnable = value;
                OnPropertyChanged("KneeRightEnable");
            }
        }
        /// <summary>
        /// 右足首を利用するなら true
        /// </summary>
        public bool AnkleRightEnable
        {
            get { return _blenderJoints.AnkleRightEnable; }
            set
            {
                if (_blenderJoints.AnkleRightEnable == value)
                {
                    return;
                }
                _blenderJoints.AnkleRightEnable = value;
                OnPropertyChanged("AnkleRightEnable");
            }
        }
        /// <summary>
        /// 右足を利用するなら true
        /// </summary>
        public bool FootRightEnable
        {
            get { return _blenderJoints.FootRightEnable; }
            set
            {
                if (_blenderJoints.FootRightEnable == value)
                {
                    return;
                }
                _blenderJoints.FootRightEnable = value;
                OnPropertyChanged("FootRightEnable");
            }
        }
        /// <summary>
        /// おしりの左を利用するなら true
        /// </summary>
        public bool HipLeftEnable
        {
            get { return _blenderJoints.HipLeftEnable; }
            set
            {
                if (_blenderJoints.HipLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.HipLeftEnable = value;
                OnPropertyChanged("HipLeftEnable");
            }
        }
        /// <summary>
        /// 左膝を利用するなら true
        /// </summary>
        public bool KneeLeftEnable
        {
            get { return _blenderJoints.KneeLeftEnable; }
            set
            {
                if (_blenderJoints.KneeLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.KneeLeftEnable = value;
                OnPropertyChanged("KneeLeftEnable");
            }
        }
        /// <summary>
        /// 左足首を利用するなら true
        /// </summary>
        public bool AnkleLeftEnable
        {
            get { return _blenderJoints.AnkleLeftEnable; }
            set
            {
                if (_blenderJoints.AnkleLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.AnkleLeftEnable = value;
                OnPropertyChanged("AnkleLeftEnable");
            }
        }
        /// <summary>
        /// 左足を利用するなら true
        /// </summary>
        public bool FootLeftEnable
        {
            get { return _blenderJoints.FootLeftEnable; }
            set
            {
                if (_blenderJoints.FootLeftEnable == value)
                {
                    return;
                }
                _blenderJoints.FootLeftEnable = value;
                OnPropertyChanged("FootLeftEnable");
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KinectDataSenderViewModel()
        {
            _blenderJoints = new BlenderJoints();
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
        /// 
        /// </summary>
        public string Error
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                try
                {
                    if (
                        HeadEnable && string.IsNullOrEmpty(HeadName) ||
                        ShoulderCenterEnable && string.IsNullOrEmpty(ShoulderCenterName) ||
                        ShoulderRightEnable && string.IsNullOrEmpty(ShoulderRightName) ||
                        ElbowRightEnable && string.IsNullOrEmpty(ElbowRightName) ||
                        WristRightEnable && string.IsNullOrEmpty(WristRightName) ||
                        HandRightEnable && string.IsNullOrEmpty(HandRightName) ||
                        ShoulderLeftEnable && string.IsNullOrEmpty(ShoulderLeftName) ||
                        ElbowLeftEnable && string.IsNullOrEmpty(ElbowLeftName) ||
                        WristLeftEnable && string.IsNullOrEmpty(WristLeftName) ||
                        HandLeftEnable && string.IsNullOrEmpty(HandLeftName) ||
                        SpineEnable && string.IsNullOrEmpty(SpineName) ||
                        HipCenterEnable && string.IsNullOrEmpty(HipCenterName) ||
                        HipRightEnable && string.IsNullOrEmpty(HipRightName) ||
                        KneeRightEnable && string.IsNullOrEmpty(KneeRightName) ||
                        AnkleRightEnable && string.IsNullOrEmpty(AnkleRightName) ||
                        FootRightEnable && string.IsNullOrEmpty(FootRightName) ||
                        HipLeftEnable && string.IsNullOrEmpty(HipLeftName) ||
                        KneeLeftEnable && string.IsNullOrEmpty(KneeLeftName) ||
                        AnkleLeftEnable && string.IsNullOrEmpty(AnkleLeftName) ||
                        FootLeftEnable && string.IsNullOrEmpty(FootLeftName)
                    ) {
                        return "利用する Joint に名前が入力されていません。";
                    }
                    return null;
                }
                finally
                {
                    // CanExecuteChanged イベントの発行
                    CommandManager.InvalidateRequerySuggested();
                }

                // TODO: コマンド作成
            }
        }
    }
}
