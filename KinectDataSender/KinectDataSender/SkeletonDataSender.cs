using System.Net;
using Bespoke.Common.Osc;
using Microsoft.Kinect;

namespace KinectDataSender
{
    /// <summary>
    /// スケルトンデータセンダー
    /// </summary>
    public class SkeletonDataSender
    {
        private IPEndPoint _sourceEndPoint;
        private IPEndPoint _destinationEndPoint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ipAddress">送信先を指す IPAddress オブジェクト</param>
        /// <param name="port">送信先のポート番号</param>
        public SkeletonDataSender(IPAddress ipAddress, int port)
        {
            _sourceEndPoint      = new IPEndPoint(ipAddress, port);
            _destinationEndPoint = new IPEndPoint(ipAddress, port);
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SkeletonDataSender()
        {
        }

        /// <summary>
        /// 送信
        /// </summary>
        /// <param name="skeleton">スケルトンデータ</param>
        /// <param name="userNo">ユーザー No（Kinect で認識された人物を識別する番号）</param>
        /// <param name="blenderJoints">Blender 上での Joint 名</param>
        public void Send(Skeleton skeleton, uint userNo, BlenderJoints blenderJoints)
        {
            OscMessage message = new OscMessage(_sourceEndPoint, "/skeleton");
            message.Append(userNo.ToString());
            foreach (Joint joint in skeleton.Joints)
            {
                if (blenderJoints.GetEnable(joint.JointType))
                {
                    message.Append(blenderJoints.GetName(joint.JointType));
                    message.Append(joint.Position.X.ToString());
                    message.Append(joint.Position.Y.ToString());
                    message.Append(joint.Position.Z.ToString());
                }
            }
            message.Send(_destinationEndPoint);
        }
    }
}
