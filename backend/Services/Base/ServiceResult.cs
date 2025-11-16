namespace backend.Services.Base
{
    public class ServiceResult
    {
        public required bool IsSuccess { get; init; }
        public required string Message { get; init; }
    }
}
