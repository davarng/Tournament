﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto;

/// <summary>
/// Dto for patching game data.
/// </summary>
public record GamePatchDto
{
    public string? Title { get; set; }
    public DateTime? Time { get; set; }
}
