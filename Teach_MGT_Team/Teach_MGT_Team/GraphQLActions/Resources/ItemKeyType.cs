using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teach_MGT_Team.GraphQLActions.Resources
{
    public class ItemKey
    {
        public string Tag { get; set; }
        public string Value { get; set; }
    }

    public class ItemKeyType : ObjectGraphType<ItemKey>
    {
        public ItemKeyType()
        {
            Name = "ItemKeyType";
            Field(x => x.Tag).Description("The tag of the Item List");
            Field(x => x.Value).Description("The Value of the Result List.");
        }
    }
}
