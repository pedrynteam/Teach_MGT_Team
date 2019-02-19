using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.Models;
using Teach_MGT_Team.TeamAPI.MVC;

namespace Teach_MGT_Team.TeamAPI.GraphQL
{
    public class TeamType : ObjectGraphType<Team>
    {
        public TeamType(MVCDbContext _contextMVC)
        {
            Name = "Team";
            Field(x => x.TeamId).Description("The id of the Team");
            Field(x => x.Name).Description("The name of the Team");
            Field<ListGraphType<PlayerType>>(
                "players",
                resolve: context => _contextMVC.Player.Where(x => x.TeamId == context.Source.TeamId)
                );
        }
    }

    public class TeamInputType : InputObjectGraphType<Team>
    {
        public TeamInputType(MVCDbContext _contextMVC)
        {
            Name = "TeamInput";
            Field(x => x.TeamId).Description("The id of the Team");
            Field(x => x.Name).Description("The name of the Team");
        }
    }
}
