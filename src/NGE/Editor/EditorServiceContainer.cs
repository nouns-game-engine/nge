using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;

namespace NGE.Editor;

public sealed class EditorServiceContainer : IServiceContainer
{
    private readonly GameServiceContainer container;

    public EditorServiceContainer(GameServiceContainer container)
    {
        this.container = container;
    }

    public object? GetService(Type serviceType)
    {
        return container.GetService(serviceType);
    }

    public void AddService(Type serviceType, ServiceCreatorCallback callback)
    {
        throw new NotSupportedException();
    }

    public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
    {
        throw new NotSupportedException();
    }

    public void AddService(Type serviceType, object serviceInstance)
    {
        container.AddService(serviceType, serviceInstance);
    }

    public void AddService(Type serviceType, object serviceInstance, bool promote)
    {
        container.AddService(serviceType, serviceInstance);
    }

    public void RemoveService(Type serviceType)
    {
        container.RemoveService(serviceType);
    }

    public void RemoveService(Type serviceType, bool promote)
    {
        container.RemoveService(serviceType);
    }
}