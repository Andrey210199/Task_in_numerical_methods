using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessSqruare
{
    class LessSqruare
    {

        private Dictionary<string,double> calcSumm(double[,] matrix)
        {
            Dictionary<string, double> summ = new Dictionary<string, double>();
            int len = matrix.GetLength(0);
            summ.Add("i", len);
            summ.Add("x", 0);
            summ.Add("y", 0);
            summ.Add("x^2", 0);
            summ.Add("xy", 0);

            for(int i = 0; i < len; i++)
            {
                summ["x"] += matrix[i,0];
                summ["y"] += matrix[i,1];
                summ["x^2"] += Math.Pow(matrix[i, 0],2);
                summ["xy"] += matrix[i, 0] * matrix[i, 1];
            }


            return summ;
        }

        private List<double> calcAB(Dictionary<string, double> summ)
        {
            List<double> ab = new List<double>();
            double c = summ["i"] * summ["x^2"] - Math.Pow(summ["x"],2);
            double a = (summ["y"] * summ["x^2"] - summ["x"] * summ["xy"])/c;
            double b = (summ["i"]* summ["xy"] - summ["x"] * summ["y"])/c;
            ab.AddRange([a,b]);

            return ab;
        }

        private Dictionary<string, List<double>> calcY(double[,] matrix, List<double> ab)
        {
            Dictionary<string, List<double>> res = new Dictionary<string, List<double>>();
            res.Add("x", new List<double>());
            res.Add("y", new List<double>());
            int len = matrix.GetLength(0);

            for(int i = 0; i < len; i++)
            {
                double y = ab[0] + ab[1]*matrix[i,0];

                res["x"].Add(matrix[i, 0]);
                res["y"].Add(y);
            }

            return res;
        }

        public Dictionary<string, List<double>> calc(double[,] matrix)
        {
            var summ = calcSumm(matrix);
            var ab = calcAB(summ);
            return calcY(matrix, ab);
        }
    }
}
