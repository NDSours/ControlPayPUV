using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NPOI.SS.UserModel;

namespace ControlPayPUV.Loaders.PUV
{
    public class HeaderParser
    {
        private SettingPUV Setting = new SettingPUV();

        public string Column_GUID { get; set; } = "GUID заявления";
        public string Column__SNILS { get; set; } = "СНИЛС заявителя";
        public string Column_Surname { get; set; } = "Фамилия заявителя";
        public string Column_Name { get; set; } = "Имя заявителя";
        public string Column_Patr { get; set; } = "Отчество заявителя";
        public string Column_DateBirthday { get; set; } = "Дата рождения";
        public string Column_PhoneNumber { get; set; } = "Телефон заявителя";
        public string Column_Vid_Pay { get; set; } = "Вид выплаты";
        public string Column_Osnovanie_Pay { get; set; } = "Основание назначения";
        public string Column_DateInnings { get; set; } = "Дата подачи заявления";
        public string Column_DateRegistration { get; set; } = "Дата регистрации заявления";
        public string Column_RegNumber { get; set; } = "Регистрационный номер заявления";
        public string Column_Status { get; set; } = "Статус заявления";
        public string Column_DateResolved { get; set; } = "Дата решения";
        public string Column_TypeResolved { get; set; } = "Принятое решение";
        public string Column_RejectionReason { get; set; } = "Причина отказа";

        public HeaderParser()
        { }

        public SettingPUV ParsePUV(ISheet sheet)
        {
            var currentrow = sheet.GetRow(0);
            for (int cell = 0; cell <= currentrow.LastCellNum; cell++)
            {
                var currentCell = currentrow.GetCell(cell);
                if (currentCell != null)
                {
                    string value = default;
                    if (currentCell.CellType == CellType.String)
                        value = currentCell.StringCellValue;
                    else if (currentCell.CellType == CellType.Numeric)
                        value = currentCell.NumericCellValue.ToString();

                    if (this.Column_DateBirthday == value)
                        Setting.Column_DateBirthday = cell;
                    if (this.Column_DateInnings == value)
                        Setting.Column_DateInnings = cell;
                    if(this.Column_DateRegistration == value)
                        Setting.Column_DateRegistration = cell;
                    if(this.Column_DateResolved == value)
                        Setting.Column_DateResolved = cell;
                    if(this.Column_GUID == value)
                        Setting.Column_GUID = cell;
                    if (this.Column_Name == value)
                        Setting.Column_Name = cell;
                    if (this.Column_Osnovanie_Pay == value)
                        Setting.Column_Osnovanie_Pay = cell;
                    if (this.Column_Patr == value)
                        Setting.Column_Patr = cell;
                    if (this.Column_PhoneNumber == value)
                        Setting.Column_PhoneNumber = cell;
                    if (this.Column_RegNumber == value)
                        Setting.Column_RegNumber = cell;
                    if (this.Column_RejectionReason == value)
                        Setting.Column_RejectionReason = cell;
                    if (this.Column_Status == value)
                        Setting.Column_Status = cell;
                    if (this.Column_Surname == value)
                        Setting.Column_Surname = cell;
                    if (this.Column_TypeResolved == value)
                        Setting.Column_TypeResolved = cell;
                    if (this.Column_Vid_Pay == value)
                        Setting.Column_Vid_Pay = cell;
                    if (this.Column__SNILS == value)
                        Setting.Column__SNILS = cell;

                }
            }

            //Проверка, все ли значения были считаны
            if (Setting.Column_DateBirthday == -1)
                throw new Exception("Не найдена колонка: "+this.Column_DateBirthday);
            if (Setting.Column_DateResolved == -1)
                throw new Exception("Не найдена колонка: "+this.Column_DateResolved);
            if (Setting.Column_PhoneNumber == -1)
                throw new Exception("Не найдена колонка: " + this.Column_PhoneNumber);
            if (Setting.Column_DateInnings == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateInnings);
            if (Setting.Column_DateRegistration == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateRegistration);
            if (Setting.Column_TypeResolved == -1)
                throw new Exception("Не найдена колонка: " + this.Column_TypeResolved);
            if (Setting.Column_GUID == -1)
                throw new Exception("Не найдена колонка: " + this.Column_GUID);
            if (Setting.Column_Name == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Name);
            if (Setting.Column_Osnovanie_Pay == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Osnovanie_Pay);
            if (Setting.Column_Patr == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Patr);
            if (Setting.Column_RejectionReason == -1)
                throw new Exception("Не найдена колонка: " + this.Column_RejectionReason);
            if(Setting.Column_RegNumber == -1)
                throw new Exception("Не найдена колонка: "+this.Column_RegNumber);
            if (Setting.Column_Status == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Status);
            if (Setting.Column_Surname == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Surname);
            if (Setting.Column_Vid_Pay == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Vid_Pay);
            if (Setting.Column__SNILS == -1)
                throw new Exception("Не найдена колонка: " + this.Column__SNILS);

            return Setting;
        }

