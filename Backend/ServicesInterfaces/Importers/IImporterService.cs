using Models.Importers;

namespace ServicesInterfaces.Importers;

public interface IImporterService
{
    List<string> LoadImporters();
    List<ImportedDevice> Import(string importerName, string source);
}