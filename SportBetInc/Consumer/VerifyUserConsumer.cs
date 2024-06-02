using MassTransit;
using Shared.Messages;
using SportBetInc.Models;
using SportBetInc.Repositories;

namespace SportBetInc.Consumer
{
    public class VerifyUserConsumer(IUserRepository userRepository) : IConsumer<UserExistsRequest>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Consume(ConsumeContext<UserExistsRequest> context)
        {
            var userId = context.Message.UserId;

            var response = new UserExistsResponse
            {
                UserIsValid = false
            };

            if (userId == null)
            {
                await context.RespondAsync(response);
                return;
            }

            User? user = await _userRepository.GetUserInfoById(userId);

            if (user  == null)
            {
                await context.RespondAsync(response);
                return;
            }

            response.UserIsValid = true;

            await context.RespondAsync(response);

            return;
        }
    }

    
}
