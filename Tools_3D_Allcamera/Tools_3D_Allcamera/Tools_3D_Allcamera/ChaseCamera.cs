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
    public class ChaseCamera : Camera
    {
        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }

        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }

        public Vector3 PositionOffset { get; set; }
        public Vector3 TargetOffset { get; set; }

        public Vector3 RelativeCameraRotation { get; set; }

        float springiness = .15f;

        //Springiness is the force that keeps the camera behind the object
        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        //Constructore
        public ChaseCamera(Vector3 PositionOffset, Vector3 TargetOffset,
            Vector3 RelativeCameraRotation, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
        }

        //Follows the targets rotation and target
        public void Move(Vector3 newFollowTargetPosition,
            Vector3 newFollowTargetRotation)
        {
            this.FollowTargetPosition = newFollowTargetPosition;
            this.FollowTargetRotation = newFollowTargetRotation;
        }

        //The camera rotation will change through the rotation change
        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeCameraRotation += RotationChange;
        }
        //Update
        public override void Update()
        {
            //Put together rotation of the model and camera to control it
            //The camera position correctly with the conditions of the model
            Vector3 combinedRotation = FollowTargetRotation +
                RelativeCameraRotation;
            
            //Calculate the rotation camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            //calculate were the camera should be 
            Vector3 desiredPosition = FollowTargetPosition +
                Vector3.Transform(PositionOffset, rotation);
            
            //Switch between the actual position and the wished position 
            Position = Vector3.Lerp(Position, desiredPosition, Springiness);

            //Calculate the new target from rotation matrix
            Target = FollowTargetPosition +
                Vector3.Transform(TargetOffset, rotation);
            
            //Set up the vector for matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            
            //Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
