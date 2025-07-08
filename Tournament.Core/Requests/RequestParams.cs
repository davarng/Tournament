using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Requests;

public class RequestParams
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(2, 20)]
    public int PageSize { get; set; } = 5;
}

public class TournamentRequestParams : RequestParams
{
    public bool IncludeGames { get; set; } = false;
}
