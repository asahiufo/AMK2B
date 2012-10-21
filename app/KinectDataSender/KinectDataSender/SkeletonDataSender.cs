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
        /// <param name="blenderOptions">Blender 側へ反映する際のオプション</param>
        /// <param name="jointsOption">Joint 単位の設定</param>
        public void Send(Skeleton skeleton, uint userNo, BlenderOptions blenderOptions, JointsOption jointsOption)
        {
            double sizeProportion = blenderOptions.SizeProportion;
            double centerX = blenderOptions.CenterX;
            double centerY = blenderOptions.CenterY;
            double centerZ = blenderOptions.CenterZ;
            int oppositeAdjust = 1;
            if (blenderOptions.Opposite)
            {
                oppositeAdjust = -1;
            }

            OscMessage message = new OscMessage(_sourceEndPoint, "/skeleton");
            message.Append(userNo.ToString());

            foreach (Joint joint in skeleton.Joints)
            {
                if (joint.TrackingState == JointTrackingState.NotTracked)
                {
                    continue;
                }

                JointType jointType = joint.JointType;
                if (jointsOption.GetEnable(jointType))
                {
                    message.Append(jointsOption.GetName(jointType));

                    // Kinect
                    // x 軸: 横（右が正）
                    // y 軸: 高さ（上が正）
                    // z 軸: 奥行き（人間から見てカメラがある方向が正）
                    //
                    //  ↓
                    //
                    // Blender
                    // x 軸: 横
                    // y 軸: 奥行き
                    // z 軸: 高さ

                    double locationX = (joint.Position.X - centerX - jointsOption.GetOriginX(jointType)) * sizeProportion;
                    double locationY = (joint.Position.Z - centerZ - jointsOption.GetOriginZ(jointType)) * sizeProportion * oppositeAdjust;
                    double locationZ = (joint.Position.Y - centerY - jointsOption.GetOriginY(jointType)) * sizeProportion;

                    message.Append(locationX.ToString());
                    message.Append(locationY.ToString());
                    message.Append(locationZ.ToString());
                }
            }
            message.Send(_destinationEndPoint);
        }
    }
}
