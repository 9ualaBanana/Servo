using Microsoft.Extensions.DependencyInjection;

namespace Servo;

public class ServiceDescriptor
{
    public readonly Type ServiceType;
    public readonly Type? ImplementationType;
    public object? ServiceInstance => _serviceInstance ?? ServiceFactory?.Invoke();
    readonly object? _serviceInstance;
    public readonly Func<object>? ServiceFactory;
    public readonly ServiceLifetime Lifetime;


    #region Initialization
    public ServiceDescriptor(Type serviceType, object service) : this(
        serviceType,
        serviceType,
        factory: null,
        service,
        ServiceLifetime.Singleton)
    {
    }

    public ServiceDescriptor(Type serviceType, Func<object> factory, ServiceLifetime lifetime) : this(
        serviceType,
        serviceType,
        factory,
        service: null,
        lifetime)
    {
    }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime) : this(
        serviceType,
        implementationType,
        factory: null,
        service: null,
        lifetime)
    {
    }

    ServiceDescriptor(
        Type serviceType,
        Type? implementationType,
        Func<object>? factory,
        object? service,
        ServiceLifetime lifetime)
    {
        implementationType?.EnsureIsImplementationOf(serviceType);

        ServiceType = serviceType;
        ImplementationType = implementationType;
        ServiceFactory = factory;
        _serviceInstance = service;
        Lifetime = lifetime;
    }
    #endregion


    #region StaticLifetimeInitializers

    #region Transient
    public static ServiceDescriptor Transient<TService, TImplementation>(Func<TImplementation> factory)
        where TService : class
        where TImplementation : class, TService =>
        Describe(factory, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient<TService>(Func<TService> factory)
        where TService : class =>
        Describe(factory, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient(Func<object> factory) =>
        Describe(factory, ServiceLifetime.Transient);

    public static ServiceDescriptor Transient<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Describe(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);

    public static ServiceDescriptor Transient(Type serviceType, Type implementationType) =>
        Describe(serviceType, implementationType, ServiceLifetime.Transient);
    #endregion

    #region Scoped
    public static ServiceDescriptor Scoped<TService, TImplementation>(Func<TImplementation> factory)
        where TService : class
        where TImplementation : class, TService =>
        Describe(factory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped<TService>(Func<TService> factory)
        where TService : class =>
        Describe(factory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped(Func<object> factory) =>
        Describe(factory, ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Describe(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);

    public static ServiceDescriptor Scoped(Type serviceType, Type implementationType) =>
        Describe(serviceType, implementationType, ServiceLifetime.Scoped);
    #endregion

    #region Singleton
    public static ServiceDescriptor Singleton<TService, TImplementation>(Func<TImplementation> factory)
        where TService : class
        where TImplementation : class, TService =>
        Describe(factory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService>(Func<TService> factory)
        where TService : class =>
        Describe(factory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton(Func<object> factory) =>
        Describe(factory, ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService =>
        Describe(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);

    public static ServiceDescriptor Singleton<TService>(TService service)
        where TService : class =>
        Singleton(typeof(TService), service);

    public static ServiceDescriptor Singleton(Type serviceType, object service) =>
        new(serviceType, service);

    public static ServiceDescriptor Singleton(Type serviceType, Type implementationType) =>
        Describe(serviceType, implementationType, ServiceLifetime.Singleton);
    #endregion

    public static ServiceDescriptor Describe<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService =>
        Describe(typeof(TService), typeof(TImplementation), lifetime);

    public static ServiceDescriptor Describe(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        new(serviceType, implementationType, lifetime);

    public static ServiceDescriptor Describe<TService>(
        Func<TService> factory,
        ServiceLifetime lifetime) where TService : class
    {
        object downcastedFactory() => factory();
        return new(typeof(TService), downcastedFactory, lifetime);
    }

    #endregion
}


static class TypeExtensions
{
    internal static void EnsureIsImplementationOf(this Type implementationType, Type serviceType)
    {
        if (!implementationType.IsAssignableTo(serviceType))
            throw new ArgumentException(
                $"{nameof(implementationType)} must derive from {nameof(serviceType)}.",
                nameof(implementationType));
    }
}
