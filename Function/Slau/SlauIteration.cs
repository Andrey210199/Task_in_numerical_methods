using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlauIteration
{
    class SlauIteration
    {
        double[,] Matrix;
        double Fault;
        double[] xArr;
       public SlauIteration(double[,] matrix, double fault, double x1, double x2, double x3, double x4)
        {
            Matrix = matrix;
            Fault = fault;
            xArr = [x1, x2, x3, x4];
        }
        private double[] calcX()
        {
            double[] curX = xArr;
            double curFault = 1;
            int i = 0;

            while(curFault >= Fault)
            {
                double x1 = (Matrix[0, 4] - Matrix[0, 1] * curX[1]- Matrix[0, 2] * curX[2]- Matrix[0, 3] * curX[3]) / Matrix[0,0];
                double x2 = (Matrix[1,4] - Matrix[1,0]*x1 - Matrix[1, 2] * curX[2] - Matrix[1,3]* curX[3]) / Matrix[1, 1];
                double x3 = (Matrix[2, 4] - Matrix[2, 0] * x1 - Matrix[2, 1] * x2 - Matrix[2, 3] * curX[3]) / Matrix[2, 2];
                double x4 = (Matrix[3, 4] - Matrix[3, 0] * x1 - Matrix[3, 1] * x2 - Matrix[3, 2] * x3) / Matrix[3, 3];
                double []fX = new double[4];
                fX[0] = Math.Abs(x1 - curX[0]);
                fX[1] = Math.Abs(x2 - curX[1]);
                fX[2] = Math.Abs(x3 - curX[2]);
                fX[3] = Math.Abs(x4 - curX[3]);

                curFault = fX.Max();
                curX = [x1, x2, x3, x4];

                if (i==100000)
                {
                    break;
                }
                i++;
            }
            return curX;
        }

        public Dictionary<string, double> calc()
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            double[] xArr = calcX();

            for (int i = 0; i < xArr.Length; i++)
            {
                res.Add($"x{i+1}",xArr[i]);
            }

            return res;

        }
    }
}
