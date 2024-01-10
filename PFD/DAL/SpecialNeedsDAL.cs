using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
	public class SpecialNeedsDAL
	{
		private IConfiguration Configuration { get; }
		private SqlConnection conn;

		public SpecialNeedsDAL()
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
			Configuration = builder.Build();
			string strConn = Configuration.GetConnectionString(
			"OCBCConnectionString");
			conn = new SqlConnection(strConn);
		}

		public List<SpecialNeeds> GetSpecialNeeds()
		{
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = @"SELECT * FROM SpecialNeeds OrderBy recordID";
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<SpecialNeeds> specialNeeds = new List<SpecialNeeds>();
			while (reader.Read())
			{
				SpecialNeeds specialNeed = new SpecialNeeds 
				{ 
					recordID = reader.GetInt32(0),
					userID = reader.GetInt32(1),
					helperID = reader.GetInt32(2),
					type = reader.GetString(3)
				};
				specialNeeds.Add(specialNeed);
			}
			reader.Close();
			conn.Close();
			return specialNeeds;
		}

        public void CreateSpecialNeeds(SpecialNeeds specialNeeds)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO dbo.SpecialNeeds (userID, helperID, type) " + "VALUES (@userID, @helperID, @type)";

            cmd.Parameters.AddWithValue("@userID", specialNeeds.userID);
            cmd.Parameters.AddWithValue("@helperID", specialNeeds.helperID);
            cmd.Parameters.AddWithValue("@type", specialNeeds.type);
          
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            
        }
    }
}
