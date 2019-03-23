using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tools_3D_Allcamera
{
    public class FreeCamera : Camera 
    {
        //Pitch and Yaw
        public float Yaw { get; set; }
        public float Pitch { get; set; }

        //Position and Target
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }
        //Movement of the total camera
        private Vector3 translation;

        //Constructor
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, 
            GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            translation = Vector3.Zero;
        }
        //Rotating method
        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }
        //Moving method
        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }
        //Update
        public override void Update()
        {
            //Calculate rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            //Move position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            //Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            //Calculate up Vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            //Calculate view matrix
            View = Matrix.CreateLookAt(Position, Target, up);

        }
    }
}
