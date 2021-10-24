using RestWithAspNet5.Authentication.Model;
using RestWithAspNet5.Authentication.VO;

namespace RestWithAspNet5.Authentication.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
        User ValidateCredentials(string userName);
        User RefreshUserInfo(User user);

        bool RevokeToken(string username);
    }
}
