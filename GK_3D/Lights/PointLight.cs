using GK_3D.Matrices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Lights
{
    class PointLight : ILight
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
        public ModelMatrix _ModelMatrix { get; set; }
        public Vector4 RotatedDir { get; set; }

        public PointLight(Vector4 LightPos, Color LightColor, double kd = 0.2, double ks = 0.2, int m = 10)
        {
            this.LightPos = LightPos;
            this.LightColor = LightColor;
            this.kd = kd;
            this.ks = ks;
            this.m = m;
            this.isReflector = false;

            this._ModelMatrix = new ModelMatrix();
        }
    }
}
