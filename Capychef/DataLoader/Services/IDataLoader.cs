namespace Capychef.Data;

public interface IDataLoader
{
    Task LoadFromStream(Stream stream);
}