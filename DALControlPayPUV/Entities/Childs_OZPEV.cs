using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALControlPayPUV.Entities
{
    [Table("Childs_OZPEV")]
    public class Childs_OZPEV
    {
        [Key]
        public long ID { get; set; }

        [ForeignKey("DocumentPay")]
        public long DocumentPayId { get; set; }
        public virtual DocumentPay DocumentPay { get; set; }

        [StringLength(150)]
        public string Surname { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Patr { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? act_Data { get; set; }

        [StringLength(50)]
        public string act_Number { get; set; }

        [StringLength(1000)]
        public string act_NameZaks { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateBirthday { get; set; }

        [StringLength(20)]
        public string SNILS { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateStartPay { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateEndPay { get; set; }


    }
}
