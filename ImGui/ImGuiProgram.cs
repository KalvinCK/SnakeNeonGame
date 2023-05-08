using ImGuiNET;

using Vector4 = System.Numerics.Vector4;

namespace MyGame
{
    public class ImGuiProgram
    {
        public static float velSnake = 0.05f;
        public static void RenderFrame()
        {
            ImGui.StyleColorsClassic();

            ImGui.Begin("Scene Details");
            ImGui.Text($"Frames: {TimerGL.FramesForSecond} | Time: {TimerGL.Time.ToString("0.0")}");
            ImGui.NewLine();

            
            ImGui.NewLine();
            ImGui.Checkbox("Enable Bloom", ref Values.isRenderBloom);

            if(Values.isRenderBloom)
            {
                ImGui.SliderFloat("Bloom Exposure", ref Values.new_bloom_exp, 0.0f, 1.0f);
                ImGui.SliderFloat("Bloom Strength", ref Values.new_bloom_streng, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Gamma", ref Values.new_bloom_gama, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Spacing Filter", ref Values.filterRadius, 0.0f, 0.01f, "%.7f");
                ImGui.SliderFloat("Bloom Film Grain", ref Values.new_bloom_filmGrain, -0.1f, 0.1f, "%.7f");
                ImGui.SliderFloat("Bloom Nitidez Strength", ref Values.nitidezStrengh, 0.0f, 0.2f, "%.7f");
                ImGui.SliderInt("Bloom Vibrance", ref Values.vibrance, -255, 100);
                ImGui.Checkbox("Active Negative", ref Values.activeNegative);

            }
            ImGui.SliderFloat("Neon Force", ref Values.ForceLightScene, 20f, 30f, "%.7f");

            ImGui.SliderFloat("Pos", ref velSnake, 0f, 0.5f, "%.7f");

            ImGui.End();
           
        }
    }
}