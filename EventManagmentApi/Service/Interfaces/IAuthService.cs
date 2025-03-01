using System.Threading.Tasks;

namespace EventManagmentApi.Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}
