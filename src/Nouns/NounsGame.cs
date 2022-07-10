using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#if !WASM
using Nouns.Editor;
#endif

namespace Nouns;

internal class NounsGame : 
    #if !WASM 
    EditableGame
    #else   
    Game
    #endif
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable (retained)
    private readonly GraphicsDeviceManager graphics;

    private SpriteBatch spriteBatch = null!;
    
    public NounsGame()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.PreferMultiSampling = true;
        Content.RootDirectory = "Content";

        Window.Title = "nounsgame.wtf";
        Window.AllowUserResizing = true;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
#if !WASM 
        InitializeEditor();
#endif

        // calls LoadContent();
        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        base.LoadContent();
    }

    protected override void UnloadContent()
    {
        Content.Unload();
        spriteBatch.Dispose();
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime); // required to call FrameworkDispatcher

        Input.Update(IsActive);
        
#if !WASM
        if (devMenuEnabled)
            UpdateEditor(gameTime);

        else if (Input.KeyWentDown(Keys.F1))
            devMenuEnabled = !devMenuEnabled;
#endif
    }

    protected override void Draw(GameTime gameTime)
    {
#if !WASM
        GraphicsDevice.SetRenderTarget(renderTarget);
#endif
        GraphicsDevice.Clear(Color.Black);

#if !WASM
        GraphicsDevice.SetRenderTarget(null);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
        spriteBatch.End();

        if (devMenuEnabled)
        {
            imGui.BeforeLayout(gameTime);
            DrawEditor(gameTime);
            imGui.AfterLayout();
        }
#endif
        base.Draw(gameTime);
    }
}