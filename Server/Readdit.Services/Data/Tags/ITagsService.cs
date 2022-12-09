using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Tags;

public interface ITagsService
{
    Task<IEnumerable<T>> GetAllAsync<T>();
    
    Task<IEnumerable<Tag>> GetAllByNamesAsync(IEnumerable<string> tagNames);
}