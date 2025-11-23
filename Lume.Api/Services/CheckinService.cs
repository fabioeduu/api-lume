using Lume.Api.DTOs;
using Lume.Api.Models;
using Lume.Api.Repositories;

namespace Lume.Api.Services
{
    public interface ICheckinService
    {
        Task<CheckinDto?> CreateCheckinAsync(int userId, CreateCheckinDto createDto);
        Task<CheckinDto?> GetCheckinAsync(int checkinId);
        Task<IEnumerable<CheckinHistoryDto>> GetUserCheckinsAsync(int userId);
        Task<IEnumerable<CheckinHistoryDto>> GetUserCheckinsAsync(int userId, DateTime fromDate, DateTime toDate);
        Task<CheckinDto?> UpdateCheckinAsync(int checkinId, CreateCheckinDto updateDto);
        Task<bool> DeleteCheckinAsync(int checkinId);
    }

    public class CheckinService : ICheckinService
    {
        private readonly ICheckinRepository _checkinRepository;

        public CheckinService(ICheckinRepository checkinRepository)
        {
            _checkinRepository = checkinRepository;
        }

        public async Task<CheckinDto?> CreateCheckinAsync(int userId, CreateCheckinDto createDto)
        {
            // Validate emotion level
            if (createDto.EmotionalLevel < 1 || createDto.EmotionalLevel > 10)
                return null;

            var checkin = new Checkin
            {
                UserId = userId,
                Emotion = createDto.Emotion,
                EmotionalLevel = createDto.EmotionalLevel,
                Notes = createDto.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdCheckin = await _checkinRepository.AddAsync(checkin);

            return new CheckinDto
            {
                Id = createdCheckin.Id,
                UserId = createdCheckin.UserId,
                Emotion = createdCheckin.Emotion,
                EmotionalLevel = createdCheckin.EmotionalLevel,
                Notes = createdCheckin.Notes,
                CreatedAt = createdCheckin.CreatedAt
            };
        }

        public async Task<CheckinDto?> GetCheckinAsync(int checkinId)
        {
            var checkin = await _checkinRepository.GetByIdAsync(checkinId);
            if (checkin == null)
                return null;

            return new CheckinDto
            {
                Id = checkin.Id,
                UserId = checkin.UserId,
                Emotion = checkin.Emotion,
                EmotionalLevel = checkin.EmotionalLevel,
                Notes = checkin.Notes,
                CreatedAt = checkin.CreatedAt
            };
        }

        public async Task<IEnumerable<CheckinHistoryDto>> GetUserCheckinsAsync(int userId)
        {
            var checkins = await _checkinRepository.GetUserCheckinsAsync(userId);
            return checkins.Select(c => new CheckinHistoryDto
            {
                Id = c.Id,
                Emotion = c.Emotion,
                EmotionalLevel = c.EmotionalLevel,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<IEnumerable<CheckinHistoryDto>> GetUserCheckinsAsync(int userId, DateTime fromDate, DateTime toDate)
        {
            var checkins = await _checkinRepository.GetUserCheckinsAsync(userId, fromDate, toDate);
            return checkins.Select(c => new CheckinHistoryDto
            {
                Id = c.Id,
                Emotion = c.Emotion,
                EmotionalLevel = c.EmotionalLevel,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<CheckinDto?> UpdateCheckinAsync(int checkinId, CreateCheckinDto updateDto)
        {
            var checkin = await _checkinRepository.GetByIdAsync(checkinId);
            if (checkin == null)
                return null;

            if (updateDto.EmotionalLevel < 1 || updateDto.EmotionalLevel > 10)
                return null;

            checkin.Emotion = updateDto.Emotion;
            checkin.EmotionalLevel = updateDto.EmotionalLevel;
            checkin.Notes = updateDto.Notes;
            checkin.UpdatedAt = DateTime.UtcNow;

            var updatedCheckin = await _checkinRepository.UpdateAsync(checkin);

            return new CheckinDto
            {
                Id = updatedCheckin.Id,
                UserId = updatedCheckin.UserId,
                Emotion = updatedCheckin.Emotion,
                EmotionalLevel = updatedCheckin.EmotionalLevel,
                Notes = updatedCheckin.Notes,
                CreatedAt = updatedCheckin.CreatedAt
            };
        }

        public async Task<bool> DeleteCheckinAsync(int checkinId)
        {
            return await _checkinRepository.DeleteAsync(checkinId);
        }
    }
}
