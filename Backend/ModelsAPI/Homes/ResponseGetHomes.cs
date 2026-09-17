using ModelInterface.Homes;

namespace ModelsAPI.Homes;

public class ResponseGetHomes
{
    public List<ResponseGetHome> Homes { get; set; }

    public ResponseGetHomes()
    {
        Homes = new List<ResponseGetHome>();
    }
    public ResponseGetHomes(List<AHome> homes)
    {
        Homes = new List<ResponseGetHome>();
        foreach (var home in homes)
        {
            Homes.Add(new ResponseGetHome(home));
        }
    }
}