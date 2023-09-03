using TransactionApi.Model;

namespace TransactionApi.Indentity.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJSONWebToken(UserModel userModel);
    }
}