namespace ModelsAPI.Importers;

public class RequestImport
{ 
        public string ImporterName { get; set; }
        public string SourcePath { get; set; }
        
        public RequestImport(string importerName, string sourcePath)
        {
            ImporterName = importerName;
            SourcePath = sourcePath;
        }
}