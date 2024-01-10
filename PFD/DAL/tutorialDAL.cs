using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
    public class tutorialDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public tutorialDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }

        public List<tutorial> GetTutorial()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from tutorial";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<tutorial> tutorialList = new List<tutorial>();
            while (reader.Read())
            {
                tutorial tutorial = new tutorial
                {
                    tutID = reader.GetInt32(0),
                    tutName = reader.GetString(1),
                    tutImg = reader.GetString(2),
                    tutCat = reader.GetString(3),
                };
                tutorialList.Add(tutorial);
            }
            reader.Close();
            conn.Close();
            return tutorialList;
        }

        // Get the tutorial guide steps
        public List<guide> GetGuide(int tutID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from guide where tutID = @tutID";
            cmd.Parameters.AddWithValue("@tutID", tutID);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<guide> guideList = new List<guide>();
            while (reader.Read())
            {
                guide guide = new guide
                {
                    stepNum = reader.GetInt32(1),
                    video = reader.GetString(2),
                    img = reader.GetString(3),
                    textContent = reader.GetString(4),
                    tutID = reader.GetInt32(5),
                };
                guideList.Add(guide);
            }
            reader.Close();
            conn.Close();
            return guideList;
        }

    }
}
