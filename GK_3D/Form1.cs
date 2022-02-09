using GK_3D.DirBitmap;
using GK_3D.FillingPolygon;
using GK_3D.Lights;
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
using System.Timers;
using System.Windows.Forms;

namespace GK_3D
{
    public partial class Form1 : Form
    {
        private DirectBitmap PictureBoxBitmap { get; set; }
        private double[,] Zbufor { get; set; }
        private ViewMatrix _ViewMatrix { get; set; }
        private ProjectionMatrix _ProjectionMatrix { get; set; }
        private (int X, int Y) TableDimensions { get; set; }
        private int CurrentCamera { get; set; }
        private Vector3 BallMove;
        private List<ViewMatrix> Cameras;
        private List<IShape> Shapes;
        private List<ILight> Lights;
        private Sphere MovingBall;
        private Shapes.Rectangle RotatingCube;
        private ReflectorLight reflector;
        private int shading;
        private float angleZ;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBoxBitmap = new DirectBitmap(mainPicturebox.Width, mainPicturebox.Height);
            mainPicturebox.Image = PictureBoxBitmap.Bitmap;
            shading = 0;
            angleZ = 0;

            Shapes = new List<IShape>();
            Lights = new List<ILight>();
            Cameras = new List<ViewMatrix>();

            _ProjectionMatrix = new ProjectionMatrix(1, 100, 60, 1);

            InitCameras(new Vector3(18f, 7.5f, 25f), new Vector3(5, 7.5f, 8));

            Zbufor = new double[mainPicturebox.Width, mainPicturebox.Height];
            Utilities.SetZbufor(Zbufor);

            Shapes.Rectangle table = new Shapes.Rectangle(10, 15, 8, Color.Green, Color.Green);
            Shapes.Rectangle cube = new Shapes.Rectangle(1, 1, 1, Color.Red, Color.Red);
            RotatingCube = cube;
            TableDimensions = (10, 15);

            Sphere sphere = new Sphere(10, 10, 0.2f, Color.White);

            sphere.MoveByVector(new Vector3(5f, 7.5f, 8.2f));
            cube.MoveByVector(new Vector3(8f, 12f, 8.5f));

            MovingBall = sphere;
            Shapes.Add(table);
            Shapes.Add(sphere);
            Shapes.Add(cube);

            PointLight light1 = new PointLight(new Vector4(5, 5f, 35, 1), Color.LightYellow);
            PointLight light2 = new PointLight(new Vector4(5, 10f, 35, 1), Color.LightYellow);
            ReflectorLight refLight = new ReflectorLight(new Vector4(5f, 7.7f, 8.1f, 1), Vector4.Normalize(new Vector4(5f, 7.7f, 8.1f, 1)), Color.LightYellow);
            Lights.Add(light1);
            Lights.Add(light2);
            Lights.Add(refLight);

            reflector = refLight;

            InitStationaryBalls();
            InitSidesOfTable();

            BallMove = new Vector3(-0.1f, 0.12f, 0f);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

            timer.Interval = 50;
            timer.Tick += OnTimedEvent;
            timer.Start();

            mainPicturebox.Refresh();
        }

        private void mainPicturebox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(PictureBoxBitmap.Bitmap);
            g.Clear(Color.Black);
            Utilities.SetZbufor(Zbufor);

            var ProjCamera = Utilities.ProjectPoint(new Vector4(Cameras[CurrentCamera]._CameraPosition.X - 0.01f, Cameras[CurrentCamera]._CameraPosition.Y, Cameras[CurrentCamera]._CameraPosition.Z, 1),
                new ModelMatrix(), _ViewMatrix, _ProjectionMatrix);
            var Camera = Utilities.ConvertToPictureBox(ProjCamera, mainPicturebox);

