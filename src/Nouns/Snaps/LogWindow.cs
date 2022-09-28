using System;
using System.Diagnostics;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Editor;

namespace Nouns.Snaps
{
    public class LogWindow : TraceListener, IEditorWindow
    {
        private readonly StringBuilder buffer = new();
        private bool tail;

        public bool Enabled => true;
        public ImGuiWindowFlags Flags => ImGuiWindowFlags.None;
        public string Label => "Log";
        public string Shortcut => "Ctrl+L";
        public int Width => 500;
        public int Height => 400;

        public LogWindow()
        {
            Trace.Listeners.Add(this);
        }

        public void Clear()
        {
            buffer.Clear();
        }

        public void Layout(IEditingContext context, GameTime gameTime, ref bool opened)
        {
            if (ImGui.Button("Clear"))
                Clear();
            ImGui.Separator();
            ImGui.BeginChild("scrolling");
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(0, 1));
            if (buffer.Length > 0)
                ImGui.TextUnformatted(buffer.ToString());
            if (tail)
                ImGui.SetScrollHereY(1.0f);
            tail = false;
            ImGui.PopStyleVar();
            ImGui.EndChild();
        }

        private void AddLog(string? message, params object[] args)
        {
            if (message == null) return;
            buffer.AppendFormat(message, args);
            tail = true;
        }

        #region TraceListener

        public override void Write(string? message)
        {
            AddLog(message);
        }

        public override void WriteLine(string? message)
        {
            AddLog(message + Environment.NewLine);
        }

        #endregion
    }
}