using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Shapes
{
    public class Rectangle : IShape
    {
        private List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> cube { get; set; }
        private Vector3 Center;
        public Vector3 ShapeCenter { get => Center; set => Center = value; }

        public Rectangle(float x, float y, float z, Color UpColor, Color SidesColor)
        {
            this.Center = new Vector3(0.5f, 5f, 0.5f);

            cube = new List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)>();

            //Front face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(x, 0, z, 1), SidesColor));
            cube.Add((new Vector4(x, 0, z, 1), new Vector4(x, y, z, 1), new Vector4(x, y, 0, 1), SidesColor));

            //Behind face
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(0, y, 0, 1), new Vector4(0, 0, z, 1), SidesColor));
            cube.Add((new Vector4(0, 0, z, 1), new Vector4(0, y, z, 1), new Vector4(0, y, 0, 1), SidesColor));

            //Left face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(x, 0, z, 1), SidesColor));
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(x, 0, z, 1), new Vector4(0, 0, z, 1), SidesColor));

            //Right face
            cube.Add((new Vector4(x, y, 0, 1), new Vector4(0, y, 0, 1), new Vector4(x, y, z, 1), SidesColor));
            cube.Add((new Vector4(x, y, z, 1), new Vector4(0, y, z, 1), new Vector4(0, y, 0, 1), SidesColor));

            //Upper face
            cube.Add((new Vector4(x, 0, z, 1), new Vector4(0, 0, z, 1), new Vector4(x, y, z, 1), UpColor));
            cube.Add((new Vector4(0, 0, z, 1), new Vector4(x, y, z, 1), new Vector4(0, y, z, 1), UpColor));

            //Down face
            cube.Add((new Vector4(x, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(0, 0, 0, 1), SidesColor));
            cube.Add((new Vector4(0, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(0, y, 0, 1), SidesColor));
        }

        public void MoveByVector(Vector3 vec)
        {
            for(int i = 0; i < cube.Count; i++)
            {
                cube[i] = (new Vector4(cube[i].v1.X + vec.X, cube[i].v1.Y + vec.Y, cube[i].v1.Z + vec.Z, 1),
                           new Vector4(cube[i].v2.X + vec.X, cube[i].v2.Y + vec.Y, cube[i].v2.Z + vec.Z, 1),
                           new Vector4(cube[i].v3.X + vec.X, cube[i].v3.Y + vec.Y, cube[i].v3.Z + vec.Z, 1),
                           cube[i].col);
            }

            ShapeCenter = new Vector3(Center.X + vec.X, Center.Y + vec.Y, Center.Z + vec.Z); 
        }

        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> GetShape()
        {
            return cube;
        }
    }
}
