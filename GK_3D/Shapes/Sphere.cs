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
    public class Sphere : IShape
    {
        private List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)> sphere { get; set; }
        /// <summary>
        /// Creates 3D mesh of sphere
        /// Center of sphere is in (0, 0, 0)
        /// </summary>
        /// <param name="m">Parallel count</param>
        /// <param name="n">Approximation of each parallel</param>
        /// <param name="r">Radius of sphere</param>
        /// 
        private Vector3 Center;
        public float Radius;
        public Vector3 ShapeCenter { get => Center; set => Center = value; }
        public ModelMatrix _ModelMatrix { get;}

        public Sphere(int meridians, int parallels, float r, Color col)
        {
            Center = new Vector3(0f, 0f, 0f);
            Radius = r;
            this._ModelMatrix = new ModelMatrix();

            sphere = new List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)>();
            List<Vector4> vertices = new List<Vector4>();

            vertices.Add(new Vector4(0f, r, 0f, 1f));
            
            for(int j = 0; j < parallels - 1; j++)
            {
                double polar = Math.PI * (double)(j + 1) / (double)(parallels);
                double sp = Math.Sin(polar);
                double cp = Math.Cos(polar);

                for(int i = 0; i < meridians; i++)
                {
                    double azimuth = 2.0 * Math.PI * (double)(i) / (double)(meridians);
                    double sa = Math.Sin(azimuth);
                    double ca = Math.Cos(azimuth);

                    float x = (float)(r * sp * ca);
                    float y = (float)(r * cp);
                    float z = (float)(r * sp * sa);
                    vertices.Add(new Vector4(x, y, z, 1));
                }
            }

            vertices.Add(new Vector4(0f, -r, 0f, 1));

            for(int i = 0; i < meridians; i++)
            {
                int a = i + 1;
                int b = (i + 1) % meridians + 1;
                sphere.Add((vertices[0], vertices[b], vertices[a], col, GetNormal(vertices[0], vertices[b], vertices[a])));
            }

            for(int j = 0; j < parallels - 2; j++)
            {
                int aStart = j*meridians + 1;
                int bStart = (j + 1) * meridians + 1;

                for(int i = 0; i < meridians; i++)
                {
                    int a = aStart + i;
                    int a1 = aStart + (i + 1) % meridians;
                    int b = bStart + i;
                    int b1 = bStart + (i + 1) % meridians;
                    //Add Quad
                    sphere.Add((vertices[a], vertices[a1], vertices[b1], col, GetNormal(vertices[a], vertices[a1], vertices[b1])));
                    sphere.Add((vertices[a], vertices[b1], vertices[b], col, GetNormal(vertices[a], vertices[b1], vertices[b])));
                }
            }

            for(int i = 0; i < meridians; i++)
            {
                int a = i + meridians * (parallels - 2) + 1;
                int b = (i + 1)%meridians + meridians*(parallels - 2) + 1;
                sphere.Add((vertices[vertices.Count - 1], vertices[a], vertices[b], col, GetNormal(vertices[vertices.Count - 1], vertices[a], vertices[b])));
            }
        }

        public List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col, Vector4 normal)> GetShape()
        {
            return sphere;
        }

        public void MoveByVector(Vector3 vec)
        {
            _ModelMatrix.Translate(vec);
            Center = new Vector3(Center.X + vec.X, Center.Y + vec.Y, Center.Z + vec.Z);
        }

        public void RotateX(float angle)
        {
            _ModelMatrix.Translate(-1 * Center);
            _ModelMatrix.RotateX(angle);
            _ModelMatrix.Translate(Center);
        }

        private Vector4 GetNormal(Vector4 v1, Vector4 v2, Vector4 v3)
        {
            Vector3 a = new Vector3(v1.X, v1.Y, v1.Z);
            Vector3 b = new Vector3(v2.X, v2.Y, v2.Z);
            Vector3 c = new Vector3(v3.X, v3.Y, v3.Z);

            var dir = Vector3.Cross(b - a, c - a);
            var norm = Vector3.Normalize(dir);
            return new Vector4(norm.X, norm.Y, norm.Z, 0);
        }
    }
}
