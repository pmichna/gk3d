using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gk3d
{
    class Camera
    {
        private Viewport _viewPort;
        private float _leftRightRotation;
        private float _upDownRoatation;
        private const float RotationSpeed = 0.005f;
        private Vector3 _cameraPosition;
        private MouseState _originalMouseState;

        public Matrix ProjectionMatrix { get; private set; }
        public Matrix ViewMatrix { get; private set; }

        public Camera(Viewport viewPort)
            : this(viewPort, new Vector3(0, 0, 0), 0, 0)
        { }

        public Camera(Viewport viewPort, Vector3 startingPosition, float leftRightRotation, float upDownRotation)
        {
            _leftRightRotation = leftRightRotation;
            _upDownRoatation = upDownRotation;
            _cameraPosition = startingPosition;
            _viewPort = viewPort;

            const float viewAngle = MathHelper.PiOver4;
            const float nearPlane = 0.5f;
            const float farPlane = 1000.0f;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(viewAngle, viewPort.AspectRatio, nearPlane, farPlane);

            UpdateViewMatrix();
            Mouse.SetPosition(viewPort.Width / 2, viewPort.Height / 2);
            _originalMouseState = Mouse.GetState();
        }

        public void Update(MouseState currentMouseState, KeyboardState keyState)
        {
            ProcessMouse(currentMouseState);
            ProcessKeyboard(keyState);
        }

        private void ProcessKeyboard(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.W))
                AddToCameraPosition(Vector3.Forward);
            if (keyState.IsKeyDown(Keys.S))
                AddToCameraPosition(Vector3.Backward);
            if (keyState.IsKeyDown(Keys.D))
                AddToCameraPosition(Vector3.Right);
            if (keyState.IsKeyDown(Keys.A))
                AddToCameraPosition(Vector3.Left);
            if (keyState.IsKeyDown(Keys.Q))
                AddToCameraPosition(Vector3.Up);
            if (keyState.IsKeyDown(Keys.Z))
                AddToCameraPosition(Vector3.Down);
        }

        private void ProcessMouse(MouseState mouseState)
        {
            if (mouseState == _originalMouseState) return;
            float xDifference = mouseState.X - _originalMouseState.X;
            float yDifference = mouseState.Y - _originalMouseState.Y;
            _leftRightRotation -= RotationSpeed*xDifference;
            _upDownRoatation -= RotationSpeed*yDifference;
            Mouse.SetPosition(_viewPort.Width/2, _viewPort.Height/2);
            UpdateViewMatrix();
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            const float moveSpeed = 1f;
            var cameraRotation = Matrix.CreateRotationX(_upDownRoatation) * Matrix.CreateRotationY(_leftRightRotation);
            var rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            _cameraPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            var cameraRotation = Matrix.CreateRotationX(_upDownRoatation) * Matrix.CreateRotationY(_leftRightRotation);

            var cameraOriginalTarget = new Vector3(0, 0, -1);
            var cameraOriginalUpVector = new Vector3(0, 1, 0);

            var cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            var cameraFinalTarget = _cameraPosition + cameraRotatedTarget;

            var cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            ViewMatrix = Matrix.CreateLookAt(_cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}