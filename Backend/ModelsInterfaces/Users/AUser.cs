using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ModelException;
using ModelInterface.Users.UserType;

namespace ModelInterface.Users;

public abstract class AUser
{
    public virtual int Id { get; init; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public DateTime CreationDate { get; set; }
    public string Password { get; set; }
    public virtual string Email { get; set; }
    public  virtual List<AUserType> Roles { get; set; } = new ();
    public virtual bool IsAdmin() => HasRole(RolesEnum.Admin);
    public bool IsCompanyOwner() => HasRole(RolesEnum.CompanyOwner);
    public bool IsHomeUser() => HasRole(RolesEnum.HomeUser);
    public AAdmin? Admin => Roles.OfType<AAdmin>().FirstOrDefault();
    public ACompanyOwner? CompanyOwner => Roles.OfType<ACompanyOwner>().FirstOrDefault();
    public AHomeUser? HomeUser => Roles.OfType<AHomeUser>().FirstOrDefault();
    
    public string? ProfilePhoto => HomeUser?.ProfilePhoto;
    
    protected AUser()
    {
    }
    
    protected AUser(string firstName, string lastName, string email, string password)
    {
        Validate(firstName, lastName, email, password);
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        CreationDate = DateTime.Now;
    }
    
    private void Validate(string firstName, string lastName, string email, string password)
    {
        if (!ValidateName(firstName)) throw new BadRequestException("Invalid first name");
        if (!ValidateName(lastName)) throw new BadRequestException("Invalid last name");
        if (!ValidateEmail(email)) throw new BadRequestException("Invalid email");
        if (!ValidatePassword(password)) throw new BadRequestException("Invalid password");
    }
    
    public bool HasRole(RolesEnum role)
    {
        return Roles.Any(r => r.Role == role);
    }
    
    public virtual bool HasRole(string role)
    {
        if (Enum.TryParse<RolesEnum>(role, out var roleEnum))
        {
            return HasRole(roleEnum);
        }
        
        return false;
    }
    
    private static bool ValidateName(string name)
    {
        Regex letters = new Regex(@"^[a-zA-ZÀ-ÿ\s]*$");
        return !string.IsNullOrWhiteSpace(name) && letters.IsMatch(name);
    }
    
    private static bool ValidateEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
    }
    
    private static bool ValidatePassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length > 6 && new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(password);
        
    }
    public virtual void AddRole(AUserType userType)
    {
        Roles.Add(userType);
    }

    public abstract void AddRole(RolesEnum role);
    
    public List<string> GetRoles()
    {
        return Roles.Select(r => r.Role.ToString()).ToList();
    }
   

    public void Update(string prop, object? value)
    {
        switch (prop)
        {
            case "Role": AddRole((string?)value); break;
        }
    }
    
    private void AddRole(string? value)
    {
        if (value == null) throw new NotFoundException("Role can't be null");
        if (ValidateHasRole(value)) throw new ConflictException("User already has this role");
        
        RolesEnum roleEnum = Enum.Parse<RolesEnum>(value);
        
        if (RolesAreIncompatible(roleEnum)) throw new ConflictException("User can't have this role");
        
        AddRole(roleEnum);
    }
    
    private bool ValidateHasRole(string role)
    {
        if (!Enum.TryParse(role, out RolesEnum roleEnum))
        {
            throw new ConflictException("Invalid role");
        }
        
        return HasRole(roleEnum);
    }
    
    private bool RolesAreIncompatible(RolesEnum role)
    {
        return (role == RolesEnum.CompanyOwner && IsAdmin())
               || (role == RolesEnum.Admin && IsCompanyOwner());
    }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}