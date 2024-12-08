using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NonlinearEquation
{
    internal class NonlinearEquation
    {
        private double myFunc(double i, double n, double m, double c, double t = 1)
        {
            return t*Math.Pow(i, n) + m * i - c;
        }

        private Dictionary<string, int> changeSign(double n, double m, double c)
        {
            Dictionary<string, int> change = new Dictionary<string, int>();
            double pre = 0;

            for (int i = 0; i < 1000; i++)
            {
                double re = myFunc(i,n,m,c);

                if (re < 0)
                    change["min"] = i;
                else
                    change["max"] = i;

                if (change.ContainsKey("max") && change.ContainsKey("min"))
                {
                    if (pre < 0 && re >= 0 || pre >= 0 && re < 0)
                        break;
                }
                pre = re;

                if (i == 1000)
                    change["нет"] = 0;
            }

            return change;
           
        }

        private Dictionary<string, List<double>> newton(Dictionary<string, int> span, double n, double m, double c, double e)
        {
            Dictionary<string, List<double> > req = new Dictionary<string, List<double>>();
            double start = 0;
            double fault = 1;
            req.Add("x", new List<double>());
            req.Add("y", new List<double>());

            foreach (var i in span)
            {
                double f = myFunc(i.Value, n, m, c);
                double f2 = n * (n - 1) * Math.Pow(i.Value, n - 2);
                if (f * f2 > 0)
                    start = i.Value;
            }

            while(fault >= e)
            {
                double f = myFunc(start, n, m, c);
                double f1 = n*Math.Pow(start, n-1)+m;
                req["x"].Add(start);
                req["y"].Add(f);

                double next = start - f / f1;
                fault = Math.Abs(start - next);
                start = next;
            }

            return req;
        }

        public Dictionary<string, List<double>> calculate(double n, double m, double c, double e)
        {
            Dictionary<string, List<double>> req = new Dictionary<string, List<double>>();

           var change = changeSign(n, m, c);
            
            if(!change.ContainsKey("нет"))
            {
                req = newton(change, n,m,c,e);
            }
            else
            {
                req.Add("x", new List<double>());
                req.Add("y", new List<double>());
            }

            return req;
        }
    }
}
