using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectDataSender
{
    /// <summary>
    /// Blender 上での Joint 名
    /// </summary>
    class BlenderJoints
    {
        private StringBuilder _headName;
        private StringBuilder _shoulderCenterName;
        private StringBuilder _shoulderRightName;
        private StringBuilder _elbowRightName;
        private StringBuilder _wristRightName;
        private StringBuilder _handRightName;
        private StringBuilder _shoulderLeftName;
        private StringBuilder _elbowLeftName;
        private StringBuilder _wristLeftName;
        private StringBuilder _handLeftName;
        private StringBuilder _spineName;
        private StringBuilder _hipCenterName;
        private StringBuilder _hipRightName;
        private StringBuilder _kneeRightName;
        private StringBuilder _ankleRightName;
        private StringBuilder _footRightName;
        private StringBuilder _hipLeftName;
        private StringBuilder _kneeLeftName;
        private StringBuilder _ankleLeftName;
        private StringBuilder _footLeftName;

        private IDictionary<JointType, StringBuilder> _jointMap;

        /// <summary>
        /// 頭
        /// </summary>
        public string HeadName
        {
            get { return _headName.ToString();  }
            set { _headName.Length = 0; _headName.Clear(); _headName.Append(value); }
        }
        /// <summary>
        /// 肩中央
        /// </summary>
        public string ShoulderCenterName
        {
            get { return _shoulderCenterName.ToString(); }
            set { _shoulderCenterName.Length = 0; _shoulderCenterName.Clear(); _shoulderCenterName.Append(value); }
        }
        /// <summary>
        /// 右肩
        /// </summary>
        public string ShoulderRightName
        {
            get { return _shoulderRightName.ToString(); }
            set { _shoulderRightName.Length = 0; _shoulderRightName.Clear(); _shoulderRightName.Append(value); }
        }
        /// <summary>
        /// 右肘
        /// </summary>
        public string ElbowRightName
        {
            get { return _elbowRightName.ToString(); }
            set { _elbowRightName.Length = 0; _elbowRightName.Clear(); _elbowRightName.Append(value); }
        }
        /// <summary>
        /// 右手首
        /// </summary>
        public string WristRightName
        {
            get { return _wristRightName.ToString(); }
            set { _wristRightName.Length = 0; _wristRightName.Clear(); _wristRightName.Append(value); }
        }
        /// <summary>
        /// 右手のひら
        /// </summary>
        public string HandRightName
        {
            get { return _handRightName.ToString(); }
            set { _handRightName.Length = 0; _handRightName.Clear(); _handRightName.Append(value); }
        }
        /// <summary>
        /// 左肩
        /// </summary>
        public string ShoulderLeftName
        {
            get { return _shoulderLeftName.ToString(); }
            set { _shoulderLeftName.Length = 0; _shoulderLeftName.Clear(); _shoulderLeftName.Append(value); }
        }
        /// <summary>
        /// 左肘
        /// </summary>
        public string ElbowLeftName
        {
            get { return _elbowLeftName.ToString(); }
            set { _elbowLeftName.Length = 0; _elbowLeftName.Clear(); _elbowLeftName.Append(value); }
        }
        /// <summary>
        /// 左手首
        /// </summary>
        public string WristLeftName
        {
            get { return _wristLeftName.ToString(); }
            set { _wristLeftName.Length = 0; _wristLeftName.Clear(); _wristLeftName.Append(value); }
        }
        /// <summary>
        /// 左手のひら
        /// </summary>
        public string HandLeftName
        {
            get { return _handLeftName.ToString(); }
            set { _handLeftName.Length = 0; _handLeftName.Clear(); _handLeftName.Append(value); }
        }
        /// <summary>
        /// 鳩尾（背骨）
        /// </summary>
        public string SpineName
        {
            get { return _spineName.ToString(); }
            set { _spineName.Length = 0; _spineName.Clear(); _spineName.Append(value); }
        }
        /// <summary>
        /// おしり中心
        /// </summary>
        public string HipCenterName
        {
            get { return _hipCenterName.ToString(); }
            set { _hipCenterName.Length = 0; _hipCenterName.Clear(); _hipCenterName.Append(value); }
        }
        /// <summary>
        /// おしりの右
        /// </summary>
        public string HipRightName
        {
            get { return _hipRightName.ToString(); }
            set { _hipRightName.Length = 0; _hipRightName.Clear(); _hipRightName.Append(value); }
        }
        /// <summary>
        /// 右膝
        /// </summary>
        public string KneeRightName
        {
            get { return _kneeRightName.ToString(); }
            set { _kneeRightName.Length = 0; _kneeRightName.Clear(); _kneeRightName.Append(value); }
        }
        /// <summary>
        /// 右足首
        /// </summary>
        public string AnkleRightName
        {
            get { return _ankleRightName.ToString(); }
            set { _ankleRightName.Length = 0; _ankleRightName.Clear(); _ankleRightName.Append(value); }
        }
        /// <summary>
        /// 右足
        /// </summary>
        public string FootRightName
        {
            get { return _footRightName.ToString(); }
            set { _footRightName.Length = 0; _footRightName.Clear(); _footRightName.Append(value); }
        }
        /// <summary>
        /// おしりの左
        /// </summary>
        public string HipLeftName
        {
            get { return _hipLeftName.ToString(); }
            set { _hipLeftName.Length = 0; _hipLeftName.Clear(); _hipLeftName.Append(value); }
        }
        /// <summary>
        /// 左膝
        /// </summary>
        public string KneeLeftName
        {
            get { return _kneeLeftName.ToString(); }
            set { _kneeLeftName.Length = 0; _kneeLeftName.Clear(); _kneeLeftName.Append(value); }
        }
        /// <summary>
        /// 左足首
        /// </summary>
        public string AnkleLeftName
        {
            get { return _ankleLeftName.ToString(); }
            set { _ankleLeftName.Length = 0; _ankleLeftName.Clear(); _ankleLeftName.Append(value); }
        }
        /// <summary>
        /// 左足
        /// </summary>
        public string FootLeftName
        {
            get { return _footLeftName.ToString(); }
            set { _footLeftName.Length = 0; _footLeftName.Clear(); _footLeftName.Append(value); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlenderJoints()
        {
            _headName           = new StringBuilder();
            _shoulderCenterName = new StringBuilder();
            _shoulderRightName  = new StringBuilder();
            _elbowRightName     = new StringBuilder();
            _wristRightName     = new StringBuilder();
            _handRightName      = new StringBuilder();
            _shoulderLeftName   = new StringBuilder();
            _elbowLeftName      = new StringBuilder();
            _wristLeftName      = new StringBuilder();
            _handLeftName       = new StringBuilder();
            _spineName          = new StringBuilder();
            _hipCenterName      = new StringBuilder();
            _hipRightName       = new StringBuilder();
            _kneeRightName      = new StringBuilder();
            _ankleRightName     = new StringBuilder();
            _footRightName      = new StringBuilder();
            _hipLeftName        = new StringBuilder();
            _kneeLeftName       = new StringBuilder();
            _ankleLeftName      = new StringBuilder();
            _footLeftName       = new StringBuilder();

            _jointMap = new SortedDictionary<JointType, StringBuilder>();
            _jointMap.Add(JointType.Head, _headName);
            _jointMap.Add(JointType.ShoulderCenter, _shoulderCenterName);
            _jointMap.Add(JointType.ShoulderRight, _shoulderRightName);
            _jointMap.Add(JointType.ElbowRight, _elbowRightName);
            _jointMap.Add(JointType.WristRight, _wristRightName);
            _jointMap.Add(JointType.HandRight, _handRightName);
            _jointMap.Add(JointType.ShoulderLeft, _shoulderLeftName);
            _jointMap.Add(JointType.ElbowLeft, _elbowLeftName);
            _jointMap.Add(JointType.WristLeft, _wristLeftName);
            _jointMap.Add(JointType.HandLeft, _handLeftName);
            _jointMap.Add(JointType.Spine, _spineName);
            _jointMap.Add(JointType.HipCenter, _hipCenterName);
            _jointMap.Add(JointType.HipRight, _hipRightName);
            _jointMap.Add(JointType.KneeRight, _kneeRightName);
            _jointMap.Add(JointType.AnkleRight, _ankleRightName);
            _jointMap.Add(JointType.FootRight, _footRightName);
            _jointMap.Add(JointType.HipLeft, _hipLeftName);
            _jointMap.Add(JointType.KneeLeft, _kneeLeftName);
            _jointMap.Add(JointType.AnkleLeft, _ankleLeftName);
            _jointMap.Add(JointType.FootLeft, _footLeftName);

            // TODO: 各Jointを使用するかどうかのフラグも必要なはず。
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~BlenderJoints()
        {
        }

        /// <summary>
        /// Kinect の JointType から Blender の Joint 名を取得
        /// </summary>
        /// <param name="jointType">Kinect の JointType</param>
        /// <returns>Blender の Joint 名</returns>
        public string GetName(JointType jointType)
        {
            return _jointMap[jointType].ToString();
        }
    }
}
