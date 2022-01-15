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
        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> GetShape();
    }
}
