using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GK3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        //Geometric info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        //Landscape Coordinates
        int width = 3, lenght = 2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f),
                               GraphicsDevice.DisplayMode.AspectRatio,
                1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                          Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = true;

            basicEffect.LightingEnabled = false;

            //Geometry  - a simple triangle about the origin
            triangleVertices = new VertexPositionColor[6*width*lenght];
            Vector3[,] landscapeCoordinates = new Vector3[width +1,lenght +1];

            for (int i = 0; i < width+1; i++)
            {
                for (int j = 0; j < lenght+1; j++)
                {
                    landscapeCoordinates[i,j] = new Vector3(i * 10, 0, j * 10);
                }
            }


                    for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    triangleVertices[6 * j + i * lenght * 6 + 0] = new VertexPositionColor(landscapeCoordinates[i, j], Color.Green);
                    triangleVertices[6 * j + i * lenght * 6 + 1] = new VertexPositionColor(landscapeCoordinates[i, j + 1], Color.Red);
                    triangleVertices[6 * j + i * lenght * 6 + 2] = new VertexPositionColor(landscapeCoordinates[i + 1, j], Color.Blue);
                    triangleVertices[6 * j + i * lenght * 6 + 3] = new VertexPositionColor(landscapeCoordinates[i + 1, j + 1], Color.Blue);
                    triangleVertices[6 * j + i * lenght * 6 + 4] = new VertexPositionColor(landscapeCoordinates[i, j + 1], Color.Violet);
                    triangleVertices[6 * j + i * lenght * 6 + 5] = new VertexPositionColor(landscapeCoordinates[i + 1, j], Color.Yellow);
                }
            }

            //Vert buffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
                           VertexPositionColor), 6 * width * lenght, BufferUsage.
                           WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y += 1f;
                camTarget.Y += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 1f;
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                         Vector3.Up);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, width * lenght *2);
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
