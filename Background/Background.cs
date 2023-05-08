using ImGuiNET;
using OpenTK.Mathematics;

namespace MyGame
{
    public class Background
    {
        private Shader shader = new Shader("resources/shaders/game/shaderBack.vert", "resources/shaders/game/shaderBack.frag");
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("iResolution", Program.Size);
            shader.SetUniform("iGamma", 1.417f);
            shader.SetUniform("iTimeX", TimerGL.Time * 0.3f);
            shader.SetUniform("iTimeY", TimerGL.Time * 0.3f);
            Quad.RenderQuad();

        }
        public void Dispose()
        {
            shader.Dispose();
        }
    }
}