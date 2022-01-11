using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserControl.Models;

namespace UserControl.Services
{
    public class TokenService
    {
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string GenerateToken(User user)
        {
            // Classe para gerar o token
            var tokenHandler = new JwtSecurityTokenHandler();

            //Nossa chave como array de bytes
            var key = Encoding.ASCII.GetBytes(Configuration["key"]);

            //Começar a criar o token
            // SecurityTokenDescriptor vai descrever todo o que nosso token tem
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Informações sobre os perfis de usuários(Claims)
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name, user.Username), //User.Identity.Name
							new Claim(ClaimTypes.Role, user.Role)      //User.IsInRole
                }),

                //Quanto tempo o token vai durar
                Expires = DateTime.UtcNow.AddHours(8),

                //As credencias que vão ser usadas para encriptar e desencriptar o token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            //Usar o tokenHandler para gerar o token com base no tokenDescriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Retornar a string do token
            return tokenHandler.WriteToken(token);
        }
    }
}