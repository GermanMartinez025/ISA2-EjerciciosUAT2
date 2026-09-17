using ModelException;
using ModelInterface.Devices;
using ModelInterface.Users;
using ModelInterface.Users.UserType;

namespace ModelInterface.Companys;

public abstract class ACompany : ICompany
{
    public virtual int Id { get; set; }
    public string Rut { get; set; }
    public string Name { get; set; }
    public string LogoType { get; set; }
    public ACompanyOwner CompanyOwner { get; set; }
    public virtual int? CompanyOwnerId { get; set; }
    public List<ADevice> Devices { get; set; } = new List<ADevice>();
    public virtual string? ValidationType { get; set; }

    protected ACompany()
    {
    }
    
    protected ACompany(string rut, string name, string logoType, ACompanyOwner companyOwner, string validationType)
    {
        ValidateAll(rut, name, logoType, companyOwner);
        ValidateValidationType(validationType);
        
        Rut = rut;
        Name = name;
        LogoType = logoType;
        CompanyOwner = companyOwner;
        CompanyOwnerId = companyOwner.Id;
        Devices = new List<ADevice>();
        ValidationType = validationType;
    }

    private static void ValidateAll(string rut, string name, string logoType, ACompanyOwner companyOwner)
    {
        ValidateCompanyOwnerIsNotNull(companyOwner);
        ValidateRutHave12Numbers(rut);
        ValidateWhenFirstTwoDigitsInRutAreLessThanOne(rut);
        ValidatePositionNineAndTenOfRut(rut);
        ValidatePositionThreeToEightOfRut(rut);
        ValidateRutLogicDigitVerificator(rut);
        ValidateWhenNameIsNull(name);
        ValidateWhenLogoTypeIsNullOrEmpty(logoType);
    }
    private static void ValidateCompanyOwnerIsNotNull(ACompanyOwner companyOwner)
    {
        if (companyOwner == null)
        {
            throw new NotFoundException("The company owner cannot be null");
        }
    }
    private static void ValidateRutHave12Numbers(string rut)
    {
        if (rut.Length != 12)
        {
            throw new BadRequestException("The rut have 12 numbers");
        }
    }
    private static void ValidateWhenFirstTwoDigitsInRutAreLessThanOne(string rut)
    {
        int firstTwoDigits = int.Parse(rut.Substring(0,2));
        
        if ( firstTwoDigits < 1 || firstTwoDigits > 21)
        {
            throw new BadRequestException("Incorrect rut, check the first 2 numbers");
        }
    }
    private static void ValidatePositionNineAndTenOfRut(string rut)
    {
        if (rut[8] != '0' || rut[9] != '0')
        {
            throw new BadRequestException("Incorrect rut, check position 8 and 9");
        }
    }
    private static void ValidatePositionThreeToEightOfRut(string rut)
    {
        for (int i = 2; i <= 7; i++)
        {
            if (rut[i] == '0')
            {
                throw new BadRequestException($"The character at position {i + 1} must not be zero."); 
            }
        }
    }
    private static void ValidateRutLogicDigitVerificator(string rut)
    {
        int[] factorsMultiplication = { 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int suma = 0;
        
        for (int i = 0; i < factorsMultiplication.Length; i++)
        {
            suma += (rut[i] - '0') * factorsMultiplication[i];
        }

        int remainder = suma % 11;
        int calculatedDigit = 11 - remainder;

        if (calculatedDigit == 11)
            calculatedDigit = 0;
        else if (calculatedDigit == 10)
            throw new BadRequestException("The calculated check digit is 10, therefore the RUT is not valid.");
        
        int lastDigit = rut[11] - '0';
        if (calculatedDigit != lastDigit)
            throw new BadRequestException("The calculated check digit does not match the RUT check digit.");
    }
    private static void ValidateWhenNameIsNull(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new NotFoundException("The company name cannot be empty or null.");
        }
    }
    private static void ValidateWhenLogoTypeIsNullOrEmpty(string logoType)
    {
        if (string.IsNullOrWhiteSpace(logoType))
        {
            throw new NotFoundException("The company name cannot be null or empty.");
        }
    }
    
    private static void ValidateValidationType(string validationType)
    {
       if(validationType == "")
       {
           throw new BadRequestException("The validation type cannot be empty.");
       }
        
    }
}