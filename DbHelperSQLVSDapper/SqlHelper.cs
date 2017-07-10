using System.Data.SqlClient;

namespace DbHelperSQLVSDapper
{
    internal static class SqlHelper
    {
        public static readonly string ConnectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=tempdb;Integrated Security=True;uid=sa;pwd=123456";

        public static  string CreateStatement =
            @"INSERT dbo.Customers (CustomerID, CompanyName) SELECT @CustomerId, @companyName";

      //  public static readonly string ReadStatement = @"SELECT * FROM dbo.Customers WHERE CustomerId = @CustomerId";
public static readonly string ReadStatement = @"SELECT top 500  * FROM dbo.Customers ";
        public static readonly string UpdateStatement =
            @"UPDATE dbo.Customers SET CompanyName = @companyName WHERE CustomerId = @CustomerId";

        public static readonly string DeleteStatement = @"DELETE FROM dbo.Customers WHERE CustomerId = @CustomerId";

       


        public static readonly string TestId = "TEST";
        public static readonly string InsertCompanyName = "Inserted company";
        public static readonly string UpdateCompanyName = "Updated company";

        internal static void EnsureDbSetup()
        {
            using (var cnn = GetOpenConnection())
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandText = @"
    if (OBJECT_ID('Customers') is null)
    begin
    CREATE TABLE [dbo].[Customers](
	    [CustomerID] [nchar](5) NOT NULL,
	    [CompanyName] [nvarchar](40) NOT NULL,
	    [ContactName] [nvarchar](30) NULL,
	    [ContactTitle] [nvarchar](30) NULL,
	    [Address] [nvarchar](60) NULL,
	    [City] [nvarchar](15) NULL,
	    [Region] [nvarchar](15) NULL,
	    [PostalCode] [nvarchar](10) NULL,
	    [Country] [nvarchar](15) NULL,
	    [Phone] [nvarchar](24) NULL,
	    [Fax] [nvarchar](24) NULL)
    end
    ";
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }
        }

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}