using OpenTK;

namespace KoC.GameEngine.Player
{
	public interface ICamera
	{
		Vector3 Target { get; set; }
		Vector3 CamPos { get; set; }
		Vector3 Up { get; set; }
		ref Matrix4 GetCameraMatrix();

	}
}
