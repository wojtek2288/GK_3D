using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Utils
{
    public static class Utilities
    {
        public static double[] Multiply(double[,] matrix1, double[] vector)
        {
            var matrix1Rows = 4;
            var matrix1Cols = 4;
            var matrix2Rows = 4;

            if (matrix1Cols != matrix2Rows)
                throw new InvalidOperationException
                  ("Product is undefined. n columns of first matrix must equal to n rows of second matrix");

            double[] product = new double[4];

            for (int matrix1_row = 0; matrix1_row < matrix1Rows; matrix1_row++)
            {
                for (int matrix1_col = 0; matrix1_col < matrix1Cols; matrix1_col++)
                {
                    product[matrix1_row] +=
                        matrix1[matrix1_row, matrix1_col] *
                        vector[matrix1_col];
                }
            }

            return product;
        }
    }
}
