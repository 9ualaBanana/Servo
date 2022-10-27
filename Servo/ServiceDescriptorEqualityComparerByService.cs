using System.Diagnostics.CodeAnalysis;

namespace Servo;

internal class ServiceDescriptorEqualityComparerByService : IEqualityComparer<ServiceDescriptor>
{
    public bool Equals(ServiceDescriptor? x, ServiceDescriptor? y)
    {
        if (x is not null) return x.ServiceType.Equals(y?.ServiceType);
        else if (y is null) return true;
        return false;
    }

    public int GetHashCode([DisallowNull] ServiceDescriptor obj) =>
        obj.ServiceType.GetHashCode();
}
