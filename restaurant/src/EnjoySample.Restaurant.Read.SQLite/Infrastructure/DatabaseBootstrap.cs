using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using EnjoySample.Restaurant.Read.Models;

namespace EnjoySample.Restaurant.Read.SQLite.Infrastructure
{
    public class DatabaseBootstrap
    {
        private static readonly SqlCreateBuilder _sqlCreateBuilder;
        private static readonly List<Type> _dtos = new List<Type>
        {
            typeof(TabModel),
            typeof(OrderItemModel)
        };

        static DatabaseBootstrap()
        {
            _sqlCreateBuilder = new SqlCreateBuilder();
        }

        public static void Create(string databaseFilename, bool force = false)
        {
            if (string.IsNullOrWhiteSpace(databaseFilename)) throw new ArgumentNullException(nameof(databaseFilename));
            
            DropAndCreate(databaseFilename, force);

            var sqLiteConnection = new SQLiteConnection($"Data Source={databaseFilename}");

            sqLiteConnection.Open();

            using (DbTransaction dbTrans = sqLiteConnection.BeginTransaction())
            {
                using (DbCommand sqLiteCommand = sqLiteConnection.CreateCommand())
                {
                    foreach (var dto in _dtos)
                    {
                        sqLiteCommand.CommandText = _sqlCreateBuilder.CreateSqlCreateStatementFromDto(dto);
                        sqLiteCommand.ExecuteNonQuery();
                    }
                }
                dbTrans.Commit();
            }

            sqLiteConnection.Close();
        }

        private static void DropAndCreate(string databaseFilename, bool force)
        {
            if (force)
            {
                if (File.Exists(databaseFilename))
                {
                    File.Delete(databaseFilename);
                }
            }

            if (!File.Exists(databaseFilename))
            {
                SQLiteConnection.CreateFile(databaseFilename);
            }
        }
    }
}
