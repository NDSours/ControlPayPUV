using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALControlPayPUV
{
    public class Filter
    {
        public Files HavingFiles { get; set; } = Files.All;
    }

    public enum Files
    {
        All,
        HaveInformationOnPay,
        NotHaveInformationOnPay
    }
}
