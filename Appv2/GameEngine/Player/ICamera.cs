using OpenTK;

namespace KoC.GameEngine.Player
{
	public interface ICamera
	{
		Vector3 Target { get; set; }
		Vector3 CamPos { get; set; }
		Vector3 Up { get; set; }
		void CameraMove(Vector3 vec,float speed);
		void CameraRotate(Vector3 angles);
		Matrix4 GetCameraMatrix { get; }

	}
}
