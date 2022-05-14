using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ELEPHANTSRPG.Objects
{
    public class Tank
    {
        Model model;
        Player player;
        Game game;
        float wheelRotation = 0;
        Matrix[] transforms;
        ModelBone backRightWheel;
        Matrix backRightTransform;

        ModelBone backLeftWheel;
        Matrix backLeftTransform;

        ModelBone frontRightWheel;
        Matrix frontRightTransform;

        ModelBone frontLeftWheel;
        Matrix frontLeftTransform;

        public Tank(Game game, Player player)
        {
            this.game = game;
            this.player = player;
            model = game.Content.Load<Model>("tank");
            transforms = new Matrix[model.Bones.Count];

            // Set the wheel fields
            backRightWheel = model.Bones["r_back_wheel_geo"];
            backRightTransform = backRightWheel.Transform;

            backLeftWheel = model.Bones["l_back_wheel_geo"];
            backLeftTransform = backLeftWheel.Transform;

            frontRightWheel = model.Bones["r_front_wheel_geo"];
            frontRightTransform = frontRightWheel.Transform;

            frontLeftWheel = model.Bones["l_front_wheel_geo"];
            frontLeftTransform = frontLeftWheel.Transform;
        }
        public void Update()
        {
            var keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.A) || keys.IsKeyDown(Keys.W) || keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.D)) wheelRotation += 0.1f;
        }

        public void Draw()
        {
            float rotation = 0f;
            switch (player.Direction)
            {
                case ELEPHANTSRPG.Objects.Direction.North:
                    rotation = MathHelper.ToRadians(180);
                    break;
                case ELEPHANTSRPG.Objects.Direction.South:
                    rotation = MathHelper.ToRadians(0);
                    break;
                case ELEPHANTSRPG.Objects.Direction.East:
                    rotation = MathHelper.ToRadians(90);
                    break;
                case ELEPHANTSRPG.Objects.Direction.West:
                    rotation = MathHelper.ToRadians(-90);
                    break;
            }

            Matrix world = (Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(new Vector3(10, 10, 0)));
            Matrix view = Matrix.CreateLookAt(new Vector3(10, 10, 60), new Vector3(10, 10, 0), Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            model.CopyAbsoluteBoneTransformsTo(transforms);

            backRightWheel.Transform = Matrix.CreateRotationX(wheelRotation) * backRightTransform;
            backLeftWheel.Transform = Matrix.CreateRotationX(wheelRotation) * backLeftTransform;
            frontRightWheel.Transform = Matrix.CreateRotationX(wheelRotation) * frontRightTransform;
            frontLeftWheel.Transform = Matrix.CreateRotationX(wheelRotation) * frontLeftTransform;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
