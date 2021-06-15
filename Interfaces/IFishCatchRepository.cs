using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;

namespace Api.Interfaces
{
    public interface IFishCatchRepository
    {
        public Task<List<FishCatchDto>> GetFishCatchesAsync();
        public Task<List<FishCatchDto>> GetPublicFishCatchesAsync(int pageSize, int pageIndex);
        public Task<List<FishCatchDto>> GetFishCatchesByUserIdAsync(int userId, int pageSize, int pageIndex, string sortBy);
        public Task<int> CountUserCatchesAsync(int userId);
        public Task<int> CountPublicCatchesAsync();
        public Task<FishCatchDto> GetFishCatchByIdAsync(int fishCatchId);
        public Task<FishCatchDto> InsertFishCatchAsync(FishCatchDto fishCatchDto);
        public Task<FishCatchDto> UpdateFishCatchAsync(int fishCatchId, FishCatchDto fishCatchDto);
        public Task<bool> DeleteFishCatchByIdAsync(int fishCatchId);
        public Task<bool> DeleteFishCatchesByUserIdAsync(int userId);
        public Task<bool> Complete();
    }
}