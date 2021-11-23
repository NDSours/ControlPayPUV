using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALControlPayPUV.Entities
{
    public class Statement
    {
        [Key]
        public long StatementId { get; set; }

        [ForeignKey("Person")]
        public long PersonId { get; set; }
        public virtual Person Person { get; set; } //ссылка на Person

        [StringLength(50)]
        public string GUID { get; set; }

        [StringLength(50)]
        public string RegNumber { get; set; }

        [StringLength(200)]
        public string Vid_Pay { get; set; }

        [StringLength(100)]
        public string Osn_Pay { get; set; }

        /// <summary>
        /// Дата подачи заявления
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateInnings { get; set; }

        /// <summary>
        /// Дата регистрации заявления
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateRegistration { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        /// <summary>
        /// Дата решения
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateResolved { get; set; }

        /// <summary>
        /// Принятое решение
        /// </summary>
        [StringLength(100)]
        public string TypeResolved { get; set; }  

        /// <summary>
        /// Причина отказа
        /// </summary>
        [StringLength(1200)]
        public string RejectionReason { get; set; }

       
        public virtual ICollection<DocumentPUV> DocumentsPUV { get; set; }
        public virtual ICollection<DocumentPay> DocumentsPay { get; set; }

        [NotMapped]
        public virtual ICollection<PayFromSEDBANK> PaysFromSEDBANK { get; set; } // это свойство не будет отражаться на БД


        public Statement()
        {
            DocumentsPUV = new HashSet<DocumentPUV>();
            DocumentsPay = new HashSet<DocumentPay>();
            PaysFromSEDBANK = new HashSet<PayFromSEDBANK>();
        }

        public override string ToString()
        {
            return $"Заявление:{this.RegNumber} Статус: {this.Status}, Назначенное решение: {this.TypeResolved}, Причина отказа: {this.RejectionReason}";
        }

    }
}
