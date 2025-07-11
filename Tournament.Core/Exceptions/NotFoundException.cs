using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Exceptions;

public class NotFoundException : Exception
{
    public string Title { get; set; }

    public static void ThrowIfNull(object value, string name)
    {
        if (value == null)
        {
            throw new NotFoundException($"{name} is not found", name);
        }
    }

    protected NotFoundException(string message, string title = "Not found") : base(message)
    {
        Title = title;
    }
}

public class TournamentNotFoundException : NotFoundException
{
    public TournamentNotFoundException(int id) : base($"The tournament with id {id} is not found")
    {

    }
}

public class GameNotFoundException : NotFoundException
{
    public GameNotFoundException(int id) : base($"The Game with id {id} is not found")
    {

    }
}
