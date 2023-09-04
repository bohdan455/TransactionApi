using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransactionStatus> TransactionStatuses { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
    }
}
