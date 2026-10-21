using System.ComponentModel.DataAnnotations;

namespace TetrisEngine.Domain.Game.Player;

public readonly struct Name
    (
    [Required(AllowEmptyStrings = false)] string value
    )
{
 public readonly string Value { get => value; }
    
}
