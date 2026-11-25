using MongoDB.Driver;
using ArquiteturaBaseApi.Models;
using ArquiteturaBaseApi.Repositories.Interfaces;

namespace ArquiteturaBaseApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<User>("Users");
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default)
    {
        return await _usersCollection
            .Find(u => u.Email == usernameOrEmail || u.Name == usernameOrEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateLastLoginAsync(string id, DateTime lastLoginAt, CancellationToken cancellationToken = default)
    {
        var update = Builders<User>.Update.Set(u => u.LastLoginAt, lastLoginAt);
        await _usersCollection.UpdateOneAsync(u => u.Id == id, update, cancellationToken: cancellationToken);
    }
}
