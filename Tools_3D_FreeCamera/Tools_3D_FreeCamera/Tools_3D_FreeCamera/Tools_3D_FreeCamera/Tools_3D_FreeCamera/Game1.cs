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

namespace Tools_3D_FreeCamera
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
        FreeCamera camera;
        MouseState lastMouseState;
        

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
                    camera = new FreeCamera(new Vector3(1000, 1000, -2000),
                        MathHelper.ToRadians(153), // Turn around 153 degress
                        MathHelper.ToRadians(5),//Pitched up 5 degress
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

            base.Update(gameTime);
        }

        //Update 
        public void updateCamera(GameTime gameTime)
        {
            //The mouse and keystates get updated 
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            //Calculate how much it should rotate
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            //Rotate Camera
            ((FreeCamera)camera).Rotate(deltaX * 0.001f, deltaY * 0.001f);

            //
            Vector3 translation = Vector3.Zero;

            //Controls
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;
            if (keyState.IsKeyDown(Keys.Space)) translation += Vector3.Up;
            if (keyState.IsKeyDown(Keys.LeftShift)) translation += Vector3.Down;

            //Moves with 3 units per milliseconds
            translation *= 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ((FreeCamera)camera).Move(translation);

            camera.Update();

            //Mouse controls
            lastMouseState = mouseState;
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
