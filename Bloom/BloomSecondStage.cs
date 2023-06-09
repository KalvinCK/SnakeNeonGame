using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace MyGame
{
    class Bloom : IDisposable
    {
        private Vector2i sizeWindow { get => Program.Size; }
        private Shader shaderBloomFinal;

        private int renderBuffer;
        private int frameBuffer;
        public int[] TexturesBuffer = new int[2];
        public int UseTexture { get => TexturesBuffer[1]; }
        private BloomFirstStage FirstStage;
        public Bloom(int NumBlomMips = 6)
        {

            shaderBloomFinal = new Shader("resources/shaders/bloom/bloom.vert", "resources/shaders/bloom/BloomFinal.frag");

            FirstStage = new BloomFirstStage(NumBlomMips);

            frameBuffer  = GL.GenFramebuffer();
            renderBuffer = GL.GenRenderbuffer();
            GL.GenTextures(2, TexturesBuffer);


            ResizedFrame();
        }
        private void ResizedFrame()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);

            for(int i = 0; i < 2; i++)
            {
                GL.BindTexture(TextureTarget.Texture2D, TexturesBuffer[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f,
                    sizeWindow.X, sizeWindow.Y, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, 
                    TextureTarget.Texture2D, TexturesBuffer[i], 0);
            }

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer , renderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, sizeWindow.X, sizeWindow.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, renderBuffer);   

            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1 }; 
            GL.DrawBuffers(2, attachments);

            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    Console.WriteLine("Framebuffer Not complete!");



            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public static bool EnableBloom { get => Values.isRenderBloom; }
        public void Active()
        {
            if(EnableBloom)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            }
        }
        public void ResizedFrameBuffer()
        {
            if(EnableBloom)
            {
                ResizedFrame();
                FirstStage.ResizedFrameBuffer();
            }
        }
        public void RenderFrame()
        {

            if(EnableBloom)
            {
                GL.Disable(EnableCap.Blend);
                FirstStage.RenderBloomTexture(UseTexture, Values.filterRadius);

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                shaderBloomFinal.Use();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, TexturesBuffer[0]);
                shaderBloomFinal.SetUniform("scene", 0);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, FirstStage.UseTexture);
                shaderBloomFinal.SetUniform("bloomBlur", 1);

                shaderBloomFinal.SetUniform("exposure", Values.new_bloom_exp);
                shaderBloomFinal.SetUniform("bloomStrength", Values.new_bloom_streng);
                shaderBloomFinal.SetUniform("gamma", Values.new_bloom_gama);
                shaderBloomFinal.SetUniform("film_grain", Values.new_bloom_filmGrain);
                shaderBloomFinal.SetUniform("elapsedTime", TimerGL.ElapsedTime);

                shaderBloomFinal.SetUniform("nitidezStrength", Values.nitidezStrengh);
                shaderBloomFinal.SetUniform("vibrance", Values.vibrance / -255f);
                shaderBloomFinal.SetUniform("activeNegative", Values.activeNegative);


                Quad.RenderQuad();
                GL.Enable(EnableCap.Blend);
            }

        }
        public void Dispose()
        {
            FirstStage.Dispose();
            shaderBloomFinal.Dispose();

            GL.DeleteTextures(2, TexturesBuffer);
            GL.DeleteRenderbuffer(renderBuffer);
            GL.DeleteFramebuffer(frameBuffer);

        }
    }
}