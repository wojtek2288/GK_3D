using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Shapes
{
    internal class Cube : IShape
    {
        private int x { get; set; }
        public Cube(int x)
        {
            this.x = x;
        }
        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> GetShape()
        {
            List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> cube = new List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)>();

            //Front face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(x, x, 0, 1), new Vector4(x, 0, x, 1), Color.Brown));
            cube.Add((new Vector4(x, 0, x, 1), new Vector4(x, x, x, 1), new Vector4(x, x, 0, 1), Color.Brown));

            //Behind face
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(0, x, 0, 1), new Vector4(0, 0, x, 1), Color.Brown));
            cube.Add((new Vector4(0, 0, x, 1), new Vector4(0, x, x, 1), new Vector4(0, x, 0, 1), Color.Brown));

            //Left face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(x, 0, x, 1), Color.Brown));
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(x, 0, x, 1), new Vector4(0, 0, x, 1), Color.Brown));

            //Right face
            cube.Add((new Vector4(x, x, 0, 1), new Vector4(0, x, 0, 1), new Vector4(x, x, x, 1), Color.Brown));
            cube.Add((new Vector4(x, x, x, 1), new Vector4(0, x, x, 1), new Vector4(0, x, 0, 1), Color.Brown));

            //Upper face
            cube.Add((new Vector4(x, 0, x, 1), new Vector4(0, 0, x, 1), new Vector4(x, x, x, 1), Color.Green));
            cube.Add((new Vector4(0, 0, x, 1), new Vector4(x, x, x, 1), new Vector4(0, x, x, 1), Color.Green));

            //Down face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(x, x, 0, 1), new Vector4(0, 0, 0, 1), Color.Brown));
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(x, x, 0, 1), new Vector4(0, x, 0, 1), Color.Brown));

            return cube;
        }
    }
}
