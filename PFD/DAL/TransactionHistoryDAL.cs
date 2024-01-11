﻿using Microsoft.Data.SqlClient;
using PFD_ASG.Models;
using System.Reflection.PortableExecutable;

namespace PFD_ASG.DAL
{
    public class TransactionHistoryDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public TransactionHistoryDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "OCBCConnectionString");
            conn = new SqlConnection(strConn);
        }

        public List<TransactionHistory> GetTransactionHistories()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM TransactionHistory OrderBy recordID";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<TransactionHistory> transactionHistories = new List<TransactionHistory>();
            while (reader.Read())
            {
                TransactionHistory transactionHistory = new TransactionHistory
                {
                    recordID = reader.GetInt32(0),
                    transactionTime = reader.GetDateTime(1),
                    description = !reader.IsDBNull(2) ? reader.GetString(2) : (string?)null,
                    senderID = reader.GetInt32(3),
                    receiverID = reader.GetInt32(4),
                    amount = reader.GetDecimal(5),
                };
                transactionHistories.Add(transactionHistory);
            }
            reader.Close();
            conn.Close();
            return transactionHistories;
        }

        //Get a transactionHistory for a specific user (BY: LIWEI)
        public List<TransactionView> GetTransactionHistory(int userId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT th.*,sender.UserName AS SenderName,receiver.UserName AS ReceiverName
								FROM TransactionHistory th
								INNER JOIN Users sender ON th.SenderID = sender.UserID
								INNER JOIN Users receiver ON th.ReceiverID = receiver.UserID  
								where senderID = @userId or receiverID = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            //Open a database Connection
            conn.Open();
            //Execute the SELECT SQL
            SqlDataReader reader = cmd.ExecuteReader();
            //Save data into a member list
            List<TransactionView> TransHistList = new List<TransactionView>();
            while (reader.Read())
            {
                TransHistList.Add(new TransactionView
                {
                    recordID = reader.GetInt32(0),
                    transactionTime = reader.GetDateTime(1),
                    description = !reader.IsDBNull(2) ? reader.GetString(2) : (string?)null,
                    senderID = reader.GetInt32(3),
                    receiverID = reader.GetInt32(4),
                    amount = reader.GetDecimal(5),
                    category = reader.GetString(7),
                    senderName = reader.GetString(8),
                    receiverName = reader.GetString(9),
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return TransHistList;
        }

        public List<TransactionView> GetSendTransactionHistory(int userId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT th.*,sender.UserName AS SenderName,receiver.UserName AS ReceiverName
								FROM TransactionHistory th
								INNER JOIN Users sender ON th.SenderID = sender.UserID
								INNER JOIN Users receiver ON th.ReceiverID = receiver.UserID  
								where senderID = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);

            //Open a database Connection
            conn.Open();

            //Execute the SELECT SQL
            SqlDataReader reader = cmd.ExecuteReader();

            //Save data into a member list
            List<TransactionView> TransHistList = new List<TransactionView>();

            while (reader.Read())
            {
                TransHistList.Add(new TransactionView
                {
                    recordID = reader.GetInt32(0),
                    transactionTime = reader.GetDateTime(1),
                    description = !reader.IsDBNull(2) ? reader.GetString(2) : (string?)null,
                    senderID = reader.GetInt32(3),
                    receiverID = reader.GetInt32(4),
                    amount = reader.GetDecimal(5),
                    category = reader.GetString(7),
                    senderName = reader.GetString(8),
                    receiverName = reader.GetString(9),
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return TransHistList;
        }

        public List<Decimal> getTransactionsTotal(int userID)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT amount FROM TransactionHistory WHERE senderID = @userID";
            cmd.Parameters.AddWithValue("@userID", userID);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<Decimal> TransTotal = new List<Decimal>();

            while (reader.Read())
            {
                TransTotal.Add(
                    reader.GetDecimal(0)

            );
            }
            reader.Close();
            conn.Close();
            return TransTotal;
        }

    }
}