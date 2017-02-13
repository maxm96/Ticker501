using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class UserInterface
    {
        static void Main(string[] args)
        {
            Account a = new Ticker501.Account();
            Stock stock = null;
            string line;
            int ans = 11;
            int amount;
            string ticker;
            string port;
            string file;
            double money;
            bool read = false;
            do
            {
                Console.Write("Enter a file name for stock list: ");
                line = Console.ReadLine();
                read = a.readFile(line);
            } while (!read);

            do
            {
                do
                {
                    Console.WriteLine("1. add funds");
                    Console.WriteLine("2. buy stock");
                    Console.WriteLine("3. sell stock");
                    Console.WriteLine("4. add portfolio");
                    Console.WriteLine("5. delete portfolio");
                    Console.WriteLine("6. view account report");
                    Console.WriteLine("7. view portfolio");
                    Console.WriteLine("8. run simulation");
                    Console.WriteLine("9. account options");
                    Console.WriteLine("0. exit");
                    Console.Write("> ");
                    line = Console.ReadLine();
                } while (!isNumber(line));
                ans = Convert.ToInt32(line);

                switch (ans)
                {
                    case 0:
                        break;
                    case 1:
                        Console.Write("Enter amount to add: ");
                        money = Convert.ToDouble(Console.ReadLine());
                        a.addFunds(money);
                        break;
                    case 2:
                        Console.Write("Enter ticker of stock: ");
                        ticker = Console.ReadLine();
                        do
                        {
                            Console.Write("Enter amount you wish to purchase: ");
                            line = Console.ReadLine();
                        } while (!isNumber(line));
                        amount = Convert.ToInt32(line);
                        Console.Write("Enter portfolio to add to: ");
                        port = Console.ReadLine();
                        a.buyStock(ticker, port, amount);
                        break;
                    case 3:
                        a.Gains = 0;
                        do
                        {
                            Console.Write("Enter portfolio to sell from: ");
                            port = Console.ReadLine();
                            Console.Write("Enter ticker: ");
                            ticker = Console.ReadLine();
                            List<Stock> list = sell(port, ticker, a);
                            do
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    Console.WriteLine("Choose one to sell: ");
                                    Console.WriteLine((i + 1) + ": " + list[i].Ticker + " " + list[i].Price + " " + list[i].Amount);
                                }
                                Console.Write("> ");
                                do
                                {
                                    line = Console.ReadLine();
                                } while (!isNumber(line));
                                ans = Convert.ToInt32(line) - 1;
                            } while (ans > list.Count || ans < 0);
                            stock = list[ans];
                            do
                            {
                                Console.Write("Enter amount to sell: ");
                                line = Console.ReadLine();
                            } while (!isNumber(line));
                            amount = Convert.ToInt32(line);
                            a.sellSpecStock(stock, port, amount);
                            Console.WriteLine("Gains/Losses: {0:N2}", a.Gains);
                            do
                            {
                                Console.WriteLine("0. Back");
                                Console.WriteLine("1. Sell more");
                                line = Console.ReadLine();
                            } while (!isNumber(line));
                            ans = Convert.ToInt32(line);
                        } while (ans != 0);
                        ans = 11;
                        Console.WriteLine("Total Gains/Losses: {0:N2}", a.Gains);
                        break;
                    case 4:
                        Console.Write("Enter portfolio name: ");
                        port = Console.ReadLine();
                        a.addPortfolio(port);
                        break;
                    case 5:
                        do
                        {
                            Console.WriteLine("You will lose all assets from a portfolio if you delete it. Continue?");
                            Console.WriteLine("0. No");
                            Console.WriteLine("1. Yes");
                            Console.Write("> ");
                            line = Console.ReadLine();
                        } while (!isNumber(line));
                        ans = Convert.ToInt32(line);
                        switch(ans)
                        {
                            case 0:
                                ans = 11;
                                break;
                            case 1:
                                Console.Write("Enter name of portfolio: ");
                                port = Console.ReadLine();
                                a.deletePortfolio(port);
                                break;
                        }
                        break;
                    case 6:
                        a.percentOfAllPort();
                        break;
                    case 7:
                        Console.Write("Enter portfolio name: ");
                        port = Console.ReadLine();
                        a.percentOfOnePort(port);
                        break;
                    case 8:
                        a.runSimulation();
                        Console.WriteLine("Simulation ran successfully");
                        break;
                    case 9:
                        do
                        {
                            do
                            {
                                Console.WriteLine("1. Save account");
                                Console.WriteLine("2. Load account");
                                Console.WriteLine("0. Back");
                                Console.Write("> ");
                                line = Console.ReadLine();
                            } while (!isNumber(line));
                            ans = Convert.ToInt32(line);
                            switch (ans)
                            {
                                case 1:
                                    Console.Write("Enter file to save to: ");
                                    file = Console.ReadLine();
                                    a.saveAccount(file);
                                    break;
                                case 2:
                                    Console.WriteLine("Enter file: ");
                                    file = Console.ReadLine();
                                    Account b = a.loadAccount(file);
                                    if (b != null)
                                    {
                                        a = b;
                                    }
                                    break;
                                case 0:
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid number");
                                    break;
                            }
                        } while (ans != 0);
                        ans = 11;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid number");
                        break;
                }
            } while (ans != 0);

        }

        /// <summary>
        /// determines if input is a number
        /// </summary>
        /// <param name="num">the string to be tested</param>
        /// <returns>true if input is number, else false</returns>
        private static bool isNumber(string num)
        {
            bool pass = true; ;
            for (int i = 0; i < num.Length; i++)
            {
                if (!Char.IsDigit(num[i]))
                {
                    pass = false;
                }
            }
            if (!pass)
            {
                Console.WriteLine("Please enter a valid number");
            }
            return pass;
        }

        /// <summary>
        /// collects stocks that are similar to the one the user is trying to sell
        /// </summary>
        /// <param name="p">the name of the portfolio</param>
        /// <param name="t">the ticker of the stock being sold</param>
        /// <param name="a">the account</param>
        /// <returns>a list of stocks that are the same, if none then null</returns>
        private static List<Stock> sell(string p, string t, Account a)
        {
            t = t.ToUpper();
            List<Stock> retList = new List<Stock>();
            Portfolio port = a.portfolioExists(p);
            if (port != null)
            {
                Stock s = a.stockExistsSell(t, port);
                if (s != null)
                {
                    foreach(Stock st in port.stockList)
                    {
                        if (st.Ticker.Equals(t))
                        {
                            retList.Add(st);
                        }
                    }
                    return retList;
                }
                else
                {
                    Console.WriteLine("Could not find stock");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Could not find portfolio");
                return null;
            }
        }
    }
}
