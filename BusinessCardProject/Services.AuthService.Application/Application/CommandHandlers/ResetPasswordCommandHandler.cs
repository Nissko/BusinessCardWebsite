using MediatR;
using Services.AuthService.Application.Application.Command;
using Services.AuthService.Application.Common.Interfaces;

namespace Services.AuthService.Application.Application.CommandHandlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepo;

        public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordResetRepository passwordResetRepo)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordResetRepo = passwordResetRepo ?? throw new ArgumentNullException(nameof(passwordResetRepo));
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var record =
                await _passwordResetRepo.GetValidRecordAsync(request.UserId, request.ResetToken, cancellationToken);
            if (record == null) throw new Exception("Токен сброса пароля недействителен или истек");

            var user = await _userRepository.GetUser(request.UserId);

            await _userRepository.UpdatePasswordAsync(user.Id, request.NewPassword);

            await _passwordResetRepo.MarkAsUsedAsync(request.UserId, request.ResetToken, cancellationToken);
            await _passwordResetRepo.RevokeAllForUserAsync(request.UserId, cancellationToken);

            return true;
        }
    }
}