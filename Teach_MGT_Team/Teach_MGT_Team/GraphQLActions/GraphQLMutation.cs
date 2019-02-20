using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.Models;
using Teach_MGT_Team.TeamAPI.Transactions;

namespace Teach_MGT_Team.GraphQLActions
{
    // Important Note: For any mutation is better to return a ResultConfirmationType and any other required object (Customer, Order, etc). 
    // So in the client will be easy to show any error instead of try to think what happened
    public class GraphQLMutation : ObjectGraphType<object>
    {

        public GraphQLMutation()
        {
            Name = "Mutation";

            // Name: createTeamAndPlayers
            // Type: CreateTeamAndPlayersTxn
            // In: New Team
            // In: New Players for Team
            // Out: Team with Players
            Field<CreateTeamAndPlayers_OutputType>(
                "createTeamAndPlayers",
                description: "Create a new team with players",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CreateTeamAndPlayers_InputType>> { Name = "input" }
                ),
                resolve: context =>
                {
                    CreateTeamAndPlayers_Input _input = context.GetArgument<CreateTeamAndPlayers_Input>("input");
                    return new CreateTeamAndPlayersTxn().Execute(_input);
                }
            );

            // Name: createTeam
            // Type: CreateTeamTxn
            // In: New Team
            // Out: Team
            Field<CreateTeam_OutputType>(
                "createTeam",
                description: "Create a new team",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CreateTeam_InputType>> { Name = "input" }
                ),
                resolve: context =>
                {
                    CreateTeam_Input _input = context.GetArgument<CreateTeam_Input>("input");
                    return new CreateTeamTxn().Execute(_input);
                }
            );

        }

    }
}
