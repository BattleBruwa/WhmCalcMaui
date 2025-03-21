using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public interface IDataAccessService
    {
        Task<int> AddTargetAsync(TargetModel target);
        Task<int> UpdateTargetAsync(TargetModel target);
        Task<TargetModel?> GetTargetByNameAsync(string name);
        Task<IEnumerable<TargetModel>> GetTargetsAsync();
        Task<int> RemoveTargetAsync(string name);
    }
}