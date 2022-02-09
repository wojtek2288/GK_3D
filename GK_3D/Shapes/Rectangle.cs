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
        private List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)> rectangle { get; set; }
        private Vector3 Center;
        public Vector3 ShapeCenter { get => Center; set => Center = value; }
        public ModelMatrix _ModelMatrix { get;}

        public Rectangle(float x, float y, float z, Color UpColor, Color SidesColor)
        {
            this.Center = new Vector3(x/2, y/2, z/2);
            this._ModelMatrix = new ModelMatrix();

            rectangle = new List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)>();

            Vector4 frontNormal = Vector4.Normalize(new Vector4(1, 0, 0, 0));
            Vector4 behindNormal = Vector4.Normalize(new Vector4(-1, 0, 0, 0));
            Vector4 leftNormal = Vector4.Normalize(new Vector4(0, -1, 0, 0));
            Vector4 rightNormal = Vector4.Normalize(new Vector4(0, 1, 0, 0));
            Vector4 upperNormal = Vector4.Normalize(new Vector4(0, 0, 1, 0));
            Vector4 downNormal = Vector4.Normalize(new Vector4(0, 0, -1, 0));

            //Front face
            rectangle.Add((new Vector4(x, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(x, 0, z, 1), SidesColor, frontNormal));
            rectangle.Add((new Vector4(x, 0, z, 1), new Vector4(x, y, z, 1), new Vector4(x, y, 0, 1), SidesColor, frontNormal));

            //Behind face
            rectangle.Add((new Vector4(0, 0, 0, 1), new Vector4(0, y, 0, 1), new Vector4(0, 0, z, 1), SidesColor, behindNormal));
            rectangle.Add((new Vector4(0, 0, z, 1), new Vector4(0, y, z, 1), new Vector4(0, y, 0, 1), SidesColor, behindNormal));

            //Left face
            rectangle.Add((new Vector4(x, 0, 0, 1), new Vector4(0, 0, 0, 1), new Vector4(x, 0, z, 1), SidesColor, leftNormal));
            rectangle.Add((new Vector4(0, 0, 0, 1), new Vector4(x, 0, z, 1), new Vector4(0, 0, z, 1), SidesColor, leftNormal));

            //Right face
            rectangle.Add((new Vector4(x, y, 0, 1), new Vector4(0, y, 0, 1), new Vector4(x, y, z, 1), SidesColor, rightNormal));
            rectangle.Add((new Vector4(x, y, z, 1), new Vector4(0, y, z, 1), new Vector4(0, y, 0, 1), SidesColor, rightNormal));

            //Upper face
            rectangle.Add((new Vector4(x, 0, z, 1), new Vector4(0, 0, z, 1), new Vector4(x, y, z, 1), UpColor, upperNormal));
            rectangle.Add((new Vector4(0, 0, z, 1), new Vector4(x, y, z, 1), new Vector4(0, y, z, 1), UpColor, upperNormal));

            //Down face
            rectangle.Add((new Vector4(x, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(0, 0, 0, 1), SidesColor, downNormal));
            rectangle.Add((new Vector4(0, 0, 0, 1), new Vector4(x, y, 0, 1), new Vector4(0, y, 0, 1), SidesColor, downNormal));
        }

        public void MoveByVector(Vector3 vec)
        {
            _ModelMatrix.Translate(vec);

            ShapeCenter = new Vector3(Center.X + vec.X, Center.Y + vec.Y, Center.Z + vec.Z); 
        }

        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)> GetShape()
        {
            return rectangle;
        }

        public void RotateX(float angle)
        {
            _ModelMatrix.Translate(-1 * Center);
            _ModelMatrix.RotateX(angle);
            _ModelMatrix.Translate(Center);
        }

        public void RotateY(float angle)
        {
            _ModelMatrix.Translate(-1 * Center);
            _ModelMatrix.RotateY(angle);
            _ModelMatrix.Translate(Center);
        }
        public void RotateZ(float angle)
        {
            _ModelMatrix.Translate(-1 * Center);
            _ModelMatrix.RotateZ(angle);
            _ModelMatrix.Translate(Center);
        }

    }
}
