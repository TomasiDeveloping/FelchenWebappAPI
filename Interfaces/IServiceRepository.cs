using Api.Helper;

namespace Api.Interfaces;

public interface IServiceRepository
{
    public Task<bool> SendContactMailAsync(Contact contact);
}