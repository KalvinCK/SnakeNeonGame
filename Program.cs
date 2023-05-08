using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace MyGame
{
    public class Program
    {
        public static GameWindow window = new GameWindow(Window.gws, Window.nws)
        {
            VSync = VSyncMode.Adaptive
        };
        public static Vector2i Size = new Vector2i(window.Size.X, window.Size.Y);
        private static Game ?game;
        private static Text ?text;
        // private static ImGuiController ?imGuiController;
        public static bool fullscreen = false;
        private static void Main()
        {
            window.Load += delegate
            {
                StartGlobal();

                // imGuiController = new ImGuiController(window.Size);
                
                text = new Text("resources/Fonts/Wigners.otf");
                game = new Game();
            };
            window.RenderFrame += delegate(FrameEventArgs frameEventArgs)
            {
                // imGuiController!.Update(Program.window, (float)frameEventArgs.Time);
                
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


                game!.RenderFrame();

                // ImGuiProgram.RenderFrame();

                // imGuiController!.Render();
                // imGuiController!.CheckGLError("End of frame");

                window.Context.SwapBuffers();
            };

  
            window.UpdateFrame += delegate(FrameEventArgs eventArgs)
            {

                var input = window.IsKeyPressed;

                if(input(Keys.F))
                {
                    fullscreen = !fullscreen;

                    window.WindowState = fullscreen  ? WindowState.Normal : WindowState.Fullscreen;
                    window.VSync = VSyncMode.On;
                }

                TimerGL.TimerUpdateFrame(eventArgs);

                game!.UpdateFrame();


            };
            window.Resize += delegate(ResizeEventArgs resizeEventArgs)
            {
                Size = resizeEventArgs.Size;
                // imGuiController!.WindowResized(resizeEventArgs.Size);

                GL.Viewport(0, 0, resizeEventArgs.Size.X, resizeEventArgs.Size.Y);

                    
                game!.ResizedFrame();


            };
            window.MouseWheel += delegate(MouseWheelEventArgs mouseWheelEventArgs)
            {

            };
            window.TextInput += delegate(TextInputEventArgs textInputEventArgs)
            {
                // imGuiController!.PressChar((char)textInputEventArgs.Unicode);
            };
            window.Unload += delegate
            {
                game!.Dispose();

                window.Close();
            };

            window.Run();
        }        
        public static void StartGlobal()
        {
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.FramebufferSrgb);

            GL.Enable(EnableCap.Multisample);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            GL.ClearColor(new Color4(0.05f, 0.05f, 0.05f, 1f));
        }
    }
}