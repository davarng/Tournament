using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

/// <summary>
/// Dto for creating a game.
/// </summary>
public record GameCreateDto
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title is too short(Min 2 chars).")]
    [MaxLength(50, ErrorMessage = "Title is too long(Max 50 chars).")]
    public string Title { get; set; }
    public DateTime Time { get; set; }
    [Required]
    public int TournamentId { get; set; }
}
