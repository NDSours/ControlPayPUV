using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALControlPayPUV.Entities
{
    public class DocumentPUV
    {
        [Key]
        public long DocumentPUVId { get; set; }

        [ForeignKey("Statement")]
        public long StatementId { get; set; }
        public virtual Statement Statement { get; set; }

        /// <summary>
        /// Наименование документа, с которого взята информация о заявлении
        /// </summary>
        [StringLength(150)]
        public string DocumentPUVName { get; set; }

        /// <summary>
        /// Дата загрузки документа
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateLoaded { get; set; }
    }
}
