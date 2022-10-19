using System.Diagnostics;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Editor;

using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

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
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0, 1));
            if (buffer.Length > 0)
            {
                foreach (var line in buffer.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                {
                    DrawLine(line);
                }
            }
            if (tail)
                ImGui.SetScrollHereY(1.0f);
            tail = false;
            ImGui.PopStyleVar();
            ImGui.EndChild();
        }

        private static void DrawLine(string line)
        {
            Vector4 lineColor;
            string lineText;

            if (line.StartsWith("Nouns Information: 0 : "))
            {
                lineText = line.Replace("Nouns Information: 0 : ", "info: ");
                lineColor = Color.LightBlue.ToImGuiVector4();
            }
            else if(line.StartsWith("Nouns Warning: 0 : "))
            {
                lineText = line.Replace("Nouns Warning: 0 : ", "warn: ");
                lineColor = Color.LightYellow.ToImGuiVector4();
            }
            else if (line.StartsWith("Nouns Error: 0 : "))
            {
                lineText = line.Replace("Nouns Error: 0 : ", " err: ");
                lineColor = Color.Red.ToImGuiVector4();
            }
            else
            {
                lineText = "dbug: " + line;
                lineColor = Color.Gray.ToImGuiVector4();
            }

            ImGui.TextColored(lineColor, lineText);
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