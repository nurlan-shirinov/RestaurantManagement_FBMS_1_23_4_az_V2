using Application.CQRS.Users.DTOs;
using Application.Services;
using Application.Services.LogService;
using Common.Exceptions;
using Common.GlobalResponse.Generics;
using Common.Security;
using Domain.Entites;
using MediatR;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.CQRS.Users.Handlers;

public class Login
{
    public class LoginRequest : IRequest<ResponseModel<LoginResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IConfiguration configuration, ILoggerService logService) : IRequestHandler<LoginRequest, ResponseModel<LoginResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseModel<LoginResponseDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            logService.LogInfo($"{request.Email} ilə sistemə giriş etmık istədi");

            User currentUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

            if (currentUser == null)
            {
                logService.LogWarning($"{request.Email} ile istifadeci movcud deyildir");
                throw new BadRequestException("User does not exist with provided email");
            }

            var hashedPassword = PasswordHasher.ComputeStringToSha256Hash(request.Password);

            if (hashedPassword != currentUser.PasswordHash)
            {
                logService.LogWarning($"{request.Email} terefinden yalnish parol daxil edildi");
                throw new BadRequestException("Wrong password!");
            }


            List<Claim> authClaim = [
                new Claim(ClaimTypes.NameIdentifier , currentUser.Id.ToString()),
                new Claim(ClaimTypes.Name , currentUser.Name),
                new Claim(ClaimTypes.Email , currentUser.Email),
                new Claim(ClaimTypes.MobilePhone , currentUser.Phone),
                new Claim(ClaimTypes.Role , currentUser.UserRole.ToString())
                ];

            JwtSecurityToken token = TokenService.CreateToken(authClaim, configuration);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            string refreshTokenString = TokenService.GenerateRefreshToken();

            RefreshToken refreshToken = new()
            {
                Token = refreshTokenString,
                UserId = currentUser.Id,
                ExpirationDate = DateTime.Now.AddDays(Double.Parse(configuration.GetRequiredSection("JWT:RefreshTokenExpirationDays").Value!)),
            };

            await _unitOfWork.RefreshTokenRepository.SaveRefreshToken(refreshToken);
            await _unitOfWork.SaveChanges();

            LoginResponseDto response = new()
            {
                AccessToken = tokenString,
                RefreshToken = refreshTokenString
            };

            logService.LogInfo($"{request.Email} terefinden sisteme girish edildi");
            return new ResponseModel<LoginResponseDto> { Data = response };
        }
    }
}