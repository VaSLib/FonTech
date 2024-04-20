using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using FonTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FonTech.Application.Services;

public class TokenService : ITokenService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly string _jwtKey;
    private readonly string _issuer;
    private readonly string _audience;


    public TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository)
    {
        _jwtKey = options.Value.JwtKey;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _userRepository = userRepository;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)); 
        var credentialns = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.UtcNow.AddMinutes(10), credentialns);

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var radomNumbers = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(radomNumbers);

        return Convert.ToBase64String(radomNumbers);
    }

    public ClaimsPrincipal GetPrincipalFromExpriredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
            ValidateLifetime = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters,out var securityToken);

        if(securityToken is not JwtSecurityToken jwtSecurityToken||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(ErrorMessage.InvalidToken);
        }

        return claimsPrincipal;
    }

    public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
    {
        string accessToken = dto.AccessToken;
        string refreshToken = dto.RefreshToken;

        var claimPrincipal = GetPrincipalFromExpriredToken(accessToken);
        var userName = claimPrincipal.Identity?.Name;

        var user = await _userRepository.GetAll()
            .Include(x => x.UserToken)
            .FirstOrDefaultAsync(x => x.Login == userName); 

        if (user == null||user.UserToken.RefreshToken!=refreshToken||
            user.UserToken.RefreshTokenExpiryTime<=DateTime.UtcNow)
        {
            return new BaseResult<TokenDto>
            {
                ErrorMessage = ErrorMessage.InvalidClientRequest,
            };
        }
        var newAccessToken = GenerateAccessToken(claimPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        await _userRepository.UpdateAsync(user);

        return new BaseResult<TokenDto>
        {
            Data = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            }
        };

    }
}
