using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace gk3d
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private BasicEffect _effect;
        private Arena _arena;
        private Camera _camera;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1360;
            _graphics.PreferredBackBufferHeight = 760;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            Window.Title = "Pawe� Michna";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _arena = new Arena(new Vector3(0, 0, 0), 200, 50, 400, Color.Yellow);
            _camera = new Camera(GraphicsDevice.Viewport);
            _effect = new BasicEffect(GraphicsDevice);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape)) Exit();
            _camera.Update(Mouse.GetState(), keyState);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var rs = new RasterizerState {FillMode = FillMode.WireFrame};
            GraphicsDevice.RasterizerState = rs;
            DrawArena();
            base.Draw(gameTime);
        }

        private void DrawArena()
        {
            _effect.View = _camera.ViewMatrix;
            _effect.Projection = _camera.ProjectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _arena.Vertices, 0, _arena.Vertices.Length,
                    _arena.Indices, 0, _arena.Indices.Length/3, VertexPositionColor.VertexDeclaration);
            }
        }
    }
}