using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALControlPayPUV.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patr { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public int Role { get; set; } //1 - администратор, 2 - оператор
    }
}
