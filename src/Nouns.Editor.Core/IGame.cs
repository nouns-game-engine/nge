using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Editor
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
