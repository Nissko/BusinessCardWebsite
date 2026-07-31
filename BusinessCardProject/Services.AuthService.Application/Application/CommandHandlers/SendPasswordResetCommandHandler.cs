using MediatR;
using Microsoft.Extensions.Configuration;
using Requests.Email;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Common.Interfaces;
using Services.AuthService.Application.Common.Interfaces.GrpcClients;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class SendPasswordResetCommandHandler : IRequestHandler<SendPasswordResetCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepo;
        private readonly IMailServiceGrpcServiceClient _mailService;
        private readonly IConfiguration _configuration;

        public SendPasswordResetCommandHandler(
            IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepo,
            IMailServiceGrpcServiceClient mailService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordResetRepo = passwordResetRepo;
            _mailService = mailService;
            _configuration = configuration;
        }

        public async Task<bool> Handle(SendPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var userDto = await _userRepository.GetUserByEmail(request.Email);
            var userId = userDto.Id;

            await _passwordResetRepo.CreateRecordAsync(userId, cancellationToken);

            var clientUrl = _configuration.GetValue<string>("FrontendLink:Url") 
                            ?? "https://it-bytecode.splinterkeenetic.netcraze.club";
            var resetLink = $"{clientUrl}/reset-password/{userId}";

            /*TODO: сделать шаблон письма*/
            await _mailService.SendEmailMessageAsync(new EmailMessageRequest(
                To: [request.Email],
                Cc: [],
                Bcc: [],
                From: _configuration["MailSettings:From"]!,
                DisplayName: _configuration["MailSettings:DisplayName"]!,
                ReplyTo: "",
                ReplyToName: "",
                Subject: "Сброс пароля",
                Body: resetLink,
                IsHtml: false,
                TemplateName: null
            ));

            return true;
        }
    }
}