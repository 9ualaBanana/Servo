using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Servo;

public class ServiceCollection : ICollection<ServiceDescriptor>
{
    readonly Dictionary<Type, List<ServiceDescriptor>> _services = new();


    #region LifetimeAdds

    #region Transient

    #region AddTransient
    public ServiceCollection AddTransient<TService>() where TService : class =>
        Add<TService>(ServiceLifetime.Transient);

    public ServiceCollection AddTransient(Type serviceType) =>
        Add(serviceType, ServiceLifetime.Transient);

    public ServiceCollection AddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Add<TService, TImplementation>(ServiceLifetime.Transient);

    public ServiceCollection AddTransient(Type serviceType, Type implementationType) =>
        Add(serviceType, implementationType, ServiceLifetime.Transient);

    public ServiceCollection AddTransient<TService>(Func<TService> factory) where TService : class =>
        Add(factory, ServiceLifetime.Transient);

    public ServiceCollection AddTransient(Type serviceType, Func<object> factory) =>
        Add(serviceType, factory, ServiceLifetime.Transient);
    #endregion

    #region TryAddTransient
    public ServiceCollection TryAddTransient<TService>() where TService : class =>
        TryAdd<TService>(ServiceLifetime.Transient);

    public ServiceCollection TryAddTransient(Type serviceType) =>
        TryAdd(serviceType, ServiceLifetime.Transient);

    public ServiceCollection TryAddTransient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        TryAdd<TService, TImplementation>(ServiceLifetime.Transient);

    public ServiceCollection TryAddTransient(Type serviceType, Type implementationType) =>
        TryAdd(serviceType, implementationType, ServiceLifetime.Transient);

    public ServiceCollection TryAddTransient<TService>(Func<TService> factory) where TService : class =>
        TryAdd(factory, ServiceLifetime.Scoped);

    public ServiceCollection TryAddTransient(Type serviceType, Func<object> factory) =>
        TryAdd(serviceType, factory, ServiceLifetime.Transient);
    #endregion

    #endregion

    #region Scoped

    #region AddScoped
    public ServiceCollection AddScoped<TService>() where TService : class =>
        Add<TService>(ServiceLifetime.Scoped);

    public ServiceCollection AddScoped(Type serviceType) =>
        Add(serviceType, ServiceLifetime.Scoped);

    public ServiceCollection AddScoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Add<TService, TImplementation>(ServiceLifetime.Scoped);

    public ServiceCollection AddScoped(Type serviceType, Type implementationType) =>
        Add(serviceType, implementationType, ServiceLifetime.Scoped);

    public ServiceCollection AddScoped<TService>(Func<TService> factory) where TService : class =>
        Add(factory, ServiceLifetime.Scoped);

    public ServiceCollection AddScoped(Type serviceType, Func<object> factory) =>
        Add(serviceType, factory, ServiceLifetime.Scoped);
    #endregion

    #region TryAddScoped
    public ServiceCollection TryAddScoped<TService>() where TService : class =>
        TryAdd<TService>(ServiceLifetime.Scoped);

    public ServiceCollection TryAddScoped(Type serviceType) =>
        TryAdd(serviceType, ServiceLifetime.Scoped);

    public ServiceCollection TryAddScoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        TryAdd<TService, TImplementation>(ServiceLifetime.Scoped);

    public ServiceCollection TryAddScoped(Type serviceType, Type implementationType) =>
        TryAdd(serviceType, implementationType, ServiceLifetime.Scoped);

    public ServiceCollection TryAddScoped<TService>(Func<TService> factory) where TService : class =>
        TryAdd(factory, ServiceLifetime.Scoped);

    public ServiceCollection TryAddScoped(Type serviceType, Func<object> factory) =>
        TryAdd(serviceType, factory, ServiceLifetime.Scoped);
    #endregion

    #endregion

    #region Singleton

    #region AddSingleton
    public ServiceCollection AddSingleton<TService>(TService service) where TService : class =>
        AddSingleton(typeof(TService), service);

    public ServiceCollection AddSingleton(Type serviceType, object service) =>
        Add(new ServiceDescriptor(serviceType, service));

    public ServiceCollection AddSingleton<TService>() where TService : class =>
        Add<TService>(ServiceLifetime.Singleton);

    public ServiceCollection AddSingleton(Type serviceType) =>
        Add(serviceType, ServiceLifetime.Singleton);

