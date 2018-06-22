using System;
using System.Collections.Generic;
using System.Text;
using SpringHeroBank.entity;
using SpringHeroBank.model;
using SpringHeroBank.utility;

namespace SpringHeroBank.controller
{
    public class TransactionController
    {
        private TransactionModel TransactionModel = new TransactionModel();
        private AccountModel AccountModel = new AccountModel();
        private string strings = null;

        public string GetListTransaction()
        {
            Console.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            Account account = null;
            var listTransactions =
                TransactionModel.getTransactionByAccountNumber(Program.currentLoggedIn.AccountNumber);
            stringBuilder.Append("Transaction history.");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}", "Transaction ID",
                "Type", "Created date", "From", "To", "Content", "Amount");
            stringBuilder.AppendLine();
            foreach (var transaction in listTransactions)
            {
                if (transaction.Type == Transaction.TransactionType.DEPOSIT)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Deposit", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else if (transaction.Type == Transaction.TransactionType.WITHDRAW)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Withdraw", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else
                {
                    if (Program.currentLoggedIn.AccountNumber == transaction.SenderAccountNumber)
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.ReceiverAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, "You", account.FullName,
                            transaction.Content, transaction.Amount);
                    }
                    else
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.SenderAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, account.FullName, "You",
                            transaction.Content, transaction.Amount);
                    }

                    stringBuilder.AppendLine();
                }
            }

            Console.WriteLine(stringBuilder);
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            strings = stringBuilder.ToString();
            return strings;
        }

        public string GetListTransactionByDate()
        {
            Console.Clear();
            string[] startDate = new string[3];
            string[] endDate = new string[3];
            string startDay = "0";
            Console.WriteLine("please enter date you want to check ");
            
            Console.WriteLine("Start date:");
            Console.WriteLine("Day(DD): "); //cần validate
            startDay = Console.ReadLine();
            Console.WriteLine("Month(MM): ");
            var startMonth = Console.ReadLine();
            Console.WriteLine("Year(YYYY): ");
            var startYear = Console.ReadLine();

            Console.WriteLine();
            
            Console.WriteLine("End date:");
            Console.WriteLine("Day(DD): "); //cần validate
            endDate[2] = Console.ReadLine();
            Console.WriteLine("Month(MM): ");
            endDate[1] = Console.ReadLine();
            Console.WriteLine("Year(YYYY): ");
            endDate[0] = Console.ReadLine();

            Account account = null;
            StringBuilder stringBuilder = new StringBuilder();

            var listTransactions =
                TransactionModel.GetTransactionByDate(startDate, endDate);
            stringBuilder.Append("Transaction history.");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}", "Transaction ID",
                "Type", "Created date", "From", "To", "Content", "Amount");
            stringBuilder.AppendLine();
            foreach (var transaction in listTransactions)
            {
                if (transaction.Type == Transaction.TransactionType.DEPOSIT)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Deposit", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else if (transaction.Type == Transaction.TransactionType.WITHDRAW)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Withdraw", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else
                {
                    if (Program.currentLoggedIn.AccountNumber == transaction.SenderAccountNumber)
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.ReceiverAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, "You", account.FullName,
                            transaction.Content, transaction.Amount);
                    }
                    else
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.SenderAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, account.FullName, "You",
                            transaction.Content, transaction.Amount);
                    }

                    stringBuilder.AppendLine();
                }
            }

            Console.WriteLine(stringBuilder);
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            strings = stringBuilder.ToString();
            return strings;
        }

        public string GetListTransactionIn10Days()
        {
            Console.Clear();
            var listTransactionsIn10Days = TransactionModel.GetTransactionsIn10Days();
            Account account = null;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Transaction history.");
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}", "Transaction ID",
                "Type", "Created date", "From", "To", "Content", "Amount");
            stringBuilder.AppendLine();
            foreach (var transaction in listTransactionsIn10Days)
            {
                if (transaction.Type == Transaction.TransactionType.DEPOSIT)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Deposit", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else if (transaction.Type == Transaction.TransactionType.WITHDRAW)
                {
                    stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                        transaction.Id, "Withdraw", transaction.CreatedAt, "", "", transaction.Content,
                        transaction.Amount);
                    stringBuilder.AppendLine();
                }
                else
                {
                    if (Program.currentLoggedIn.AccountNumber == transaction.SenderAccountNumber)
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.ReceiverAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, "You", account.FullName,
                            transaction.Content, transaction.Amount);
                    }
                    else
                    {
                        account = AccountModel.GetAccountByAccountNumber(transaction.SenderAccountNumber);
                        stringBuilder.AppendFormat("{0,-50} {1,-10} {2,-25} {3,-30} {4,-30} {5,-15} {6,-15}",
                            transaction.Id, "Transfer", transaction.CreatedAt, account.FullName, "You",
                            transaction.Content, transaction.Amount);
                    }

                    stringBuilder.AppendLine();
                }
            }

            Console.WriteLine(stringBuilder);
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            strings = stringBuilder.ToString();
            return strings;
        }

        public void PrintTransaction()
        {
            FileHandle fileHandle = new FileHandle();
            string[] Strings = {strings};
            FileHandle.WriteTransactionToFile(Strings);
        }
    }
}