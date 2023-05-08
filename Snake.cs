using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public enum Direction
    {
        Stop,
        Up,
        Down,
        Right,
        Left
    }
    public class Snake
    {
        private Shader shaderTexture => Game.shader;
        private Random rand = new Random();
        public GameMusic music = new GameMusic("resources/audio/cyberpunk_arcade_3_0.ogg", 70);
        private readonly Dictionary<Direction, Vector2> Directions = new Dictionary<Direction, Vector2>()
        {
            { Direction.Stop,  new Vector2( 0f,  0f ) },
            { Direction.Up,    new Vector2( 0f,  20f) },
            { Direction.Down,  new Vector2( 0f, -20f) },
            { Direction.Right, new Vector2( 20f, 0f)  },
            { Direction.Left,  new Vector2(-20f, 0f)  },
        };
        public Vector4 BoxHead
        {
            get => BoxBody[0];
            set
            {
                BoxBody[0] = value;
            }
        }
        public List<Vector2> posBody = new List<Vector2>();
        public List<Vector4> BoxBody = new List<Vector4>();
        public Color4 color;
        public List<Direction> directionSnake = new List<Direction>();
        public Vector2 Head
        {
            get => posBody[0];
            set
            {
                posBody[0] = value;
            }
        } 
        public Snake()
        {
            music.Play();
            InitPositions();

        }
        public void InitPositions()
        {

            vel = 0.05f;
            direction = Direction.Stop;
            elapsedTime = 0f;

            BoxBody.Clear();
            posBody.Clear();
            directionSnake.Clear();

            IncreaseBody(new Vector2(140f, 500f), Direction.Right);
            IncreaseBody(new Vector2(120f, 500f), Direction.Right);
            IncreaseBody(new Vector2(100f, 500f), Direction.Right);
            IncreaseBody(new Vector2(80f,  500f), Direction.Right);
            IncreaseBody(new Vector2(60f,  500f), Direction.Right);
            IncreaseBody(new Vector2(40f,  500f), Direction.Right);

            
        }
        public void IncreaseBody(Vector2 position = new Vector2(), Direction dir = Direction.Stop)
        {
            posBody.Add(position);
            BoxBody.Add( new Vector4(0f));
            directionSnake.Add(dir);
            color = ListColors.Colors4[rand.Next(0, ListColors.MaxColors)];
            
        }

        public void RenderFrame()
        {


            shaderTexture.Use();
            shaderTexture.SetUniform("projection", Game.projection);
            shaderTexture.SetUniform("ForceLight", 15f);
            shaderTexture.SetUniform("alpha", 1f);
            shaderTexture.SetUniform("color", color);


            for(var i = 0; i < posBody.Count; i++)
            {

                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(10f, 10f, 1.0f);
                model = model * Matrix4.CreateTranslation((posBody[i].X - 10f), (posBody[i].Y - 10f), 0.0f);
                BoxBody[i] = Collision.getBox(model, posBody[i]);
                

                shaderTexture.SetUniform("model", model);
                Quad.RenderQuad();
            }
            
        }
        private float elapsedTime = 0;
        public float vel { get; set; } = 0.05f;
        public Direction direction { get; private set; } = Direction.Stop;
        public void UpdateFrame()
        {


            var input = Program.window.IsKeyDown;

            if(input(Keys.W) && direction != Direction.Down) direction = Direction.Up;
            else if(input(Keys.S) && direction != Direction.Up) direction = Direction.Down;
            else if(input(Keys.D) && direction != Direction.Left) direction = Direction.Right;
            else if(input(Keys.A) && direction != Direction.Right) direction = Direction.Left;


            if(input(Keys.Up) && direction != Direction.Down) direction = Direction.Up;
            else if(input(Keys.Down) && direction != Direction.Up) direction = Direction.Down;
            else if(input(Keys.Right) && direction != Direction.Left) direction = Direction.Right;
            else if(input(Keys.Left) && direction != Direction.Right) direction = Direction.Left;

            var Time = TimerGL.Time;

            if(Time - elapsedTime > vel)
            {
                elapsedTime = Time;

                if(direction != Direction.Stop)
                {
                    for(int i = posBody.Count - 1; i > 0; i--)
                    {
                        posBody[i] = posBody[i - 1];

                        directionSnake[i] = directionSnake[i - 1];
                        

                    }

                }
                

                directionSnake[0] = direction;
                posBody[0] += Directions[direction];

                var Width = Game.Width;
                var Height = Game.Height;

                var posX = posBody[0].X;
                var posY = posBody[0].Y;

                if(posBody[0].X < 20f)
                    posBody[0] = new Vector2(Width, posY);
                else if(posBody[0].X > Width)
                    posBody[0] = new Vector2(20f, posY);

                if(posBody[0].Y < 20f)
                    posBody[0] = new Vector2(posX, Height);
                else if(posBody[0].Y > Height)
                    posBody[0] = new Vector2(posX, 20f);

                
            }
        }

        public void Dispose()
            => music.Dispose();

    }
}