        public SettingPUV ParsePUV(string[] column)
        {
            int cell = 0;
            foreach(var item in column)
            {
                if (this.Column_DateBirthday == item)
                    Setting.Column_DateBirthday = cell;
                if (this.Column_DateInnings == item)
                    Setting.Column_DateInnings = cell;
                if (this.Column_DateRegistration == item)
                    Setting.Column_DateRegistration = cell;
                if (this.Column_DateResolved == item)
                    Setting.Column_DateResolved = cell;
                if (this.Column_GUID == item)
                    Setting.Column_GUID = cell;
                if (this.Column_Name == item)
                    Setting.Column_Name = cell;
                if (this.Column_Osnovanie_Pay == item)
                    Setting.Column_Osnovanie_Pay = cell;
                if (this.Column_Patr == item)
                    Setting.Column_Patr = cell;
                if (this.Column_PhoneNumber == item)
                    Setting.Column_PhoneNumber = cell;
                if (this.Column_RegNumber == item)
                    Setting.Column_RegNumber = cell;
                if (this.Column_RejectionReason == item)
                    Setting.Column_RejectionReason = cell;
                if (this.Column_Status == item)
                    Setting.Column_Status = cell;
                if (this.Column_Surname == item)
                    Setting.Column_Surname = cell;
                if (this.Column_TypeResolved == item)
                    Setting.Column_TypeResolved = cell;
                if (this.Column_Vid_Pay == item)
                    Setting.Column_Vid_Pay = cell;
                if (this.Column__SNILS == item)
                    Setting.Column__SNILS = cell;

                cell++;
            }

            //Проверка, все ли значения были считаны
            if (Setting.Column_DateBirthday == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateBirthday);
            if (Setting.Column_DateResolved == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateResolved);
            if (Setting.Column_PhoneNumber == -1)
                throw new Exception("Не найдена колонка: " + this.Column_PhoneNumber);
            if (Setting.Column_DateInnings == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateInnings);
            if (Setting.Column_DateRegistration == -1)
                throw new Exception("Не найдена колонка: " + this.Column_DateRegistration);
            if (Setting.Column_TypeResolved == -1)
                throw new Exception("Не найдена колонка: " + this.Column_TypeResolved);
            if (Setting.Column_GUID == -1)
                throw new Exception("Не найдена колонка: " + this.Column_GUID);
            if (Setting.Column_Name == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Name);
            if (Setting.Column_Osnovanie_Pay == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Osnovanie_Pay);
            if (Setting.Column_Patr == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Patr);
            if (Setting.Column_RejectionReason == -1)
                throw new Exception("Не найдена колонка: " + this.Column_RejectionReason);
            if (Setting.Column_RegNumber == -1)
                throw new Exception("Не найдена колонка: " + this.Column_RegNumber);
            if (Setting.Column_Status == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Status);
            if (Setting.Column_Surname == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Surname);
            if (Setting.Column_Vid_Pay == -1)
                throw new Exception("Не найдена колонка: " + this.Column_Vid_Pay);
            if (Setting.Column__SNILS == -1)
                throw new Exception("Не найдена колонка: " + this.Column__SNILS);

            return Setting;
        }

        public void LoadFromXmlSerializer()
        {
            string filename = Environment.CurrentDirectory + "\\settingHeaders.xml";
            XmlSerializer xmlFormat = new XmlSerializer(typeof(HeaderParser));
            using (Stream fStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                HeaderParser tempSetting = (HeaderParser)xmlFormat.Deserialize(fStream);

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
            string filename = Environment.CurrentDirectory + "\\settingHeaders.xml";
            XmlSerializer xmlFormat = new XmlSerializer(typeof(HeaderParser));
            using (Stream fStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }
        }
    }
}
