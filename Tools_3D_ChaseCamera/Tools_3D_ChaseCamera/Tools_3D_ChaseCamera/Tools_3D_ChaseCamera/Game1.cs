using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tools_3D_ChaseCamera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //A list for the model sides and vertexes
        List<CustomModel> models = new List<CustomModel>();

        //Variable
        Camera camera;
        

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

                    //Box 
                    models.Add(new CustomModel(Content.Load<Model>(@"test"),
                        Vector3.Zero, Vector3.Zero, new Vector3(100f), GraphicsDevice));

                    //Ground
                    models.Add(new CustomModel(Content.Load<Model>(@"ground"),
                       Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

                    //The camera and it's turn and roll functiond
                    camera = new ChaseCamera(new Vector3 (1000, 1000, -2000),
                        Vector3.Zero,
                        Vector3.Zero,
                        GraphicsDevice);
             

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //FullScreen
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

            //Update method
            updateCamera(gameTime);
            updateModel2(gameTime);
            camera.Update();
            base.Update(gameTime);
        }

        void updateModel(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            //Rotate the object
            if (keyState.IsKeyDown(Keys.W))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.S))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            models[0].rotation += rotChange * 0.25f;

            //The object will only move when space is pressed
            if (!keyState.IsKeyDown(Keys.Space))
                return;

            //Decide which direction that the object should move to 
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                models[0].rotation.Y, models[0].rotation.X, models[0].rotation.Z);

            //Move the object in the direction given after the rotation
            models[0].position += Vector3.Transform(Vector3.Forward, rotation)
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;

           
        }

        void updateModel2(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            //
            if (keyState.IsKeyDown(Keys.W))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.S))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            models[0].position += rotChange * 10.0f;

            //
            Matrix position = Matrix.CreateFromYawPitchRoll(
                models[0].rotation.Y, models[0].rotation.X, models[0].rotation.Z);

            //
            models[0].position += Vector3.Transform(rotChange, position)
                * (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

        }


        //Update 
        void updateCamera(GameTime gameTime)
        {
            //Move the camera to the object position and rotation
            ((ChaseCamera)camera).Move(models[0].position, models[0].rotation);
            //update the camera 
            camera.Update();

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            //For each for every model created 
            foreach (CustomModel model in models)
                model.draw(camera.View, camera.Projection);


            base.Draw(gameTime);
        }
    }
}
