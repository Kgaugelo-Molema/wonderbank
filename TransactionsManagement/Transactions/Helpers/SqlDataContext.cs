using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Transactions.Models;

namespace Transactions.Helpers
{
    public class SqlDataContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public SqlDataContext(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<TransactionsModel> Transactions { get; set; }
    }
}
