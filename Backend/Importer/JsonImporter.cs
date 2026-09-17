using ImporterInterfaces;
using ModelException;
using Models.Importers;
using Newtonsoft.Json;

namespace Importer;

public class JsonImporter : IImporter
{
    public string Name => "JsonImporter";

    public List<ImportedDevice> Import(string source)
    {
        if (!File.Exists(source))
        {
            throw new FileNotFoundException($"File not found: {source}");
        }

        var jsonContent = File.ReadAllText(source);
        var devices = JsonConvert.DeserializeObject<JsonDevices>(jsonContent);

        if (devices?.Dispositivos == null)
        {
            throw new BadRequestException("The JSON content is not in the expected format.");
        }

        return devices.Dispositivos.Select(d => new ImportedDevice(
            d.Id,
            d.Tipo,
            d.Nombre,
            d.Modelo,
            d.Fotos.Select(f => new Photo(f.Path, f.Es_Principal)).ToList(),
            d.Person_Detection,
            d.Movement_Detection,
            false
            
        )).ToList();
    }

    private class JsonDevices
    {
        public List<JsonDevice> Dispositivos { get; set; }
    }

    private class JsonDevice
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Modelo { get; set; }
        public List<JsonPhoto> Fotos { get; set; }
        public bool? Person_Detection { get; set; }
        public bool? Movement_Detection { get; set; }
    }

    private class JsonPhoto
    {
        public string Path { get; set; }
        public bool Es_Principal { get; set; }
    }
}