using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Slau
{
    class Slau
    {
        private double[,] invertMatrix(double[,] matrix, int collumn)
        {
            int n = matrix.GetLength(0);
            double[,] augmented = new double[n, n*2];

            // Initialize augmented matrix with the input matrix and the identity matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = matrix[i, j];
                    augmented[i, j + n] = (i == j) ? 1 : 0;
                }
            }

            // Apply Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                int pivotRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(augmented[j, i]) > Math.Abs(augmented[pivotRow, i]))
                    {
                        pivotRow = j;
                    }
                }

                if (pivotRow != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = augmented[i, k];
                        augmented[i, k] = augmented[pivotRow, k];
                        augmented[pivotRow, k] = temp;
                    }
                }

                if (Math.Abs(augmented[i, i]) < 1e-10)
                {
                    return new double[,] { { } };
                }

                double pivot = augmented[i, i];
                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= pivot;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = augmented[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            augmented[j, k] -= factor * augmented[i, k];
                        }
                    }
                }
            }

            double[,] result = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, j + n];
                }
            }

            return result;
            
        }

        private Dictionary<string, double> multiplyMatrix(double[,] a, double[,] b)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            int r = a.GetLength(0);
            int c = a.GetLength(1);
            int m = b.GetLength(1)-1;
             
            for(int i=0; i< r; i++)
            {
                result.Add($"x{i + 1}", 0);
                for(int j=0; j<c; j++)
                {
                  result[$"x{i+1}"] += a[i,j]*b[j,m];

                }
            }

            return result;
        }
        public Dictionary<string, double> calc(double[,] matrix,int collumn)
        {
            var text =  invertMatrix(matrix, collumn);
            return multiplyMatrix(text, matrix);
        }
    }
}
