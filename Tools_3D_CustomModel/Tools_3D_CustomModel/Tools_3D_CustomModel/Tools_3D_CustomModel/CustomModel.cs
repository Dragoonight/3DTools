using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_3D_CustomModel
{
    public class CustomModel
    {
        //
        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }
        public Vector3 scale { get; set; }

        //
        public Model model { get; private set; }

        //
        private Matrix[] modelTransforms;

        //
        private GraphicsDevice graphicsDevice;

        //
        List<CustomModel> models = new List<CustomModel>();

        //
        public void customModel(Model model, Vector3 position, Vector3 rotation,
            Vector3 scale, GraphicsDevice graphicsDevice)
        {
            //
            this.model = model;

            //
            modelTransforms = new Matrix[model.Bones.Count];

            //
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            //
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            //
            this.graphicsDevice = graphicsDevice;
        }

        //
        public void draw(Matrix view, Matrix Projection)
        {
            //
            Matrix baseWorld = Matrix.CreateScale(scale)
                * Matrix.CreateFromYawPitchRoll(
                    rotation.Y, rotation.X, rotation.Z)
                    * Matrix.CreateTranslation(position);

            //
            foreach (ModelMesh mesh in model.Meshes)
            {
                //Calculate each mesh's world matrix
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] *
                    baseWorld;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect effect = (BasicEffect)part.Effect;

                    //Set the world view, and protection matrices to the effect
                    effect.World = localWorld;
                    effect.View = view;
                    effect.Projection = Projection;

                    effect.EnableDefaultLighting();

                }
                //Draw the mesh
                mesh.Draw();
            }




        }
    }
}
