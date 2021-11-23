namespace DALControlPayPUV.Migrations
{
    using MySql.Data.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DALControlPayPUV.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<DALControlPayPUV.Entities.MyDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
        }

        protected override void Seed(DALControlPayPUV.Entities.MyDBContext context)
        {
            if(context.Users.Count() == 0)
            {
                // создаем пользователей для просмотра
                User admin = new User
                {
                    Login = "Admin",
                    Password = "admin",
                    Name = "Дмитрий",
                    Surname = "Непомнящий",
                    Patr = "Сергеевич",
                    Position = "Главный специалист-эксперт",
                    Role = 1,
                    Department = "Группа эксплуатации и сопровождения информационных подсистем"
                };
                User oper = new User
                {
                    Login = "Operator",
                    Password = "operator",
                    Name = "Дмитрий",
                    Surname = "Непомнящий",
                    Patr = "Сергеевич",
                    Position = "Главный специалист-эксперт",
                    Role = 2,
                    Department = "Группа эксплуатации и сопровождения информационных подсистем"
                };
                context.Users.Add(admin);
                context.Users.Add(oper);
                context.SaveChanges();
            }
          
        }
    }
}
