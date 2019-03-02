using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_3D_FreeCamera
{
    public class CustomModel
    {
        //Position, Rotation and Scale
        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }
        public Vector3 scale { get; set; }

        //The 3d model
        public Model model { get; private set; }

        //The model transform
        private Matrix[] modelTransforms;

        //The graphicsDevice
        private GraphicsDevice graphicsDevice;

      

        //The Contrsuctor
        public CustomModel(Model model, Vector3 position, Vector3 rotation,
            Vector3 scale, GraphicsDevice graphicsDevice)
        {
            
            this.model = model;
            //ModelTransforms will get a new matrix value that has the model bones count
            modelTransforms = new Matrix[model.Bones.Count];
            //Now it will give the modelTranforms value to model CopyAbsoluteBoneTransformsTo variable
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.graphicsDevice = graphicsDevice;
        }

        //Draw method
        public void draw(Matrix view, Matrix Projection)
        {
            //The baseWorld will include the scale, Pitch and all of the roations and position.
            Matrix baseWorld = Matrix.CreateScale(scale)
                * Matrix.CreateFromYawPitchRoll(
                    rotation.Y, rotation.X, rotation.Z)
                    * Matrix.CreateTranslation(position);

            //Is a loop that will loop until every side of the model is changed
            foreach (ModelMesh mesh in model.Meshes)
            {
                //Calculate each mesh's world matrix
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] *
                    baseWorld;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //Basic effect that is already a variable/class that will change the faces to the texture we want
                    BasicEffect effect = (BasicEffect)part.Effect;

                    //Set the world view, and protection matrices to the effect
                    effect.World = localWorld;
                    effect.View = view;
                    effect.Projection = Projection;
                    //Enables the default light
                    effect.EnableDefaultLighting();

                }
                //Draw the mesh
                mesh.Draw();
            }




        }
    }
}