    public ServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Add<TService, TImplementation>(ServiceLifetime.Singleton);

    public ServiceCollection AddSingleton(Type serviceType, Type implementationType) =>
        Add(serviceType, implementationType, ServiceLifetime.Singleton);

    public ServiceCollection AddSingleton<TService>(Func<TService> factory) where TService : class =>
        Add(factory, ServiceLifetime.Singleton);

    public ServiceCollection AddSingleton(Type serviceType, Func<object> factory) =>
        Add(serviceType, factory, ServiceLifetime.Singleton);
    #endregion

    #region TryAddSingleton
    public ServiceCollection TryAddSingleton<TService>(TService service) where TService : class =>
        TryAddSingleton(typeof(TService), service);

    public ServiceCollection TryAddSingleton(Type serviceType, object service) =>
        TryAdd(new ServiceDescriptor(serviceType, service));

    public ServiceCollection TryAddSingleton<TService>() where TService : class =>
        TryAdd<TService>(ServiceLifetime.Singleton);

    public ServiceCollection TryAddSingleton(Type serviceType) =>
        TryAdd(serviceType, ServiceLifetime.Singleton);

    public ServiceCollection TryAddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        TryAdd<TService, TImplementation>(ServiceLifetime.Singleton);

    public ServiceCollection TryAddSingleton(Type serviceType, Type implementationType) =>
        TryAdd(serviceType, implementationType, ServiceLifetime.Singleton);

    public ServiceCollection TryAddSingleton<TService>(Func<TService> factory) where TService : class =>
        TryAdd(factory, ServiceLifetime.Singleton);

    public ServiceCollection TryAddSingleton(Type serviceType, Func<object> factory) =>
        TryAdd(serviceType, factory, ServiceLifetime.Singleton);
    #endregion

    #endregion

    #endregion

    #region RemoveAll
    public ServiceCollection RemoveAll<TService>() => RemoveAll(typeof(TService));

    public ServiceCollection RemoveAll(Type serviceType)
    { _services.Remove(serviceType); return this; }
    #endregion

    #region ICollection
    public int Count => _services.Aggregate(0, (all, service) => service._Implementations().Count);

    public bool IsReadOnly => false;

    #region Adds

    #region Add
    public ServiceCollection Add(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        foreach (var serviceDescriptor in serviceDescriptors)
            Add(serviceDescriptor);
        return this;
    }

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
    public ServiceCollection TryAddEnumerable(ServiceDescriptor serviceDescriptor) =>
        _AddCore(serviceDescriptor, Add_.IfImplementationNotRegistered);

    public ServiceCollection TryAddEnumerable(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        foreach (var serviceDescriptor in serviceDescriptors)
            _AddCore(serviceDescriptor, Add_.IfImplementationNotRegistered);
        return this;
    }

    public ServiceCollection TryAdd(IEnumerable<ServiceDescriptor> serviceDescriptors)
    {
        foreach (var serviceDescriptor in serviceDescriptors)
            TryAdd(serviceDescriptor);
        return this;
    }

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

    public ServiceCollection TryAdd(ServiceDescriptor serviceDescriptor) =>
        _AddCore(serviceDescriptor, Add_.IfServiceNotRegistered);

    public ServiceCollection TryAdd<TService>(Func<TService> factory, ServiceLifetime lifetime)
        where TService : class =>
        _AddCore(ServiceDescriptor.Describe(factory, lifetime), Add_.IfServiceNotRegistered);

    public ServiceCollection TryAdd(Type serviceType, Func<object> factory, ServiceLifetime lifetime) =>
        _AddCore(new ServiceDescriptor(serviceType, factory, lifetime), Add_.IfServiceNotRegistered);
    #endregion

    ServiceCollection _AddCore(ServiceDescriptor serviceDescriptor, Add_ add = Add_.Always)
    {
        if (_services.ContainsKey(serviceDescriptor.ServiceType))
            if (add is Add_.IfServiceNotRegistered) return this;
        else _services.Add(serviceDescriptor.ServiceType, new());
            
        if (_services.TryGetValue(serviceDescriptor.ServiceType, out var serviceImplementations))
        {
            if (serviceImplementations.Contains(serviceDescriptor) && add is Add_.IfImplementationNotRegistered)
                return this;
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
