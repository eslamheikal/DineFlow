namespace Shared.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IgnoreLoggingAttribute : Attribute { }