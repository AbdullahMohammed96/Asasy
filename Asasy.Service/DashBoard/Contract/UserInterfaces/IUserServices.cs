using Asasy.Domain.ViewModel.Users;

namespace Asasy.Service.DashBoard.Contract.UserInterfaces
{
    public interface IUserServices
    {
        Task<List<UsersViewModel>> GetUsers();
        Task<CheckDataViewModel> CreateUser(CreateUserViewModel model);
        Task<CheckDataViewModel> Delete(string id);

        Task<bool> ChangeState(string UserId);
    }
}
