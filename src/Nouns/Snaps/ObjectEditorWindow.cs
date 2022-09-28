using System;
using System.Collections;
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
        public bool Enabled => objectUnderEdit != null;

        public ImGuiWindowFlags Flags => ImGuiWindowFlags.AlwaysAutoResize;
        public string Label => type.Name;
        public string Shortcut => null!;
        public int Width => 0;
        public int Height => 0;

        private readonly T objectUnderEdit;
        private readonly Type type;

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Dictionary<Type, MemberInfo[]> cache = new();

        public ObjectEditorWindow(T objectUnderEdit)
        {
            this.objectUnderEdit = objectUnderEdit;
            type = objectUnderEdit?.GetType() ?? throw new NullReferenceException();
        }

        public void Layout(IEditingContext context, GameTime gameTime, ref bool opened)
        {
            if (objectUnderEdit == null)
                return;

            LayoutMembers(objectUnderEdit, null, objectUnderEdit, GetOrCacheMembersForType(type));
        }

        private static void SetValue(MemberInfo member, Type memberType, object? parent, MemberInfo? parentMember, ref object target, object valueToSet)
        {
            if (valueToSet is bool b) 
            {
                if (memberType != typeof(bool))
                    return;

                switch (member)
                {
                    case FieldInfo f:
                        f.SetValue(target, !b);
                        break;
                    case PropertyInfo p:
                        p.SetValue(target, !b);
                        break;
                }
            }
            else
            {
                switch (member)
                {
                    case FieldInfo f:
                        f.SetValue(target, valueToSet);
                        MaybeSetParentValueType(parent, parentMember, target);
                        break;
                    case PropertyInfo p:
                        p.SetValue(target, valueToSet);
                        MaybeSetParentValueType(parent, parentMember, target);
                        break;
                }
            }
        }

        private static void MaybeSetParentValueType(object? parent, MemberInfo? parentMember, object target)
        {
            if (target.GetType().IsValueType && parent != null && parentMember != null)
            {
                switch (parentMember)
                {
                    case FieldInfo pf:
                        pf.SetValue(parent, target);
                        break;
                    case PropertyInfo pp:
                        pp.SetValue(target, target);
                        break;
                }
            }
        }

        private static void ReadOnlyValue(string name, ref object v)
        {
            ImGui.TextDisabled(name);
        }

        private void ReadWriteValue(string name, MemberInfo member, object? parent, MemberInfo? parentMember, ref object target, ref object valueToSet)
        {
            var valueType = valueToSet.GetType();

            if (valueType == typeof(string))
            {
                var s = (string)valueToSet;
                if (ImGui.InputText(name, ref s, 100))
                    SetValue(member, valueType, parent, parentMember, ref target, s);
            }
            else if (valueType == typeof(int))
            {
                var i = (int)valueToSet;
                if (ImGui.InputInt(name, ref i))
                    SetValue(member, valueType, parent, parentMember, ref target, i);
            }
            else if (valueType == typeof(float))
            {
                var f = (float)valueToSet;
                if (ImGui.InputFloat(name, ref f))
                    SetValue(member, valueType, parent, parentMember, ref target, f);
            }
            else if (valueType == typeof(double))
            {
                var d = (double)valueToSet;
                if (ImGui.InputDouble(name, ref d))
                    SetValue(member, valueType, parent, parentMember, ref target, d);
            }
            else if(valueType == typeof(bool))
            {
                var b = (bool)valueToSet;
                if (ImGui.Checkbox(name, ref b))
                    SetValue(member, valueType, parent, parentMember, ref target, !b);
            }
            else if(valueToSet is Enum e)
            {
                var current = (int) Convert.ChangeType(valueToSet, e.GetTypeCode());
                var names = Enum.GetNames(valueType);
                if (ImGui.Combo(name, ref current, names, names.Length))
                {
                    SetValue(member, valueType, parent, parentMember, ref target, current);
                    ImGui.EndCombo();
                }
            }
            else
            {
                var visible = true;
                ImGui.CollapsingHeader(member.Name, ref visible);
                LayoutMembers(parent, member, valueToSet, GetOrCacheMembersForType(valueToSet.GetType()));
            }
        }

        public object Object => objectUnderEdit!;

        private static IEnumerable<MemberInfo> GetOrCacheMembersForType(Type type)
        {
            if(!cache.TryGetValue(type, out var members))
            {
                var properties = type.GetProperties()
                    // currently does not support collection types
                    .Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                    ;

                cache.Add(type, members = properties.Cast<MemberInfo>()
                    .Concat(
                        type.GetFields()
                            // currently does not support collection types
                            .Where(f => !typeof(IEnumerable).IsAssignableFrom(f.FieldType))
                        ).ToArray());
            }

            return members;
        }

        private void LayoutMembers(object? parent, MemberInfo? parentMember, object instance, IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
            {
                if (Attribute.IsDefined(member, typeof(NonEditableAttribute)))
                    continue;

                switch (member)
                {
                    case FieldInfo f:
                    {
                        var v = f.GetValue(instance);
                        ReadWriteValue(f.Name, f, parent, parentMember, ref instance, ref v!);
                        break;
                    }
                    case PropertyInfo p:
                    {
                        var v = p.GetValue(instance);
                        switch (p.CanRead)
                        {
                            case true when !p.CanWrite:
                                ReadOnlyValue(p.Name, ref v!);
                                break;
                            case true when p.CanWrite:
                                ReadWriteValue(p.Name, p, parent, parentMember, ref instance, ref v!);
                                break;
                        }

                        break;
                    }
                }
            }
        }
    }
}