using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
    public class bankAccountDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public bankAccountDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }
        public void CreateBankAccount(bankAccount bankAccount)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO dbo.bankAccount (accountNumber, balance, userID) " +
                                  "VALUES (@AccountNumber, @Balance, @UserID)";

            cmd.Parameters.AddWithValue("@AccountNumber", bankAccount.accountNumber);
            cmd.Parameters.AddWithValue("@Balance", bankAccount.balance);
            cmd.Parameters.AddWithValue("@UserID", bankAccount.userID);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
