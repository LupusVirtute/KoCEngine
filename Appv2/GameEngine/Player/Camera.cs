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
		private Matrix4 CameraMatrix;
		public Matrix4 GetCameraMatrix
		{
			get
			{
				return CameraMatrix;
			}
		}
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

		public Camera(Vector3 tTarget,Vector3 CamPos)
		{
			_Target = tTarget;
			_Target.Normalize();
			_Up = Up;
			_Up = new Vector3(0.0f,1.0f,0.0f);
			_Up.Normalize();
			_Front = new Vector3(0f,0f,-1.0f);
			_Front.Normalize();
			_CamPos = CamPos;
			CameraRotate(new Vector3(1.0f,1.0f,1.0f));
			CalculateCamMatrix();
		}
		private void CalculateCamMatrix()
		{
			CameraMatrix = Matrix4.LookAt(_CamPos,_CamPos + _Target, _Up) * Matrix4.CreateTranslation(-_CamPos);
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
			CameraRotate(new Vector3(0.11f, .11f, .11f));
			CalculateCamMatrix();

		}
		public Vector3 rot = new Vector3();
		public void CameraRotate(Vector3 Angles)
		{
			Angles.X %= 360;
			Angles.Y %= 360;
			Angles.Z %= 360;
			Angles.X = QuickMaths.DegreeToRadian(Angles.X);
			Angles.Y = QuickMaths.DegreeToRadian(Angles.Y);
			Angles.Z = QuickMaths.DegreeToRadian(Angles.Z);
			rot = Angles;
			Quaternion quaternion = new Quaternion(rot.X,rot.Y,rot.Z);
			UpdateTarget(quaternion);
			CalculateCamMatrix();
		}
		private void UpdateTarget(Quaternion quat)
		{
			_Target = new Vector3(quat.ToAxisAngle());
		}
		public bool IsPointInCameraView(Vector3 point)
		{

			return false;
		}
	}	
}
