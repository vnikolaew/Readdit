using System.ComponentModel.DataAnnotations;

namespace Readdit.Services.Data.Posts.Models;

public class UpdatePostInputModel : BasePostInputModel
{
    [Required]
    public string PostId { get; set; }
}