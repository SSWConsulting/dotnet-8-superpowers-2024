using JusticeVoter.Shared;

namespace JusticeVoter.Server.Services;

public class HeroService : IHeroService
{
    private static readonly string imageBaseUrl = "https://gordonbeemingcom.blob.core.windows.net/dotnetsuperpowers";
    
    public static List<Hero> Supermen =
    [
        new Hero
        {
            Id = 0,
            ActorName = "Christopher Reeve",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/christopher-reeve.png",
            Context = "Superman: The Movie",
            Year = 1978
        },
        new Hero
        {
            Id = 1,
            ActorName = "Brandon Routh",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/brandon-routh.jpg",
            Context = "Superman Returns",
            Year = 2006
        },
        new Hero
        {
            Id = 2,
            ActorName = "Dean Cain",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/dean-cain.jpg",
            Context = "Lois & Clark",
            Year = 1994
        },
        new Hero
        {
            Id = 3,
            ActorName = "Henry Cavill",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/henry-cavill.jpg",
            Context = "Man of Steel",
            Year = 2013
        },
        new Hero
        {
            Id = 4,
            ActorName = "Tom Welling",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/tom-welling.jpg",
            Context = "Smallville",
            Year = 2001
        },
        new Hero
        {
            Id = 5,
            ActorName = "Tyler Hoechlin",
            ImageSource = $"{imageBaseUrl}/Heroes/Superman/tyler-hoechlin.jpg",
            Context = "Superman & Lois",
            Year = 2021
        }
    ];

    public static List<Hero> Batmen =
    [
        new Hero
        {
            Id = 6,
            ActorName = "Adam West",
            ImageSource = $"{imageBaseUrl}/Heroes/Batman/adam-west.jpg",
            Context = "Batman",
            Year = 1966
        },
        new Hero
        {
            Id = 7,
            ActorName = "Ben Affleck",
            ImageSource = $"{imageBaseUrl}/Heroes/Batman/ben-affleck.jpg",
            Context = "Superman v Batman",
            Year = 2016
        },
        new Hero
        {
            Id = 8,
            ActorName = "Christian Bale",
            ImageSource = $"{imageBaseUrl}/Heroes/Batman/christian-bale.jpg",
            Context = "Batman Begins",
            Year = 2005
        },
        new Hero
        {
            Id = 9,
            ActorName = "Michael Keaton",
            ImageSource = $"{imageBaseUrl}/Heroes/Batman/michael-keaton.jpg",
            Context = "Batman",
            Year = 1989
        },
        new Hero
        {
            Id = 10,
            ActorName = "Robert Pattinson",
            ImageSource = $"{imageBaseUrl}/Heroes/Batman/robert-pattinson.jpg",
            Context = "The Batman",
            Year = 2022
        }
    ];

    public static List<Hero> Flashes =
    [
        new Hero
        {
            Id = 11,
            ActorName = "Ezra Miller",
            ImageSource = $"{imageBaseUrl}/Heroes/Flash/ezra-miller.jpg",
            Context = "Justice League",
            Year = 2017
        },
        new Hero
        {
            Id = 12,
            ActorName = "John Wesley Ship",
            ImageSource = $"{imageBaseUrl}/Heroes/Flash/john-wesley-ship.jpg",
            Context = "The Flash",
            Year = 1990
        },
        new Hero
        {
            Id = 13,
            ActorName = "Grant Gustin",
            ImageSource = $"{imageBaseUrl}/Heroes/Flash/grant-gustin.jpg",
            Context = "The Flash",
            Year = 2014
        },
        new Hero
        {
            Id = 14,
            ActorName = "Kyle Gallner",
            ImageSource = $"{imageBaseUrl}/Heroes/Flash/kyle-gallner.jpg",
            Context = "Smallville",
            Year = 2001
        }
    ];

    public static List<Hero> WonderWomen =
    [
        new Hero
        {
            Id = 15,
            ActorName = "Gal Gadot",
            ImageSource = $"{imageBaseUrl}/Heroes/WonderWoman/gal-gadot.jpg",
            Context = "Batman v Superman",
            Year = 2015
        },
        new Hero
        {
            Id = 16,
            ActorName = "Lynda Carter",
            ImageSource = $"{imageBaseUrl}/Heroes/WonderWoman/lynda-carter.jpg",
            Context = "Wonder Woman",
            Year = 1975
        }
    ];

    public List<Hero> GetHeroes(string hero)
    {
        return hero switch
        {
            "superman" => Supermen,
            "batman" => Batmen,
            "flash" => Flashes,
            "wonderwoman" => WonderWomen,
            _ => throw new Exception("Hero not found"),
        };
    }

    public static Dictionary<int, int> VoteTally = [];

    public void AddVote(int heroId)
    {
        if (VoteTally.TryGetValue(heroId, out int tally))
        {
            VoteTally[heroId] = tally + 1;
        }
        else
        {
            VoteTally.Add(heroId, 1);
        }
    }

    public int GetVoteTally(int heroId)
    {
        if (VoteTally.TryGetValue(heroId, out int tally))
        {
            return tally;
        }

        return 0;
    }

    public void ClearVotes()
    {
        VoteTally?.Clear();
    }
}
