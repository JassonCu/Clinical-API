namespace Clinical.Interface.Interfaces
{
    public interface ISetupRepository
    {
        Task<bool> IsInitializedAsync();
        Task<bool> InitAsync(string username, string email, string passwordHash, string firstName, string lastName);
    }
}
