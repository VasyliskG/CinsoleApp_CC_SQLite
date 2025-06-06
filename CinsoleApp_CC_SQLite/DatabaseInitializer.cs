using System;
using System.Data.SQLite;
using System.IO;

namespace CinsoleApp_CC_SQLite;

public static class DatabaseInitializer
{
    public static void CreateDatabase()
    {
        string connectionString = $"Data Source=app.db;Version=3;";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createTableQueryUsers = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FristName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Email TEXT NOT NULL UNIQUE
                    );";

            string createTableQueryProducts = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price REAL
                    );";
            
            using (var command = new SQLiteCommand(createTableQueryUsers, connection))
            {
                command.ExecuteNonQuery();
            }
            
            using (var command = new SQLiteCommand(createTableQueryProducts, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}   