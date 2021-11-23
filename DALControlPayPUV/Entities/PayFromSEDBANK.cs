using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALControlPayPUV.Entities
{
    [NotMapped]
    public class PayFromSEDBANK
    {
        
        public string TypePay { get; set; }
        public string SubTypePay { get; set; }
        public int MonthPay { get; set; }
        public int YearPay { get; set; }
        public decimal AmountPay { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DatePP { get; set; }
        public string NumberPP { get; set; }
        public string SNILS { get; set; }
        public string NameDeliveryOrg { get; set; }
        public int Reester { get; set; }
        public bool haveOZAC { get; set; }
        public string DescriptionCredited { get; set; }


        public override string ToString()
        {
            return this.TypePay + " " + this.SubTypePay + " " + this.AmountPay + " " + DatePP.ToString("dd.MM.yyyy") + " " + NumberPP;
        }
    }
}
