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

namespace Tools_CustomModel
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


            //For loop for Y Value
            for (int y = 0; y < 2; y++)
                //For loop for X value 
                for (int x = 0; x < 3; x++)
                {
                    //The Positions 
                    Vector3 position = new Vector3(
                        -200 + x * 200, -200 + y * 300, 0);
                    //Scale and Rotation
                    models.Add(new CustomModel(Content.Load<Model>(@"test"),
                        position,
                        new Vector3(0, MathHelper.ToRadians(90) * (y * 3 + 3), 0),
                        new Vector3(10f), GraphicsDevice));
                }

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Creates the camera view 3d position, Where is faces and which direction (UP) is.  
            Matrix view = Matrix.CreateLookAt(
                new Vector3(0, 300, 500),
                new Vector3(0, 0, 0),
                Vector3.Up);

            //Makes the perspective view, by setting angles, aspect ratio and near and far plains.
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio,
                0.9f, 10000.0f);


            //For each for every model created 
            foreach (CustomModel model in models)
                model.draw(view, projection);


            base.Draw(gameTime);
        }
    }
}
