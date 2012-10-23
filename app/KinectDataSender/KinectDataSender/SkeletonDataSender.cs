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
            bool mirror = blenderOptions.Mirror;

            OscMessage message = new OscMessage(_sourceEndPoint, "/skeleton");
            message.Append(userNo.ToString());

            foreach (Joint joint in skeleton.Joints)
            {
                if (joint.TrackingState == JointTrackingState.NotTracked)
                {
                    continue;
                }

                JointType jointType = joint.JointType;

                double originX = jointsOption.GetOriginX(jointType);
                double originY = jointsOption.GetOriginY(jointType);
                double originZ = jointsOption.GetOriginZ(jointType);

                if (mirror)
                {
                    if (jointType == JointType.ShoulderRight) { jointType = JointType.ShoulderLeft; }
                    else if (jointType == JointType.ShoulderLeft) { jointType = JointType.ShoulderRight; }
                    else if (jointType == JointType.ElbowRight) { jointType = JointType.ElbowLeft; }
                    else if (jointType == JointType.ElbowLeft) { jointType = JointType.ElbowRight; }
                    else if (jointType == JointType.WristRight) { jointType = JointType.WristLeft; }
                    else if (jointType == JointType.WristLeft) { jointType = JointType.WristRight; }
                    else if (jointType == JointType.HandRight) { jointType = JointType.HandLeft; }
                    else if (jointType == JointType.HandLeft) { jointType = JointType.HandRight; }
                    else if (jointType == JointType.HipRight) { jointType = JointType.HipLeft; }
                    else if (jointType == JointType.HipLeft) { jointType = JointType.HipRight; }
                    else if (jointType == JointType.KneeRight) { jointType = JointType.KneeLeft; }
                    else if (jointType == JointType.KneeLeft) { jointType = JointType.KneeRight; }
                    else if (jointType == JointType.AnkleRight) { jointType = JointType.AnkleLeft; }
                    else if (jointType == JointType.AnkleLeft) { jointType = JointType.AnkleRight; }
                    else if (jointType == JointType.FootRight) { jointType = JointType.FootLeft; }
                    else if (jointType == JointType.FootLeft) { jointType = JointType.FootRight; }
                }

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

                    double locationX = 0;
                    double locationY = 0;
                    double locationZ = 0;
                    if (!mirror)
                    {
                        locationX = (joint.Position.X - centerX - originX) * sizeProportion;
                        locationY = (joint.Position.Z - centerZ - originZ) * sizeProportion;
                        locationZ = (joint.Position.Y - centerY - originY) * sizeProportion;
                    }
                    else
                    {
                        locationX = (joint.Position.X - centerX - originX) * sizeProportion * -1;
                        locationY = (joint.Position.Z - centerZ - originZ) * sizeProportion * -1;
                        locationZ = (joint.Position.Y - centerY - originY) * sizeProportion;
                    }

                    message.Append(locationX.ToString());
                    message.Append(locationY.ToString());
                    message.Append(locationZ.ToString());
                }
            }
            message.Send(_destinationEndPoint);
        }
    }
}
