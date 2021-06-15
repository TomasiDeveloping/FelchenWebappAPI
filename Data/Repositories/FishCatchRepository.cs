using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Entities;
using Api.Interfaces;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repositories
{
    public class FishCatchRepository : IFishCatchRepository
    {
        private readonly FelchenContext _context;
        private readonly ILoggerService _loggerService;

        public FishCatchRepository(FelchenContext context, ILoggerService loggerService)
        {
            _context = context;
            _loggerService = loggerService;
        }
        public async Task<List<FishCatchDto>> GetFishCatchesAsync()
        {
            return await _context.FischCatches
                .Select(c => new FishCatchDto()
                {
                    Id = c.Id,
                    Weather = c.Weather,
                    AirPressure = c.AirPressure,
                    AirTemperature = c.AirTemperature,
                    AllowPublic = c.AllowPublic,
                    CatchDate = c.CatchDate,
                    DeepLocation = c.DeepLocation,
                    HookSize = c.HookSize,
                    LakeName = c.LakeName,
                    NymphColor = c.NymphColor,
                    NymphHead = c.NymphHead,
                    NymphName = c.NymphName,
                    UserId = c.UserId,
                    WaterTemperature = c.WaterTemperature,
                    WindSpeed = c.WindSpeed,
                    DeepFishCatch = c.DeepFishCatch
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<FishCatchDto>> GetPublicFishCatchesAsync(int pageSize, int pageIndex)
        {
            return await _context.FischCatches
                .Select(c => new FishCatchDto()
                {
                    Id = c.Id,
                    Weather = c.Weather,
                    AirPressure = c.AirPressure,
                    AirTemperature = c.AirTemperature,
                    AllowPublic = c.AllowPublic,
                    CatchDate = c.CatchDate,
                    DeepLocation = c.DeepLocation,
                    HookSize = c.HookSize,
                    LakeName = c.LakeName,
                    NymphColor = c.NymphColor,
                    NymphHead = c.NymphHead,
                    NymphName = c.NymphName,
                    UserId = c.UserId,
                    WaterTemperature = c.WaterTemperature,
                    WindSpeed = c.WindSpeed,
                    DeepFishCatch = c.DeepFishCatch
                })
                .AsNoTracking()
                .Where(c => c.AllowPublic)
                .OrderByDescending(c => c.CatchDate)
                .Skip((pageIndex -1) * pageSize).Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<FishCatchDto>> GetFishCatchesByUserIdAsync(int userId, int pageSize, int pageIndex, string sortBy)
        {
            if (sortBy == "desc")
            {
                return await _context.FischCatches
                    .Select(c => new FishCatchDto()
                    {
                        Id = c.Id,
                        Weather = c.Weather,
                        AirPressure = c.AirPressure,
                        AirTemperature = c.AirTemperature,
                        AllowPublic = c.AllowPublic,
                        CatchDate = c.CatchDate,
                        DeepLocation = c.DeepLocation,
                        HookSize = c.HookSize,
                        LakeName = c.LakeName,
                        NymphColor = c.NymphColor,
                        NymphHead = c.NymphHead,
                        NymphName = c.NymphName,
                        UserId = c.UserId,
                        WaterTemperature = c.WaterTemperature,
                        WindSpeed = c.WindSpeed,
                        DeepFishCatch = c.DeepFishCatch
                    })
                    .AsNoTracking()
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.CatchDate)
                    .Skip((pageIndex -1) * pageSize).Take(pageSize)
                    .ToListAsync();
            }
            return await _context.FischCatches
                    .Select(c => new FishCatchDto()
                    {
                        Id = c.Id,
                        Weather = c.Weather,
                        AirPressure = c.AirPressure,
                        AirTemperature = c.AirTemperature,
                        AllowPublic = c.AllowPublic,
                        CatchDate = c.CatchDate,
                        DeepLocation = c.DeepLocation,
                        HookSize = c.HookSize,
                        LakeName = c.LakeName,
                        NymphColor = c.NymphColor,
                        NymphHead = c.NymphHead,
                        NymphName = c.NymphName,
                        UserId = c.UserId,
                        WaterTemperature = c.WaterTemperature,
                        WindSpeed = c.WindSpeed,
                        DeepFishCatch = c.DeepFishCatch
                    })
                    .AsNoTracking()
                    .Where(c => c.UserId == userId)
                    .OrderBy(c => c.CatchDate)
                    .Skip((pageIndex -1) * pageSize).Take(pageSize)
                    .ToListAsync();
        }

        public async Task<int> CountUserCatchesAsync(int userId)
        {
            return await _context.FischCatches
                .AsNoTracking()
                .CountAsync(c => c.UserId == userId);
        }

        public async Task<int> CountPublicCatchesAsync()
        {
            return await _context.FischCatches
                .AsNoTracking()
                .CountAsync(c => c.AllowPublic);
        }

        public async Task<FishCatchDto> GetFishCatchByIdAsync(int fishCatchId)
        {
            return await _context.FischCatches
                .Select(c => new FishCatchDto()
                {
                    Id = c.Id,
                    Weather = c.Weather,
                    AirPressure = c.AirPressure,
                    AirTemperature = c.AirTemperature,
                    AllowPublic = c.AllowPublic,
                    CatchDate = c.CatchDate,
                    DeepLocation = c.DeepLocation,
                    HookSize = c.HookSize,
                    LakeName = c.LakeName,
                    NymphColor = c.NymphColor,
                    NymphHead = c.NymphHead,
                    NymphName = c.NymphName,
                    UserId = c.UserId,
                    WaterTemperature = c.WaterTemperature,
                    WindSpeed = c.WindSpeed,
                    DeepFishCatch = c.DeepFishCatch
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == fishCatchId);
        }

        public async Task<FishCatchDto> InsertFishCatchAsync(FishCatchDto fishCatchDto)
        {
            var newFishCatch = new FishCatch()
            {
                Weather = fishCatchDto.Weather,
                AirPressure = fishCatchDto.AirPressure,
                AirTemperature = fishCatchDto.AirTemperature,
                AllowPublic = fishCatchDto.AllowPublic,
                CatchDate = fishCatchDto.CatchDate,
                DeepLocation = fishCatchDto.DeepLocation,
                HookSize = fishCatchDto.HookSize,
                LakeName = fishCatchDto.LakeName,
                NymphColor = fishCatchDto.NymphColor,
                NymphHead = fishCatchDto.NymphHead,
                NymphName = fishCatchDto.NymphName,
                WaterTemperature = fishCatchDto.WaterTemperature,
                UserId = fishCatchDto.UserId,
                WindSpeed = fishCatchDto.WindSpeed,
                DeepFishCatch = fishCatchDto.DeepFishCatch
            };
            
            await _context.FischCatches.AddAsync(newFishCatch);
            
            var checkInsert = await Complete();
            fishCatchDto.Id = newFishCatch.Id;
            return checkInsert ? fishCatchDto : null;
        }

        public async Task<FishCatchDto> UpdateFishCatchAsync(int fishCatchId, FishCatchDto fishCatchDto)
        {
            var fishCatchToUpdate = await _context.FischCatches.FindAsync(fishCatchId);
            if (fishCatchToUpdate == null) return null;
            fishCatchToUpdate.Weather = fishCatchDto.Weather;
            fishCatchToUpdate.AirPressure = fishCatchDto.AirPressure;
            fishCatchToUpdate.AirTemperature = fishCatchDto.AirTemperature;
            fishCatchToUpdate.AllowPublic = fishCatchDto.AllowPublic;
            fishCatchToUpdate.CatchDate = fishCatchDto.CatchDate;
            fishCatchToUpdate.DeepLocation = fishCatchDto.DeepLocation;
            fishCatchToUpdate.HookSize = fishCatchDto.HookSize;
            fishCatchToUpdate.LakeName = fishCatchDto.LakeName;
            fishCatchToUpdate.NymphColor = fishCatchDto.NymphColor;
            fishCatchToUpdate.NymphHead = fishCatchDto.NymphHead;
            fishCatchToUpdate.NymphName = fishCatchDto.NymphName;
            fishCatchToUpdate.UserId = fishCatchDto.UserId;
            fishCatchToUpdate.WaterTemperature = fishCatchDto.WaterTemperature;
            fishCatchToUpdate.WindSpeed = fishCatchDto.WindSpeed;
            fishCatchToUpdate.DeepFishCatch = fishCatchDto.DeepFishCatch;

            var checkUpdate = await Complete();
            return checkUpdate ? fishCatchDto : null;
        }

        public async Task<bool> DeleteFishCatchByIdAsync(int fishCatchId)
        {
            var fishCatchToDelete = await _context.FischCatches.FindAsync(fishCatchId);
            if (fishCatchToDelete == null) return false;
            _context.FischCatches.Remove(fishCatchToDelete);
            return await Complete();
        }

        public async Task<bool> DeleteFishCatchesByUserIdAsync(int userId)
        {
            var fishCatchesToDelete = await _context.FischCatches
                .Where(c => c.UserId == userId).ToListAsync();
            if (fishCatchesToDelete.Count <= 0) return false;
            _context.FischCatches.RemoveRange(fishCatchesToDelete);
            return await Complete();
        }

        public async Task<bool> Complete()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                var log = new Log()
                {
                    Message = "Error in FishCatchRepository",
                    CratedAt = DateTime.Now,
                    ExceptionMessage = e.Message,
                    InnerException = e.InnerException?.ToString(),
                    LogTypeId = Convert.ToInt32(Constantes.LogTypeNames.Error)
                };
                await _loggerService.InsertLogAsync(log);
                return false;
            }
        }
    }
}