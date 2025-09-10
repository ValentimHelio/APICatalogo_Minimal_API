using APICatalogo.Models;

namespace APICatalogo.Services
{
    public interface ITokenService
    {
        string GerarToken(string Key, string issuer, string audiencie, UserModel user);
    }
}
