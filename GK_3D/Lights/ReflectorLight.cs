using GK_3D.Matrices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GK_3D.Extension;

namespace GK_3D.Lights
{
    internal class ReflectorLight : ILight
    {
        public double kd { get; set; }
        public double ks { get; set; }
        public int m { get; set; }
        public Color LightColor { get; set; }
        public Vector4 LightPos { get; set; }
        public bool isReflector { get; set; }
        public Vector4 ReflectorDirection { get; set; }
        public Vector3 ProcessedDir { get; set; }
        public Vector3 ProcessedPos { get; set; }
        public Vector4 RotatedDir { get; set; }
        public ModelMatrix _ModelMatrix { get; set; }

        public ReflectorLight(Vector4 LightPos, Vector4 RefDir, Color LightColor, double kd = 0.5, double ks = 0.5, int m = 10)
        {
            this.LightPos = LightPos;
            this.LightColor = LightColor;
            this.kd = kd;
            this.ks = ks;
            this.m = m;
            this.isReflector = true;
            this.ReflectorDirection = RefDir;

            this._ModelMatrix = new ModelMatrix();
        }
        public void RotateZ(float angle)
        {
            Vector4 res = ReflectorDirection.ApplyMatrix(new Matrix4x4((float)Math.Cos(Math.PI * angle / 180), (float)(-Math.Sin(Math.PI * angle / 180)), 0, 0,
                                        (float)(Math.Sin(Math.PI * angle / 180)), (float)(Math.Cos(Math.PI * angle / 180)), 0, 0,
                                        0, 0, 1, 0,
                                        0f, 0f, 0f, 1));

            this.RotatedDir = Vector4.Normalize(res);
        }

        public void RotateX(float angle)
        {
            Vector4 res = ReflectorDirection.ApplyMatrix(new Matrix4x4(1, 0, 0, 0,
                                        0, (float)(Math.Cos(Math.PI * angle / 180)), (float)(-Math.Sin(Math.PI * angle / 180)), 0,
                                        0, (float)(Math.Sin(Math.PI * angle / 180)), (float)(Math.Cos(Math.PI * angle / 180)), 0,
                                        0f, 0f, 0f, 1));

            this.RotatedDir = Vector4.Normalize(res);
        }

        public void RotateY(float angle)
        {
            Vector4 res = ReflectorDirection.ApplyMatrix(new Matrix4x4((float)Math.Cos(Math.PI * angle / 180), 0, (float)(Math.Sin(Math.PI * angle / 180)), 0,
                                        0, 1, 0, 0,
                                        (float)(-Math.Sin(Math.PI * angle / 180)), 0, (float)(Math.Cos(Math.PI * angle / 180)), 0,
                                        0f, 0f, 0f, 1));

            this.RotatedDir = Vector4.Normalize(res);
        }
    }
}
