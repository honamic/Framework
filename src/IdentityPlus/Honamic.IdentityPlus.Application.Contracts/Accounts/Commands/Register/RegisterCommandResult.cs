namespace Honamic.IdentityPlus.Application.Accounts.Commands.Register;

public class RegisterCommandResult
{
    public bool Succeeded { get; init; }
    public List<string>? Errors { get; set; }

    public override string ToString()
    {
        return Succeeded && Errors is null ? "Succeeded" : string.Join(',', Errors!);
    }
}