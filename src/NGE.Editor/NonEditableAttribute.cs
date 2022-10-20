using System;

namespace NGE.Editor;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NonEditableAttribute : Attribute { }