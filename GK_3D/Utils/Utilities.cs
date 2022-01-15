using GK_3D.Matrices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_3D.Utils
{
    public static class Utilities
    {
        public static Vector4 Multiply(Matrix4x4 matrix, Vector4 self)
        {
            return new Vector4(
                matrix.M11 * self.X + matrix.M12 * self.Y + matrix.M13 * self.Z + matrix.M14 * self.W,
                matrix.M21 * self.X + matrix.M22 * self.Y + matrix.M23 * self.Z + matrix.M24 * self.W,
                matrix.M31 * self.X + matrix.M32 * self.Y + matrix.M33 * self.Z + matrix.M34 * self.W,
                matrix.M41 * self.X + matrix.M42 * self.Y + matrix.M43 * self.Z + matrix.M44 * self.W
            );
        }

        public static Vector4 ProjectPoint(Vector4 point, ModelMatrix _ModelMatrix, ViewMatrix _ViewMatrix, ProjectionMatrix _ProjectionMatrix)
        {
            Vector4 projectedPoint = Multiply(_ProjectionMatrix.Matrix, Multiply(_ViewMatrix.Matrix, Multiply(_ModelMatrix.Matrix, point)));
            projectedPoint.X = projectedPoint.X / projectedPoint.W;
            projectedPoint.Y = projectedPoint.Y / projectedPoint.W;

            return projectedPoint;
        }

        public static Vector3 ConvertToPictureBox(Vector4 projectedPoint, PictureBox mainPictureBox)
        {
            int x = (int)Math.Round(mainPictureBox.Width / 2 + mainPictureBox.Width / 2 * (projectedPoint.X));
            int y = (int)Math.Round(mainPictureBox.Height / 2 - mainPictureBox.Height / 2 * (projectedPoint.Y));

            return new Vector3(x, y, projectedPoint.Z);
        }

        public static void SetZbufor(double[,] Zbufor)
        {
            for(int i = 0; i < Zbufor.GetLength(0); i++)
            {
                for(int j = 0; j < Zbufor.GetLength(1); j++)
                {
                    Zbufor[i, j] = double.MaxValue;
                }
            }
        }
    }
}
