using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public interface IDataAccessService
    {
        Task AddTargetAsync(TargetModel target);
        Task<TargetModel?> GetTargetByNameAsync(string name);
        Task<IEnumerable<TargetModel>> GetTargetsAsync();
        Task<int> RemoveTargetAsync(string name);
    }
}