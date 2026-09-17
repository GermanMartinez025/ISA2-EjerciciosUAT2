using Models.Importers;

namespace ControllersInterfaces.Importers;

public interface IImporterController
{
    List<string> GetAllImporters();
    List<ImportedDevice> Import(string importerName, string sourcePath, int companyId, string token);
    
}