namespace Application.Interfaces.IServices
{
    public interface IUserContextService
    {
        int? GetUserId();
        string? GetUserEmail();
        bool IsAuthenticated();
        bool IsInRole(string role);
    }
}

