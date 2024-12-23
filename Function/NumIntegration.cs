using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NumIntegration
{
    class NumIntegration
    {
        private double calcH(int n, double[] abcmm1)
        {
            return (abcmm1[1] - abcmm1[0]) / n;
        }

        private double calcFx(double[] abcmm1, double x)
        {
            return abcmm1[3] * Math.Pow(x, abcmm1[2]) - abcmm1[4] + Math.Sqrt(x);
        }

        private Dictionary<string, List<double>> calcXFX(int n, double[] abcmm1, double h)
        {
            Dictionary<string, List<double>> xFx = new Dictionary<string, List<double>>();
            xFx.Add("x", new List<double>());
            xFx.Add("fx", new List<double>());
            xFx.Add("summFx", new List<double>());
            xFx["summFx"].Add(0);
            double x = abcmm1[0];

            for(int i=0; i<=n; i++)
            {
                double fx = calcFx(abcmm1, x);
                xFx["x"].Add(x);
                xFx["fx"].Add(fx);
                xFx["summFx"][0] += fx;
                x += h;
            }

            return xFx;
        }

        private double leftRightRect(Dictionary<string, List<double>> xFx, double h, int i)
        {
            return (xFx["summFx"][0] - xFx["fx"][i])*h;
        }

        private double calcMiddleRect(Dictionary<string, List<double>> xFx, double[] abcmm1, double h)
        {
            int len = xFx["x"].Count;
            double middleSumm = 0;

            for(int i=0; i<len-1; i++)
            {
                double x = xFx["x"][i] + h / 2;
                middleSumm += calcFx(abcmm1, x);
            }

            return middleSumm *h;
        }

        private double calcTrapezoid(Dictionary<string, List<double>> xFx, double h)
        {
            int len = xFx["fx"].Count-1;
            double start = xFx["fx"][0];
            double end = xFx["fx"][len];
            return ((start+end)/2+ xFx["summFx"][0]-start-end) * h;
        }

        private double calcParabol(Dictionary<string, List<double>> xFx, double h)
        {
            int len =xFx["fx"].Count;
            double summ = 0;

            for(int i=0; i < len; i++)
            {
                if(i==0 || i==len-1)
                {
                    summ += xFx["fx"][i];
                }
                else if(i%2 ==0)
                {
                    summ += 2 * xFx["fx"][i];
                }
                else
                {
                    summ += 4 * xFx["fx"][i];
                }
            }

            return h/3 * summ;
        }

        public Dictionary<string, double> calc(Dictionary<string,double> variables)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            double[] abcmm1 = [ variables["a"], variables["b"], variables["c"], variables["m"], variables["m1"] ];
            int n =(int) variables["n"];

            double h = calcH(n, abcmm1);
            var xFx = calcXFX(n, abcmm1, h);
            double leftRect = leftRightRect(xFx, h, n);
            double rightRect = leftRightRect(xFx, h, 0);
            double middleRect = calcMiddleRect(xFx, abcmm1, h);
            double trapezoid = calcTrapezoid(xFx, h);
            double parab = calcParabol(xFx, h);

            res.Add("Левые прямоугольники", leftRect);
            res.Add("Правые прямоугольники", rightRect);
            res.Add("Средние прямоугольники", middleRect);
            res.Add("Метод трапеций", trapezoid);
            res.Add("Метод парабол", parab);

            return res;
        }
    }
}
