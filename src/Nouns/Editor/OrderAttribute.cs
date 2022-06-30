namespace Nouns.Editor;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class OrderAttribute : Attribute
{
    public OrderAttribute(int order)
    {
        Order = order;
    }

    public int Order { get; }
}