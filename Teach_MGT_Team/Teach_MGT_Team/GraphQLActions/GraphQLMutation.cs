using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.Models;

namespace Teach_MGT_Team.GraphQLActions
{
    // Important Note: For any mutation is better to return a ResultConfirmationType and any other required object (Customer, Order, etc). 
    // So in the client will be easy to show any error instead of try to think what happened
    public class GraphQLMutation : ObjectGraphType<object>
    {
        public GraphQLMutation(MVCDbContext _context)
        {
            Name = "Mutation";            
        }        
    }
}
