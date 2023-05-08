using OpenTK.Mathematics;

namespace MyGame
{
    class Collision
    {
        public static bool Detect(Vector4 boxA, Vector4 boxB)
        => (!(boxA.X > boxB.Z || boxA.Y < boxB.W || boxA.Z < boxB.X || boxA.W > boxB.Y));
        private static Vector2 mousePos
        {
            get => new Vector2(Program.window.MousePosition.X, Game.Height - Program.window.MousePosition.Y);
        }
        public static bool DetectMouse(Vector4 boxA)
        {
            Vector4 boxB  = new Vector4()
            {
                X = mousePos.X - 0.2f, 
                Y = mousePos.Y + 0.2f, 
                Z = mousePos.X + 0.2f, 
                W = mousePos.Y - 0.2f,
            };
            
            return (!(boxA.X > boxB.Z || boxA.Y < boxB.W || boxA.Z < boxB.X || boxA.W > boxB.Y));
        }
        public static Vector4 getBox(Matrix4 model, Vector2 pos)
        {
            return new Vector4()
            {
                X  = pos.X - model.M11 / 2, 
                Y  = pos.Y + model.M22 / 2, 
                Z  = pos.X + model.M11 / 2, 
                W  = pos.Y - model.M22 / 2,

            };
        }
    }
}