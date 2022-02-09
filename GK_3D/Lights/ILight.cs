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
    public interface ILight
    {
        double kd { get; set; }
        double ks { get; set; }
        int m { get; set; }
        Color LightColor { get; set; }
        Vector4 LightPos { get; set; }
        Vector3 ProcessedPos { get; set; }
        bool isReflector { get; set; }
        Vector3 ProcessedDir { get; set; }
        Vector4 ReflectorDirection { get; set; }
        ModelMatrix _ModelMatrix { get; set; }
        public Vector4 RotatedDir { get; set; }
    }
}
