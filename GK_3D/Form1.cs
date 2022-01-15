using GK_3D.DirBitmap;
using GK_3D.FillingPolygon;
using GK_3D.Matrices;
using GK_3D.Shapes;
using GK_3D.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_3D
{
    public partial class Form1 : Form
    {
        private DirectBitmap PictureBoxBitmap { get; set; }
        private double[,] Zbufor { get; set; }
        private ModelMatrix _ModelMatrix { get; set; }
        private ViewMatrix _ViewMatrix { get; set; }
        private ProjectionMatrix _ProjectionMatrix { get; set; }
        private List<List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)>> Shapes;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBoxBitmap = new DirectBitmap(mainPicturebox.Width, mainPicturebox.Height);
            mainPicturebox.Image = PictureBoxBitmap.Bitmap;

            Shapes = new List<List<(Vector4 v1, Vector4 v2, Vector4 v3, Color col)>>();

            _ModelMatrix = new ModelMatrix();
            _ViewMatrix = new ViewMatrix();
            _ProjectionMatrix = new ProjectionMatrix(1, 100, 60, 1);

            Zbufor = new double[mainPicturebox.Width, mainPicturebox.Height];
            Utilities.SetZbufor(Zbufor);

            Cube cube = new Cube(1);
            Shapes.Add(cube.GetShape());
            mainPicturebox.Refresh();
        }

        private void mainPicturebox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(PictureBoxBitmap.Bitmap);
            g.Clear(Color.White);
            Utilities.SetZbufor(Zbufor);

            foreach (var shape in Shapes)
            {
                //Parallel.For(0, shape.Count, i =>
                //{
                //    var ProjPoint1 = Utilities.ProjectPoint(shape[i].v1, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                //    var Point1 = Utilities.ConvertToPictureBox(ProjPoint1, mainPicturebox);

                //    var ProjPoint2 = Utilities.ProjectPoint(shape[i].v2, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                //    var Point2 = Utilities.ConvertToPictureBox(ProjPoint2, mainPicturebox);

                //    var ProjPoint3 = Utilities.ProjectPoint(shape[i].v3, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                //    var Point3 = Utilities.ConvertToPictureBox(ProjPoint3, mainPicturebox);

                //    Fill.FillPolygon(new List<Vector3>() { Point1, Point2, Point3 }, PictureBoxBitmap, shape[i].col, Zbufor);
                //});
                foreach (var triangle in shape)
                {
                    var ProjPoint1 = Utilities.ProjectPoint(triangle.v1, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point1 = Utilities.ConvertToPictureBox(ProjPoint1, mainPicturebox);

                    var ProjPoint2 = Utilities.ProjectPoint(triangle.v2, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point2 = Utilities.ConvertToPictureBox(ProjPoint2, mainPicturebox);

                    var ProjPoint3 = Utilities.ProjectPoint(triangle.v3, _ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point3 = Utilities.ConvertToPictureBox(ProjPoint3, mainPicturebox);

                    Fill.FillPolygon(new List<Vector3>() { Point1, Point2, Point3 }, PictureBoxBitmap, triangle.col, Zbufor);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            float mov = 0.1f;
            if (e.KeyCode == Keys.Right)
            {
                _ViewMatrix.ChangeCameraPosition(new Vector3(_ViewMatrix._CameraPosition.X, _ViewMatrix._CameraPosition.Y + mov, _ViewMatrix._CameraPosition.Z));
                mainPicturebox.Refresh();
            }
            else if(e.KeyCode == Keys.Left)
            {
                _ViewMatrix.ChangeCameraPosition(new Vector3(_ViewMatrix._CameraPosition.X, _ViewMatrix._CameraPosition.Y - mov, _ViewMatrix._CameraPosition.Z));
                mainPicturebox.Refresh();
            }
            else if(e.KeyCode == Keys.Up)
            {
                _ViewMatrix.ChangeCameraPosition(new Vector3(_ViewMatrix._CameraPosition.X, _ViewMatrix._CameraPosition.Y, _ViewMatrix._CameraPosition.Z + mov));
                mainPicturebox.Refresh();
            }
            else if (e.KeyCode == Keys.Down)
            {
                _ViewMatrix.ChangeCameraPosition(new Vector3(_ViewMatrix._CameraPosition.X, _ViewMatrix._CameraPosition.Y, _ViewMatrix._CameraPosition.Z - mov));
                mainPicturebox.Refresh();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Zbufor = new double[mainPicturebox.Width, mainPicturebox.Height];
            Utilities.SetZbufor(Zbufor);

            PictureBoxBitmap = new DirectBitmap(mainPicturebox.Width, mainPicturebox.Height);
            mainPicturebox.Image = PictureBoxBitmap.Bitmap;

            mainPicturebox.Refresh();
            mainPicturebox.Refresh();
        }
    }
}
