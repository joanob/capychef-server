namespace Capychef.DataLoader.Services;

public interface IDataLoader
{
    Task LoadFromStream(Stream stream);
}