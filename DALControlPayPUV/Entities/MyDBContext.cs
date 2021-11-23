using System.Data.Entity;

namespace DALControlPayPUV.Entities
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("DefaultConnection") { }

        public DbSet<Childs_OZPEV> Childs { get; set; }
        public DbSet<DocumentPay> DocumentsPay { get; set; }
        public DbSet<DocumentPUV> DocumentsPUV { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Person>() // экземпляр Person может иметь несколько заявлений
               .HasMany(c => c.Statements)
               .WithRequired(o => o.Person)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Statement>() //экземпляр Statement может иметь несколько документов PUV
                .HasMany(s => s.DocumentsPUV)
                .WithRequired(s => s.Statement)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Statement>() //экземпляр Statement может иметь несколько документов по выплате
                .HasMany(s => s.DocumentsPay)
                .WithRequired(s => s.Statement)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<DocumentPay>() //экземпляр DocumentPay может иметь несколько ссылок на детей
                .HasMany(d => d.Childs)
                .WithRequired(d => d.DocumentPay)
                .WillCascadeOnDelete(true);
        }

    }
}
