﻿using System;
using System.IO;
using ServerSyncMobileApp.Persistence;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ServerSyncMobileApp.Persistence
{
    public class SQLiteDb:ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "MySQLite.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}