using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ControlPayPUV.Loaders.PUV
{
    public class SettingPUV
    {
        public int Column_GUID { get; set; } = -1;
        public int Column__SNILS { get; set; } = -1;
        public int Column_Surname { get; set; } = -1;
        public int Column_Name { get; set; } = -1;
        public int Column_Patr { get; set; } = -1;
        public int Column_DateBirthday { get; set; } = -1;
        public int Column_PhoneNumber { get; set; } = -1;
        public int Column_Vid_Pay { get; set; } = -1;
        public int Column_Osnovanie_Pay { get; set; } = -1;
        public int Column_DateInnings { get; set; } = -1;
        public int Column_DateRegistration { get; set; } = -1;
        public int Column_RegNumber { get; set; } = -1;
        public int Column_Status { get; set; } = -1;
        public int Column_DateResolved { get; set; } = -1;
        public int Column_TypeResolved { get; set; } = -1;
        public int Column_RejectionReason { get; set; } = -1;
       

        public void LoadFromXmlSerializer()
        {
            string filename = Environment.CurrentDirectory + "\\setting.xml";
            XmlSerializer xmlFormat = new XmlSerializer(typeof(SettingPUV));
            using (Stream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SettingPUV tempSetting = (SettingPUV)xmlFormat.Deserialize(fStream);
           
                this.Column_DateBirthday = tempSetting.Column_DateBirthday;
                this.Column_PhoneNumber = tempSetting.Column_PhoneNumber;
                this.Column_DateInnings = tempSetting.Column_DateInnings;
                this.Column_DateRegistration = tempSetting.Column_DateRegistration;
                this.Column_DateResolved = tempSetting.Column_DateResolved;
                this.Column_TypeResolved = tempSetting.Column_TypeResolved;
                this.Column_GUID = tempSetting.Column_GUID;
                this.Column_Name = tempSetting.Column_Name;
                this.Column_Osnovanie_Pay = tempSetting.Column_Osnovanie_Pay;
                this.Column_Patr = tempSetting.Column_Patr;
                this.Column_RejectionReason = tempSetting.Column_RejectionReason;
                this.Column_RegNumber = tempSetting.Column_RegNumber;
                this.Column_Status = tempSetting.Column_Status;
                this.Column_Surname = tempSetting.Column_Surname;
                this.Column_Vid_Pay = tempSetting.Column_Vid_Pay;
                this.Column__SNILS = tempSetting.Column__SNILS;
            }
        }

        public void SaveToXmlSerializer()
        {
            string filename = Environment.CurrentDirectory + "\\setting.xml";
            XmlSerializer xmlFormat = new XmlSerializer(typeof(SettingPUV));
            using (Stream fStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }
        }


    }
}
