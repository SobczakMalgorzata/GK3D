using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D
{
    public class CameraController
    {
        public Matrix ViewMatrix { get; private set; }

        public Vector3 cameraPosition;
        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.3f;
        const float moveSpeed = 30.0f;

        public CameraController(Vector3 cameraPosition)
        {
            this.cameraPosition = cameraPosition;
        }

        public void Update(float timeDifference)
        {
            ProcessInput(timeDifference);
        }

        public void ProcessInput(float amount)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.W))
                updownRot += rotationSpeed * 10* amount;
            if (keyState.IsKeyDown(Keys.S))
                updownRot -= rotationSpeed * 10 * amount;
            if (keyState.IsKeyDown(Keys.D))
                leftrightRot -= rotationSpeed * 10 * amount;
            if (keyState.IsKeyDown(Keys.A))
                leftrightRot += rotationSpeed * 10 * amount;

            UpdateViewMatrix();

            if (keyState.IsKeyDown(Keys.Up))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.PageUp))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.PageDown))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

    }
}
