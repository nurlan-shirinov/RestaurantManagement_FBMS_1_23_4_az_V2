namespace Application.CQRS.Users.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
