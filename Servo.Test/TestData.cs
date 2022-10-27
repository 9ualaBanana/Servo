using Microsoft.Extensions.DependencyInjection;

namespace Servo.Test;

internal static class TestData
{
    internal static Type _serviceType = typeof(SystemException);
    internal static Type _derivedServiceType = typeof(ArgumentException);
    internal static Type _parentServiceType = typeof(Exception);
    internal static SystemException _factory() => new();
    internal static ArgumentException _derivedFactory() => new();
    internal static SystemException _serviceInstance = new();
    internal static ServiceLifetime _lifetime = ServiceLifetime.Transient;

    internal static ServiceDescriptor _serviceDescriptor = new(typeof(int), default(int));
}
