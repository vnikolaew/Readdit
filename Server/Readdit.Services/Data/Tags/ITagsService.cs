using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Tags;

public interface ITagsService
{
    Task<IEnumerable<Tag>> GetAllAsync();
    
    Task<IEnumerable<Tag>> GetAllByNamesAsync(IEnumerable<string> tagNames);
}