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

namespace Tools_CustomModel
{

    //A class that needs someone to use it aka abusive relationship
    public abstract class Camera
    {
        //Matrix variables for the view and projections
        Matrix view;
        Matrix projection;
       
        //Get sets the projection property
        public Matrix Projection
        {
            get { return projection; }
            //Set will be protected from other classes
            protected set
            {
                //Will be given the matrix value again
                projection = value;
                //Generate the method generateFrustrum
                generateFrustrum();
            }
        }

        //Get sets the View property
        public Matrix View
        {
            get { return view; }
            //Set will be protected from other classes
            protected set
            {
                //Will be given the matrix value again
                view = value;
                //Generate the method generateFrustrum
                generateFrustrum();
            }
        }

        //Creates the Frustrum space and get, private set
        public BoundingFrustum Fustrum { get; private set; }

        //Creates the GraphicsDevice and Get, set
        protected GraphicsDevice GraphicsDevice { get; set; }

        //Constructor
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;

            generatePerspectiveProjectMatrix(MathHelper.PiOver4);
        }


        //Method
        public void generatePerspectiveProjectMatrix(float FieldOfView)
        {
            //A new presentation variable wil get the graphicsdevice presentation paremeter
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            //The aspectRatio will get pp
            float aspectRatio = (float)pp.BackBufferWidth /
                (float)pp.BackBufferHeight;
            //Makes the perspective projection, by setting angles, aspect ratio and near and far plains.
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }
        //Virtual Update for the other classes to change it
        public virtual void Update()
        {
        }
        //Generate Fustrum Method
        private void generateFrustrum()
        {
            //The new viewProjection will get the view multiplied with the projection
            Matrix viewProjection = View * Projection;
            //Fustrum will get a new value that has the new viewProjection
            Fustrum = new BoundingFrustum(viewProjection);
        }
        //Bool method if there is a sphere
        public bool BoundingVolumeIsView(BoundingSphere sphere)
        {
            //The returned fustrum value spehere will not be overlaped between the bounding values 
            return (Fustrum.Contains(sphere) != ContainmentType.Disjoint);
        }
        //Bool method if there is a box 
        public bool BoundingVolumeIsView(BoundingBox box)
        {
            //The returned fustrum value box will not be overlaped between the bounding values 
            return (Fustrum.Contains(box) != ContainmentType.Disjoint);
        }
    }

}
