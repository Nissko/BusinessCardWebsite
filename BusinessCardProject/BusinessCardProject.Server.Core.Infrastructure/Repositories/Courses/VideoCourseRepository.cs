using BusinessCardProject.Server.Core.Application.Application.Extensions;
using BusinessCardProject.Server.Core.Application.Common.Interfaces;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using BusinessCardProject.Server.Core.Domain.Aggregates.Course;
using ContractualDtos.DTO.Course.VideoCourse.Dtos;
using ContractualDtos.DTO.Course.VideoCourse.Requests;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace BusinessCardProject.Server.Core.Infrastructure.Repositories.Courses;

public class VideoCourseRepository : IVideoCourseRepository
{
    private readonly IProjectDbContext _context;

    public VideoCourseRepository(IProjectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<VideoCourseDtos>> GetAllAsync()
    {
        try
        {
            var data = await _context.VideoCourse.ToListAsync();
            return GetVideoCourseDto(data);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<bool?> Create(CreateVideoCourseRequestDto dto)
    {
        try
        {
            var dateTimeNow = SystemClock.Instance.GetCurrentInstant();
            var courseModule = await _context.CourseModule.FindAsync(dto.CourseModuleId);
            var courseAuthor = await _context.CourseAuthor.FindAsync(dto.CourseAuthorId);
            if (courseModule == null || courseAuthor == null) return null;
            var newVideoCourse = new VideoCourseEntity(dto.LinkOnYoutube, dto.LinkOnRutube, dto.LinkOnVkVideo,
                dto.CourseAuthorId, dto.Name, dto.Description, dto.ImgUrl, dateTimeNow, dto.Price, dto.Discount,
                dto.IsShow, dto.DisplayOrder, dto.CourseModuleId, dto.IsFree);
            
            _context.VideoCourse.Add(newVideoCourse);
            await SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<VideoCourseDtos?> Read(Guid id)
    {
        try
        {
            var videoCourse = await _context.VideoCourse.FindAsync(id);
            return videoCourse == null ? null : GetVideoCourseDto(videoCourse);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<VideoCourseDtos?> Update(UpdateVideoCourseRequestDto dto)
    {
        try
        {
            var videoCourse = await _context.VideoCourse.FindAsync(dto.Id);
            if (videoCourse == null) return null;

            videoCourse.Update(dto.LinkOnYoutube, dto.LinkOnRutube, dto.LinkOnVkVideo, dto.CourseAuthorId,
                dto.CourseModuleId, dto.Name, dto.Description, dto.ImgUrl, dto.Price, dto.Discount, dto.IsShow,
                dto.DisplayOrder, dto.IsFree);
            _context.VideoCourse.Update(videoCourse);
            await SaveChanges();

            return GetVideoCourseDto(videoCourse);
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
            var videoCourse = await _context.VideoCourse.FindAsync([id]);
            if (videoCourse == null) return false;

            _context.VideoCourse.Remove(videoCourse);
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
    private static VideoCourseDtos GetVideoCourseDto(VideoCourseEntity e)
    {
        return new VideoCourseDtos(
            e.Id,
            e.Name,
            e.Description,
            e.Img,
            e.DatePublished.ToLocalString(),
            e.Price,
            e.Discount,
            e.Rate,
            e.IsFree,
            e.IsShow,
            e.DisplayOrder,
            e.CourseAuthorId,
            e.CourseModuleId,
            e.LinkOnYoutube,
            e.LinkOnRutube,
            e.LinkOnVkVideo
        );
    }

    /// <summary>
    /// Формирование DTOs для return
    /// </summary>
    private static List<VideoCourseDtos> GetVideoCourseDto(List<VideoCourseEntity> entities)
    {
        return entities.Select(e => new VideoCourseDtos(
            e.Id,
            e.Name,
            e.Description,
            e.Img,
            e.DatePublished.ToLocalString(),
            e.Price,
            e.Discount,
            e.Rate,
            e.IsFree,
            e.IsShow,
            e.DisplayOrder,
            e.CourseAuthorId,
            e.CourseModuleId,
            e.LinkOnYoutube,
            e.LinkOnRutube,
            e.LinkOnVkVideo
        )).ToList();
    }
}