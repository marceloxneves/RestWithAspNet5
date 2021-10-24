using RestWithAspNet5.Authentication.VO;

namespace RestWithAspNet5.Authentication.Services
{
    public interface ILoginService
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string username);
    }
}
