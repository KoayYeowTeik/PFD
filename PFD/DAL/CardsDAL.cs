using Emgu.CV.Features2D;
using Microsoft.Data.SqlClient;
using PFD_ASG.Models;

namespace PFD_ASG.DAL
{
	public class CardsDAL
	{
		private IConfiguration Configuration { get; }
		private SqlConnection conn;

		public CardsDAL()
		{
			var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");
			Configuration = builder.Build();
			string strConn = Configuration.GetConnectionString(
			"OCBCConnectionString");
			conn = new SqlConnection(strConn);
		}

		public List<Cards> GetCards()
		{
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = @"SELECT * FROM Cards OrderBy userID";
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();
			List<Cards> cards = new List<Cards>();
			while (reader.Read())
			{
				Cards card = new Cards
				{
					userID = reader.GetInt32(0),
					cardNumber = reader.GetString(1),
					cVV = reader.GetString(2),
					expDate = reader.GetDateTime(3),
					cardType = reader.GetString(4),
					billingAddress = reader.GetString(5)
				};
				cards.Add(card);
			}
			reader.Close();
			conn.Close();
			return cards;
		}

        public void CreateCard(Cards card)
        {
            SqlCommand cmd = conn.CreateCommand();
            string generatedCard;
            do
            {
                generatedCard = GenerateRandCard();
            }
            while (
                CheckUniqueCard(generatedCard)
            );

            string generatedcVV;
            do
            {
                generatedcVV = GenerateRandcVV();
            }
            while (
                CheckUniquecVV(generatedcVV)
            );

            cmd.CommandText = "INSERT INTO dbo.Cards (userID, cardNumber, cVV, expDate, cardType, billingAddress) " +
                  "VALUES (@UserID, @CardNumber, @CVV, @ExpDate, @CardType, @BillingAddress);";

            cmd.Parameters.AddWithValue("@UserID", card.userID);
            cmd.Parameters.AddWithValue("@CardNumber", generatedCard);
            cmd.Parameters.AddWithValue("@CVV", generatedcVV);
            cmd.Parameters.AddWithValue("@ExpDate", DateTime.Now.AddYears(5).ToString("MM/yy"));
            cmd.Parameters.AddWithValue("@CardType", card.cardType);
            cmd.Parameters.AddWithValue("@BillingAddress", card.billingAddress);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            
        }

        private string GenerateRandCard()
        {
            string rand = new Random().Next(100000000, 999999999).ToString();
            string lastChar = new Random().Next(1000000, 9999999).ToString();
            return rand + lastChar;
        }

        private bool CheckUniqueCard(string cardNumber)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Cards WHERE cardNumber = @cardNumber";
                cmd.Parameters.AddWithValue("@cardNumber", cardNumber);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count > 0; //If count > 0 it will be true and generate a new number
            }
        }

        private string GenerateRandcVV()
        {
            string rand = new Random().Next(100, 999).ToString();
            return rand;
        }

        private bool CheckUniquecVV(string cVV)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM dbo.Cards WHERE cVV = @cVV";
                cmd.Parameters.AddWithValue("@cVV", cVV);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();

                return count > 0; //If count > 0 it will be true and generate a new number
            }
        }
    }
}
