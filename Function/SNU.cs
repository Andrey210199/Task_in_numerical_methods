using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNU
{
    class SNU
    {
        private Dictionary<string,double> compute(Dictionary<string, double> item)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            int i = 0;
            double x = 0;
            double y = 0;
            double curFault = 1;
            double fy = -1;
            double gx = 1;

            while (true)
            {
                double currX = x - item["m"];
                double currY = y + item["m1"];
                double f = Math.Sin(currX) - y - item["n"];
                double g = x + Math.Sin(currY) - item["n1"];
                double fx = Math.Cos(currX);
                double gy = Math.Cos(currY);
                double d = fx* gy - gx* fy;
                double dx = (g * fy - f * gy) / d;
                double dy = (f * gx - g * fx) / d;
                double newX = dx + x;
                double newY = dy + y;

                double faultX = Math.Abs(newX - x);
                double faultY = Math.Abs(newY - y);

                curFault = Math.Max(faultX, faultY);
                x = newX;
                y = newY;

                if(curFault <= item["fault"])
                {
                    break;
                }

                if(i >= 10000000)
                {
                    break;
                }
                i++;
            }
            res.Add("x", x);
            res.Add("y", y);
            return res;
        }

        public Dictionary<string, double> calc(Dictionary<string, double> item)
        {
            return compute(item);
        }
    }
}
