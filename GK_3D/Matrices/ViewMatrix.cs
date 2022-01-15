using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK_3D.Matrices
{
    public class ViewMatrix
    {
        public Matrix4x4 Matrix { get; set; }
        public Vector3 _CameraPosition { get; private set; }
        public Vector3 _CameraTarget { get; private set; }
        public Vector3 _UpVector { get; private set; }

        public ViewMatrix()
        {
            this._CameraPosition = new Vector3(3f, 0.5f, 0.5f);
            this._CameraTarget = new Vector3(0f, 0.5f, 0.5f);
            this._UpVector = new Vector3(0f, 0f, 1f);
            Matrix = new Matrix4x4(0f, 1f, 0f, -0.5f,
                                   0f, 0f, 1f, -0.5f,
                                   1f, 0f, 0f, -3f,
                                   0f, 0f, 0f, 1f);
        }

        public void ChangeCameraPosition(Vector3 CameraPosition)
        {
            this._CameraPosition = CameraPosition;

            ChangeParams();
        }

        public void ChangeCameraTarget(Vector3 CameraTarget)
        {
            this._CameraTarget = CameraTarget;

            ChangeParams();
        }

        public void ChangeUpVector(Vector3 UpVector)
        {
            this._UpVector = UpVector;

            ChangeParams();
        }

        private void ChangeParams()
        {
            Vector3 zAxis = this._CameraPosition - this._CameraTarget;
            zAxis = Vector3.Normalize(zAxis);

            Vector3 xAxis = Vector3.Cross(this._UpVector, zAxis);
            xAxis = Vector3.Normalize(xAxis);

            Vector3 yAxis = Vector3.Cross(zAxis, xAxis);

            Matrix4x4 invertedMatrix = new Matrix4x4(xAxis.X, yAxis.X, zAxis.X, this._CameraPosition.X,
                                                     xAxis.Y, yAxis.Y, zAxis.Y, this._CameraPosition.Y,
                                                     xAxis.Z, yAxis.Z, zAxis.Z, this._CameraPosition.Z,
                                                     0f, 0f, 0f, 1f);

            if (Matrix4x4.Invert(invertedMatrix, out Matrix4x4 res) == true)
                Matrix = res;
            else
                throw new Exception("Cannot invert matrix");
        }
    }
}
