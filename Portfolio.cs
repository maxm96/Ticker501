using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Portfolio
    {
        private List<Stock> _stockList; //list of stocks for portfolio
        private int _stocks; //the total amount of stocks in the portfolio
        private double _money; //the total amount of money invested in the portfolio
        private string _name;

        public List<Stock> stockList
        {
            get
            {
                return _stockList;
            }
        }
        public int Stocks
        {
            get
            {
                int amount = 0;
                foreach(Stock s in this._stockList)
                {
                    amount += s.Amount;
                }
                _stocks = amount;
                return _stocks;
            }
            set
            {
                _stocks = value;
            }
        }
        public double Money
        {
            get
            {
                double total = 0;
                foreach(Stock s in this._stockList)
                {
                    total += s.Price * s.Amount;
                }
                _money = total;
                return _money;
            }
            set
            {
                _money = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public Portfolio(string name)
        {
            _stockList = new List<Stock>();
            _stocks = 0;
            _money = 0;
            _name = name;
        }

        /// <summary>
        /// adds a stock to the stock list
        /// </summary>
        /// <param name="s">the stock to add</param>
        public void addStock(Stock s, int amount)
        {
            bool inList = false;
            foreach(Stock stock in _stockList)
            {
                if (s.Ticker.Equals(stock.Ticker) && s.Price.Equals(stock.Price))
                {
                    stock.Amount += amount;
                    inList = true;
                }
            }
            if(!inList)
            {
                _stockList.Add(new Stock(s.Ticker, s.CompanyName, s.Price, amount));
            }
            _money += (s.Price * s.Amount);
            _stocks += s.Amount;
        }

        /// <summary>
        /// sells a stock in the portfolio
        /// </summary>
        /// <param name="s">the stock to sell</param>
        /// <param name="amount">the number of stocks to sell</param>
        /// <returns>returns the amount of money gained from selling the stock</returns>
        public double sellStock(Stock s, int amount)
        {
            double total;
            int index = this._stockList.IndexOf(s);
            if (index > -1)
            {
                if (this._stockList[index].Amount < amount)
                {
                    return -0.01;
                }
                else
                {
                    this._stockList[index].Amount -= amount;
                    total = this._stockList[index].Price * amount;
                    _stocks -= amount;
                    if (this._stockList[index].Amount == 0)
                    {
                       this. _stockList.Remove(s);
                    }
                    _money -= total;
                    return total;
                }
            }
            return -0.01;
        }

        /// <summary>
        /// returns the percentage of a single stock in the portfolio
        /// </summary>
        /// <param name="s">the stock to calculate</param>
        /// <returns>the percentage of the stock in the portfolio</returns>
        public double getPercentOfStock(Stock s)
        {
            if (_stockList.Contains(s))
            {
                return (Convert.ToDouble(s.Amount) / Convert.ToDouble(this.Stocks)) * 100;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// calculates the percent of money spent on one stock in the portfolio
        /// </summary>
        /// <param name="s">the stock that is being calculated</param>
        /// <returns>the amount of money gained from selling or a -1 if there is nothing to sell</returns>
        public double getPercentOfMoney(Stock s)
        {
            if (_stockList.Contains(s))
            {
                double total = s.Amount * s.Price;
                return (total / this.Money) * 100;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// sells all stock in a portfolio
        /// </summary>
        /// <returns>the amount of money gained from selling</returns>
        public double sellAll()
        {
            double amount = 0;
            for (int i = 0; i < _stockList.Count; i++)
            {
                amount += (_stockList[i].Price * _stockList[i].Amount);
                _stockList.Remove(_stockList[i]);
            }
            return amount;
        }
    }
}
