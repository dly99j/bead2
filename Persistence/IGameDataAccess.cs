using System.Threading.Tasks;

namespace bead2.Persistence
{
    public interface IGameDataAccess
    {
        Task<GameTable> LoadAsync(string path);
    }
}