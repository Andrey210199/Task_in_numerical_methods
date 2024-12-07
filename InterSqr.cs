using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterSqr
{
    class InterSqr
    {
        double[,] Matrix;
        double Found;
        public InterSqr(double[,] matrix, double found) 
        { 
            Matrix = matrix;
            Found = found;
        }

        private Dictionary<string, List<double>> calcUVH()
        {
            Dictionary<string, List<double>> uv = new Dictionary<string, List<double>>();
            uv.Add("u", new List<double>());
            uv.Add("v", new List<double>());
            uv.Add("h", new List<double>());     
            double u = 0;
            double v = 0;
            int len = Matrix.GetLength(0);

            for (int i = 2; i< len; i++)
            {
                double preH = Matrix[i-1, 0] - Matrix[i-2, 0];
                double currH = Matrix[i, 0] - Matrix[i-1, 0];
                double b = 2 * (preH + currH);
                double d = 3 * ((Matrix[i, 1] - Matrix[i-1, 1])/currH - (Matrix[i - 1, 1] - Matrix[i-2,1])/preH );
                if(i ==2)
                {
                    u = -currH /b;
                    v = d/b;
                }
                else
                {
                    double dividor = (preH * u + b);
                    double currU = -currH / dividor;
                    double currV = (d - preH *v) / dividor;
                    u= currU;
                    v= currV;
                }
                uv["h"].Add(preH);
                uv["u"].Add(u);
                uv["v"].Add(v);

                if(i ==len-1)
                {
                    uv["h"].Add(currH);
                }
            }

            return uv;
        }

        private double[] calcC(Dictionary<string, List<double>> uv)
        {
            int length = uv["u"].Count;
            double [] cArr = new double[length+2];
            double c = 0;

            for (int i = length-1; i >= 0; i--)
            {
                
                c = uv["u"][i] * c + uv["v"][i];
                cArr[i+1] = c;
            }

            return cArr;
        }

        private double calcY(double[] cArr, Dictionary<string, List<double>> uvh)
        {
            double x = 0;
            double b = 0;
            double d = 0;
            double preX = 0;
            double preY = 0;
            double c = 0;


            for (int i = 1; i < Matrix.GetLength(0); i++)
            {
                 double h = uvh["h"][i-1];
                 double hY = Matrix[1, i] - Matrix[1, i-1];
                 b = hY /h  - (cArr[i]+ 2*cArr[i-1]) * (h / 3);
                 d = (cArr[i] - cArr[i-1]) / (3 / h);
                if (Matrix[0,i] > Found)
                {
                    preX = Matrix[0,i-1];
                    preY = Matrix[1,i-1];
                    c = cArr[i-1];
                    break;
                }
            }

            double yH = Found - preX;
            x = preY + b * yH+ c * Math.Pow(yH, 2) + d * Math.Pow(yH, 3);
            return x;
        }

        private Dictionary<string, List<double>> res(double inte, double x)
        {
            Dictionary<string, List<double>> points = new Dictionary<string, List<double>>();
            points.Add("x", new List<double>());
            points.Add("y", new List<double>());
            bool implant = false;

            for(int i = 0; i<= Matrix.GetLength(0); i++)
            {
               
                 if(!implant && Matrix[i,0] > inte)
                {
                    points["x"].Add(inte);
                    points["y"].Add(x);
                    implant = true;
                }
                else if(!implant)
                {
                    points["x"].Add(Matrix[i, 0]);
                    points["y"].Add(Matrix[i, 1]);
                }
                 else
                {
                    points["x"].Add(Matrix[i-1, 0]);
                    points["y"].Add(Matrix[i-1, 1]);
                }

            }

            return points;
        }

        public Dictionary<string, List<double>> calc()
        {
            
            Dictionary<string, List<double>> uv = calcUVH();
            double[] cArr = calcC(uv);
            double foundY = calcY(cArr, uv);

            return res(Found, foundY);
        }
    }
}
