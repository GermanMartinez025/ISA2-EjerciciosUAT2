using IRepositories.Repositories.UserRepositories;
using ModelException;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using ServicesInterfaces.Users;

namespace Services.Users;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    
    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public AAdmin? DeleteAdmin(int adminId)
    {
        var admins = _adminRepository.GetAll();
        
        if (admins.Count == 1)
        {
            throw new ForbiddenException("Cannot delete the only admin");
        }
        
        var admin = admins.FirstOrDefault(x => x.UserId == adminId);
        if (admin == null) return null;
        
        _adminRepository.Delete(admin);
        
        return admin;
    }
}