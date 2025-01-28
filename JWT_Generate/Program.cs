using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


//TODO: Codigo foi criado dentro do visual studio, se rodar o mesmo dentro 
// do visual, funciona corretamente, porem ao testar o mesmo codigo no codebyte
// o mesmo nao funciona, caso queiram apenas copiar esse codigo e testar no visual studio
// caso queiram ver criei um reposotorio com os dois projetos, de json Cleaning e o JWT
// https://github.com/gustavorr21/Testes-BliteTI

public class MainClass
{
    string secret = "your-very-long-secret-key-1234567890123456";

    public static string GenerateJwtWithFixedClaims(string secret, string issuer, string audience, string sub, string jti, long iat)
    {

        var varFiltersCg = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var claims = new[]
        {
            new System.Security.Claims.Claim("sub", sub),
            new System.Security.Claims.Claim("jti", jti),
            new System.Security.Claims.Claim("iat", iat.ToString())
        };

        var credentials = new SigningCredentials(varFiltersCg, SecurityAlgorithms.HmacSha256);

        var jwtHeader = new JwtHeader(credentials);
        var jwtPayload = new JwtPayload(issuer, audience, claims, DateTime.UtcNow, DateTime.MaxValue);

        var jwt = new JwtSecurityToken(jwtHeader, jwtPayload);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static void Main(string[] args)
    {
        string jwt = GenerateJwtWithFixedClaims("your-very-long-secret-key-1234567890123456", "your-issuer", "your-audience", "sub-value-1", "jti-value-1", 1626300000);

        Console.WriteLine(jwt);
    }
}