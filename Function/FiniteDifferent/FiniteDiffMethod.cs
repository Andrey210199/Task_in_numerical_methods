using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteDiffMethod
{
    class FiniteDiffMethod
    {
        int N;
        double H;
        double[] StartX;
        double StartY;
        double[] MC;
        double[] XARR;

        public FiniteDiffMethod(double h, double[] startX, double startY, double[] mC) 
        {
            H = h;
            StartX = startX;
            StartY = startY;
            MC = mC;
            double n = (startX[1] - startX[0])/h;
            N = (int) n;
            XARR = calcX();

        }

        private double[] calcX()
        {
            double[] xArr = new double[N+1];
            double x = StartX[0];
            xArr[0] = x;

            for (int i = 1; i <= N; i++)
            {
                xArr[i] = x + H;
                x = xArr[i];
            }
            return xArr;
        }

        private Dictionary<string, List<double>> createDXY()
        {
            Dictionary<string, List<double>> d = new Dictionary<string, List<double>>();
            d.Add("x", new List<double>());
            d.Add("y", new List<double>());
            return d;
        }

        private void addInDXY(ref Dictionary<string, List<double>> d, double x, double y)
        {
            d["x"].Add(x);
            d["y"].Add(y);
        }

        private double calcFX(double x, double y)
        {
            return MC[0]*x + MC[1]*y;
        }

        private double calcHFX(double fx)
        {
            return H * fx;
        }

        private double[] calcFxY(double x, double currY)
        {
            double currFx = calcFX(x, currY);
            double nextY = currY + calcHFX(currFx);
            return [currFx, nextY];
        }

        private Dictionary<string, List<double>> Euler(double[] xArr)
        {
            Dictionary<string, List<double>> euler = createDXY();
            double currY = StartY;

            foreach (double x in xArr)
            {
                addInDXY(ref euler, x, currY);
                currY = calcFxY(x, currY)[1];
            }

            return euler;
        }

        private Dictionary<string, List<double>> modEuler(double[] xArr)
        {
            Dictionary<string, List<double>> mEuler = createDXY();
            double currY = StartY;
            addInDXY(ref mEuler, xArr[0], currY);

            for (int i=1; i<xArr.Length; i++)
            {
                double[] fxPy = calcFxY(xArr[i-1], currY); 

                double pFx = calcFX(xArr[i], fxPy[1]);
                double middleFx = (fxPy[0] + pFx) / 2;
                double nextY =currY + calcHFX(middleFx);

                addInDXY(ref mEuler, xArr[i], nextY);
                currY = nextY;
            }

            return mEuler;
        }

        private Dictionary<string, List<double>> rungeKut(double[] xArr, double h)
        {
            Dictionary<string, List<double>> rKut = createDXY();
            double currY = StartY;

            foreach (double x in xArr)
            {
                double k0 = h*calcFX(x, currY);
                double xMH = x + h/2;
                double k1 = h* calcFX(xMH,(currY + k0 / 2));
                double k2 = h* calcFX(xMH, (currY + k1 / 2));
                double k3 = h* calcFX((x + h), (currY + k2));
                double pY = (k0 + 2 * k1 + 2 * k2 + k3) / 6;
                addInDXY(ref rKut, x, currY);
                currY += pY;
            }

            return rKut;
        }

        public Dictionary<string, List<double>> calcEuler()
        {
            return Euler(XARR);
        }

        public Dictionary<string, List<double>> calcModEuler()
        {
            return modEuler(XARR);
        }

        public Dictionary<string, List<double>> calcRungeKut()
        {
            return rungeKut(XARR, H);
        }

    }
}
