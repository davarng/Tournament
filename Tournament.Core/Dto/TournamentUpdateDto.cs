﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

/// <summary>
/// Dto for updating a tournament.
/// </summary>
public record TournamentUpdateDto
{
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title is too short(Min 2 chars).")]
    [MaxLength(50, ErrorMessage = "Title is too long(Max 50 chars).")]
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
}
