using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teach_MGT_Team.GraphQLActions
{
    public class GraphQLSchema : Schema
    {
        public GraphQLSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<GraphQLAppQuery>();
            Mutation = resolver.Resolve<GraphQLMutation>();
        }
    }
}
