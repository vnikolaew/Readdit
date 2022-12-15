using System.ComponentModel.DataAnnotations.Schema;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class UserScore : BaseEntity<string>
{
    [ForeignKey(nameof(Id))]
    public ApplicationUser User { get; set; }

    public int PostsScore { get; set; }
    
    public int CommentsScore { get; set; }
}