using OpenTK.Windowing.Desktop; // Janela
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Mathematics;


using StbImageSharp;


namespace MyGame
{
    static class Window
    {
        public static GameWindowSettings gws = GameWindowSettings.Default;
        public static NativeWindowSettings nws = NativeWindowSettings.Default;
        static Window()
        {
            gws = GameWindowSettings.Default;
            nws = NativeWindowSettings.Default;

            nws.Title = "Snake Neon";
            nws.Location = new Vector2i(100, 100);
            nws.Size = new Vector2i(1280, 720);
            nws.SrgbCapable = true;
            nws.StartFocused = true;
            nws.StartVisible = true;
            nws.AutoLoadBindings = true;
            nws.API = ContextAPI.OpenGL;
            nws.APIVersion = Version.Parse("4.6");
            nws.Flags = ContextFlags.Default;
            nws.WindowState = WindowState.Fullscreen;
            nws.Flags = ContextFlags.Debug;

            using(Stream stream = File.OpenRead("resources/textures/snake-icn.png"))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                nws.Icon = new WindowIcon( new Image(image.Width, image.Height, image.Data));
            }
        }
    }
}