using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlauRun
{
    class SlauRun
    {
        private double[,] transformMatrix( double[,] matrix)
        {
            double[,] newMatrix = new double[matrix.GetLength(0), matrix.GetLength(1)-1];
            int length = matrix.GetLength(0);
            for(int i = 0; i< length; i++)
            {
                for(int j = 0; j<length; j++)
                {
                    
                    if (matrix[i,j] !=0)
                    {
                        if(i==0)
                        {
                            newMatrix[0, 0] = 0;
                            newMatrix[i, j+1] = matrix[i, j];
                        }
                        else if(i == length-1)
                        {
                            newMatrix[i, j-2] = matrix[i, j];
                            newMatrix[i, 2] = 0;
                        }
                        else if(i ==1)
                        {
                            newMatrix[i, j] = matrix[i, j];
                        }
                        else
                        {
                            newMatrix[i, j-1] = matrix[i, j];
                        }
                       
                    }
                    newMatrix[i, length-1] = matrix[i, length];
                }
            }

            return newMatrix;
        }

        private double[,] uV(double[,] matrix)
        {
            double[,] uv = new double[2, matrix.GetLength(0)];
            int length = matrix.GetLength(0);
           
            for(int i = 0; i< length; i++)
            {

                if(i==0)
                {
                    uv[0, i] = - matrix[i, 2]/ matrix[i,1];
                    uv[1, i] = matrix[i, 3] / matrix[i, 1];
                }
                else
                {
                    uv[0,i] = -matrix[i, 2] / (matrix[i, 0] * uv[0,i-1] + matrix[i,1]);
                    uv[1, i] = (matrix[i, 3] - matrix[i, 0] * uv[1, i - 1])/ (matrix[i, 0] * uv[0, i - 1] + matrix[i, 1]);
                }

            }

            return uv;
        }

        private Dictionary<string, double> calcX(double[,] uv)
        {
            Dictionary<string, double> res = new Dictionary<string, double>();
            int len = uv.GetLength(1)-1;
            double preX = 1;

            for (int i = len; i>=0;i--)
            {
                double x = preX * uv[0, i] + uv[1, i];
                preX = x;
                res.Add($"x{i+1}", x);
            }
            return res;
        }

        public Dictionary<string, double> calc(double[,] matrix)
        {
            double[,] newMatrix = transformMatrix(matrix);
            double[,] uv = uV(newMatrix);
            return calcX(uv);
        }
    }
}
