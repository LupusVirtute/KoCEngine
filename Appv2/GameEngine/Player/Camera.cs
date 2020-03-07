using OpenTK;
using System;

namespace KoC.GameEngine.Player
{
	public class Camera : ICamera
	{
		private Vector3 _Target;
		private Vector3 _CamPos;
		private Vector3 _Front;
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
			_Up = new Vector3(0.0f,1.0f,0.0f);
			_Front = new Vector3(0f,0f,-1.0f);
			_CamPos = CamPos;
			CameraMatrix = Matrix4.LookAt(_CamPos, _CamPos + _Front,_Up ) * Matrix4.CreateTranslation(-_CamPos);
			CalculateCamMatrix();
		}
		private void CalculateCamMatrix()
		{
			CameraMatrix = Matrix4.LookAt(_CamPos, _CamPos + _Front, _Up) * Matrix4.CreateTranslation(-_CamPos);
		}
		public Matrix4 GetCameraMatrix
		{
			get
			{
				return CameraMatrix;
			}
		}
		public void CameraMove(Vector3 vec,float speed)
		{
			if(vec.X < 0) 
			{
				_CamPos -= Vector3.Normalize(Vector3.Cross(_Front,_Up)) *speed;
			}
			else if(vec.X > 0)
			{
				_CamPos += Vector3.Normalize(Vector3.Cross(_Front, _Up)) * speed;

			}
			if (vec.Y < 0)
			{
				_CamPos += _Up * speed;
			}
			else if (vec.Y > 0)
			{
				_CamPos -= _Up * speed;
			}
			if(vec.Z < 0)
			{
				_CamPos += _Front*speed;
			}
			else if(vec.Z > 0) 
			{
				_CamPos -= _Front*speed;

			}			
			//CameraRotate(new Vector3(0.0f,0.0f,0.0f));

			CalculateCamMatrix();
		}
		public void CameraRotate(Vector3 Angles)
		{
			float camY = (float)Math.Sin(QuickMaths.DegreeToRadian(Angles.Y));
			float cosPitch = (float)Math.Cos(QuickMaths.DegreeToRadian(Angles.Y));
			Angles.X = (float)Math.Sin(QuickMaths.DegreeToRadian(Angles.X)) * cosPitch;
			Angles.Z = (float)Math.Cos(QuickMaths.DegreeToRadian(Angles.Z)) * cosPitch;
			_Target = new Vector3(Angles.X, camY, Angles.Z);
			CalculateCamMatrix();
		}
		public bool IsPointInCameraView(Vector3 point)
		{

			return false;
		}
	}	
}
