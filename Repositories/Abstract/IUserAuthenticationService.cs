using ProyectoVideoteca.Models.DTO;

namespace ProyectoVideoteca.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);

        Task<Status> RegistrationAsync(RegistrationModel model);

        //tareas asincronicas o varias tareas a la vez
        Task LogoutAsync();
    }
}
