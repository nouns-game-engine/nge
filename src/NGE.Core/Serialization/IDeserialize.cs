﻿namespace NGE.Core.Serialization;

public interface IDeserialize<in TContext> where TContext : IDeserializeContext
{
    void Deserialize(TContext context);
}