            foreach (var light in Lights)
            {
                var ProjLight = Utilities.ProjectPoint(light.LightPos, light._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                var Light = Utilities.ConvertToPictureBox(ProjLight, mainPicturebox);

                if (light.isReflector)
                {
                    var ProjDir = Utilities.ProjectPoint(light.RotatedDir, light._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Dir = Utilities.ConvertToPictureBox(ProjDir, mainPicturebox);
                    Dir = Vector3.Normalize(Dir);

                    light.ProcessedDir = Dir;
                }

                light.ProcessedPos = Light;
            }

            foreach (var shape in Shapes)
            {
                Color col = Color.Black;
                if(shading == 2)
                {
                    var ProjNormal = Utilities.ProjectPoint(shape.GetShape()[0].normal, shape._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Normal = Utilities.ConvertToPictureBox(ProjNormal, mainPicturebox);
                    Normal = Vector3.Normalize(Normal); 
                }
                foreach (var triangle in shape.GetShape())
                {
                    var ProjNormal = Utilities.ProjectPoint(triangle.normal, shape._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Normal = Utilities.ConvertToPictureBox(ProjNormal, mainPicturebox);
                    Normal = Vector3.Normalize(Normal);

                    var ProjPoint1 = Utilities.ProjectPoint(triangle.v1, shape._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point1 = Utilities.ConvertToPictureBox(ProjPoint1, mainPicturebox);

                    var ProjPoint2 = Utilities.ProjectPoint(triangle.v2, shape._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point2 = Utilities.ConvertToPictureBox(ProjPoint2, mainPicturebox);

                    var ProjPoint3 = Utilities.ProjectPoint(triangle.v3, shape._ModelMatrix, _ViewMatrix, _ProjectionMatrix);
                    var Point3 = Utilities.ConvertToPictureBox(ProjPoint3, mainPicturebox);

                    Fill.FillPolygon(new List<Vector3>() { Point1, Point2, Point3 }, PictureBoxBitmap, triangle.col, Zbufor, Normal, Lights, Camera, shading);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (CurrentCamera + 1 > Cameras.Count - 1)
                    CurrentCamera = 0;
                else
                    CurrentCamera++;

                _ViewMatrix = Cameras[CurrentCamera];
                mainPicturebox.Refresh();
            }
            else if(e.KeyCode == Keys.Left)
            {
                if(CurrentCamera - 1 < 0)
                    CurrentCamera = Cameras.Count - 1;
                else
                    CurrentCamera--;

                _ViewMatrix = Cameras[CurrentCamera];
                mainPicturebox.Refresh();
            }
            else if(e.KeyCode == Keys.Space)
            {
                shading++;
                if (shading == 3)
                    shading = 0;
                mainPicturebox.Refresh();
            }
            else if(e.KeyCode == Keys.W)
            {
                Cameras[CurrentCamera].ChangeCameraPosition(new Vector3(Cameras[CurrentCamera]._CameraPosition.X, Cameras[CurrentCamera]._CameraPosition.Y, Cameras[CurrentCamera]._CameraPosition.Z + 0.1f));
            }
            else if(e.KeyCode == Keys.A)
            {
                Cameras[CurrentCamera].ChangeCameraPosition(new Vector3(Cameras[CurrentCamera]._CameraPosition.X, Cameras[CurrentCamera]._CameraPosition.Y - 0.1f, Cameras[CurrentCamera]._CameraPosition.Z));
            }
            else if(e.KeyCode == Keys.D)
            {
                Cameras[CurrentCamera].ChangeCameraPosition(new Vector3(Cameras[CurrentCamera]._CameraPosition.X, Cameras[CurrentCamera]._CameraPosition.Y + 0.1f, Cameras[CurrentCamera]._CameraPosition.Z));
            }
            else if(e.KeyCode == Keys.S)
            {
                Cameras[CurrentCamera].ChangeCameraPosition(new Vector3(Cameras[CurrentCamera]._CameraPosition.X, Cameras[CurrentCamera]._CameraPosition.Y, Cameras[CurrentCamera]._CameraPosition.Z - 0.1f));
            }
            mainPicturebox.Refresh();
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            if (MovingBall.ShapeCenter.X + MovingBall.Radius + BallMove.X > TableDimensions.X || MovingBall.ShapeCenter.X + MovingBall.Radius + BallMove.X < 0)
                BallMove.X *= -1f;
            else if (MovingBall.ShapeCenter.Y + MovingBall.Radius + BallMove.Y > TableDimensions.Y || MovingBall.ShapeCenter.Y + MovingBall.Radius + BallMove.Y < 0)
                BallMove.Y *= -1f;

            MovingBall.MoveByVector(BallMove);
            reflector.LightPos = new Vector4(reflector.LightPos.X + BallMove.X, reflector.LightPos.Y + BallMove.Y, reflector.LightPos.Z + BallMove.Z, 1);

            angleZ += 20;
            RotatingCube.RotateZ(5);
            reflector.RotateZ(angleZ);
            Cameras[1].ChangeCameraTarget(MovingBall.ShapeCenter);
            Cameras[2].ChangeCameraPosition(new Vector3(Cameras[2]._CameraPosition.X + BallMove.X, Cameras[2]._CameraPosition.Y + BallMove.Y, Cameras[2]._CameraPosition.Z + BallMove.Z));

            mainPicturebox.Refresh();
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

        private void InitCameras(Vector3 StartingPos, Vector3 StartingTarget)
        {
            ViewMatrix StationaryCamera = new ViewMatrix();
            StationaryCamera.ChangeCameraPosition(StartingPos);
            StationaryCamera.ChangeCameraTarget(StartingTarget);

            ViewMatrix ObjectCamera = new ViewMatrix();
            ObjectCamera.ChangeCameraPosition(StartingPos);
            ObjectCamera.ChangeCameraTarget(StartingTarget);

            ViewMatrix MovingCamera = new ViewMatrix();
            MovingCamera.ChangeCameraPosition(StartingPos);
            MovingCamera.ChangeCameraTarget(StartingTarget);

            Cameras.Add(StationaryCamera);
            Cameras.Add(ObjectCamera);
            Cameras.Add(MovingCamera);

            _ViewMatrix = Cameras[0];
            CurrentCamera = 0;
        }
        private void InitStationaryBalls()
        {
            int k = 5;
            float Start = 4.2f;
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < k; j++)
                {
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

                    Sphere sphere = new Sphere(10, 10, 0.2f, randomColor);

                    sphere.MoveByVector(new Vector3(Start + j*0.4f, 3f + i*0.4f, 8.2f));

                    Shapes.Add(sphere);
                }
                k--;
                Start += 0.2f;
            }
        }
        private void InitSidesOfTable()
        {
            Shapes.Rectangle front = new Shapes.Rectangle(0.5f, 15f, 9f, Color.SaddleBrown, Color.SaddleBrown);
            Shapes.Rectangle back = new Shapes.Rectangle(0.5f, 15f, 9f, Color.SaddleBrown, Color.SaddleBrown);
            Shapes.Rectangle left = new Shapes.Rectangle(11f, 0.5f, 9f, Color.SaddleBrown, Color.SaddleBrown);
            Shapes.Rectangle right = new Shapes.Rectangle(11f, 0.5f, 9f, Color.SaddleBrown, Color.SaddleBrown);

            front.MoveByVector(new Vector3(10f, 0, 0));
            back.MoveByVector(new Vector3(-0.5f, 0, 0));
            left.MoveByVector(new Vector3(-0.5f, -0.5f, 0));
            right.MoveByVector(new Vector3(-0.5f, 15f, 0));

            Shapes.Add(front);
            Shapes.Add(back);
            Shapes.Add(left);
            Shapes.Add(right);
        }
    }
}
