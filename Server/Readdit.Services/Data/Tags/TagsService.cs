using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Tags;

public class TagsService : ITagsService
{
    private readonly IRepository<Tag> _tags;

    public TagsService(IRepository<Tag> tags)
    {
        _tags = tags;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
        => await _tags.All().ToListAsync();

    public async Task<IEnumerable<Tag>> GetAllByNamesAsync(IEnumerable<string> tagNames)
        => await _tags.All().Where(t => tagNames.AsEnumerable().Contains(t.Name)).ToListAsync();
}