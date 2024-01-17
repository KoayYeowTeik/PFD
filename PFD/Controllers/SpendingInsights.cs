using Microsoft.AspNetCore.Mvc;
using PFD_ASG.Models;
using PFD_ASG.DAL;
using System.Text.Json;

namespace PFD_ASG.Controllers
{

    public class spendingInsights : Controller
    {
        TransactionHistoryDAL transactionHistoryDAL = new TransactionHistoryDAL();
        int userID = 1;
        decimal total = 0;

        public ActionResult SpendingInsights()
        {
            return View();
        }

        public ActionResult insightsCalculation()
        {
            List<TransactionView> transactionList = transactionHistoryDAL.GetSendTransactionHistory(userID);
            List<Decimal> transactionTotal = transactionHistoryDAL.getTransactionsTotal(userID);

            for (int i = 0; i < transactionTotal.Count; i++)
            {
                total += transactionTotal[i];
            }
            List<TransactionView> transactionFood = new List<TransactionView>();
            List<TransactionView> transactionTransport = new List<TransactionView>();
            List<TransactionView> transactionBills = new List<TransactionView>();
            List<TransactionView> transactionTransfers = new List<TransactionView>();

            for (int i = 0; i < transactionList.Count; i++)
            {
                if (transactionList[i].category == "Food")
                {
                    transactionFood.Add(transactionList[i]);
                }

                else if (transactionList[i].category == "Transport")
                {
                    transactionTransport.Add(transactionList[i]);
                }

                else if (transactionList[i].category == "Bills")
                {
                    transactionBills.Add(transactionList[i]);
                }

                else
                {
                    transactionTransfers.Add(transactionList[i]);
                }
            }

            var JsonData = new
            {
                TransactionTotal = total,
                TransactionFood = transactionFood,
                TransactionTransport = transactionTransport,
                TransactionBills = transactionBills,
                TransactionTransfers = transactionTransfers
            };

            return Json(JsonData);
        }
    }
}