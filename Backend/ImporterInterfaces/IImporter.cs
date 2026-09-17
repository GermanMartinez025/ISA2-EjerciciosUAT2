using Models.Importers;

namespace ImporterInterfaces;

public interface IImporter
{
    string Name { get; }
    List<ImportedDevice> Import(string source);
}
