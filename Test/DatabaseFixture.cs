using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace MMB.Mangalam.Web.Test
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Connection = new NpgsqlConnection(ConnectionString.Value);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public IDbConnection Connection { get; private set; }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
