using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.NameTranslation;
using System.IO;

using System;

namespace RadShared.Data
{
    public static class SharedDbContextUtils
    {
        public static DbProvider DbProvider = DbProvider.SqlServer;

//        public static string ConnectionString = "Data Source = ";
        public static string ConnectionString = "Server=.\\SQLEXPRESS;Database=NorthWind1;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static DbContextOptionsBuilder UseGridBlazorDatabase2(this DbContextOptionsBuilder optionsBuilder)
        {
            switch (DbProvider)
            {
                case DbProvider.SqlServer:
                    return optionsBuilder.UseSqlServer(ConnectionString);
                case DbProvider.Sqlite:
                default:
                    var chinookDb = "northwind1.db;";
//                    if (!File.Exists(chinookDb))
//                    {
//                        using var resxStream = typeof(SharedDbContextUtils).Assembly.GetManifestResourceStream(
//                            $"{typeof(SharedDbContextUtils).Namespace}.chinook.db");
//                        using var fs = new FileStream(chinookDb, FileMode.Create, FileAccess.ReadWrite);
//                        resxStream.CopyTo(fs);
//                    }

 //                   Test1();

                    return optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder
                    {
                        DataSource = chinookDb,
                    }.ToString());
            }
        }
        
        public static void ApplyToModelBuilder(DatabaseFacade databaseFacade, ModelBuilder modelBuilder)
        {
            switch (DbProvider)
            {
                case DbProvider.SqlServer:
                    break;
                case DbProvider.Sqlite:
                default:
                    var mapper = new NpgsqlSnakeCaseNameTranslator();
                    foreach (var entity in modelBuilder.Model.GetEntityTypes())
                    {
                        foreach (var property in entity.GetProperties())
                        {
 //                           property.SetColumnName(mapper.TranslateMemberName(property.GetColumnName()));
 //                           property.SetColumnName(mapper.TranslateMemberName(property.GetColumnName()));
                        }

                        entity.SetTableName(mapper.TranslateTypeName(entity.GetTableName()));
                    }
                    break;
            }
        }

        public static void Test1()
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source = chinook2.db3;"))
            {
//                using(SqliteCommand cmd = new SqliteCommand("Select Count(*) From Album", conn))
                using (SqliteCommand cmd = new SqliteCommand("Select * From Album", conn))
//                using (SqliteCommand cmd = new SqliteCommand("Select AlbumId, Title, ArtistId From Album", conn))
                {
                    conn.Open();

                    using(SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
//                            Console.WriteLine(reader["Count(*)"]);
//                            Console.WriteLine(reader["AlbumId "] + " " + reader["Title"] + " " + reader["ArtistId"] + " " + reader["Column1"]);
                            Console.WriteLine(reader["AlbumId"]);
                            Console.WriteLine(reader["Title"]);
                            Console.WriteLine(reader["ArtistId"]);
//                            Console.WriteLine(reader["Column1"]);
                        }
                    }
                    conn.Close();
                }
            }
        }
    }

    public enum DbProvider
    {
        SqlServer,
        Sqlite
    }
}