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
    public class Rectangle : IShape
    {
        private List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> cube { get; set; }
        private Vector3 Center;
        public Vector3 ShapeCenter { get => Center; set => Center = value; }
        public ModelMatrix _ModelMatrix { get;}

        public Rectangle(float x, float y, float z, Color UpColor, Color SidesColor)
        {
            this.Center = new Vector3(x/2, y/2, z/2);
            this._ModelMatrix = new ModelMatrix();

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
            _ModelMatrix.Translate(vec);

            ShapeCenter = new Vector3(Center.X + vec.X, Center.Y + vec.Y, Center.Z + vec.Z); 
        }

        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)> GetShape()
        {
            return cube;
        }

        public void RotateX(float angle)
        {
            _ModelMatrix.Translate(-1 * Center);
            _ModelMatrix.RotateX(_ModelMatrix.angleX + angle);
            _ModelMatrix.Translate(Center);
        }
    }
}
