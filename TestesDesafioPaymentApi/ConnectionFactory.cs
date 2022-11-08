using DesafioPaymentApi.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace TestesDesafioPaymentApi
{
    public class ConnectionFactory : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; 

        public VendaContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<VendaContext>().UseSqlite(connection).Options;

            var context = new VendaContext(option);
            
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
