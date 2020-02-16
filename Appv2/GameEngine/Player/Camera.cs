﻿using OpenTK;

namespace KoC.GameEngine.Player
{
	class Camera : ICamera
	{
		private Vector3 _Target;
		private Vector3 _CamPos;
		private Vector3 _Up;
		public Matrix4 CameraMatrix;
		public Vector3 Target {
			get {
				return _Target;
			}
			set
			{
				_Target = value;
			} 
		}
		public Vector3 CamPos {
			get
			{
				return _CamPos;
			}
			set
			{
				_CamPos = value;
			}
		}
		public Vector3 Up
		{
			get
			{
				return _Up;
			}
			set
			{
				_Up = value;
			}

		}

		public Camera(Vector3 tTarget,Vector3 Up,Vector3 CamPos)
		{
			_Target = tTarget;
			_Up = Up;
			_CamPos = CamPos;
			CameraMatrix = Matrix4.LookAt(_Target, _Up, _CamPos) * Matrix4.CreateTranslation(-_CamPos);
		}
		private void CalculateCamMatrix()
		{
			CameraMatrix = Matrix4.LookAt(_Target, _Up, _CamPos) * Matrix4.CreateTranslation(-_CamPos);
		}
		public ref Matrix4 GetCameraMatrix()
		{
			return ref CameraMatrix;
		}
		/*private Matrix4 InitCameraTransf()
		{
			Vector3 N = _Target;
			N.Normalize();
			Vector3 U = _Up;
			U = Vector3.Cross(U, _Target);
			U.Normalize();
			Vector3 V = Vector3.Cross(N,U);
			return new Matrix4(
				U.X, U.Y, U.Z, 0.0f,
				V.X, V.Y, V.Z, 0.0f,
				N.X, N.Y, N.Z, 0.0f,
				0.0f,0.0f,0.0f,1.0f
				);

		}
		*/

		
		public bool IsPointInCameraView(Vector3 point)
		{

			return false;
		}
	}	
}
