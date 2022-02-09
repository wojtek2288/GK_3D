using GK_3D.DirBitmap;
using GK_3D.Extension;
using GK_3D.Lights;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.FillingPolygon
{
    public static class PixelColoring
    {
        public enum Component
        {
            R,
            G,
            B,
        }
        public static Color ColorPixel(Vector3 PointPos, Vector3 NormalVector, List<ILight> Lights, Color ObjColor, Vector3 CameraPos)
        {
            double ka = 0.7;
            double Rval = Map_0_255_to_0_1(ObjColor.R) * ka;
            double Gval = Map_0_255_to_0_1(ObjColor.G) * ka;
            double Bval = Map_0_255_to_0_1(ObjColor.B) * ka;

            Vector3 pDiff = CameraPos - PointPos;
            Vector3 v = Vector3.Divide(pDiff, (float)Math.Sqrt(Math.Pow(pDiff.X, 2) + Math.Pow(pDiff.Y, 2) + Math.Pow(pDiff.Z, 2)));

            foreach (var light in Lights)
            {
                Vector3 lDiff = light.ProcessedPos - PointPos;
                Vector3 li = Vector3.Divide(lDiff, (float)Math.Sqrt(Math.Pow(lDiff.X, 2) + Math.Pow(lDiff.Y, 2) + Math.Pow(lDiff.Z, 2)));

                Vector3 ri = 2 * Vector3.Dot(NormalVector, li) * NormalVector - li;

                li = Vector3.Normalize(li);
                ri = Vector3.Normalize(ri);
                v = Vector3.Normalize(v);
                
                double dist = 1;

                if (light.isReflector)
                {
                    dist = Math.Pow(Vector3.Distance(light.ProcessedPos, PointPos), 1);
                    dist /= 100;
                }

                var lR = Map_0_255_to_0_1(light.LightColor.R) / dist;
                var lG = Map_0_255_to_0_1(light.LightColor.G) / dist;
                var lB = Map_0_255_to_0_1(light.LightColor.B) / dist;

                if (light.isReflector)
                {
                    lR *= Math.Pow(Math.Max(Vector3.Dot(light.ProcessedDir, li), 0), 10);
                    lG *= Math.Pow(Math.Max(Vector3.Dot(light.ProcessedDir, li), 0), 10);
                    lB *= Math.Pow(Math.Max(Vector3.Dot(light.ProcessedDir, li), 0), 10);
                }

                Rval += light.kd * lR * Math.Max(Vector3.Dot(NormalVector, li), 0) +
                    light.ks * lR * Math.Pow(Math.Max(Vector3.Dot(v, ri), 0), light.m);

                Gval += light.kd * lG * Math.Max(Vector3.Dot(NormalVector, li), 0) +
                    light.ks * lG * Math.Pow(Math.Max(Vector3.Dot(v, ri), 0), light.m);

                Bval += light.kd * lB * Math.Max(Vector3.Dot(NormalVector, li), 0) +
                    light.ks * lB * Math.Pow(Math.Max(Vector3.Dot(v, ri), 0), light.m);
            }

            Color pixelColor = Color.FromArgb(Map_0_1_to_0_255(Rval), Map_0_1_to_0_255(Gval), Map_0_1_to_0_255(Bval));

            float fogDensity = 0.0009f;
            float factor = Vector3.Distance(PointPos, CameraPos) * fogDensity;
            float alpha = (float)(1 / Math.Exp(factor * factor));

            if (alpha > 1)
                alpha = 1;

            Color resCol = pixelColor.Blend(Color.Black, 1 - alpha);

            return resCol;
        }

        public static Color ColorInterpolatedPixel(Vector3 PointPos, Color[] Colors, List<Vector3> Triangle, float triangleDenominator)
        {
            float x = PointPos.X, y = PointPos.Y;
            float x1 = Triangle[0].X, y1 = Triangle[0].Y;
            float x2 = Triangle[1].X, y2 = Triangle[1].Y;
            float x3 = Triangle[2].X, y3 = Triangle[2].Y;

            float W1 = ((y2 - y3) * (x - x3) + (x3 - x2) * (y - y3)) / triangleDenominator;
            float W2 = ((y3 - y1) * (x - x3) + (x1 - x3) * (y - y3)) / triangleDenominator;
            float W3 = 1 - W1 - W2;

            double ColorR = Map_0_255_to_0_1((int)(W1 * Colors[0].R + W2 * Colors[1].R + W3 * Colors[2].R));
            double ColorG = Map_0_255_to_0_1((int)(W1 * Colors[0].G + W2 * Colors[1].G + W3 * Colors[2].G));
            double ColorB = Map_0_255_to_0_1((int)(W1 * Colors[0].B + W2 * Colors[1].B + W3 * Colors[2].B));

            return Color.FromArgb(Map_0_1_to_0_255(ColorR), Map_0_1_to_0_255(ColorG), Map_0_1_to_0_255(ColorB));
        }

        public static Vector3 CalculateMultiplication(Vector3[] matrix, Vector3 vec)
        {
            float sum;
            Vector3 res = new Vector3();

            for (int i = 0; i < 3; i++)
            {
                sum = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (j == 0)
                        sum += matrix[i].X * vec.X;
                    else if (j == 1)
                        sum += matrix[i].Y * vec.Y;
                    else if (j == 2)
                        sum += matrix[i].Z + vec.Z;
                }
                if (i == 0)
                    res.X = sum;
                else if (i == 1)
                    res.Y = sum;
                else if (i == 2)
                    res.Z = sum;
            }

            return Vector3.Normalize(res);
        }

        public static double CalcCos(Vector3 N, Vector3 L)
        {
            double res = N.X * L.X + N.Y * L.Y + N.Z * L.Z;
            return res > 0 ? res : 0;
        }

        public static double Map_0_255_to_0_1(int RGB)
        {
            double ret = (double)RGB / 255;
            if (ret > 1) return 1;
            if (ret < 0) return 0;
            return ret;
        }

        private static double map(double value, double from, double to)
        {
            if (value > to)
                return to;
            else if (value < to)
                return from;
            else
                return value / to;
        }

        public static int Map_0_1_to_0_255(double val)
        {
            int ret = (int)(val * 255);
            if (ret > 255) return 255;
            if (ret < 0) return 0;
            return ret;
        }
    }
}
