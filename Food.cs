using OpenTK.Mathematics;

namespace MyGame
{
     public class Food
    {
        private Shader shader { get => Game.shader; }
        private Random rand = new Random();
        public Vector2 position;
        public Vector4 box;
        public Color4 color;
        public Food(Vector2 position)
        {
            this.position = position;

            color = ListColors.Colors4[rand.Next(0, ListColors.MaxColors)]; 
        }
        public void RenderFrame()
        {
            var model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(10f, 10f, 1.0f);
            model = model * Matrix4.CreateTranslation((position.X - 10f), (position.Y - 10f) , 0.5f);
            box = Collision.getBox(model, position);

            shader.Use();
            shader.SetUniform("projection", Game.projection);
            shader.SetUniform("model", model);
            shader.SetUniform("color", color);
            shader.SetUniform("ForceLight", 30f);

            Quad.RenderQuad();
        }
    }
}