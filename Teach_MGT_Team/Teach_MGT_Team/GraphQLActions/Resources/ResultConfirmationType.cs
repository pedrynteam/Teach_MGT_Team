using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teach_MGT_Team.GraphQLActions.Resources
{
    // Class to handle confirmations to client, PASS / FAIL, Message
    public class ResultConfirmation
    {
        public string ResultCode { get; set; }
        public bool ResultPassed { get; set; }
        public string ResultMessage { get; set; } // Message to translate: ORDER_NOT_FOUND
        public string ResultDetail { get; set; } // Detail to transaction: 1   (Order number)
        public List<ItemKey> ResultItemList { get; set; }

        public static ResultConfirmation resultGood(string _ResultMessage, string _ResultDetail = "", List<ItemKey> _ResultItemList = null)
        {
            return new ResultConfirmation
            {
                ResultCode = "PASS",
                ResultPassed = true,
                ResultMessage = _ResultMessage,
                ResultDetail = _ResultDetail,
                ResultItemList = _ResultItemList
            };
        }

        public static ResultConfirmation resultBad(string _ResultMessage, string _ResultDetail = "", List<ItemKey> _ResultItemList = null)
        {
            return new ResultConfirmation
            {
                ResultCode = "FAIL",
                ResultPassed = false,
                ResultMessage = _ResultMessage,
                ResultDetail = _ResultDetail,
                ResultItemList = _ResultItemList
            };
        }
    }



    public class ResultConfirmationType : ObjectGraphType<ResultConfirmation>
    {
        public ResultConfirmationType()
        {
            Name = "ResultConfirmationType";
            Field(x => x.ResultCode).Description("The response code of the result. PASS / FAIL");
            Field(x => x.ResultPassed).Description("The status of the result. true / false");
            Field(x => x.ResultMessage).Description("The message of the result");
            Field(x => x.ResultDetail).Description("The detail of the result");
            Field<ListGraphType<ItemKeyType>>("ResultItemList",
                resolve: context => context.Source.ResultItemList,
                description: "The Item List of the result. Tag -> Value"
                );
        }
    }
}
