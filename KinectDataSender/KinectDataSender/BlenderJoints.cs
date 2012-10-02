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
    public class BlenderJoints
    {
        private JointSetting _head;
        private JointSetting _shoulderCenter;
        private JointSetting _shoulderRight;
        private JointSetting _elbowRight;
        private JointSetting _wristRight;
        private JointSetting _handRight;
        private JointSetting _shoulderLeft;
        private JointSetting _elbowLeft;
        private JointSetting _wristLeft;
        private JointSetting _handLeft;
        private JointSetting _spine;
        private JointSetting _hipCenter;
        private JointSetting _hipRight;
        private JointSetting _kneeRight;
        private JointSetting _ankleRight;
        private JointSetting _footRight;
        private JointSetting _hipLeft;
        private JointSetting _kneeLeft;
        private JointSetting _ankleLeft;
        private JointSetting _footLeft;

        private IDictionary<JointType, JointSetting> _jointMap;

        /// <summary>
        /// 頭名
        /// </summary>
        public string HeadName
        {
            get { return _head.Name; }
            set { _head.Name = value; }
        }
        /// <summary>
        /// 肩中央名
        /// </summary>
        public string ShoulderCenterName
        {
            get { return _shoulderCenter.Name; }
            set { _shoulderCenter.Name = value; }
        }
        /// <summary>
        /// 右肩名
        /// </summary>
        public string ShoulderRightName
        {
            get { return _shoulderRight.Name; }
            set { _shoulderRight.Name = value; }
        }
        /// <summary>
        /// 右肘名
        /// </summary>
        public string ElbowRightName
        {
            get { return _elbowRight.Name; }
            set { _elbowRight.Name = value; }
        }
        /// <summary>
        /// 右手首名
        /// </summary>
        public string WristRightName
        {
            get { return _wristRight.Name; }
            set { _wristRight.Name = value; }
        }
        /// <summary>
        /// 右手のひら名
        /// </summary>
        public string HandRightName
        {
            get { return _handRight.Name; }
            set { _handRight.Name = value; }
        }
        /// <summary>
        /// 左肩名
        /// </summary>
        public string ShoulderLeftName
        {
            get { return _shoulderLeft.Name; }
            set { _shoulderLeft.Name = value; }
        }
        /// <summary>
        /// 左肘名
        /// </summary>
        public string ElbowLeftName
        {
            get { return _elbowLeft.Name; }
            set { _elbowLeft.Name = value; }
        }
        /// <summary>
        /// 左手首名
        /// </summary>
        public string WristLeftName
        {
            get { return _wristLeft.Name; }
            set { _wristLeft.Name = value; }
        }
        /// <summary>
        /// 左手のひら名
        /// </summary>
        public string HandLeftName
        {
            get { return _handLeft.Name; }
            set { _handLeft.Name = value; }
        }
        /// <summary>
        /// 鳩尾（背骨）名
        /// </summary>
        public string SpineName
        {
            get { return _spine.Name; }
            set { _spine.Name = value; }
        }
        /// <summary>
        /// おしり中心名
        /// </summary>
        public string HipCenterName
        {
            get { return _hipCenter.Name; }
            set { _hipCenter.Name = value; }
        }
        /// <summary>
        /// おしりの右名
        /// </summary>
        public string HipRightName
        {
            get { return _hipRight.Name; }
            set { _hipRight.Name = value; }
        }
        /// <summary>
        /// 右膝名
        /// </summary>
        public string KneeRightName
        {
            get { return _kneeRight.Name; }
            set { _kneeRight.Name = value; }
        }
        /// <summary>
        /// 右足首名
        /// </summary>
        public string AnkleRightName
        {
            get { return _ankleRight.Name; }
            set { _ankleRight.Name = value; }
        }
        /// <summary>
        /// 右足名
        /// </summary>
        public string FootRightName
        {
            get { return _footRight.Name; }
            set { _footRight.Name = value; }
        }
        /// <summary>
        /// おしりの左名
        /// </summary>
        public string HipLeftName
        {
            get { return _hipLeft.Name; }
            set { _hipLeft.Name = value; }
        }
        /// <summary>
        /// 左膝名
        /// </summary>
        public string KneeLeftName
        {
            get { return _kneeLeft.Name; }
            set { _kneeLeft.Name = value; }
        }
        /// <summary>
        /// 左足首名
        /// </summary>
        public string AnkleLeftName
        {
            get { return _ankleLeft.Name; }
            set { _ankleLeft.Name = value; }
        }
        /// <summary>
        /// 左足名
        /// </summary>
        public string FootLeftName
        {
            get { return _footLeft.Name; }
            set { _footLeft.Name = value; }
        }

        /// <summary>
        /// 頭を利用するなら true
        /// </summary>
        public bool HeadEnable
        {
            get { return _head.Enable; }
            set { _head.Enable = value; }
        }
        /// <summary>
        /// 肩中央を利用するなら true
        /// </summary>
        public bool ShoulderCenterEnable
        {
            get { return _shoulderCenter.Enable; }
            set { _shoulderCenter.Enable = value; }
        }
        /// <summary>
        /// 右肩を利用するなら true
        /// </summary>
        public bool ShoulderRightEnable
        {
            get { return _shoulderRight.Enable; }
            set { _shoulderRight.Enable = value; }
        }
        /// <summary>
        /// 右肘を利用するなら true
        /// </summary>
        public bool ElbowRightEnable
        {
            get { return _elbowRight.Enable; }
            set { _elbowRight.Enable = value; }
        }
        /// <summary>
        /// 右手首を利用するなら true
        /// </summary>
        public bool WristRightEnable
        {
            get { return _wristRight.Enable; }
            set { _wristRight.Enable = value; }
        }
        /// <summary>
        /// 右手のひらを利用するなら true
        /// </summary>
        public bool HandRightEnable
        {
            get { return _handRight.Enable; }
            set { _handRight.Enable = value; }
        }
        /// <summary>
        /// 左肩を利用するなら true
        /// </summary>
        public bool ShoulderLeftEnable
        {
            get { return _shoulderLeft.Enable; }
            set { _shoulderLeft.Enable = value; }
        }
        /// <summary>
        /// 左肘を利用するなら true
        /// </summary>
        public bool ElbowLeftEnable
        {
            get { return _elbowLeft.Enable; }
            set { _elbowLeft.Enable = value; }
        }
        /// <summary>
        /// 左手首を利用するなら true
        /// </summary>
        public bool WristLeftEnable
        {
            get { return _wristLeft.Enable; }
            set { _wristLeft.Enable = value; }
        }
        /// <summary>
        /// 左手のひらを利用するなら true
        /// </summary>
        public bool HandLeftEnable
        {
            get { return _handLeft.Enable; }
            set { _handLeft.Enable = value; }
        }
        /// <summary>
        /// 鳩尾（背骨）を利用するなら true
        /// </summary>
        public bool SpineEnable
        {
            get { return _spine.Enable; }
            set { _spine.Enable = value; }
        }
        /// <summary>
        /// おしり中心を利用するなら true
        /// </summary>
        public bool HipCenterEnable
        {
            get { return _hipCenter.Enable; }
            set { _hipCenter.Enable = value; }
        }
        /// <summary>
        /// おしりの右を利用するなら true
        /// </summary>
        public bool HipRightEnable
        {
            get { return _hipRight.Enable; }
            set { _hipRight.Enable = value; }
        }
        /// <summary>
        /// 右膝を利用するなら true
        /// </summary>
        public bool KneeRightEnable
        {
            get { return _kneeRight.Enable; }
            set { _kneeRight.Enable = value; }
        }
        /// <summary>
        /// 右足首を利用するなら true
        /// </summary>
        public bool AnkleRightEnable
        {
            get { return _ankleRight.Enable; }
            set { _ankleRight.Enable = value; }
        }
        /// <summary>
        /// 右足を利用するなら true
        /// </summary>
        public bool FootRightEnable
        {
            get { return _footRight.Enable; }
            set { _footRight.Enable = value; }
        }
        /// <summary>
        /// おしりの左を利用するなら true
        /// </summary>
        public bool HipLeftEnable
        {
            get { return _hipLeft.Enable; }
            set { _hipLeft.Enable = value; }
        }
        /// <summary>
        /// 左膝を利用するなら true
        /// </summary>
        public bool KneeLeftEnable
        {
            get { return _kneeLeft.Enable; }
            set { _kneeLeft.Enable = value; }
        }
        /// <summary>
        /// 左足首を利用するなら true
        /// </summary>
        public bool AnkleLeftEnable
        {
            get { return _ankleLeft.Enable; }
            set { _ankleLeft.Enable = value; }
        }
        /// <summary>
        /// 左足を利用するなら true
        /// </summary>
        public bool FootLeftEnable
        {
            get { return _footLeft.Enable; }
            set { _footLeft.Enable = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BlenderJoints()
        {
            _head = new JointSetting();
            _shoulderCenter = new JointSetting();
            _shoulderRight = new JointSetting();
            _elbowRight = new JointSetting();
            _wristRight = new JointSetting();
            _handRight = new JointSetting();
            _shoulderLeft = new JointSetting();
            _elbowLeft = new JointSetting();
            _wristLeft = new JointSetting();
            _handLeft = new JointSetting();
            _spine = new JointSetting();
            _hipCenter = new JointSetting();
            _hipRight = new JointSetting();
            _kneeRight = new JointSetting();
            _ankleRight = new JointSetting();
            _footRight = new JointSetting();
            _hipLeft = new JointSetting();
            _kneeLeft = new JointSetting();
            _ankleLeft = new JointSetting();
            _footLeft = new JointSetting();

            _jointMap = new SortedDictionary<JointType, JointSetting>();
            _jointMap.Add(JointType.Head, _head);
            _jointMap.Add(JointType.ShoulderCenter, _shoulderCenter);
            _jointMap.Add(JointType.ShoulderRight, _shoulderRight);
            _jointMap.Add(JointType.ElbowRight, _elbowRight);
            _jointMap.Add(JointType.WristRight, _wristRight);
            _jointMap.Add(JointType.HandRight, _handRight);
            _jointMap.Add(JointType.ShoulderLeft, _shoulderLeft);
            _jointMap.Add(JointType.ElbowLeft, _elbowLeft);
            _jointMap.Add(JointType.WristLeft, _wristLeft);
            _jointMap.Add(JointType.HandLeft, _handLeft);
            _jointMap.Add(JointType.Spine, _spine);
            _jointMap.Add(JointType.HipCenter, _hipCenter);
            _jointMap.Add(JointType.HipRight, _hipRight);
            _jointMap.Add(JointType.KneeRight, _kneeRight);
            _jointMap.Add(JointType.AnkleRight, _ankleRight);
            _jointMap.Add(JointType.FootRight, _footRight);
            _jointMap.Add(JointType.HipLeft, _hipLeft);
            _jointMap.Add(JointType.KneeLeft, _kneeLeft);
            _jointMap.Add(JointType.AnkleLeft, _ankleLeft);
            _jointMap.Add(JointType.FootLeft, _footLeft);
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
            return _jointMap[jointType].Name;
        }

        /// <summary>
        /// Kinect の JointType から Joint の利用有無を取得
        /// </summary>
        /// <param name="jointType">Kinect の JointType</param>
        /// <returns>Joint を利用するなら true</returns>
        public bool GetEnable(JointType jointType)
        {
            return _jointMap[jointType].Enable;
        }
    }

    /// <summary>
    /// Joint 設定
    /// </summary>
    class JointSetting
    {
        private string _name;
        private bool _enable;

        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get { return _name;  }
            set { _name = value; }
        }

        /// <summary>
        /// この Joint を利用するなら true
        /// </summary>
        public bool Enable
        {
            get { return _enable;  }
            set { _enable = value; }
        }
    }
}
