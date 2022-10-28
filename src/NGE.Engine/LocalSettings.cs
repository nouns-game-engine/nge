using NGE.Engine.InputManagement;
using SDL2;

namespace NGE.Engine;

public class LocalSettings
{
    public readonly string gameName;

    public LocalSettings(string gameName)
    {
        this.gameName = gameName;
    }

    public string GetPlatformSettingsDir()
    {
        var platform = SDL.SDL_GetPlatform();
        switch (platform)
        {
            case "Linux":
            {
                var osDir = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (!string.IsNullOrEmpty(osDir))
                    return EnsureExists(Path.Combine(osDir, gameName));
                osDir = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(osDir) ? @"." : EnsureExists(Path.Combine(osDir, $".config/{gameName}"));
            }
            case "Mac OS X":
            {
                var osDir = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(osDir) ? @"." : EnsureExists(Path.Combine(osDir, "Library/Application Support/" + gameName));
            }
            case "Windows":
            {
                return @".";
            }
            default:
                throw new NotSupportedException($"SDL2 platform not supported: {platform}");
        }

        static string EnsureExists(string directory) => Directory.CreateDirectory(directory).Name;
    }
}

public class LocalSettings<TPlayerButton> : LocalSettings
    where TPlayerButton : Enum
{
    public InputBindings<TPlayerButton> inputBindings = new();

    public LocalSettings(string gameName) : base(gameName) { }
}