namespace Servo.Test;

public class ServiceDescriptorEqualityComparerByServiceTest
{
    [Fact]
    public void NullX_Equals_NullY()
    {
        ServiceDescriptor? x = null;
        ServiceDescriptor? y = null;

        new ServiceDescriptorEqualityComparerByService().Equals(x, y).Should().BeTrue();
    }

    [Fact]
    public void NullX_NotEquals_NotNullY()
    {
        ServiceDescriptor? x = null;
        ServiceDescriptor y = _serviceDescriptor;

        new ServiceDescriptorEqualityComparerByService().Equals(x, y).Should().BeFalse();
    }

    [Fact]
    public void NotNullX_NotEquals_NullY()
    {
        ServiceDescriptor x = _serviceDescriptor;
        ServiceDescriptor? y = null;

        new ServiceDescriptorEqualityComparerByService().Equals(x, y).Should().BeFalse();
    }

    [Fact]
    public void ServiceDescriptors_WithSameServiceTypes_AreEqual()
    {
        Type type = typeof(int);
        object object_ = default(int);

        ServiceDescriptor x = new(type, object_);
        ServiceDescriptor y = new(type, object_);

        new ServiceDescriptorEqualityComparerByService().Equals(x, y).Should().BeTrue();
    }

    [Fact]
    public void ServiceDescriptors_WithDifferentServiceTypes_AreNotEqual()
    {
        Type firstType = typeof(int);
        Type secondType = typeof(long);

        ServiceDescriptor x = new(firstType, default(int));
        ServiceDescriptor y = new(secondType, default(long));

        new ServiceDescriptorEqualityComparerByService().Equals(x, y).Should().BeFalse();
    }
}
