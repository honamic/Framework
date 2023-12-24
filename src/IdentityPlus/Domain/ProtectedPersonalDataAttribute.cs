

namespace Honamic.IdentityPlus.Domain;

[AttributeUsage(AttributeTargets.Property)]
public class IdenityPlusPersonalDataAttribute : Attribute
{ 
}

public class IdentityPlusProtectedPersonalDataAttribute : IdenityPlusPersonalDataAttribute
{ 
}