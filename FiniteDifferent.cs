using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteDifferent
{
    class FiniteDifferent
    {
        private void addDictionary(ref Dictionary<string, List<double>> di, double a, double b, double c, double d)
        {
            di["a"].Add(a);
            di["b"].Add(b);
            di["c"].Add(c);
            di["d"].Add(d);
        }
        private Dictionary<string, List<double>> calcABCD(double[] mc, double[,] xyArr, double h)
        {
            Dictionary<string, List<double>> abcd = new Dictionary<string, List<double>>();
            abcd.Add("a", new List<double>());
            abcd.Add("b", new List<double>());
            abcd.Add("c", new List<double>());
            abcd.Add("d", new List<double>());
            double pX = -1/mc[0];
            double i = xyArr[1,0]+h;
            double hS = Math.Pow(h,2);
            double mH = -1 / h;
            double pH = 1 / h;
            addDictionary(ref abcd, 0, mH, pH, xyArr[1,1]);

            while (i < xyArr[0,0] - h)
            {
                double a = 1 / hS - pX * (2 / h);
                double b = -2 / hS + i;
                double c = 1 / hS + pX * (2 / h);
                addDictionary(ref abcd, a,b,c, mc[1]);
                i += h;
            }
            addDictionary(ref abcd, mH, pH, 0, xyArr[0, 1]);
            return abcd;
        }

        private Dictionary<string, List<double>> calcUV(Dictionary<string, List<double>> abcd)
        {
            Dictionary<string, List<double>> uv = new Dictionary<string, List<double>>();
            uv.Add("u", new List<double>());
            uv.Add("v", new List<double>());
            double preU = 0;
            double preV = 0;

            for (int i = 0; i < abcd["a"].Count; i++)
            {
                double currU = -abcd["c"][i] / (abcd["a"][i]* preU + abcd["b"][i]);
                double currV = (abcd["d"][i] - abcd["a"][i]*preV) / (abcd["a"][i]* preU + abcd["b"][i]);
                uv["u"].Add(currU);
                uv["v"].Add(currV);
                preU = currU;
                preV = currV;
            }

            return uv;
        }

        private Dictionary<string, List<double>> calcXY(Dictionary<string, List<double>> uv, double[,] xyArr, double h)
        {
            Dictionary<string, List<double>> xy = new Dictionary<string, List<double>>();
            xy.Add("x", new List<double>());
            xy.Add("y", new List<double>());
            int n = (int) ((xyArr[0,0] - xyArr[1,0])/h);
            double currY = 0;
            double currX = xyArr[0,0];
            
            for(int i = n; i>=0; i--)
            {
                currY = uv["u"][i] * currY + uv["v"][i];
                xy["x"].Add(currX);
                xy["y"].Add(currY);
                currX -= h;
            }

            xy["x"].Reverse();
            xy["y"].Reverse();
            return xy;

        }

        public Dictionary<string, List<double>> calc(double[] mc, double[,] xyArr, double h)
        {
            var abcd = calcABCD(mc,xyArr, h);
            var uv = calcUV(abcd);
            return calcXY(uv, xyArr, h);
        }
    }
}
