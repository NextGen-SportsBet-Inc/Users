using MassTransit;
using Shared.Messages;
using SportBetInc.Models;
using SportBetInc.Repositories;

namespace SportBetInc.Consumer
{
    public class VerifyUserConsumer(IUserRepository userRepository, ILogger<VerifyUserConsumer> logger) : IConsumer<UserExistsRequest>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger _logger = logger;

        public async Task Consume(ConsumeContext<UserExistsRequest> context)
        {
            _logger.LogInformation("Received a request to check if a user exists.");
            var userId = context.Message.UserId;

            var response = new UserExistsResponse
            {
                UserIsValid = false
            };

            if (userId == null)
            {
                _logger.LogInformation("User does not have an Id present in the message received");
                await context.RespondAsync(response);
                return;
            }

            User? user = await _userRepository.GetUserInfoById(userId);

            if (user  == null)
            {
                _logger.LogInformation("User with Id {uesrId} does not exist", userId);
                await context.RespondAsync(response);
                return;
            }

            response.UserIsValid = true;

            _logger.LogInformation("Sent response to the user exists request.");
            await context.RespondAsync(response);

            return;
        }
    }

    
}
