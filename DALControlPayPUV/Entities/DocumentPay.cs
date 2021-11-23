using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALControlPayPUV.Entities
{
    public class DocumentPay
    {
        [Key]
        public long DocumentPayId { get; set; }

        [ForeignKey("Statement")]
        public long StatementId { get; set; }
        public virtual Statement Statement { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        [StringLength(150)]
        public string DocumentName { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        [StringLength(20)]
        public string TypePay { get; set; }

        /// <summary>
        /// Фамилия заявителя
        /// </summary>
        [StringLength(150)]
        public string z_Surname { get; set; }

        /// <summary>
        /// Имя заявителя
        /// </summary>
        [StringLength(150)]
        public string z_Name { get; set; }

        /// <summary>
        /// Отчество заявителя
        /// </summary>
        [StringLength(150)]
        public string z_Patr { get; set; }

        /// <summary>
        /// СНИЛС заявителя
        /// </summary>
        [StringLength(20)]
        public string z_SNILS { get; set; }

        /// <summary>
        /// Тип удостоверяющего документа
        /// </summary>
        [StringLength(100)]
        public string ud_Type { get; set; }

        /// <summary>
        /// Серия удостоверяющего документа
        /// </summary>
        [StringLength(30)]
        public string ud_Seriya { get; set; }

        /// <summary>
        /// Номер удостоверяющего документа
        /// </summary>
        [StringLength(50)]
        public string ud_Number { get; set; }

        /// <summary>
        /// Дата выдачи удостоверяющего документа
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ud_DataVidachi { get; set; }

        /// <summary>
        /// Кем выдан удостоверяющий документ
        /// </summary>
        [StringLength(200)]
        public string ud_Vidan { get; set; }

        /// <summary>
        /// Код организации выдававшей удостоверяющий документ
        /// </summary>
        [StringLength(20)]
        public string ud_Kod { get; set; }

        /// <summary>
        /// Адресс
        /// </summary>
        [StringLength(1000)]
        public string Address { get; set; }

        /// <summary>
        /// Телефонный номер
        /// </summary>
        [StringLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Дата первой выплаты
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateFirstPay { get; set; }

        /// <summary>
        /// Дата последней выплаты
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateLastPay { get; set; }

        /// <summary>
        /// Вид операции
        /// </summary>
        [StringLength(20)]
        public string Operation { get; set; }

        /// <summary>
        /// Сумма выплаты
        /// </summary>
        public decimal AmountPay { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        [StringLength(1000)]
        public string NameDeliveryOrg { get; set; }

        /// <summary>
        /// БИК банка
        /// </summary>
        [StringLength(20)]
        public string BIKDeliveryOrg { get; set; }

        /// <summary>
        /// КС счет банка
        /// </summary>
        [StringLength(100)]
        public string KCSchetDeliveryOrg { get; set; }

        /// <summary>
        /// Фамилия получателя
        /// </summary>
        [StringLength(150)]
        public string p_Surname { get; set; }

        /// <summary>
        /// Имя получателя
        /// </summary>
        [StringLength(150)]
        public string p_Name { get; set; }

        /// <summary>
        /// Отчество получателя
        /// </summary>
        [StringLength(150)]
        public string p_Patr { get; set; }

        /// <summary>
        /// Счет получателя
        /// </summary>
        [StringLength(50)]
        public string p_AccountNumber { get; set; }

        /// <summary>
        /// GUID заявления
        /// </summary>
        [StringLength(100)]
        public string GUID { get; set; }

        [StringLength(100)]
        public string GlobalID { get; set; }

        /// <summary>
        /// Регистрационный номер заявления
        /// </summary>
        [StringLength(100)]
        public string RegNumber { get; set; }

        /// <summary>
        /// Дата начала выплаты
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateStartPay { get; set; }

        /// <summary>
        /// Дата окончания выплаты
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateEndPay { get; set; }

        /// <summary>
        /// Путь для открытия файла из архива
        /// </summary>
        public string ArhivefullPath { get; set; }

        /// <summary>
        /// Ссылка на детей
        /// </summary>
        public virtual ICollection<Childs_OZPEV> Childs { get; set; }

        public DocumentPay()
        {
            Childs = new HashSet<Childs_OZPEV>();
        }

        public override bool Equals(object obj)
        {
            var tempobj = (DocumentPay)obj;
            if (this.Operation == tempobj.Operation
                && this.DateStartPay == tempobj.DateStartPay
                && this.DateEndPay == tempobj.DateEndPay)
                return true;
            else
                return false;
        }
    }
}
