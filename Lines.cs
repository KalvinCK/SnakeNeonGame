using OpenTK.Mathematics;


namespace MyGame
{
    
    public class Lines
    {
        private Shader shader => Game.shader;
        private List<Vector2> linesX = new List<Vector2>();
        private List<Vector2> linesY = new List<Vector2>();
        public Lines()
        {
            ResizeLines();
        }
        public void RenderFrame()
        {
            shader.Use();
            shader.SetUniform("projection", Game.projection);
            shader.SetUniform("ForceLight", 1f);
            shader.SetUniform("color", Color4.Black); 
            shader.SetUniform("alpha", 0.3f);
            

            var model = Matrix4.Identity;
            foreach( var item in linesX)
            {

                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(0.5f, 2000f, -0.1f);
                model = model * Matrix4.CreateTranslation(item.X, item.Y, -1.0f);

                shader.SetUniform("model", model);
                Quad.RenderQuad();
            }
            foreach( var item in linesY)
            {
                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(2000f, 0.5f, -0.1f);
                model = model * Matrix4.CreateTranslation(item.X, item.Y, -1.0f);

                shader.SetUniform("model", model);
                Quad.RenderQuad();
            }

            shader.SetUniform("alpha", 1.0f);
        }
        public void ResizeLines()
        {
            linesX.Clear();
            linesY.Clear();

            int i;
            for(i = 20; i < 2000; i += 20)
            {
                linesX.Add( new Vector2(i, 1000f));
            }
            for(i = 20; i < 2000; i += 20)
            {
                linesY.Add( new Vector2(1000f, i));
            }
        }
    }
}