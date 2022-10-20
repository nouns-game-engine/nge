using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Core.Configuration
{
    public interface IGame
    {
        string? Name { get; }
        Version? Version { get; }

        void Initialize(GameServiceContainer services);
        void Update();
        void Draw(RenderTarget2D renderTarget);

        Task[] StartBackgroundLoading();
        void OnFinishedBackgroundLoading(SpriteBatch sb);
        void Reset();
    }
}
