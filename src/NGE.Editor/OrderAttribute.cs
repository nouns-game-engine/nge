﻿using System;

namespace NGE.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OrderAttribute : Attribute
    {
        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}