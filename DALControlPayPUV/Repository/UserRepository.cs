using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALControlPayPUV.Entities;

namespace DALControlPayPUV.Repository
{
    public class UserRepository
    {
        public IEnumerable<User> Users { get; set; }
        public UserRepository()
        {
            using (var context = new MyDBContext()) // подключаемся к бд
            {
                Users = context.Users.ToArray();// вытягиваем данные о пользователях из бд
            }
        }
    }
}
