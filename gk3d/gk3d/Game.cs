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
        private const float ARENA_CENTER_X = 0;
        private const float ARENA_CENTER_Y = 0;
        private const float ARENA_CENTER_Z = 0;
        private const int ARENA_WIDTH = 200;
        private const int ARENA_HEIGHT = 50;
        private const int ARENA_DEPTH = 400;
        private const int BACK_BUFFER_WIDTH = 1360;
        private const int BACK_BUFFER_HEIGHT = 760;
        private const bool IS_FULL_SCREEN = false;
        private const string AUTHOR_NAME = "Pawel Michna";
        private static readonly Color ARENA_COLOR = Color.DarkCyan;

        private readonly GraphicsDeviceManager _graphics;
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
            _graphics.PreferredBackBufferWidth = BACK_BUFFER_WIDTH;
            _graphics.PreferredBackBufferHeight = BACK_BUFFER_HEIGHT;
            _graphics.IsFullScreen = IS_FULL_SCREEN;
            _graphics.ApplyChanges();
            Window.Title = AUTHOR_NAME;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _arena = new Arena(Content, GraphicsDevice, new Vector3(ARENA_CENTER_X, ARENA_CENTER_Y, ARENA_CENTER_Z),
                ARENA_WIDTH, ARENA_HEIGHT, ARENA_DEPTH, ARENA_COLOR);
            _camera = new Camera(GraphicsDevice.Viewport);
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
            //var rs = new RasterizerState {FillMode = FillMode.WireFrame};
            //GraphicsDevice.RasterizerState = rs;
            _arena.Draw(_camera);
            base.Draw(gameTime);
        }
    }
}