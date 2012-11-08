using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using System.Xml;

namespace KinectDataSender.Models
{
    /// <summary>
    /// パラメータファイルマネージャー
    /// </summary>
    public class ParameterFileManager
    {
        private IParameterFileData _fileData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileData">ファイルデータ</param>
        public ParameterFileManager(IParameterFileData fileData)
        {
            _fileData = fileData;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ParameterFileManager()
        {
        }

        /// <summary>
        /// パラメータファイル読込
        /// </summary>
        /// <param name="filePath">パラメータファイルパス</param>
        public void load(string filePath)
        {
            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(filePath);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    if (reader.LocalName == "TotalOptions")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                            if (reader.NodeType != XmlNodeType.Element)
                            {
                                continue;
                            }

                            if (reader.LocalName == "SizeProportion")
                            {
                                _fileData.SizeProportion = reader.ReadElementContentAsDouble();
                            }
                            if (reader.LocalName == "CenterX")
                            {
                                _fileData.CenterX = reader.ReadElementContentAsDouble();
                            }
                            if (reader.LocalName == "CenterY")
                            {
                                _fileData.CenterY = reader.ReadElementContentAsDouble();
                            }
                            if (reader.LocalName == "CenterZ")
                            {
                                _fileData.CenterZ = reader.ReadElementContentAsDouble();
                            }
                            if (reader.LocalName == "Mirror")
                            {
                                _fileData.Mirror = reader.ReadElementContentAsBoolean();
                            }
                        }
                    }

                    if (reader.LocalName == "CameraOptions")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                            if (reader.NodeType != XmlNodeType.Element)
                            {
                                continue;
                            }

