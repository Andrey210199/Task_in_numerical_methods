using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Polinom
{
    class Polinom
    {
        double[,] Matrix;
        public Polinom(double[,] matrix) 
        { 
            Matrix = matrix;
        }

        private List<double> deltaCalc()
        {
            List<double> delta = new List<double>();
            int elemCount = Matrix.GetLength(0) - 2;

            for (int i = 0; i < elemCount; i++)
            {
                double dO = (Matrix[i, 1] - Matrix[i+1, 1]) / (Matrix[i, 0] - Matrix[i+1, 0]);
                delta.Add(dO);
            }
            double dT = (delta[0] - delta[1]) / (Matrix[0, 0] - Matrix[2, 0]);
            delta.Add(dT);
            return delta;
        }

        private List<double> nXcalc(List<double> delta)
        {
            List<double> nX = new List<double>();
            int length = Matrix.GetLength(0)-1;

            for(int i = 0; i< length; i++)
            {
                double dO = delta[0] * (Matrix[i, 0] - Matrix[0, 0]);
                double dT = delta[2] * (Matrix[i, 0] - Matrix[0, 0]) * (Matrix[i, 0] - Matrix[1, 0]);
                double currNx = Matrix[0, 1] + dO + dT;
                nX.Add(currNx);
            }

            return nX;
        }

        private Dictionary<string, List<double>> res(List<double> nX)
        {
            Dictionary<string, List<double>> resu = new Dictionary<string, List<double>>();
            resu.Add("x", new List<double>());
            resu.Add("y", new List<double>());

            for(int i = 0; i< nX.Count; i++)
            {
                resu["x"].Add(Matrix[i, 0]);
                resu["y"].Add(nX[i]);
            }

            return resu;
        }

        public Dictionary<string, List<double>> calc()
        {
            List<double> delta = deltaCalc();
            List<double> nX = nXcalc(delta);
            return res(nX);
            
        }
    }
}
