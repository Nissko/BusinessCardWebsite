using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Dtos;
using ContractualDtos.DTO.Course.ProgrammingLanguageCourse.Requests;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses
{
    public class ProgrammingLanguageRepository : IProgrammingLanguageRepository
    {
        private readonly IProjectDbContext _context;

        public ProgrammingLanguageRepository(IProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ProgrammingLanguageDtos>> GetAllAsync()
        {
            try
            {
                var data = await _context.ProgrammingLanguageCourse.ToListAsync();
                return GetProgrammingLanguageDto(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool?> Create(CreateProgrammingLanguageRequestDto dto)
        {
            try
            {
                var newProgrammingLanguage = new ProgrammingLanguageCourseEntity(dto.Name, 0);
                _context.ProgrammingLanguageCourse.Add(newProgrammingLanguage);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ProgrammingLanguageDtos?> Read(Guid id)
        {
            try
            {
                var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync(id);
                return programmingLanguage == null ? null : GetProgrammingLanguageDto(programmingLanguage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ProgrammingLanguageDtos?> Update(UpdateProgrammingLanguageRequestDto dto)
        {
            try
            {
                var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync(dto.Id);
                if (programmingLanguage == null) return null;

                programmingLanguage.Update(dto.Name);
                _context.ProgrammingLanguageCourse.Update(programmingLanguage);
                await SaveChanges();

                return GetProgrammingLanguageDto(programmingLanguage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var programmingLanguage = await _context.ProgrammingLanguageCourse.FindAsync(id);

                if (programmingLanguage == null) return false;

                _context.ProgrammingLanguageCourse.Remove(programmingLanguage);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static ProgrammingLanguageDtos GetProgrammingLanguageDto(ProgrammingLanguageCourseEntity entity)
        {
            return new ProgrammingLanguageDtos(entity.Id, entity.Name, entity.CountSelectedUser);
        }

        /// <summary>
        /// Формирование DTO для return
        /// </summary>
        private static List<ProgrammingLanguageDtos> GetProgrammingLanguageDto(List<ProgrammingLanguageCourseEntity> entities)
        {
            return entities.Select(e => new ProgrammingLanguageDtos(
                e.Id,
                e.Name,
                e.CountSelectedUser
            )).ToList();
        }
    }
}