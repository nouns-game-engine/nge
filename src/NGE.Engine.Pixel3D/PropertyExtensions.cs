﻿namespace NGE.Engine.Pixel3D;

public static class PropertyExtensions
{
    public static string? GetString(this IDictionary<string, string> properties, string propertyName)
    {
        properties.TryGetValue(propertyName, out var value);
        return value;
    }
}