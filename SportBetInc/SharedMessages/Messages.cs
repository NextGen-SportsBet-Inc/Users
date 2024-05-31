namespace Shared.Messages
{
    public record UserExistsRequest 
    {
        public String? UserId { get; set; }
    }

    public record UserExistsResponse
    {
        public bool UserIsValid { get; set; }
    }

}
