using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Editor;

namespace Nouns.Snaps
{
    public class ObjectEditorWindow<T> : IEditorWindow, IEditObject
    {
        public bool Enabled => instance != null;
        public ImGuiWindowFlags Flags => ImGuiWindowFlags.AlwaysAutoResize;
        public string Label => type.Name ?? "??";
        public string Shortcut => null!;
        public int Width => 0;
        public int Height => 0;

        private readonly T instance;
        private readonly Type type;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<Type, MemberInfo[]> cache = new();

        public ObjectEditorWindow(T instance)
        {
            this.instance = instance;
            type = instance?.GetType() ?? throw new NullReferenceException();
            cache[instance.GetType()] = instance.GetType().GetProperties().Cast<MemberInfo>()
                .Concat(instance.GetType().GetFields()).ToArray();
        }

        public void Layout(IEditingContext context, GameTime gameTime, ref bool opened)
        {
            foreach (var member in cache[type])
            {
                if (Attribute.IsDefined(member, typeof(NonEditableAttribute)))
                    continue;

                switch (member)
                {
                    case FieldInfo f:
                        {
                            var v = f.GetValue(instance);
                            ReadWriteValue(f.Name, f, ref v);
                            break;
                        }
                    case PropertyInfo p:
                        {
                            var v = p.GetValue(instance);
                            switch (p.CanRead)
                            {
                                case true when !p.CanWrite:
                                    ReadOnlyValue(p.Name, ref v);
                                    break;
                                case true when p.CanWrite:
                                    ReadWriteValue(p.Name, p, ref v);
                                    break;
                            }
                            break;
                        }
                }
            }
        }

        private void SetValue(MemberInfo member, Type memberType, object value)
        {
            if (value is bool b)
            {
                if (memberType != typeof(bool))
                    return;
                switch (member)
                {
                    case FieldInfo f:
                        f.SetValue(instance, !b);
                        break;
                    case PropertyInfo p:
                        p.SetValue(instance, !b);
                        break;
                }
            }
            else
            {
                switch (member)
                {
                    case FieldInfo f:
                        f.SetValue(instance, value);
                        break;
                    case PropertyInfo p:
                        p.SetValue(instance, value);
                        break;
                }
            }
        }

        private static void ReadOnlyValue(string name, ref object v)
        {
            ImGui.TextDisabled(name);
        }

        private void ReadWriteValue(string name, MemberInfo member, ref object v)
        {
            var valueType = v.GetType();

            if (valueType == typeof(string))
            {
                var s = (string)v;
                if (ImGui.InputText(name, ref s, 100))
                    SetValue(member, valueType, s);
            }

            if (valueType == typeof(int))
            {
                var i = (int)v;
                if (ImGui.InputInt(name, ref i))
                    SetValue(member, valueType, i);
            }

            if (valueType == typeof(float))
            {
                var f = (float)v;
                if (ImGui.InputFloat(name, ref f))
                    SetValue(member, valueType, f);
            }

            if (valueType == typeof(double))
            {
                var d = (double)v;
                if (ImGui.InputDouble(name, ref d))
                    SetValue(member, valueType, d);
            }

            if (valueType == typeof(bool))
            {
                var b = (bool)v;
                if (ImGui.Checkbox(name, ref b))
                    SetValue(member, valueType, !b);
            }

            if (v is Enum e)
            {
                var current = (int)Convert.ChangeType(v, e.GetTypeCode());
                var names = Enum.GetNames(valueType);
                if (ImGui.Combo(name, ref current, names, names.Length))
                {
                    SetValue(member, valueType, current);
                    ImGui.EndCombo();
                }
            }
        }

        public object Object => instance!;
    }
}