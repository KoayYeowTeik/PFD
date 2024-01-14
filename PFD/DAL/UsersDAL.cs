using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
	public class UsersDAL
	{
		private IConfiguration Configuration { get; }
		private SqlConnection conn;

		public UsersDAL()
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
			Configuration = builder.Build();
			string strConn = Configuration.GetConnectionString(
			"OCBCConnectionString");
			conn = new SqlConnection(strConn);
		}

		public List<Users> GetUsers()
		{
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = @"SELECT * FROM Users OrderBy userID";
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<Users> users = new List<Users>();
			while (reader.Read())
			{
				Users user = new Users
				{
					userID = reader.GetInt32(0),
					userName = reader.GetString(1),
					loginID = reader.GetString(2),
					password = reader.GetString(3),
					contactNumber = reader.GetString(4),
					balance = reader.GetDecimal(5),
					accountNumber = reader.GetString(6),
					limitDay = !reader.IsDBNull(7) ? reader.GetDecimal(7) : (decimal?)null,
					limitWeek = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null,
					limitMonth = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null,
					faceID = !reader.IsDBNull(10) ? reader.GetString(10) : (string?)null,
					digitalToken = !reader.IsDBNull(11) ? reader.GetString(11) : (string?)null
				};
				users.Add(user);
			}
			reader.Close();
			conn.Close();
			return users ;
		}

        public Users getUser(int userid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to get user details
            cmd.CommandText = @"select * from users where userID=@userid";
            cmd.Parameters.AddWithValue("@userid", userid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Declare a new user object
            Users? user = null;
            while (reader.Read())
            {
                user = new Users();
                user.userID = reader.GetInt32(0);
                user.userName = reader.GetString(1);
                user.loginID = reader.GetString(2);
                user.password = reader.GetString(3);
                user.contactNumber = reader.GetString(4);
                user.balance = reader.GetDecimal(5);
                user.accountNumber = reader.GetString(6);
                user.dob = reader.GetDateTime(7);
                user.limitDay = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null;
                user.limitMonth = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null;
                user.limitWeek = !reader.IsDBNull(10) ? reader.GetDecimal(10) : (decimal?)null;
                user.faceID = !reader.IsDBNull(11) ? reader.GetString(11) : (string?)null;
                user.digitalToken = !reader.IsDBNull(12) ? reader.GetString(12) : (string?)null;
            }

            reader.Close();
            conn.Close();
            return user;
        }

        public Users getUserByAccount(string AccountNumber)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to get user details
            cmd.CommandText = @"select * from users where accountNumber=@accountNumber";
            cmd.Parameters.AddWithValue("@accountNumber", AccountNumber);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Declare a new user object
            Users? user = null;
            while (reader.Read())
            {
                user = new Users();
                user.userID = reader.GetInt32(0);
                user.userName = reader.GetString(1);
                user.loginID = reader.GetString(2);
                user.password = reader.GetString(3);
                user.contactNumber = reader.GetString(4);
                user.balance = reader.GetDecimal(5);
                user.accountNumber = reader.GetString(6);
                user.dob = reader.GetDateTime(7);
                user.limitDay = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null;
                user.limitMonth = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null;
                user.limitWeek = !reader.IsDBNull(10) ? reader.GetDecimal(10) : (decimal?)null;
                user.faceID = !reader.IsDBNull(11) ? reader.GetString(11) : (string?)null;
                user.digitalToken = !reader.IsDBNull(12) ? reader.GetString(12) : (string?)null;
            }

            reader.Close();
            conn.Close();
            return user;
        }

        public Users authenticateUser(string LoginID, string Password)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to get user details
            cmd.CommandText = @"select * from users where LoginID=@LoginID AND Password = @Password";
            cmd.Parameters.AddWithValue("@LoginID", LoginID);
            cmd.Parameters.AddWithValue("@Password", Password);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            // Declare a new user object
            Users? user = null;
            while (reader.Read())
            {
                user = new Users();
                user.userID = reader.GetInt32(0);
                user.userName = reader.GetString(1);
                user.loginID = reader.GetString(2);
                user.password = reader.GetString(3);
                user.contactNumber = reader.GetString(4);
                user.balance = reader.GetDecimal(5);
                user.accountNumber = reader.GetString(6);
                user.dob = reader.GetDateTime(7);
                user.limitDay = !reader.IsDBNull(8) ? reader.GetDecimal(8) : (decimal?)null;
                user.limitMonth = !reader.IsDBNull(9) ? reader.GetDecimal(9) : (decimal?)null;
                user.limitWeek = !reader.IsDBNull(10) ? reader.GetDecimal(10) : (decimal?)null;
                user.faceID = !reader.IsDBNull(11) ? reader.GetString(11) : (string?)null;
                user.digitalToken = !reader.IsDBNull(12) ? reader.GetString(12) : (string?)null;
            }

            reader.Close();
            conn.Close();
            return user;
        }

        // Get all accounts for a user
        public List<bankAccount> GetUserBankAccount(int userid)
		{
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to get user details
            cmd.CommandText = @"select * from bankAccount where userID=@userid";
            cmd.Parameters.AddWithValue("@userid", userid);
            //Open a database connection
            conn.Open();
           SqlDataReader reader = cmd.ExecuteReader();
            List<bankAccount> bankAccountList = new List<bankAccount>();

            while (reader.Read())
            {
                // Create bankAccount Object
                bankAccount bankAccount = new bankAccount
                {
                    accountID = reader.GetInt32(0),
                    accountNumber = reader.GetString(1),
                    balance = reader.GetDecimal(2),
                    userID = reader.GetInt32(3),
                };
                //Add bankAccount into A list
                bankAccountList.Add(bankAccount);
            }
            reader.Close();
            conn.Close();
            return bankAccountList;
        }

        public int CreateAccount(Users user)
        {
            SqlCommand cmd = conn.CreateCommand();
            string generatedNumber;
            do {
                generatedNumber = GenerateRand();
            } 
            while (
                CheckUnique(generatedNumber)
            );

            cmd.CommandText = "INSERT INTO dbo.Users (userName, LoginID, Password, contactNumber, balance, accountNumber, dob, limitDay, limitMonth, limitWeek, faceid, digitalToken) " +
    "OUTPUT INSERTED.UserID " +
    "VALUES (@UserName, @LoginID, @Password, @ContactNumber, @Balance, @AccountNumber, @DOB, @LimitDay, @LimitMonth, @LimitWeek, @FaceId, @DigitalToken);";

            cmd.Parameters.AddWithValue("@UserName", user.userName);
            cmd.Parameters.AddWithValue("@LoginID", user.loginID);
            cmd.Parameters.AddWithValue("@Password", user.password);
            cmd.Parameters.AddWithValue("@ContactNumber", user.contactNumber);
            cmd.Parameters.AddWithValue("@Balance", user.balance);
            cmd.Parameters.AddWithValue("@AccountNumber", generatedNumber);
            cmd.Parameters.AddWithValue("@DOB", user.dob);
            cmd.Parameters.AddWithValue("@LimitDay", (object)user.limitDay ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LimitMonth", (object)user.limitMonth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LimitWeek", (object)user.limitWeek ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FaceId", (object)user.faceID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DigitalToken", (object)user.digitalToken ?? DBNull.Value);
            conn.Open();
            int userId = (int)cmd.ExecuteScalar();
            conn.Close();
            return userId;
        }
        private string GenerateRand()
        {
            string rand = new Random().Next(100000000, 999999999).ToString();
            string lastChar = new Random().Next(1, 9).ToString();
            return rand + lastChar;
        }

        private bool CheckUnique(string accountNumber)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Users WHERE accountNumber = @AccountNumber";
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count > 0; //If count > 0 it will be true and generate a new number
            }
        }
    }
}
