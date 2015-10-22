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
        VertexPositionColorNormal[] triangleVertices;
        VertexBuffer vertexBuffer;

        //Landscape Coordinates
        int width = 30, lenght = 30;

        // Set the 3D model to draw.
        Model myBenchModel;
        Model myLaternModel;

        // The aspect ratio determines how to scale 3d to 2d projection.
        float aspectRatio;
        Vector3 bench1Position = new Vector3(10, 0, 30);
        Vector3 latern1Position = new Vector3(10, 0, 70);
        float bench1Rotation = 0.0f;
        float latern1Rotation = 0.0f;
        float bench2Rotation = 90.0f;
        float benchScaleRatio = 0.05f;
        float laternScaleRatio = 0.07f;

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
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            basicEffect.VertexColorEnabled = true;

            basicEffect.LightingEnabled = true;
            //basicEffect.EnableDefaultLighting();

            basicEffect.AmbientLightColor = new Vector3(0.1f,0.1f,0f);
            basicEffect.DiffuseColor = new Vector3(0.5f,0.5f,0.5f);

            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 1f, 0.5f); // a red light
            basicEffect.DirectionalLight0.Direction = new Vector3(0, -1, 0);  // coming along the _axis
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(1, 0, 0); // with green highlights
            
            //Geometry  - a simple triangle about the origin
            triangleVertices = new VertexPositionColorNormal[6 * width * lenght];
            Vector3[,] landscapeCoordinates = new Vector3[width + 1, lenght + 1];

            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < lenght + 1; j++)
                {
                    if (i < 7 && i > 4 && j < 20 && j > 10)
                        landscapeCoordinates[i, j] = new Vector3(i * 10, 10, j * 10);
                    else if (j < 7 && j > 4 && i < 20 && i > 10)
                        landscapeCoordinates[i, j] = new Vector3(i * 10, -5, j * 10);
                    else if (i == 7 || i == 9)
                        landscapeCoordinates[i, j] = new Vector3(i * 10, 10, j * 10);
                    else if (i == 8)
                        landscapeCoordinates[i, j] = new Vector3(i * 10, 20, j * 10);
                    else
                        landscapeCoordinates[i,j] = new Vector3(i * 10, 0, j * 10);
                }
            }


                    for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    triangleVertices[6 * j + i * lenght * 6 + 0] = new VertexPositionColorNormal(landscapeCoordinates[i, j], Color.Green, new Vector3(0,1,0));
                    triangleVertices[6 * j + i * lenght * 6 + 1] = new VertexPositionColorNormal(landscapeCoordinates[i, j + 1], Color.Blue, new Vector3(0, 1, 0));
                    triangleVertices[6 * j + i * lenght * 6 + 2] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j], Color.Red, new Vector3(0, 1, 0));
                    triangleVertices[6 * j + i * lenght * 6 + 3] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j + 1], Color.Green, new Vector3(0, 1, 0));
                    triangleVertices[6 * j + i * lenght * 6 + 4] = new VertexPositionColorNormal(landscapeCoordinates[i, j + 1], Color.Green, new Vector3(0, 1, 0));
                    triangleVertices[6 * j + i * lenght * 6 + 5] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j], Color.Green, new Vector3(0, 1, 0));
                }
            }

            

            //Vert buffer
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
                           VertexPositionColorNormal), 6 * width * lenght, BufferUsage.
                           WriteOnly);
            vertexBuffer.SetData<VertexPositionColorNormal>(triangleVertices);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myBenchModel = Content.Load<Model>("bench");
            myLaternModel = Content.Load<Model>("latern1");
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
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

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                
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
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, width * lenght * 2);
            }
            // TODO: Add your drawing code here

            Matrix[] transforms = new Matrix[myBenchModel.Bones.Count];
            myBenchModel.CopyAbsoluteBoneTransformsTo(transforms);


            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myBenchModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(benchScaleRatio, benchScaleRatio, benchScaleRatio)
                        * Matrix.CreateRotationY(bench1Rotation)
                        * Matrix.CreateTranslation(bench1Position);
                    effect.View = Matrix.CreateLookAt(camPosition, camTarget,
                        new Vector3(0f, 1f, 0f));// Y up
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                        GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
                                        
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

            transforms = new Matrix[myLaternModel.Bones.Count];
            myLaternModel.CopyAbsoluteBoneTransformsTo(transforms);


            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in myLaternModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateScale(laternScaleRatio, laternScaleRatio, laternScaleRatio)
                        * Matrix.CreateRotationY(latern1Rotation)
                        * Matrix.CreateTranslation(latern1Position);
                    effect.View = Matrix.CreateLookAt(camPosition, camTarget,
                        new Vector3(0f, 1f, 0f));// Y up
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                        GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public readonly static VertexDeclaration VertexDeclaration
            = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
                );

        public VertexPositionColorNormal(Vector3 pos, Color c, Vector3 n)
        {
            Position = pos;
            Color = c;
            Normal = n;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }
}
