using Microsoft.Extensions.DependencyInjection;

namespace Servo.Test;

public class ServiceDescriptorTest
{
    [Fact]
    public void ServiceType_And_Service() =>
        new ServiceDescriptor(_serviceType, _serviceInstance)
        .ShouldBeProperlyInitializedWith(_serviceType, _serviceType, ServiceLifetime.Singleton);

    [Fact]
    public void ServiceType_Factory_And_Lifetime() =>
        new ServiceDescriptor(_serviceType, _factory, _lifetime)
        .ShouldBeProperlyInitializedWith(_serviceType, _serviceType, _lifetime, _factory);

    [Fact]
    public void ServiceType_DerivedFactory_And_Lifetime() =>
        new ServiceDescriptor(_derivedServiceType, _derivedFactory, _lifetime)
        .ShouldBeProperlyInitializedWith(_derivedServiceType, _derivedServiceType, _lifetime, _derivedFactory);

    [Fact]
    public void ServiceType_DerivedImplementationType_And_Lifetime() =>
        new ServiceDescriptor(_serviceType, _derivedServiceType, _lifetime)
        .ShouldBeProperlyInitializedWith(_serviceType, _derivedServiceType, _lifetime);

    [Fact]
    public void ServiceType_ParentImplementationType_And_Lifetime_Throws()
    {
        Action initialization = () => new ServiceDescriptor(_serviceType, _parentServiceType, _lifetime);

        initialization.Should().Throw<ArgumentException>();
    }
}

internal static class ObjectExtensions
{
    internal static void ShouldBeProperlyInitializedWith(
        this ServiceDescriptor serviceDescriptor,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime,
        Func<object>? factory = null)
    {
        serviceDescriptor.ServiceType.Should().Be(serviceType);
        serviceDescriptor.ImplementationType.Should().Be(implementationType);
        serviceDescriptor.ServiceFactory?.Invoke().Should().BeOfType(factory?.Invoke().GetType());

        if (serviceDescriptor.ServiceInstance is null)
            serviceDescriptor.ServiceInstance.Should().BeNull();
        else
            serviceDescriptor.ServiceInstance.Should().BeOfType(implementationType);

        serviceDescriptor.Lifetime.Should().Be(lifetime);
    }
}