                            if (reader.LocalName == "KinectElevationAngle")
                            {
                                _fileData.KinectElevationAngle = reader.ReadElementContentAsInt();
                            }
                            if (reader.LocalName == "ColorDrawEnable")
                            {
                                _fileData.ColorDrawEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "DepthDrawEnable")
                            {
                                _fileData.DepthDrawEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "SkeletonDrawEnable")
                            {
                                _fileData.SkeletonDrawEnable = reader.ReadElementContentAsBoolean();
                            }
                        }
                    }

                    if (reader.LocalName == "DetailOptions")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                            if (reader.NodeType != XmlNodeType.Element)
                            {
                                continue;
                            }

                            if (reader.LocalName == "HeadName")
                            {
                                _fileData.HeadName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "ShoulderCenterName")
                            {
                                _fileData.ShoulderCenterName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "ShoulderRightName")
                            {
                                _fileData.ShoulderRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "ElbowRightName")
                            {
                                _fileData.ElbowRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "WristRightName")
                            {
                                _fileData.WristRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "HandRightName")
                            {
                                _fileData.HandRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "ShoulderLeftName")
                            {
                                _fileData.ShoulderLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "ElbowLeftName")
                            {
                                _fileData.ElbowLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "WristLeftName")
                            {
                                _fileData.WristLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "HandLeftName")
                            {
                                _fileData.HandLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "SpineName")
                            {
                                _fileData.SpineName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "HipCenterName")
                            {
                                _fileData.HipCenterName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "HipRightName")
                            {
                                _fileData.HipRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "KneeRightName")
                            {
                                _fileData.KneeRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "AnkleRightName")
                            {
                                _fileData.AnkleRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "FootRightName")
                            {
                                _fileData.FootRightName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "HipLeftName")
                            {
                                _fileData.HipLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "KneeLeftName")
                            {
                                _fileData.KneeLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "AnkleLeftName")
                            {
                                _fileData.AnkleLeftName = reader.ReadElementContentAsString();
                            }
                            if (reader.LocalName == "FootLeftName")
                            {
                                _fileData.FootLeftName = reader.ReadElementContentAsString();
                            }

                            if (reader.LocalName == "HeadEnable")
                            {
                                _fileData.HeadEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "ShoulderCenterEnable")
                            {
                                _fileData.ShoulderCenterEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "ShoulderRightEnable")
                            {
                                _fileData.ShoulderRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "ElbowRightEnable")
                            {
                                _fileData.ElbowRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "WristRightEnable")
                            {
                                _fileData.WristRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "HandRightEnable")
                            {
                                _fileData.HandRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "ShoulderLeftEnable")
                            {
                                _fileData.ShoulderLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "ElbowLeftEnable")
                            {
                                _fileData.ElbowLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "WristLeftEnable")
                            {
                                _fileData.WristLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "HandLeftEnable")
                            {
                                _fileData.HandLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "SpineEnable")
                            {
                                _fileData.SpineEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "HipCenterEnable")
                            {
                                _fileData.HipCenterEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "HipRightEnable")
                            {
                                _fileData.HipRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "KneeRightEnable")
                            {
                                _fileData.KneeRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "AnkleRightEnable")
                            {
                                _fileData.AnkleRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "FootRightEnable")
                            {
                                _fileData.FootRightEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "HipLeftEnable")
                            {
                                _fileData.HipLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "KneeLeftEnable")
                            {
                                _fileData.KneeLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "AnkleLeftEnable")
                            {
                                _fileData.AnkleLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                            if (reader.LocalName == "FootLeftEnable")
                            {
                                _fileData.FootLeftEnable = reader.ReadElementContentAsBoolean();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// パラメータファイル保存
        /// </summary>
        /// <param name="filePath">パラメータファイルパス</param>
        public void save(string filePath)
        {
            XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("AMK2BOptions");

            writer.WriteStartElement("TotalOptions");
            writer.WriteStartElement("SizeProportion"); writer.WriteValue(_fileData.SizeProportion); writer.WriteEndElement();
            writer.WriteStartElement("CenterX"); writer.WriteValue(_fileData.CenterX); writer.WriteEndElement();
            writer.WriteStartElement("CenterY"); writer.WriteValue(_fileData.CenterY); writer.WriteEndElement();
            writer.WriteStartElement("CenterZ"); writer.WriteValue(_fileData.CenterZ); writer.WriteEndElement();
            writer.WriteStartElement("Mirror"); writer.WriteValue(_fileData.Mirror); writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("CameraOptions");
            writer.WriteStartElement("KinectElevationAngle"); writer.WriteValue(_fileData.KinectElevationAngle); writer.WriteEndElement();
            writer.WriteStartElement("ColorDrawEnable"); writer.WriteValue(_fileData.ColorDrawEnable); writer.WriteEndElement();
            writer.WriteStartElement("DepthDrawEnable"); writer.WriteValue(_fileData.DepthDrawEnable); writer.WriteEndElement();
            writer.WriteStartElement("SkeletonDrawEnable"); writer.WriteValue(_fileData.SkeletonDrawEnable); writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteStartElement("DetailOptions");
            writer.WriteStartElement("HeadName"); writer.WriteValue(_fileData.HeadName); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderCenterName"); writer.WriteValue(_fileData.ShoulderCenterName); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderRightName"); writer.WriteValue(_fileData.ShoulderRightName); writer.WriteEndElement();
            writer.WriteStartElement("ElbowRightName"); writer.WriteValue(_fileData.ElbowRightName); writer.WriteEndElement();
            writer.WriteStartElement("WristRightName"); writer.WriteValue(_fileData.WristRightName); writer.WriteEndElement();
            writer.WriteStartElement("HandRightName"); writer.WriteValue(_fileData.HandRightName); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderLeftName"); writer.WriteValue(_fileData.ShoulderLeftName); writer.WriteEndElement();
            writer.WriteStartElement("ElbowLeftName"); writer.WriteValue(_fileData.ElbowLeftName); writer.WriteEndElement();
            writer.WriteStartElement("WristLeftName"); writer.WriteValue(_fileData.WristLeftName); writer.WriteEndElement();
            writer.WriteStartElement("HandLeftName"); writer.WriteValue(_fileData.HandLeftName); writer.WriteEndElement();
            writer.WriteStartElement("SpineName"); writer.WriteValue(_fileData.SpineName); writer.WriteEndElement();
            writer.WriteStartElement("HipCenterName"); writer.WriteValue(_fileData.HipCenterName); writer.WriteEndElement();
            writer.WriteStartElement("HipRightName"); writer.WriteValue(_fileData.HipRightName); writer.WriteEndElement();
            writer.WriteStartElement("KneeRightName"); writer.WriteValue(_fileData.KneeRightName); writer.WriteEndElement();
            writer.WriteStartElement("AnkleRightName"); writer.WriteValue(_fileData.AnkleRightName); writer.WriteEndElement();
            writer.WriteStartElement("FootRightName"); writer.WriteValue(_fileData.FootRightName); writer.WriteEndElement();
            writer.WriteStartElement("HipLeftName"); writer.WriteValue(_fileData.HipLeftName); writer.WriteEndElement();
            writer.WriteStartElement("KneeLeftName"); writer.WriteValue(_fileData.KneeLeftName); writer.WriteEndElement();
            writer.WriteStartElement("AnkleLeftName"); writer.WriteValue(_fileData.AnkleLeftName); writer.WriteEndElement();
            writer.WriteStartElement("FootLeftName"); writer.WriteValue(_fileData.FootLeftName); writer.WriteEndElement();

            writer.WriteStartElement("HeadEnable"); writer.WriteValue(_fileData.HeadEnable); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderCenterEnable"); writer.WriteValue(_fileData.ShoulderCenterEnable); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderRightEnable"); writer.WriteValue(_fileData.ShoulderRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("ElbowRightEnable"); writer.WriteValue(_fileData.ElbowRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("WristRightEnable"); writer.WriteValue(_fileData.WristRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("HandRightEnable"); writer.WriteValue(_fileData.HandRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("ShoulderLeftEnable"); writer.WriteValue(_fileData.ShoulderLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("ElbowLeftEnable"); writer.WriteValue(_fileData.ElbowLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("WristLeftEnable"); writer.WriteValue(_fileData.WristLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("HandLeftEnable"); writer.WriteValue(_fileData.HandLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("SpineEnable"); writer.WriteValue(_fileData.SpineEnable); writer.WriteEndElement();
            writer.WriteStartElement("HipCenterEnable"); writer.WriteValue(_fileData.HipCenterEnable); writer.WriteEndElement();
            writer.WriteStartElement("HipRightEnable"); writer.WriteValue(_fileData.HipRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("KneeRightEnable"); writer.WriteValue(_fileData.KneeRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("AnkleRightEnable"); writer.WriteValue(_fileData.AnkleRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("FootRightEnable"); writer.WriteValue(_fileData.FootRightEnable); writer.WriteEndElement();
            writer.WriteStartElement("HipLeftEnable"); writer.WriteValue(_fileData.HipLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("KneeLeftEnable"); writer.WriteValue(_fileData.KneeLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("AnkleLeftEnable"); writer.WriteValue(_fileData.AnkleLeftEnable); writer.WriteEndElement();
            writer.WriteStartElement("FootLeftEnable"); writer.WriteValue(_fileData.FootLeftEnable); writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
        }
    }
}
