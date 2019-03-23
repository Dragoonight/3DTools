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

namespace Tools_3D_Allcamera
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //camerastate
        public enum cameraStates
        {
            FreeCamera,
            ArcBallCamera,
            ChaseRotationCamera,
            ChaseCamera
        }

        cameraStates cameraState = cameraStates.FreeCamera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //A list for the model sides and vertexes
        private List<CustomModel> models = new List<CustomModel>();

        //Variable
        FreeCamera freeCamera;
        ArcBallCamera arcBallCamera;
        ChaseCamera chaseCamera;
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

            lastMouseState = Mouse.GetState();



            //Box 
            models.Add(new CustomModel(Content.Load<Model>(@"test"),
                        Vector3.Zero, Vector3.Zero, new Vector3(100f), GraphicsDevice));

                    //Ground
                    models.Add(new CustomModel(Content.Load<Model>(@"ground"),
                       Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

                    //The camera and it's turn and roll functiond
                    freeCamera = new FreeCamera(new Vector3(1000, 1000, -2000),
                        MathHelper.ToRadians(153), 
                        MathHelper.ToRadians(5),
                        GraphicsDevice);

            //The arcball camera gets the neccessary information
            arcBallCamera = new ArcBallCamera(models[0].position, 0, 0, 0, MathHelper.PiOver2, 1200, 1000, 2000, GraphicsDevice);

            //The camera and it's turn and roll functiond
            chaseCamera = new ChaseCamera(new Vector3(0, 500, 2000),
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

            switch (cameraState)
            {

                case cameraStates.FreeCamera:
                    //Updates the freecamera method
                    freeCameraUpdate(gameTime);

                    //Changing the cameratate to the corresponding class
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    {
                        cameraState = cameraStates.ArcBallCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                    {
                        cameraState = cameraStates.ChaseCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    {
                        cameraState = cameraStates.ChaseRotationCamera;
                    }


                    break;

                case cameraStates.ArcBallCamera:

                    //Updates the arcballCamera method 
                    arcBallCameraUpdate(gameTime);

                    //Changing the camerastate to the corresponding class
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                    {
                        cameraState = cameraStates.FreeCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                    {
                        cameraState = cameraStates.ChaseCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    {
                        cameraState = cameraStates.ChaseRotationCamera;
                    }


                    break;

                case cameraStates.ChaseCamera:

                    //Updates the chaseCamera method
                    ChaseCamera(gameTime);

                    //Updates the camera method
                    UpdateCamera(gameTime);

                    //Changing the cameratate to the corresponding class
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                    {
                        cameraState = cameraStates.FreeCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    {
                        cameraState = cameraStates.ArcBallCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    {
                        cameraState = cameraStates.ChaseRotationCamera;
                    }


                    break;
                case cameraStates.ChaseRotationCamera:
                    //Updates the ChaseRotationCamera method
                    ChaseRotationCamera(gameTime);

                    //Updates the camera method
                    UpdateCamera(gameTime);

                    //Changing the camerastate to the corresponding class
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                    {
                        cameraState = cameraStates.FreeCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    {
                        cameraState = cameraStates.ArcBallCamera;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                    {
                        cameraState = cameraStates.ChaseCamera;
                    }
                    break;

            }
            

            base.Update(gameTime);
        }

        //Update 
        public void freeCameraUpdate(GameTime gameTime)
        {
            //The mouse and keystates get updated 
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            //Calculate how much it should rotate
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            //Rotate Camera
            ((FreeCamera)freeCamera).Rotate(deltaX * 0.001f, deltaY * 0.001f);

            //The Transformation is zero in value 
            Vector3 translation = Vector3.Zero;

            //The transformation will change depending on the new information
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;
            if (keyState.IsKeyDown(Keys.Space)) translation += Vector3.Up;
            if (keyState.IsKeyDown(Keys.LeftShift)) translation += Vector3.Down;

            //Moves with 3 units per milliseconds
            translation *= 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ((FreeCamera)freeCamera).Move(translation);

            //Calls in free camera class
            freeCamera.Update();

            //Mouse controls
            lastMouseState = mouseState;
        }

        public void arcBallCameraUpdate(GameTime gameTime)
        {
            //The mouse and keystates get updated 
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Calculate how much the camera should rotate
            float deltaX = lastMouseState.X - mouseState.X;
            float deltaY = lastMouseState.Y - mouseState.Y;

            // Rotate camera
            ((ArcBallCamera)arcBallCamera).Rotate(deltaX * 0.01f, deltaY * 0.01f);

            // Calculate scroll wheel 
            float scrollDelta = lastMouseState.ScrollWheelValue - (float)mouseState.ScrollWheelValue;

            // Move camera
            ((ArcBallCamera)arcBallCamera).Move(scrollDelta);

            // Update camera
            arcBallCamera.Update();

            //Update the target 
            arcBallCamera.Target = models[0].position;

            // Update lastMouseState
            lastMouseState = mouseState;
        }

        void ChaseRotationCamera(GameTime gameTime)
        {
            //The mouse and keystates get updated 
            KeyboardState keyState = Keyboard.GetState();

            //Rotation change
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
            //The models rotation will change dependent on rotation change times 0.25 
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

        void ChaseCamera(GameTime gameTime)
        {
            //The mouse and keystates get updated 
            KeyboardState keyState = Keyboard.GetState();

            //Rotation change
            Vector3 rotChange = new Vector3(0, 0, 0);

            //Rotate the object      
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            //The models rotation will change dependent on rotation change times 0.25 
            models[0].rotation += rotChange * 0.25f;

            //The object will only move when space is pressed
            if (!keyState.IsKeyDown(Keys.W))
                return;

            //Decide which direction that the object should move to 
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                models[0].rotation.Y, models[0].rotation.X, models[0].rotation.Z);

            //Move the object in the direction given after the rotation
            models[0].position += Vector3.Transform(Vector3.Forward, rotation)
                                  * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;

        }


        //Update 
        void UpdateCamera(GameTime gameTime)
        {
            //Move the camera to the object position and rotation
            ((ChaseCamera)chaseCamera).Move(models[0].position, models[0].rotation);
            //update the camera 
            chaseCamera.Update();

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            //A for each for every gamestate will change the view and projection
            foreach (CustomModel model in models)
                if (cameraState == cameraStates.FreeCamera)
                model.draw(freeCamera.View, freeCamera.Projection);
                else if (cameraState == cameraStates.ArcBallCamera)          
                model.draw(arcBallCamera.View, arcBallCamera.Projection);

                else if (cameraState == cameraStates.ChaseCamera)
                model.draw(chaseCamera.View, chaseCamera.Projection);
                else if (cameraState == cameraStates.ChaseRotationCamera)
                    model.draw(chaseCamera.View, chaseCamera.Projection);

            
            base.Draw(gameTime);
        }
    }
}
