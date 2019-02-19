using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.Models;
using Teach_MGT_Team.TeamAPI.MVC;

namespace Teach_MGT_Team.TeamAPI.GraphQL
{
    public class PlayerType : ObjectGraphType<Player>
    {
        public PlayerType(MVCDbContext _contextMVC)
        {
            Name = "Player";
            Field(x => x.PlayerId).Description("The id of the Player");
            Field(x => x.Name).Description("The name of the Player");

            Field<TeamType>("Team",
                resolve: context => _contextMVC.Team.FindAsync(context.Source.TeamId));
        }
    }

    public class PlayerInputType : InputObjectGraphType<Player>
    {
        public PlayerInputType(MVCDbContext _contextMVC)
        {
            Name = "PlayerInput";
            Field(x => x.PlayerId).Description("The id of the Player");
            Field(x => x.Name).Description("The name of the Player");
        }
    }
}
