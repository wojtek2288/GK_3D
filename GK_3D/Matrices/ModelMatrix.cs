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
        public float angleZ { get; private set; }
        public ModelMatrix()
        {
            this.Matrix = new Matrix4x4(1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1);
        }

        public void Translate(Vector3 vec)
        {
            Matrix4x4 TranslationMatrix = new Matrix4x4(1, 0, 0, vec.X,
                                                        0, 1, 0, vec.Y,
                                                        0, 0, 1, vec.Z,
                                                        0, 0, 0, 1);

            this.Matrix = Matrix4x4.Multiply(TranslationMatrix, this.Matrix);
        }

        public void RotateX(float angle)
        {
            this.angleX = angle;

            Matrix4x4 RotationMatrix = new Matrix4x4(1, 0, 0, 0,
                                        0, (float)(Math.Cos(Math.PI*angle/180)), (float)(-Math.Sin(Math.PI * angle /180)), 0,
                                        0, (float)(Math.Sin(Math.PI * angle /180)), (float)(Math.Cos(Math.PI * angle /180)), 0,
                                        0f, 0f, 0f, 1);

            this.Matrix = Matrix4x4.Multiply(RotationMatrix, this.Matrix);
        }

        public void RotateY(float angle)
        {
            this.angleY = angle;
            Matrix4x4 RotationMatrix = new Matrix4x4((float)Math.Cos(Math.PI*angle/180), 0, (float)(Math.Sin(Math.PI * angle / 180)), 0,
                                        0, 1, 0, 0,
                                        (float)(-Math.Sin(Math.PI * angle / 180)), 0, (float)(Math.Cos(Math.PI * angle / 180)), 0,
                                        0f, 0f, 0f, 1);

            this.Matrix = Matrix4x4.Multiply(RotationMatrix, this.Matrix);
        }

        public void RotateZ(float angle)
        {
            this.angleZ = angle;

            Matrix4x4 RotationMatrix = new Matrix4x4((float)Math.Cos(Math.PI * angle / 180), (float)(-Math.Sin(Math.PI * angle / 180)), 0, 0,
                                        (float)(Math.Sin(Math.PI * angle / 180)), (float)(Math.Cos(Math.PI * angle / 180)), 0, 0,
                                        0, 0, 1, 0,
                                        0f, 0f, 0f, 1);

            this.Matrix = Matrix4x4.Multiply(RotationMatrix, this.Matrix);
        }
    }
}
