﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;
using RadShared.Data;

namespace Rad3.Models.Domian
{
    public partial class dbContext : DbContext
    {
        public static DbProvider DbProvider = DbProvider.Sqlite;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (DbProvider)
            {
                case DbProvider.SqlServer:
                    if (!optionsBuilder.IsConfigured)
                    {
                        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog=Northwind1;Integrated Security=True;MultipleActiveResultSets=True");
                    }
                    break;

                case DbProvider.Sqlite:
                default:
  //                  optionsBuilder.UseSqlite(@"Data Source = /home/runner/Rad/Rad3/northwind.db;");
                    optionsBuilder.UseSqlite(@"Data Source = \northwind.db;");
                    break;
            }
        }
    }
}
