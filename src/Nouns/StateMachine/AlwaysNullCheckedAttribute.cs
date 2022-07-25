using System;

namespace Nouns.StateMachine
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class AlwaysNullCheckedAttribute : Attribute { }
}