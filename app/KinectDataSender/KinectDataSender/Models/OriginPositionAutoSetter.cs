using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Threading;
using KinectDataSender.Models.Events;

namespace KinectDataSender.Models
{
    /// <summary>
    /// 原点座標を自動設定するクラス
    /// </summary>
    public class OriginPositionAutoSetter
    {
        /// <summary>
        /// クラスの状態
        /// </summary>
        public enum OriginPositionAutoSetterStatus
        {
            /// <summary>
            /// 自動設定が開始されていない、または実設定が終わった直後
            /// </summary>
            NOT_STARTING,
            /// <summary>
            /// 自動設定が開始され、実設定がされるまで待機している状態
            /// </summary>
            STARTING,
            /// <summary>
            /// 実設定が可能な状態
            /// </summary>
            SETTABLE
        }

        private const int _WAITING_TIME = 5;

        /// <summary>
        /// 更新イベント
        /// </summary>
        public event EventHandler<EventArgs> Update;

        private JointsOption _jointsOption;

        private bool _addedEventListener;
        private Timer _timer;

        private int _remainingTime;
        private bool _alreadySet;
        private DateTime _lastSetTime;

        private OriginPositionAutoSetterStatus _status;

        /// <summary>
        /// 設定を実際に実行するまでの残り時間
        /// </summary>
        public int RemainingTime
        {
            get { return _remainingTime; }
        }

        /// <summary>
        /// 既に設定されているなら true
        /// </summary>
        public bool AlreadySet
        {
            get { return _alreadySet; }
        }

        /// <summary>
        /// 最後に実設定がされた時間
        /// </summary>
        public DateTime LastSetTime
        {
            get { return _lastSetTime; }
        }

        /// <summary>
        /// 状態
        /// </summary>
        public OriginPositionAutoSetterStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="jointsOption">Joint 単位の設定</param>
        public OriginPositionAutoSetter(JointsOption jointsOption)
        {
            _jointsOption = jointsOption;

            _addedEventListener = false;
            _timer = null;

            _remainingTime = 0;
            _alreadySet = false;

            _status = OriginPositionAutoSetterStatus.NOT_STARTING;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~OriginPositionAutoSetter()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
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
            _addedEventListener = false;
        }

        /// <summary>
        /// 自動設定開始
        /// </summary>
        public void StartAutoSetting()
        {
            if (!_addedEventListener)
            {
                throw new InvalidOperationException("イベントリスナーが登録されていません。");
            }
            _timer = new Timer(_TimerCallback, null, 1000, 1000);
            _remainingTime = _WAITING_TIME;
            _status = OriginPositionAutoSetterStatus.STARTING;

            EventHandler<EventArgs> eventHandler = Update;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// タイマーコールバック関数
        /// </summary>
        /// <param name="state">ステータス</param>
        private void _TimerCallback(object state)
        {
            _remainingTime--;
            if (_remainingTime <= 0)
            {
                _status = OriginPositionAutoSetterStatus.SETTABLE;

                _timer.Dispose();
                _timer = null;
            }

            EventHandler<EventArgs> eventHandler = Update;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// スケルトンデータ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void kinectManager_SkeletonUpdate(object sender, SkeletonUpdateEventArgs e)
        {
            // 実設定可能でないなら終了
            if (_status != OriginPositionAutoSetterStatus.SETTABLE)
            {
                return;
            }

            KinectSensor kinect = e.Kinect;
            SkeletonFrame skeletonFrame = e.SkeletonFrame;

            Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
            skeletonFrame.CopySkeletonDataTo(skeletons);

            IList<JointDrawPosition> jointDrawPositionList = new List<JointDrawPosition>();

            bool success = false;

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                {
                    continue;
                }

                // 実設定
                _SetOriginPosition(skeleton);
                success = true;
            }

            if (success)
            {
                _alreadySet = true;
                _lastSetTime = DateTime.Now;
            }
            _status = OriginPositionAutoSetterStatus.NOT_STARTING;

            EventHandler<EventArgs> eventHandler = Update;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        /// <summary>
        /// 原点座標設定
        /// </summary>
        /// <param name="skeleton">スケルトンデータ</param>
        private void _SetOriginPosition(Skeleton skeleton)
        {
            foreach (Joint joint in skeleton.Joints)
            {
                if (joint.TrackingState == JointTrackingState.NotTracked)
                {
                    continue;
                }
                _jointsOption.SetOriginPosition(
                    joint.JointType,
                    joint.Position.X,
                    joint.Position.Y,
                    joint.Position.Z
                );
            }
        }
    }
}
