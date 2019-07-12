// Lic:
// NIL And Lua Adventures (NALA)
// Core
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 19.07.12
// EndLic



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrickyUnits;

using Bubble;

namespace NALA {


    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class NALACore : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static NALACore Core { get; private set; } = null;

        public NALACore() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Core = this;
            UseJCR6.BubbleInit.Init();
            Dirry.InitAltDrives();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            #region base init! (generated code)
            base.Initialize();
            #endregion

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            #region Tricky's Quick Monogame Graphics
            if (SBubble.JCR == null) System.Diagnostics.Debug.WriteLine("EEP! JCR resource is null! But how?");
            TQMG.Init(graphics, GraphicsDevice, spriteBatch, SBubble.JCR);
            #endregion

            BubbleInit.LetsGo();


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {


            FlowManager.Update(gameTime);

            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            if (FlowManager.TimeToDie)
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);

            FlowManager.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}



