﻿using System;
using SpringHeroBank.entity;
using SpringHeroBank.model;
using SpringHeroBank.utility;

namespace SpringHeroBank.controller
{
    public class AccountController
    {
        private AccountModel model = new AccountModel();

        public void Register()
        {
            Console.Clear();
            Console.WriteLine("Please enter account information");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();
            Console.WriteLine("Confirm Password: ");
            var cpassword = Console.ReadLine();
            Console.WriteLine("Identity Card: ");
            var identityCard = Console.ReadLine();
            Console.WriteLine("Full Name: ");
            var fullName = Console.ReadLine();
            Console.WriteLine("Email: ");
            var email = Console.ReadLine();
            Console.WriteLine("Phone: ");
            var phone = Console.ReadLine();
            var account = new Account(username, password, cpassword, identityCard, phone, email, fullName);
            var errors = account.CheckValid();
            if (errors.Count == 0)
            {
                model.Save(account);
                Console.WriteLine("Register success!");
                Console.ReadLine();
            }
            else
            {
                Console.Error.WriteLine("Please fix following errors and try again.");
                foreach (var messagErrorsValue in errors.Values)
                {
                    Console.Error.WriteLine(messagErrorsValue);
                }

                Console.ReadLine();
            }
        }

        public Boolean DoLogin()
        {
            // Lấy thông tin đăng nhập phía người dùng.
            Console.Clear();
            Console.WriteLine("Please enter account information");
            Console.WriteLine("-----------------------------------");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            var account = new Account(username, password);
            // Tiến hành validate thông tin đăng nhập. Kiểm tra username, password khác null và length lớn hơn 0.
            var errors = account.ValidLoginInformation();
            if (errors.Count > 0)
            {
                Console.WriteLine("Invalid login information. Please fix errors below.");
                foreach (var messagErrorsValue in errors.Values)
                {
                    Console.Error.WriteLine(messagErrorsValue);
                }

                Console.ReadLine();
                return false;
            }

            account = model.GetAccountByUserName(username);
            if (account == null)
            {
                // Sai thông tin username, trả về thông báo lỗi không cụ thể.
                Console.WriteLine("Invalid login information. Please try again.");
                return false;
            }

            // Băm password người dùng nhập vào kèm muối và so sánh với password đã mã hoá ở trong database.
            if (account.Password != Hash.GenerateSaltedSHA1(password, account.Salt))
            {
                // Sai thông tin password, trả về thông báo lỗi không cụ thể.
                Console.WriteLine("Invalid login information. Please try again.");
                return false;
            }

            // Login thành công. Lưu thông tin đăng nhập ra biến static trong lớp Program.
            Program.currentLoggedIn = account;
            return true;
        }

        public void Withdraw()
        {
            Console.Clear();
            Console.WriteLine("Withdraw.");
            Console.WriteLine("---------------------------------");
            Console.Write("Please enter amount to withdraw: ");
            var amount = Utility.GetUnsignDecimalNumber();
            Console.Write("Please enter message content: ");
            var content = Console.ReadLine();
//            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            var historyTransaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Type = Transaction.TransactionType.WITHDRAW,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = Transaction.ActiveStatus.DONE,
                CreatedAt = DateTime.Now.ToString("yy-MM-dd HH:mm:ss")
            };
            if (model.UpdateBalance(Program.currentLoggedIn, historyTransaction))
            {
                Console.WriteLine("Transaction success!");
            }
            else
            {
                Console.WriteLine("Transaction fails, please try again!");
            }

            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }

        public void Deposit()
        {
            Console.Clear();
            Console.WriteLine("Deposit.");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please enter amount to deposit: ");
            var amount = Utility.GetUnsignDecimalNumber();
            Console.WriteLine("Please enter message content: ");
            var content = Console.ReadLine();
//            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            var historyTransaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Type = Transaction.TransactionType.DEPOSIT,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = Transaction.ActiveStatus.DONE,
                CreatedAt = DateTime.Now.ToString("yy-MM-dd HH:mm:ss")
            };
            if (model.UpdateBalance(Program.currentLoggedIn, historyTransaction))
            {
                Console.WriteLine("Transaction success!");
            }
            else
            {
                Console.WriteLine("Transaction fails, please try again!");
            }

            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }

        public void Transfer()
        {
            Console.Clear();
            Console.WriteLine("Transfer.");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please enter account Number to transfer");
            var accountNumber = Console.ReadLine();
            var receiverAccount = model.GetAccountByAccountNumber(accountNumber);
            Console.WriteLine("You want to transfer to: " + receiverAccount.FullName + "? (Y/N)");
            var choice = Console.ReadLine();
            if (choice == "N" || choice == "n")
            {
                Console.WriteLine("You choose No.");
            }
            else if (choice == "Y" || choice == "y")
            {
                Console.WriteLine("Please enter amount to transfer: ");
                var amount = Decimal.Parse(Console.ReadLine());
                if (!(Program.currentLoggedIn.Balance >= amount))
                {
                    Console.WriteLine("Not enough balance to transfer");
                }
                else
                {
                    Console.WriteLine("Please enter message content");
                    var content = Console.ReadLine();

                    if (Program.currentLoggedIn.Balance >= amount)
                    {
                        Program.currentLoggedIn.Balance -= amount;
                        var historyTransaction = new Transaction
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = Transaction.TransactionType.TRANSFER,
                            Amount = amount,
                            Content = content,
                            SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                            ReceiverAccountNumber = receiverAccount.AccountNumber,
                            Status = Transaction.ActiveStatus.DONE,
                            CreatedAt = DateTime.Now.ToString("yy-MM-dd HH:mm:ss")
                        };
                        
                        if (model.UpdateBalance(Program.currentLoggedIn, receiverAccount, historyTransaction))
                        {
                            Console.WriteLine("Transaction success!");
                        }
                        else
                        {
                            Console.WriteLine("Transaction fails, please try again!");
                        }

                        Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
                        Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
                        Console.WriteLine("Press enter to continue!");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter your choice");
            }
        }

        public void CheckBalance() 
        {
            Console.Clear();
            Program.currentLoggedIn = model.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Account Information");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Full name: " + Program.currentLoggedIn.FullName);
            Console.WriteLine("Account number: " + Program.currentLoggedIn.AccountNumber);
            Console.WriteLine("Balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }
    }
}