﻿using NGE.Core.Configuration;

namespace NGE
{
    // ReSharper disable once UnusedMember.Global
    internal static class Program
    {
        public static void Main(params string[] args)
        {
            Core.Bootstrap.Init();

            var configuration = Config.GetOrCreateConfiguration();
            CommandLine.ProcessArguments(ref configuration, args);

            using var game = new NounsGame(configuration, args);
            game.Run();
        }
    }
}
