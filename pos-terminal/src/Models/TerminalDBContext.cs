using Microsoft.EntityFrameworkCore;

namespace EatAndDrink.Models
{
    public class TerminalDBContext : DbContext
    {
        private readonly String _connectionString = "Data Source=DESKTOP-PFAQS2O;Initial Catalog=pos_terminal;Persist Security Info=True;User ID = sa;Password=3922;";
        public DbSet<Terminal> Terminal { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(_connectionString);
    }
}
