using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using ConsoleApp3.model;
using MySql.Data.MySqlClient;
using Transaction = SpringHeroBank.entity.Transaction;

namespace SpringHeroBank.model
{
    public class TransactionModel
    {
        public List<Transaction> getTransactionByAccountNumber(string accountNumber)
        {
            DbConnection.Instance().OpenConnection();
            var listTransaction = new List<Transaction>();

            var sqlQuery =
                "select * from `transactions` where receiverAccountNumber = @accountnumber or senderAccountNumber = @accountnumber";
            var cmd = new MySqlCommand(sqlQuery, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@accountnumber", accountNumber);
            var transactionReader = cmd.ExecuteReader();
            while (transactionReader.Read())
            {
                Transaction transaction = new Transaction()
                {
                    Amount = transactionReader.GetDecimal("amount"),
                    Content = transactionReader.GetString("content"),
                    Id = transactionReader.GetString("id"),
                    Type = (Transaction.TransactionType) transactionReader.GetInt32("type"),
                    SenderAccountNumber = transactionReader.GetString("senderAccountNumber"),
                    ReceiverAccountNumber = transactionReader.GetString("receiverAccountNumber"),
                    Status = (Transaction.ActiveStatus) transactionReader.GetInt32("status"),
                    CreatedAt = transactionReader.GetMySqlDateTime("createdAt").ToString()
                };
                listTransaction.Add(transaction);
            }
            transactionReader.Close();
            DbConnection.Instance().CloseConnection();
            return listTransaction;
        }

        public static List<Transaction> GetTransactionByDate(string[] startDate, string[] endDate)
        {
            DbConnection.Instance().OpenConnection();
            var listTransaction = new List<Transaction>();

            var sqlQuery =
                "SELECT * FROM `transactions` WHERE (`receiverAccountNumber` = @accountnumber OR `senderAccountNumber` = @accountnumber) AND `createdAt` BETWEEN @startdate and @enddate ORDER BY `createdAt` DESC";
            var cmd = new MySqlCommand(sqlQuery, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@accountnumber", Program.currentLoggedIn.AccountNumber);
            cmd.Parameters.AddWithValue("@startdate", startDate[0] + "-" + startDate[1] + "-" + startDate[2] + "*");
            cmd.Parameters.AddWithValue("@enddate", endDate[0] + "-" + endDate[1] + "-" + endDate[2] + "*");
            var transactionReader = cmd.ExecuteReader();
            while (transactionReader.Read())
            {
                Transaction transaction = new Transaction()
                {
                    Amount = transactionReader.GetDecimal("amount"),
                    Content = transactionReader.GetString("content"),
                    Id = transactionReader.GetString("id"),
                    Type = (Transaction.TransactionType) transactionReader.GetInt32("type"),
                    SenderAccountNumber = transactionReader.GetString("senderAccountNumber"),
                    ReceiverAccountNumber = transactionReader.GetString("receiverAccountNumber"),
                    Status = (Transaction.ActiveStatus) transactionReader.GetInt32("status"),
                    CreatedAt = transactionReader.GetMySqlDateTime("createdAt").ToString()
                };
                listTransaction.Add(transaction);
            }
            transactionReader.Close();
            DbConnection.Instance().CloseConnection();
            return listTransaction;
        }

        public static List<Transaction> GetTransactionsIn10Days()
        {
            DbConnection.Instance().OpenConnection();
            var listTransaction = new List<Transaction>();

            var sqlQuery =
                "SELECT * FROM `transactions` WHERE (`receiverAccountNumber` = @accountnumber OR `senderAccountNumber` = @accountnumber) AND `createdAt` BETWEEN adddate(now(),-10) and now() ORDER BY `createdAt` DESC";
            var cmd = new MySqlCommand(sqlQuery, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@accountnumber", Program.currentLoggedIn.AccountNumber);
            var transactionReader = cmd.ExecuteReader();
            while (transactionReader.Read())
            {
                Transaction transaction = new Transaction()
                {
                    Amount = transactionReader.GetDecimal("amount"),
                    Content = transactionReader.GetString("content"),
                    Id = transactionReader.GetString("id"),
                    Type = (Transaction.TransactionType) transactionReader.GetInt32("type"),
                    SenderAccountNumber = transactionReader.GetString("senderAccountNumber"),
                    ReceiverAccountNumber = transactionReader.GetString("receiverAccountNumber"),
                    Status = (Transaction.ActiveStatus) transactionReader.GetInt32("status"),
                    CreatedAt = transactionReader.GetMySqlDateTime("createdAt").ToString()
                };
                listTransaction.Add(transaction);
            }
            transactionReader.Close();
            DbConnection.Instance().CloseConnection();
            return listTransaction;
        }
    }
}