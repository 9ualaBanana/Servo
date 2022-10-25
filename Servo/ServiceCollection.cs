﻿using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Servo;

public class ServiceCollection : ICollection<ServiceDescriptor>
{
    readonly Dictionary<Type, List<ServiceDescriptor>> _services = new();

    #region ICollection
    public int Count => _services.Aggregate(0, (all, service) => service._Implementations().Count);

    public bool IsReadOnly => false;

    #region Adds

    #region Add
    public ServiceCollection Add<TService>(ServiceLifetime lifetime)
    where TService : class =>
        Add<TService, TService>(lifetime);

    public ServiceCollection Add(Type serviceType, ServiceLifetime lifetime) =>
        Add(serviceType, serviceType, lifetime);

    public ServiceCollection Add<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService =>
        Add(typeof(TService), typeof(TImplementation), lifetime);

    public ServiceCollection Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

    public ServiceCollection Add(ServiceDescriptor serviceDescriptor) => _AddCore(serviceDescriptor);

    public ServiceCollection Add<TService>(Func<TService> factory, ServiceLifetime lifetime)
        where TService : class =>
        _AddCore(ServiceDescriptor.Describe(factory, lifetime));

    public ServiceCollection Add(Type serviceType, Func<object> factory, ServiceLifetime lifetime) =>
        _AddCore(new ServiceDescriptor(serviceType, factory, lifetime));

    void ICollection<ServiceDescriptor>.Add(ServiceDescriptor serviceDescriptor) => Add(serviceDescriptor);
    #endregion

    #region TryAdd
    public ServiceCollection TryAdd<TService>(ServiceLifetime lifetime)
    where TService : class =>
        TryAdd<TService, TService>(lifetime);

    public ServiceCollection TryAdd(Type serviceType, ServiceLifetime lifetime) =>
        TryAdd(serviceType, serviceType, lifetime);

    public ServiceCollection TryAdd<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService =>
        TryAdd(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));

    public ServiceCollection TryAdd(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        TryAdd(new ServiceDescriptor(serviceType, implementationType, lifetime));

    public ServiceCollection TryAdd(ServiceDescriptor serviceDescriptor) => _AddCore(serviceDescriptor, try_: true);

    public ServiceCollection TryAdd<TService>(Func<TService> factory, ServiceLifetime lifetime)
        where TService : class =>
        _AddCore(ServiceDescriptor.Describe(factory, lifetime), try_: true);

    public ServiceCollection TryAdd(Type serviceType, Func<object> factory, ServiceLifetime lifetime) =>
        _AddCore(new ServiceDescriptor(serviceType, factory, lifetime), try_: true);
    #endregion

    ServiceCollection _AddCore(ServiceDescriptor serviceDescriptor, bool try_ = false)
    {
        if (!_services.ContainsKey(serviceDescriptor.ServiceType))
            _services.Add(serviceDescriptor.ServiceType, new());
        if (_services.TryGetValue(serviceDescriptor.ServiceType, out var serviceImplementations))
        {
            if (try_ && serviceImplementations.Contains(serviceDescriptor)) return this;
            serviceImplementations.Add(serviceDescriptor);
        }
        return this;
    }
    #endregion

    public void Clear() => _services.Clear();

    public bool Contains(ServiceDescriptor serviceDescriptor) =>
        _services.Any(service => service._Implementations().Contains(serviceDescriptor));

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        foreach (var service in _services)
        {
            service._Implementations().CopyTo(array, arrayIndex);
            arrayIndex += service._Implementations().Count;
        }
    }

    public bool Remove(ServiceDescriptor serviceDescriptor)
    {
        bool isRemoved;
        foreach (var service in _services)
        {
            if (isRemoved = service._Implementations().Remove(serviceDescriptor))
                return true;
        }
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ServiceDescriptor> GetEnumerator() =>
        _services.SelectMany(service => service._Implementations()).GetEnumerator();
    #endregion
}

static class ServiceExtensions
{
    internal static List<ServiceDescriptor> _Implementations(
        this KeyValuePair<Type, List<ServiceDescriptor>> service
        ) => service.Value;
}