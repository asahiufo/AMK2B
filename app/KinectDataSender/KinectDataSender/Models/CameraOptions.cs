
namespace KinectDataSender.Models
{
    /// <summary>
    /// カメラ設定
    /// </summary>
    public class CameraOptions
    {
        private int _elevationAngle;
        private bool _colorDrawEnable;
        private bool _depthDrawEnable;
        private bool _skeletonDrawEnable;

        /// <summary>
        /// カメラ角度
        /// </summary>
        public int ElevationAngle
        {
            get { return _elevationAngle; }
            set { _elevationAngle = value; }
        }

        /// <summary>
        /// RGB カメラのデータを描画するなら true
        /// </summary>
        public bool ColorDrawEnable
        {
            get { return _colorDrawEnable; }
            set { _colorDrawEnable = value; }
        }
        /// <summary>
        /// 深度カメラのデータを描画するなら true
        /// </summary>
        public bool DepthDrawEnable
        {
            get { return _depthDrawEnable; }
            set { _depthDrawEnable = value; }
        }
        /// <summary>
        /// スケルトンデータを描画するなら true
        /// </summary>
        public bool SkeletonDrawEnable
        {
            get { return _skeletonDrawEnable; }
            set { _skeletonDrawEnable = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CameraOptions()
        {
            _elevationAngle = 0;
            _colorDrawEnable = true;
            _depthDrawEnable = false;
            _skeletonDrawEnable = true;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~CameraOptions()
        {
        }
    }
}
