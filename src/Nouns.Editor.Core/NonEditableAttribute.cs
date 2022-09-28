using System;

namespace Nouns.Editor;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NonEditableAttribute : Attribute { }