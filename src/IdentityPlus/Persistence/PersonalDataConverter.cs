using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Honamic.IdentityPlus.Persistence;

internal sealed class PersonalDataConverter : ValueConverter<string, string>
{
    public PersonalDataConverter(IPersonalDataProtector protector)
        : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
    {

    }
}
