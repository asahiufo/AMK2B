
namespace KinectDataSender.Models
{
    /// <summary>
    /// ジョイント描画位置
    /// </summary>
    public class JointDrawPosition
    {
        private double _x;
        private double _y;

        /// <summary>
        /// x 座標
        /// </summary>
        public double X
        {
            get { return _x;  }
            set { _x = value; }
        }

        /// <summary>
        /// y 座標
        /// </summary>
        public double Y
        {
            get { return _y;  }
            set { _y = value; }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JointDrawPosition()
        {
            _x = 0;
            _y = 0;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~JointDrawPosition()
        {
        }
    }
}
