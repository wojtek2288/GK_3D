using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Matrices
{
    public class ModelMatrix
    {
        public Matrix4x4 Matrix { get; private set; }
        public float angleX { get; private set; }
        public float angleY { get; private set; }
        public ModelMatrix()
        {
            this.Matrix = new Matrix4x4(1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1);
        }

        public void RotateX(float angle)
        {
            this.angleX = angle;
            this.Matrix = new Matrix4x4((float)(Math.Cos(Math.PI * angle / 180.0)), (float)(-1 * Math.Sin(Math.PI * angle / 180.0)), 0f, 0.1f,
                                        (float)(Math.Sin(Math.PI * angle / 180.0)), (float)(Math.Cos(Math.PI * angle / 180.0)), 0f, 0.2f,
                                        0f, 0f, 1f, 0.3f,
                                        0f, 0f, 0f, 1);

        }

        public void RotateY(float angle)
        {
            this.angleY = angle;
            this.Matrix = new Matrix4x4(1f, 0f, 0f, -0.1f,
                                        0f, (float)(Math.Cos(2 * Math.PI * angle / 180.0)), (float)(-1 * Math.Sin(2 * Math.PI * angle / 180.0)), -0.2f,
                                        0f, (float)(Math.Sin(2 * Math.PI * angle / 180.0)), (float)(Math.Cos(2 * Math.PI * angle / 180.0)), 0.3f,
                                        0f, 0f, 0f, 1);
        }
    }
}
