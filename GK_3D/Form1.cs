using GK_3D.DirBitmap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK_3D
{
    public partial class Form1 : Form
    {
        private DirectBitmap? PictureBoxBitmap { get; set; }
        private double[,]? zbufor { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PictureBoxBitmap = new DirectBitmap(mainPicturebox.Width, mainPicturebox.Height);
            mainPicturebox.Image = PictureBoxBitmap.Bitmap;
            zbufor = new double[mainPicturebox.Width, mainPicturebox.Height];
        }

        private void mainPicturebox_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
