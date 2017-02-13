using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{

    class Stock
    {
        private string _ticker; //ticker abbreviation
        private string _companyName; //full company name
        private double _price; //price of one stock
        private int _amount; //how many stocks are owned

        public string Ticker
        {
            get
            {
                return _ticker;
            }
        }
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
        }
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        public Stock(string t, string c, double p, int a)
        {
            _ticker = t;
            _companyName = c;
            _price = p;
            _amount = a;
        }

        /// <summary>
        /// to string override for stocks
        /// </summary>
        /// <returns>returns a string displaying data about a stock</returns>
        public override string ToString()
        {
            string result = "";
            result += "Company name: " + this.CompanyName;
            result += "\nTicker: " + this.Ticker;
            result += "\nPrice: " + this.Price;
            result += "\nAmount owned: " + this.Amount;
            return result;
        }
    }
}
