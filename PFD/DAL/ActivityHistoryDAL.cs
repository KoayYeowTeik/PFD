using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
	public class ActivityHistoryDAL
	{
		private IConfiguration Configuration { get; }
		private SqlConnection conn;

		public ActivityHistoryDAL()
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
			Configuration = builder.Build();
			string strConn = Configuration.GetConnectionString(
			"OCBCConnectionString");
			conn = new SqlConnection(strConn);
		}

		public List<ActivityHistory> GetActivityHistories()
		{
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = @"SELECT * FROM ActivityHistory OrderBy recordID";
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<ActivityHistory> activityHistories = new List<ActivityHistory>();
			while (reader.Read())
			{
				ActivityHistory activityHistory = new ActivityHistory
				{
					recordID = reader.GetInt32(0),
					activityTime = reader.GetDateTime(1),
					description = reader.GetString(2),
					userID = reader.GetInt32(3)
				};
			    activityHistories.Add(activityHistory);
			}
			reader.Close();
			conn.Close();
			return activityHistories;
		}
		
	}
}
