using GK_3D.Matrices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Shapes
{
    internal interface IShape
    {
        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)> GetShape();
        //TODO Use matrix multiplication instead of vector moving
        public void MoveByVector(Vector3 vec);
        public void RotateX(float angle);
        public Vector3 ShapeCenter { get; set; }
        public ModelMatrix _ModelMatrix { get;}
    }
}
