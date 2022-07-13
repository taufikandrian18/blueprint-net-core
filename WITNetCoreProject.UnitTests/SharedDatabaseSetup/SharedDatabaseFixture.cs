using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.UnitTests.SharedDatabaseSetup {

    // all this statement is about making an environment for in memory database using sql lite
    public class SharedDatabaseFixture : IDisposable {

        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        // declare db name for in memory database
        private string dbName = "IntegrationTestsDatabase.db";
        public SharedDatabaseFixture() {

            Connection = new SqliteConnection($"Filename={dbName}");

            Seed();

            Connection.Open();
        }

        //open connection
        public DbConnection Connection { get; }

        // mirroring object from repository context to create in memory database using sql lite

#nullable enable

        public RepositoryContext CreateContext(DbTransaction? transaction = null) {

#nullable disable

            var context = new RepositoryContext(new DbContextOptionsBuilder<RepositoryContext>().UseSqlite(Connection).Options);

            if (transaction != null) {

                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        //method seed data from in memory database
        private void Seed() {

            lock (_lock) {

                if (!_databaseInitialized) {

                    using (var context = CreateContext()) {

                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        DatabaseSetup.SeedData(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }

        // dispose data from in memory database
        public void Dispose() => Connection.Dispose();
    }
}
