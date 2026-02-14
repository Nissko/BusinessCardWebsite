using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using ContractualDtos.DTO.Course.CourseAuthor.Dtos;
using ContractualDtos.DTO.Course.CourseAuthor.Requests;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses
{
    public class CourseAuthorRepository : ICourseAuthorRepository
    {
        private readonly IProjectDbContext _context;

        public CourseAuthorRepository(IProjectDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<CourseAuthorDtos>> GetAllAsync()
        {
            try
            {
                var data = await _context.CourseAuthor.ToListAsync();
                return GetCourseAuthorDto(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool?> Create(CreateCourseAuthorRequestDto dto)
        {
            try
            {
                var newCourseAuthor = new CourseAuthorEntity(dto.Surname, dto.Name, dto.Patronymic, dto.NickName);
            
                _context.CourseAuthor.Add(newCourseAuthor);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CourseAuthorDtos?> Read(Guid id)
        {
            try
            {
                var courseAuthor = await _context.CourseAuthor.FindAsync([id]);
                if (courseAuthor == null) return null;

                return GetCourseAuthorDto(courseAuthor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CourseAuthorDtos?> Update(UpdateCourseAuthorRequestDto dto)
        {
            try
            {
                var courseAuthor = await _context.CourseAuthor.FindAsync(dto.Id);
                if (courseAuthor == null) return null;
            
                courseAuthor.Update(dto.Surname, dto.Name, dto.Patronymic, dto.NickName);
                _context.CourseAuthor.Update(courseAuthor);
                await SaveChanges();

                return GetCourseAuthorDto(courseAuthor);
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
                var courseAuthor = await _context.CourseAuthor.FindAsync([id]);
                if (courseAuthor == null) return false;

                _context.CourseAuthor.Remove(courseAuthor);
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
        private static CourseAuthorDtos GetCourseAuthorDto(CourseAuthorEntity e)
        {
            return new CourseAuthorDtos(
                e.Id,
                e.Surname,
                e.Name,
                e.Patronymic,
                e.Nickname
            );
        }

        /// <summary>
        /// Формирование DTOs для return
        /// </summary>
        private static List<CourseAuthorDtos> GetCourseAuthorDto(List<CourseAuthorEntity> entities)
        {
            return entities.Select(e => new CourseAuthorDtos(
                e.Id,
                e.Surname,
                e.Name,
                e.Patronymic,
                e.Nickname
            )).ToList();
        }
    }
}