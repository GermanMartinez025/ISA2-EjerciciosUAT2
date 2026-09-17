using System.Reflection;
using ImporterInterfaces;
using ModelException;
using Models.Importers;
using ServicesInterfaces.Importers;

namespace Services.Importers;

    public class ImporterService : IImporterService
    {
        private readonly Dictionary<string, Type> _importerTypes;

        public ImporterService(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                throw new NotFoundException($"The directory '{path}' does not exist.");
            }

            _importerTypes = directoryInfo
                .GetFiles("*.dll")
                .SelectMany(file =>
                {
                    var assembly = Assembly.LoadFile(file.FullName);
                    return assembly.GetTypes()
                        .Where(t => typeof(IImporter).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                })
                .ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);
        }

        public List<string> LoadImporters()
        {
            return _importerTypes.Keys.ToList();
        }

        public List<ImportedDevice> Import(string importerName, string source)
        {
            if (!_importerTypes.ContainsKey(importerName))
            {
                throw new NotFoundException($"Importer '{importerName}' not found.");
            }

            var importer = (IImporter)Activator.CreateInstance(_importerTypes[importerName]);
            return importer.Import(source);
        }
    }
