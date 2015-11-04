using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GK3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        bool basic;
        //Camera
        public CameraController Camera;
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        //BasicEffect for rendering
        //BasicEffect basicEffect;
        BasicEffect basicEffect;
        Effect myEffect;

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
        Vector3 latern2Position = new Vector3(150, 0, 0);
        float bench1Rotation = (float)(Math.PI / 180) * -90.0f;
        float latern1Rotation = (float)(Math.PI / 180) * -90.0f;
        float latern2Rotation = (float)(Math.PI / 180) * -90.0f;
        float bench2Rotation = (float)(Math.PI / 180) * 90.0f;
        float benchScaleRatio = 10f;
        float laternScaleRatio = 10f;

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
            basic = false;
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            Camera = new CameraController(camPosition);
            Camera.ProcessInput(0f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
                new Vector3(0f, 1f, 0f));// Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            if (basic)
            {
                //BasicEffect
                basicEffect = new BasicEffect(GraphicsDevice);
                basicEffect.Alpha = 1f;

                basicEffect.VertexColorEnabled = true;

                basicEffect.LightingEnabled = true;
                basicEffect.EnableDefaultLighting();

                basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0f);
                basicEffect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);

                basicEffect.DirectionalLight0.Enabled = true;
                basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f); // a red light
                basicEffect.DirectionalLight0.Direction = new Vector3(0, 1, 0);  // coming along the _axis
                basicEffect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 0); // with green highlights
            }
            else
            {

                //myEffect = Content.Load<Effect>("fxs");
                myEffect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f, 0.1f, 0.1f));
                myEffect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0.5f, 0.5f));
                myEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                myEffect.Parameters["xView"].SetValue(Camera.ViewMatrix);
                myEffect.Parameters["xWorld"].SetValue(worldMatrix);
                ShaderSetUp(myEffect);
            }

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
                        landscapeCoordinates[i, j] = new Vector3(i * 10, 0, j * 10);
                }
            }


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < lenght; j++)
                {
                    Vector3 w, v, normal;
                    w = (landscapeCoordinates[i + 1, j] - landscapeCoordinates[i, j]) / 10;
                    v = (landscapeCoordinates[i, j + 1] - landscapeCoordinates[i, j]) / 10;
                    normal = Vector3.Cross(w, v);
                    normal.Normalize();
                    triangleVertices[6 * j + i * lenght * 6 + 0] = new VertexPositionColorNormal(landscapeCoordinates[i, j], Color.Green, normal);
                    triangleVertices[6 * j + i * lenght * 6 + 1] = new VertexPositionColorNormal(landscapeCoordinates[i, j + 1], Color.Green, normal);
                    triangleVertices[6 * j + i * lenght * 6 + 2] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j], Color.Green, normal);
                    w = (landscapeCoordinates[i, j + 1] - landscapeCoordinates[i + 1, j + 1]) / 10;
                    v = (landscapeCoordinates[i + 1, j] - landscapeCoordinates[i + 1, j + 1]) / 10;
                    normal = Vector3.Cross(w, v);
                    normal.Normalize();
                    triangleVertices[6 * j + i * lenght * 6 + 3] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j + 1], Color.Green, normal);
                    triangleVertices[6 * j + i * lenght * 6 + 4] = new VertexPositionColorNormal(landscapeCoordinates[i, j + 1], Color.Green, normal);
                    triangleVertices[6 * j + i * lenght * 6 + 5] = new VertexPositionColorNormal(landscapeCoordinates[i + 1, j], Color.Green, normal);
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
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            

            myBenchModel = Content.Load<Model>("bench");
            myLaternModel = Content.Load<Model>("latern1");
            if (basic)
            {
            }
            else
            {
                myEffect = Content.Load<Effect>("shaderPointLight");
                foreach (ModelMesh mesh in myBenchModel.Meshes)
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                        meshPart.Effect = myEffect.Clone();
                foreach (ModelMesh mesh in myLaternModel.Meshes)
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                        meshPart.Effect = myEffect.Clone();
            }
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
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            Camera.ProcessInput(timeDifference);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (basic)
            {
                basicEffect.Projection = projectionMatrix;
                basicEffect.View = Camera.ViewMatrix;
                basicEffect.World = worldMatrix;
            }
            else
            {
                myEffect.Parameters["xCameraPosition"].SetValue(new Vector4(Camera.cameraPosition, 1));

                myEffect.Parameters["xProjection"].SetValue(projectionMatrix);
                myEffect.Parameters["xView"].SetValue(Camera.ViewMatrix);
                myEffect.Parameters["xWorld"].SetValue(worldMatrix);
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;
            if (basic)
            {
                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, width * lenght * 2);
                }

                // Draw the model. A model can have multiple meshes, so loop.
                drawModel(myBenchModel, benchScaleRatio, 0, bench1Rotation, 0, bench1Position);

                // Draw the model. A model can have multiple meshes, so loop.
                drawModel(myLaternModel, laternScaleRatio, 0, latern2Rotation, 0, latern2Position);
                drawModel(myLaternModel, laternScaleRatio, 0, latern1Rotation, 0, latern1Position);
            }
            else
            {
                foreach (EffectPass pass in myEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, width * lenght * 2);
                }

                // Draw the model. A model can have multiple meshes, so loop.
                drawModel(myBenchModel, benchScaleRatio, 0, bench1Rotation, 0, bench1Position, "PointLight");

                // Draw the model. A model can have multiple meshes, so loop.
                drawModel(myLaternModel, laternScaleRatio, 0, latern2Rotation, 0, latern2Position, "PointLight");
                drawModel(myLaternModel, laternScaleRatio, 0, latern1Rotation, 0, latern1Position, "PointLight");
            }

            // TODO: Add your drawing code here



            base.Draw(gameTime);
        }

        public void ShaderSetUp(Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["PointLight"];
            effect.Parameters["xCameraPosition"].SetValue(new Vector4(Camera.cameraPosition, 1));
            effect.Parameters["AmbientColor"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1f));
            effect.Parameters["Light1Position"].SetValue(new Vector4(150, 0, 0, 1));
            effect.Parameters["Light1Range"].SetValue(100.0f);
            effect.Parameters["Light1DiffuseColor"].SetValue(new Vector3(1f));
            effect.Parameters["Light1SpecularColor"].SetValue(new Vector4(1, 1, 1, 200));
            effect.Parameters["Light2Position"].SetValue(new Vector4(10, 0, 70, 1));
            effect.Parameters["Light2Range"].SetValue(100.0f);
            effect.Parameters["Light2DiffuseColor"].SetValue(new Vector3(1f));
            effect.Parameters["Light2SpecularColor"].SetValue(new Vector4(1, 1, 1, 200));
        }

        public void drawModel(Model model, float scaleRatio,float rotationX,float rotationY,float rotationZ,Vector3 position)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(scaleRatio, scaleRatio, scaleRatio)
                        * Matrix.CreateRotationX(rotationX)
                        * Matrix.CreateRotationY(rotationY)
                        * Matrix.CreateRotationZ(rotationZ)
                        * Matrix.CreateTranslation(position);
                    effect.View = Camera.ViewMatrix;// Y up
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f),
                        GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
                }
                // Draw the mesh, using the effects set above.
            mesh.Draw();
            }
        }
        private void drawModel(Model model, float scaleRatio, float rotationX, float rotationY, float rotationZ, Vector3 position, string technique)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (Effect effect in mesh.Effects)
                {
                    Matrix worldTemp = new Matrix();
                    worldTemp = Matrix.CreateScale(scaleRatio, scaleRatio, scaleRatio)
                        * Matrix.CreateRotationX(rotationX)
                        * Matrix.CreateRotationY(rotationY)
                        * Matrix.CreateRotationZ(rotationZ)
                        * Matrix.CreateTranslation(position);
                    ShaderSetUp(effect);
                    effect.CurrentTechnique = effect.Techniques[technique];
                    //effect.Parameters["xWorld"].SetValue(worldTemp);
                    effect.Parameters["xWorld"].SetValue(Matrix.CreateScale(scaleRatio, scaleRatio, scaleRatio)
                        * Matrix.CreateRotationX(rotationY) * Matrix.CreateTranslation(position));
                    effect.Parameters["xView"].SetValue(Camera.ViewMatrix);// Y up
                    effect.Parameters["xProjection"].SetValue(projectionMatrix);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
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
