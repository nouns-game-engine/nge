using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nouns.Editor
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetEditorTypes(this Assembly assembly)
        {
            var editorTypes = assembly.GetTypes()
                .Where(t => !t.IsInterface)
                .Where(t =>
                    t.Implements<IEditorWindow>() ||
                    t.Implements<IEditorMenu>() ||
                    t.Implements<IEditorDropHandler>());
            
            return editorTypes;
        }
    }
}
