using Filework_MVVM_B1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filework_MVVM_B1
{
    internal class DBContext : DbContext
    {
        public DbSet<StringModel> Lines { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=localSQliteDb.db");
        }
    }
}
