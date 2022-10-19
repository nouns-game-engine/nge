using Nouns.Core.Configuration;
using Nouns.Graphics.Core;

namespace NGE
{
    // ReSharper disable once UnusedMember.Global
    internal static class Program
    {
        public static void Main(params string[] args)
        {
            Bootstrap.Init();
            var configuration = Config.GetOrCreateConfiguration();
            using var game = new NounsGame(configuration, args);
            game.Run();
        }
    }
}
