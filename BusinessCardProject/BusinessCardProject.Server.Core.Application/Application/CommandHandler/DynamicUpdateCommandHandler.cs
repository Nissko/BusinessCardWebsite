using BusinessCardProject.Server.Core.Application.Application.Command;
using BusinessCardProject.Server.Core.Application.Common.Enums;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.CustomMediator;
using BusinessCardProject.Server.Core.Application.Common.Interfaces.IRepository.Course;
using ContractualDtos.DTO.Course.CourseTheme.Requests;

namespace BusinessCardProject.Server.Core.Application.Application.CommandHandler
{
    public class DynamicUpdateCommandHandler : IRequestHandler<DynamicUpdateCommand, bool>
    {
        private readonly ICourseThemeRepository _courseThemeRepository;
        private readonly ICourseModuleRepository _courseModuleRepository;
        private readonly IVideoCourseRepository _videoCourseRepository;

        public DynamicUpdateCommandHandler(ICourseThemeRepository courseThemeRepository,
            ICourseModuleRepository courseModuleRepository, IVideoCourseRepository videoCourseRepository)
        {
            _courseThemeRepository =
                courseThemeRepository ?? throw new ArgumentNullException(nameof(courseThemeRepository));
            _courseModuleRepository =
                courseModuleRepository ?? throw new ArgumentNullException(nameof(courseModuleRepository));
            _videoCourseRepository =
                videoCourseRepository ?? throw new ArgumentNullException(nameof(videoCourseRepository));
        }

        public async Task<bool> Handle(DynamicUpdateCommand request, CancellationToken ct)
        {
            //Проверка на пустоту
            if (string.IsNullOrEmpty(request.EntityApiName) || string.IsNullOrEmpty(request.FieldApiName) ||
                string.IsNullOrEmpty(request.Value) || !Guid.TryParse(request.Id.ToString(), out var recordId))
            {
                return false;
            }

            switch (request.EntityApiName)
            {
                case nameof(TypeOfEntityType.CourseThemes):
                    var resultCourseThemeUpdate = await _courseThemeRepository.Update(
                        new UpdateCourseThemeRequestDto(recordId, request.FieldApiName, request.Value));
                    return resultCourseThemeUpdate;
                case nameof(TypeOfEntityType.CourseModule):
                    return true;
                    break;
                case nameof(TypeOfEntityType.VideoCourse):
                    return true;
                    break;
                default:
                    return false;
                    break;
            }
        }
    }
}