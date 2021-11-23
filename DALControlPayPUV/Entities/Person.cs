using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALControlPayPUV.Entities
{
    public class Person
    {
        [Key]
        public long PersonId { get; set; }

        [StringLength(20)]
        public string SNILS { get; set; }

        [StringLength(150)]
        public string Surname { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Patr { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateBirthday { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Statement> Statements { get; set; }  

        public Person()
        {
            Statements = new HashSet<Statement>();
        }

        public string GetSNILS()
        {
            string temp = this.SNILS;
            if (temp.Length == 10)
                temp = temp.Insert(0, "0");

            temp = temp.Insert(3, "-");
            temp = temp.Insert(7, "-");
            temp = temp.Insert(11, " ");

            return temp;
        }

    }
}
