using ProyectoVideoteca.Models.DTO;

namespace ProyectoVideoteca.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);

        Task<Status> RegistrationAsync(RegistrationModel model);
        Task<Status> RemoveAsync(RegistrationModel model);
        Task<Status> EditAsync(RegistrationModel model);

        //tasks asyncs
        Task LogoutAsync();
    }
}
