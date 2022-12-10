using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Tags;

public class TagsService : ITagsService
{
    private readonly IRepository<Tag> _tags;

    public TagsService(IRepository<Tag> tags)
        => _tags = tags;

    public async Task<IEnumerable<T>> GetAllAsync<T>()
        => await _tags
            .AllAsNoTracking()
            .To<T>()
            .ToListAsync();

    public async Task<IEnumerable<Tag>> GetAllByNamesAsync(IEnumerable<string> tagNames)
        => await _tags
            .All()
            .Where(t => tagNames.AsEnumerable().Contains(t.Name))
            .ToListAsync();
}