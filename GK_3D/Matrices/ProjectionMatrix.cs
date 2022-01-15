using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Matrices
{
    public class ProjectionMatrix
    {
        public Matrix4x4 Matrix { get; private set; }
        public float fov { get; private set; }
        private float e { get; set; }
        public float a { get; private set; }
        public float f { get; private set; }
        public float n { get; private set; }

        public ProjectionMatrix(float n, float f, float fov, float a)
        {
            this.fov = fov;
            this.e = (float)(1/Math.Tan(fov*Math.PI/180/2));
            this.a = a;
            this.f = f;
            this.n = n;
            Matrix = new Matrix4x4(e, 0f, 0f, 0f,
                                   0, e / a, 0f, 0f,
                                   0f, 0f, -(f + n) / (f - n), -2f * f * n / (f - n),
                                   0f, 0f, -1f, 0f);
        }

        public void ChangeFov(float fov)
        {
            this.fov = fov;
            this.e = (float)(1 / Math.Tan(fov * Math.PI / 180 / 2));

            Matrix = new Matrix4x4(e, 0f, 0f, 0f,
                       0, e / a, 0f, 0f,
                       0f, 0f, -(f + n) / (f - n), -2f * f * n / (f - n),
                       0f, 0f, -1f, 0f);
        }
    }
}
