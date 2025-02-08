namespace FCSP.DTOs.Authentication;

public class UserRegisterResponse
{
    public long Id { get; set; }

    public string Token { get; set; } = string.Empty;
}
