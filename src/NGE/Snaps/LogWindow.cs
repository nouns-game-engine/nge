using System.Diagnostics;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using NGE.Editor;
using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

namespace NGE.Snaps
{
    // ReSharper disable once UnusedMember.Global
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

        private bool queueBuffer;
        private string cachedBuffer = "";

        public void UpdateLayout(IEditingContext context, GameTime gameTime)
        {
            if (queueBuffer)
            {
                cachedBuffer = buffer.ToString();
                queueBuffer = false;
            }
        }

        public void DrawLayout(IEditingContext context, GameTime gameTime, ref bool opened)
        {
            if (ImGui.Button("Clear"))
                Clear();
            
            ImGui.Separator();
            ImGui.BeginChild("scrolling");
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0, 1));
            if (buffer.Length > 0)
            {
                foreach (var line in cachedBuffer.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
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

            if (line.StartsWith("NGE Information: 0 : "))
            {
                lineText = line.Replace("NGE Information: 0 : ", "info: ");
                lineColor = Color.LightBlue.ToImGuiVector4();
            }
            else if(line.StartsWith("NGE Warning: 0 : "))
            {
                lineText = line.Replace("NGE Warning: 0 : ", "warn: ");
                lineColor = Color.Yellow.ToImGuiVector4();
            }
            else if (line.StartsWith("NGE Error: 0 : "))
            {
                lineText = line.Replace("NGE Error: 0 : ", " err: ");
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
            queueBuffer = true;
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