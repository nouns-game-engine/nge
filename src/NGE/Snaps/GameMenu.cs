using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Core.Configuration;
using Nouns.Editor;

namespace NGE.Snaps
{
    // ReSharper disable once UnusedMember.Global
    [Order(-1)]
    internal class GameMenu : IEditorMenu
    {
        private readonly IGame? game;

        public bool Enabled => true;
        public string Label => game?.Name ?? "Game";

        public GameMenu(IGame? game)
        {
            this.game = game;
        }

        public void Layout(IEditingContext context, GameTime gameTime)
        {
            if (game?.Version != null)
            {
                const string version = "Version:";
                ImGui.Text(version);
                ImGui.SameLine(ImGui.CalcTextSize(version).X + 10f);
                ImGui.TextDisabled(game?.Version.ToString());
            }
        }
    }
}
