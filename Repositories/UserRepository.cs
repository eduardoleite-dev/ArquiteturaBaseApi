using MongoDB.Driver;
using ArquiteturaBaseApi.DTOs;
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

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _usersCollection
            .Find(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail, CancellationToken cancellationToken = default)
    {
        return await _usersCollection
            .Find(u => u.Email == usernameOrEmail || u.Name == usernameOrEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken);
    }

    public async Task UpdateLastLoginAsync(string id, DateTime lastLoginAt, CancellationToken cancellationToken = default)
    {
        var update = Builders<User>.Update.Set(u => u.LastLoginAt, lastLoginAt);
        await _usersCollection.UpdateOneAsync(u => u.Id == id, update, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _usersCollection.DeleteOneAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<PagedResult<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Empty;
        var totalItems = await _usersCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var items = await _usersCollection
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>(items, totalItems, pageNumber, pageSize);
    }
}
