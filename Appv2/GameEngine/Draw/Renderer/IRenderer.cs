using KoC.GameEngine.Player;

namespace KoC.GameEngine.Draw.Renderer
{
    public interface IRenderer
    {
        void RenderCall();
        void Delete();
        bool IsOrtho();
        void SwitchOrtho();
        void ReloadProjections(float asp);
        void ReloadProjections(float Width,float Height);
        float FOV { get; set; }
        float RenderDistance { get; set; }
        ICamera GetCamera { get; }
    }
}
