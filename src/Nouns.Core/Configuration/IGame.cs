using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Core.Configuration
{
    public interface IGame
    {
        void Initialize(GameServiceContainer services);
        void Update();
        void Draw(RenderTarget2D renderTarget);

        Task[] StartBackgroundLoading();
        void OnFinishedBackgroundLoading(SpriteBatch sb);
        void Reset();
    }
}
