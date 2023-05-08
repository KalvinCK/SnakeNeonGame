using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Game : IDisposable
    {
        private static Random rand = new Random();
        public static float Width { get => Program.Size.X; } 
        public static float Height { get => Program.Size.Y; } 
        public static Matrix4 projection { get => Matrix4.CreateOrthographicOffCenter(0f, Program.Size.X, 0f, Program.Size.Y, -10f, 10000f); }
        public static Shader shader = new Shader("resources/shaders/game/shader.vert", "resources/shaders/game/shader.frag");
        public static Shader shaderTexture = new Shader("resources/shaders/game/shader.vert", "resources/shaders/game/shaderTexture.frag");
        private Background background = new Background();
        private Bloom bloom = new Bloom();
        private Snake snake = new Snake();
        private Food food = new Food(new Vector2
        {
            X = 20 * rand.Next(1, (int)Width  / 20),
            Y = 20 * rand.Next(1, (int)Height / 20),
        });
        private Lines lines = new Lines();
        private Sounds soundFood = new Sounds("resources/audio/Menu1A.wav", 50);
        private int Score = 0;
        private int increaseScore = 100;
        private float elapsedTime = 0f;
        private Color4 colorScore = Color4.AliceBlue;
        public void RenderFrame()
        {
            if(!Program.window.IsFocused)
                return;
            
            bloom.Active();

            lines.RenderFrame();
            snake.RenderFrame();
            food.RenderFrame();



            var Timer = TimerGL.Time;
            if(Timer - elapsedTime > 0.5f)
            {
                colorScore = ListColors.Colors4[rand.Next(0, ListColors.MaxColors)];
                elapsedTime = Timer;
            }
            

            background.RenderFrame();

            bloom.RenderFrame();

            Text.RenderText("SCORE " + Score, Width / 2 - 50f, Height - 50f, 1.0f, colorScore, 1.0f);
            Text.RenderText("FPS: " + TimerGL.FramesForSecond, 5f, Height - 15f, 0.5f, Color4.Lime, 1.0f);

        }
        public void UpdateFrame()
        {
            if(!Program.window.IsFocused)
                return;

            if(Program.window.IsKeyPressed(Keys.Escape))
            {
                if(snake.direction == Direction.Stop)
                {
                    Program.window.Close();
                }
                else
                {
                    snake.InitPositions();
                    Score = 0;
                    increaseScore = 100;
                }
            }

            snake.UpdateFrame();
            LogicCollision();


        }

        public void LogicCollision()
        {
            if(Collision.Detect(snake.BoxHead, food.box))
            {
                Vector2 pos = Vector2.Zero;
                bool result = true;

                do
                {
                    pos.X = 20 * rand.Next(1, (int)Width / 20);
                    pos.Y = 20 * rand.Next(1, (int)Height  / 20);

                    result = (from item in snake.posBody select item).Contains(pos);

                } while (result);

                food = new Food(pos);
                soundFood.Play();
                Score += 10;
                snake.IncreaseBody();
            }

            if(Score == increaseScore)
            {
                increaseScore += increaseScore;
                snake.vel -= 0.005f;

            }

            if(snake.direction != Direction.Stop)
            {
                for(int i = 3; i < snake.BoxBody.Count; i++)
                {
                    if(Collision.Detect(snake.BoxHead, snake.BoxBody[i]))
                    {
                        snake.InitPositions();
                        Score = 0;
                        increaseScore = 100;


                    }
                }
            }
        }
        public void ResizedFrame()
        {
            if(!Program.window.IsFocused)
                return;
            
            
            var pos = food.position;
            if(pos.X > Width)
                pos.X = Width;

            if(pos.Y > Height)
                pos.Y = Height;

            var foodColor = food.color;

            food = new Food(pos);
            lines.ResizeLines();                
            bloom.ResizedFrameBuffer();

        }
        public void Dispose()
        {   
            shader.Dispose();
            snake.Dispose();
            soundFood.Dispose();
            background.Dispose();
            
        }
    }
}

