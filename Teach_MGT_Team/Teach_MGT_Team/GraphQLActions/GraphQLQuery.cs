using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.Models;
using Teach_MGT_Team.TeamAPI.GraphQL;

namespace Teach_MGT_Team.GraphQLActions
{
    // To use in controller
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; } //https://github.com/graphql-dotnet/graphql-dotnet/issues/389
    }

    public class GraphQLAppQuery : ObjectGraphType
    {
        public GraphQLAppQuery(MVCDbContext contextMVC)
        {
            Name = "Query";

            Field<ListGraphType<TeamType>>(
                "teams",
                resolve: context => contextMVC.Team.ToList()
                );

            Field<TeamType>(
                "team",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "teamId" }), // lowercase
                resolve: context => contextMVC.Team.FindAsync(context.GetArgument<int>("teamId")) // lowercase
            );

            Field<ListGraphType<PlayerType>>(
                "players",
                resolve: context => contextMVC.Player.ToList()
                );

            Field<PlayerType>(
                "player",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "playerId" }),
                resolve: context => contextMVC.Player.FindAsync(context.GetArgument<int>("playerId"))
            );
        }
    }
}