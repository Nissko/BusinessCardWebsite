using ByteCodePlatform.API.ProtoMappers.Course.Contents;
using ByteCodePlatform.API.ProtoMappers.Course.Modules;
using ByteCodePlatform.API.ProtoMappers.Course.ProgramLanguages;
using ByteCodePlatform.API.ProtoMappers.Course.Themes;
using ByteCodePlatform.Application.Application.Extensions;
using ByteCodePlatform.Application.Common.Interfaces.Repositories;
using ByteCodePlatform.Domain.Enums;
using CourseService.Proto;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using UpdateCourseContentRequest = CourseService.Proto.UpdateCourseContentRequest;
using UpdateCourseModuleRequest = CourseService.Proto.UpdateCourseModuleRequest;
using UpdateCourseThemeRequest = CourseService.Proto.UpdateCourseThemeRequest;

namespace ByteCodePlatform.API.Services
{
    [Authorize]
    public class CourseGrpcService : CourseService.Proto.CourseService.CourseServiceBase
    {
        private readonly ICourseRepository _courseService;

        public CourseGrpcService(ICourseRepository courseService)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        }

        public override async Task<CheckGrpcCourseTimingResponse> CheckGrpcCourseTiming(
            CheckGrpcCourseTimingRequest request, ServerCallContext context)
        {
            var resultCheck = await _courseService.CheckGrpcCourseTiming();
            
            return new CheckGrpcCourseTimingResponse
            {
                DateTime = resultCheck.DateTime.ToTimestamp(),
                Health = resultCheck.Health,
                GrpcService = resultCheck.GrpcServiceName
            };
        }

        #region ProgrammingLanguage

        [AllowAnonymous]
        public override async Task<ProgrammingLanguagesInfoResponse> GetProgrammingLanguages(
            GetProgrammingLanguagesRequest request, ServerCallContext context)
        {
            try
            {
                var programmingLanguages = await _courseService.GetProgrammingLanguages();
                return new()
                {
                    ProgrammingLanguages = { programmingLanguages.ToProtoProgramLanguageInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        #endregion

        #region CourseTheme

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseThemeInfoResponse> AddCourseTheme(AddCourseThemeRequest request,
            ServerCallContext context)
        {
            try
            {
                var newCourseTheme = await _courseService.AddCourseTheme(new(
                    request.ProgrammingLanguageId.ToGuid(), request.AuthorId.ToGuid(), request.Name,
                    request.Description,
                    request.AvatarUrl, request.Price, request.OldPrice));
                return newCourseTheme.ToProtoCourseThemeInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<CourseThemesInfoResponse> GetCourseThemes(GetCourseThemesRequest request,
            ServerCallContext context)
        {
            try
            {
                var courseThemes = await _courseService.GetCourseThemes();
                return new()
                {
                    CourseThemes = { courseThemes.ToProtoCourseThemeInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CourseThemeInfoResponse> GetCourseTheme(GetCourseThemeRequest request, ServerCallContext context)
        {
            try
            {
                var courseTheme = await _courseService.GetCourseTheme(request.Id.ToGuid());
                return courseTheme.ToProtoCourseThemeInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseThemeInfoResponse> UpdateCourseTheme(UpdateCourseThemeRequest request,
            ServerCallContext context)
        {
            try
            {
                double? price = request.HasPrice ? request.Price : null;
                double? oldPrice = request.HasOldPrice ? request.OldPrice : null;

                var updateCourseTheme = await _courseService.UpdateCourseTheme(new(request.Id.ToGuid(),
                    request.ProgrammingLanguageId.ToGuidOrNull(), request.Name, request.Description, request.AvatarUrl,
                    price, oldPrice, request.IsShow, request.DisplayOrder, request.IsFree));
                return updateCourseTheme.ToProtoCourseThemeInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        #endregion

        #region CourseModule

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseModuleInfoResponse> AddCourseModule(AddCourseModuleRequest request,
            ServerCallContext context)
        {
            try
            {
                var newCourseModule = await _courseService
                    .AddCourseModule(
                        new(request.CourseThemeId.ToGuid(), request.Name));
                return newCourseModule.ToProtoCourseModuleInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<CourseModulesInfoResponse> GetCourseModules(GetCourseModulesRequest request,
            ServerCallContext context)
        {
            try
            {
                var courseModules = await _courseService.GetCourseModules();
                return new()
                {
                    CourseModules = { courseModules.ToProtoCourseModuleInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CourseModulesInfoResponse> GetCourseModulesFromCourse(GetCourseModulesFromCourseRequest request, ServerCallContext context)
        {
            try
            {
                var courseModules = await _courseService.GetCourseModulesFromCourse(request.CourseId.ToGuid());
                return new()
                {
                    CourseModules = { courseModules.ToProtoCourseModuleInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseModuleInfoResponse> UpdateCourseModule(UpdateCourseModuleRequest request,
            ServerCallContext context)
        {
            try
            {
                var updateCourseModule = await _courseService.UpdateCourseModule(new(request.Id.ToGuid(),
                    request.CourseThemeId.ToGuidOrNull(), request.Name, request.IsShow, request.DisplayOrder));
                return updateCourseModule.ToProtoCourseModuleInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        #endregion

        #region CourseContent

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseContentInfoResponse> AddCourseContent(AddCourseContentRequest request,
            ServerCallContext context)
        {
            try
            {
                var newCourseContent = await _courseService.AddCourseContent(
                    new(request.CourseModuleId.ToGuid(), request.Name, request.LinkOnRutube,
                        request.LinkOnVk, request.LinkOnYoutube, request.ImgUrl));
                return newCourseContent.ToProtoCourseContentInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [AllowAnonymous]
        public override async Task<CourseContentsInfoResponse> GetCourseContents(GetCourseContentsRequest request,
            ServerCallContext context)
        {
            try
            {
                var courseContents = await _courseService.GetCourseContents();
                return new()
                {
                    CourseContents = { courseContents.ToProtoCourseContentInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        [Authorize(Roles = UserRoleStaticEnum.Admin)]
        public override async Task<CourseContentInfoResponse> UpdateCourseContent(UpdateCourseContentRequest request,
            ServerCallContext context)
        {
            try
            {
                var updateCourseContent = await _courseService.UpdateCourseContent(new(request.Id.ToGuid(),
                    request.CourseModuleId.ToGuidOrNull(), request.Name, request.LinkOnRutube, request.LinkOnVk,
                    request.LinkOnYoutube, request.ImgUrl, request.IsShow, request.DisplayOrder));
                return updateCourseContent.ToProtoCourseContentInfo();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        #endregion
    }
}