using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NGE.Core;
using NGE.Core.Configuration;

namespace Platformer
{
    // ReSharper disable once UnusedMember.Global (Reflection)
    public class PlatformerGame : IGame
    {
        public string Name => "Platformer";
        public Version? Version => Assembly.GetExecutingAssembly().GetName().Version;

        public void Initialize(GameServiceContainer services)
        {
            Content = services.GetRequiredService<ContentManager>();
        }

        public void LoadContent(bool isAssetRebuild)
        {
           Reset();
        }

        public void UnloadContent(bool isAssetRebuild)
        {
            
        }

        public ContentManager Content { get; set; } = null!;

        public void Update()
        {
            
        }

        public void Draw(RenderTarget2D renderTarget)
        {
            renderTarget.GraphicsDevice.Clear(Color.SkyBlue);
        }

        public Task[] StartBackgroundLoading()
        {
            // simulate background loading (to test loading screen)
            var backgroundTasks = new List<Task>();
            backgroundTasks.Add(Task.Delay(TimeSpan.FromSeconds(10)));
            return backgroundTasks.ToArray();
        }

        public void OnFinishedBackgroundLoading(SpriteBatch? sb)
        {
            
        }

        public void Reset()
        {
            
        }
    }
}