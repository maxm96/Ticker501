using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Account
    {
        public List<Portfolio> _portfolioList;
        public List<Stock> _availableStocks;
        private double _money;
        private double _assetMoney;
        private int _stocks;
        private double _gains;

        public double Money
        {
            get
            {
                return _money;
            }
        }
        public double Assets
        {
            get
            {
                return _assetMoney;
            }
        }
        public int Stocks
        {
            get
            {
                return _stocks;
            }
        }
        public double Gains
        {
            get
            {
                return _gains;
            }
            set
            {
                _gains = value;
            }
        }

        public Account()
        {
            _portfolioList = new List<Portfolio>();
            _availableStocks = new List<Stock>();
            _assetMoney = 0;
            _money = 0;
            _stocks = 0;
            _gains = 0;
        }

        public Account(List<Portfolio> pList, List<Stock> sList, double asset, double money, int stocks, double gains)
        {
            _portfolioList = pList;
            _availableStocks = sList;
            _assetMoney = asset;
            _money = money;
            _stocks = stocks;
            _gains = gains;
        }

        /// <summary>
        /// adds money to the account
        /// </summary>
        /// <param name="amount">the amount to add</param>
        public void addFunds(double amount)
        {
            _money += amount;
        }

        /// <summary>
        /// adds a portfolio to the account
        /// </summary>
        /// <param name="name">the name of the portfolio</param>
        public void addPortfolio(string name)
        {
            bool exists = false;
            foreach(Portfolio p in this._portfolioList)
            {
                if(p.Name.Equals(name))
                {
                    exists = true;
                }
            }
            if (exists)
            {
                Console.WriteLine("error: that name is already in use");
            }
            else
            {
                _portfolioList.Add(new Portfolio(name));
                Console.WriteLine(name + " has been created");
            }
        }

        /// <summary>
        /// buys stock from list of stocks
        /// </summary>
        /// <param name="t">the name of the ticker</param>
        /// <param name="p">the name of the portfolio</param>
        /// <param name="amount">the amount to purchase</param>
        public void buyStock(string t, string p, int amount)
        {
            t = t.ToUpper();
            Stock s = stockExistsBuy(t);
            Portfolio po = portfolioExists(p);
            if (s == null)
            {
                Console.WriteLine("error: stock does not exist");
                return;
            }
            if (po == null)
            {
                Console.WriteLine("error: that portfolio does not exist");
            }
            else
            {
                double cost = s.Price * amount;
                if (_money >= cost + 9.99)
                {
                    po.addStock(s, amount);
                    _money -= cost + 9.99;
                    _stocks += amount;
                    _assetMoney += cost;
                }
                else
                {
                    Console.WriteLine("error: you do not have enough funds");
                }
            }
        }

        public void sellSpecStock(Stock s, string po, int amount)
        {
            Portfolio p = portfolioExists(po);
            if (p != null)
            {
                if (amount <= s.Amount)
                {
                    double gain = p.sellStock(s, amount);
                    if (gain != -0.01)
                    {
                        double newPrice = 0;
                        foreach (Stock st in _availableStocks)
                        {
                            if (st.Ticker.Equals(s.Ticker))
                            {
                                newPrice = st.Price * amount;
                            }
                        }
                        double difference = gain - newPrice;
                        _gains -= difference;
                        _money += newPrice;
                        _assetMoney -= gain;
                        _stocks -= amount;
                    }
                    else
                    {
                        Console.WriteLine("error: you do not own that many of " + s.Ticker);
                    }
                }
            }
            else
            {
                Console.WriteLine("Portfolio does not exist");
            }
        }

        /// <summary>
        /// sells a stock
        /// </summary>
        /// <param name="t">the ticker of the stock</param>
        /// <param name="p">the portfolio to sell from</param>
        /// <param name="amount">the amount to sell</param>
        public void sellStock(string t, string p, int amount)
        {
            t = t.ToUpper();
            Portfolio po = portfolioExists(p);
            if (po != null)
            {
                Stock st = stockExistsSell(t, po);
                if (st != null && amount <= st.Amount)
                {
                    double gain = po.sellStock(st, amount);
                    if (gain != -0.01)
                    {
                        double newPrice = 0;
                        foreach(Stock s in _availableStocks)
                        {
                            if(s.Ticker.Equals(st.Ticker))
                            {
                                newPrice = s.Price * amount;
                            }
                        }
                        double difference = gain - newPrice;
                        _gains -= difference;
                        _money += newPrice;
                        _assetMoney -= gain;
                        _stocks -= amount; 
                    }
                    else
                    {
                        Console.WriteLine("error: you do not own that many of " + st.Ticker);
                    }
                }
                else
                {
                    Console.WriteLine("error: could not complete transaction, stock may not exist"); 
                }
            }
            else
            {
                Console.WriteLine("error: could not find portfolio");
            }
        }

        /// <summary>
        /// determines if a stock exists
        /// </summary>
        /// <param name="s">the name of the stock to check</param>
        /// <returns>returns the stock if found, else null</returns>
        private Stock stockExistsBuy(string s)
        {
            foreach (Stock st in this._availableStocks)
            {
                if (st.Ticker.Equals(s))
                {
                    return st;
                }
            }
            return null;
        }

        /// <summary>
        /// determines if a stock exists
        /// </summary>
        /// <param name="s">the name of the stock</param>
        /// <param name="p">the portfolio to check</param>
        /// <returns>the stock if found, else null</returns>
        public Stock stockExistsSell(string s, Portfolio p)
        {
            foreach (Stock st in p.stockList)
            {
                if (st.Ticker.Equals(s))
                {
                    return st;
                }
            }
            return null;
        }

        /// <summary>
        /// determines if a portfolio exists
        /// </summary>
        /// <param name="p">the name of the portfolio</param>
        /// <returns>returns the portfolio if found, else null</returns>
        public Portfolio portfolioExists(string p)
        {
            foreach (Portfolio po in this._portfolioList)
            {
                if (po.Name.Equals(p))
                {
                    return po;
                }
            }
            return null;
        }

        /// <summary>
        /// displays information of entire account
        /// </summary>
        public void percentOfAllPort()
        {
            double assets = _assetMoney;
            double stocks = Convert.ToDouble(_stocks);
            Console.WriteLine("Account funds: {0:N2}", Money);
            Console.WriteLine("Money in assets: {0:N2}", Assets);
            foreach(Portfolio p in this._portfolioList)
            {
                Console.WriteLine(p.Name + ":");
                Console.Write("${0:N2}", p.Money);
                Console.WriteLine(" {0:N2}%", (p.Money / assets) * 100);
                Console.WriteLine(p.Stocks + " stock(s): {0:N2}%", (Convert.ToDouble(p.Stocks) / stocks) * 100);
                Console.WriteLine("\tStocks:");
                foreach(Stock s in p.stockList)
                {
                    Console.WriteLine("\tCompany: " + s.CompanyName);
                    Console.WriteLine("\tTicker: " + s.Ticker);
                    Console.Write("\tInvestment: ${0:N2}", s.Price * s.Amount);
                    Console.WriteLine(" for {0:N2}%", p.getPercentOfMoney(s));
                    Console.WriteLine("\tAmount: " + s.Amount + " stocks for {0:N2}%\n", p.getPercentOfStock(s));
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// displays information of a single portfolio
        /// </summary>
        /// <param name="p">the name of the portofolio</param>
        public void percentOfOnePort(string p)
        {
            Portfolio port = portfolioExists(p);
            if (port == null)
            {
                Console.WriteLine("error: could not find portfolio " + p);
            }
            else
            {
                double assets = _assetMoney;
                int stocks = _stocks;
                Console.WriteLine(port.Name + ":");
                Console.Write("${0:N2}", port.Money);
                Console.WriteLine(" {0:N2}%", (port.Money / assets) * 100);
                Console.WriteLine(port.Stocks + " stock(s): {0:N2}%", (port.Stocks / stocks) * 100);
                Console.WriteLine("\tStocks:");
                foreach (Stock s in port.stockList)
                {
                    Console.WriteLine("\tCompany: " + s.CompanyName);
                    Console.WriteLine("\tTicker: " + s.Ticker);
                    Console.Write("\tInvestment: ${0:N2}", s.Price * s.Amount);
                    Console.WriteLine(" for {0:N2}%", port.getPercentOfMoney(s));
                    Console.WriteLine("\tAmount: " + s.Amount + " stocks for {0:N2}%\n", port.getPercentOfStock(s));
                }
            }
        }

        /// <summary>
        /// creates and adds stocks to a list from a file
        /// </summary>
        /// <param name="fileName">the file to read from</param>
        public bool readFile(string fileName)
        {
            string line;
            string[] lineArray;
            bool read = false;
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    while (reader.Peek() != -1)
                    {
                        line = reader.ReadLine();
                        lineArray = line.Split('-');
                        string ticker = lineArray[0];
                        if (lineArray.Length > 1)
                        {
                            string comp = lineArray[1];
                            line = "";
                            for (int i = 1; i < lineArray[2].Length; i++)
                            {
                                line += lineArray[2][i];
                            }
                            double price = Convert.ToDouble(line);
                            _availableStocks.Add(new Stock(ticker, comp, price, 0));
                        }
                        read = true;
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Error: could not find specified file");
            }
            return read;
        }

        /// <summary>
        /// randomly changes stocks prices
        /// </summary>
        public void runSimulation()
        {
            Random r = new Random();
            for (int i = 0; i < _availableStocks.Count; i++)
            {
                if (r.Next(0, 2) == 1)
                {
                    double price = _availableStocks[i].Price;
                    double newPrice = price + r.Next(0, 20);
                    _availableStocks[i].Price = newPrice;
                }
                else
                {
                    double price = _availableStocks[i].Price;
                    double newPrice = price + r.Next(-20, 0);
                    _availableStocks[i].Price = newPrice;
                }
            }
        }

        /// <summary>
        /// saves account to a file to be loaded later
        /// </summary>
        /// <param name="file">the file to save to</param>
        public void saveAccount(string file)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(this._availableStocks.Count); //save _availableStocks
                    foreach(Stock s in this._availableStocks)
                    {
                        writer.WriteLine(s.Ticker + "-" + s.CompanyName + "-" + s.Price);
                    }
                    writer.WriteLine(this._portfolioList.Count);
                    foreach (Portfolio p in this._portfolioList) //save portfolio list and associated stocks
                    {
                        writer.WriteLine(p.Name);
                        writer.WriteLine(p.stockList.Count);
                        foreach(Stock st in p.stockList)
                        {
                            writer.WriteLine(st.Ticker + "-" + st.CompanyName + "-" + st.Price + "-" + st.Amount);
                        }
                    }
                    writer.WriteLine(this._assetMoney + "_" + this._gains + "_" + this._money + "_" + this._stocks);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// reads a file to load a previously saved account
        /// </summary>
        /// <param name="file">the file to read</param>
        /// <returns>the account on the file</returns>
        public Account loadAccount(string file)
        {
            List<Portfolio> pList = new List<Portfolio>();
            List<Stock> sList = new List<Stock>();
            Portfolio port;
            string line;
            string[] lineArray;
            int num;
            double assets = 0;
            double money = 0;
            double gains = 0;
            int stocks = 0;
            bool read = true;
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    if (reader.Peek() != 0)
                    {
                        num = Convert.ToInt32(reader.ReadLine()); //read stock list
                        for (int j = 0; j < num; j++)
                        {
                            line = reader.ReadLine();
                            lineArray = line.Split('-');
                            string ticker = lineArray[0];
                            if (lineArray.Length > 1)
                            {
                                string comp = lineArray[1];
                                line = lineArray[2];
                                double price = Convert.ToDouble(line);
                                sList.Add(new Stock(ticker, comp, price, 0));
                            }
                        }
                        if (reader.Peek() != 0)
                        {
                            num = Convert.ToInt32(reader.ReadLine()); //read portfolios and associated stocks
                            for (int j = 0; j < num; j++)
                            {
                                line = reader.ReadLine();
                                port = new Portfolio(line);
                                int num2 = Convert.ToInt32(reader.ReadLine());
                                for (int k = 0; k < num2; k++)
                                {
                                    line = reader.ReadLine();
                                    lineArray = line.Split('-');
                                    string ticker = lineArray[0];
                                    if (lineArray.Length > 1)
                                    {
                                        string comp = lineArray[1];
                                        line = lineArray[2];
                                        double price = Convert.ToDouble(line);
                                        int amount = Convert.ToInt32(lineArray[3]);
                                        port.stockList.Add(new Stock(ticker, comp, price, amount));
                                    }
                                }
                                pList.Add(port);
                            }
                        }
                    }
                    line = reader.ReadLine();
                    lineArray = line.Split('_');
                    assets = Convert.ToDouble(lineArray[0]);
                    gains = Convert.ToDouble(lineArray[1]);
                    money = Convert.ToDouble(lineArray[2]);
                    stocks = Convert.ToInt32(lineArray[3]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not read file");
                read = false;
            }
            if (read)
            {
                return new Account(pList, sList, assets, money, stocks, gains);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// deletes a single portfolio from the account
        /// </summary>
        /// <param name="p">the name of the portfolio to delete</param>
        public void deletePortfolio(string p)
        {
            Portfolio port = portfolioExists(p);
            if (port != null)
            {
                _assetMoney -= port.Money;
                _stocks -= port.Stocks;
                _portfolioList.Remove(port);
            }
            else
            {
                Console.WriteLine("Portfolio does not exist");
            }
        }
    }
}
