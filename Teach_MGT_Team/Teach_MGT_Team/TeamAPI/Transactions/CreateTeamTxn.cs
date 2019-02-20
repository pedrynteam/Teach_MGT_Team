using GraphQL.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teach_MGT_Team.GraphQLActions.Resources;
using Teach_MGT_Team.Models;
using Teach_MGT_Team.TeamAPI.GraphQL;
using Teach_MGT_Team.TeamAPI.MVC;

namespace Teach_MGT_Team.TeamAPI.Transactions
{
    // 1. Create Model: Input and Output
    public class CreateTeam_Input
    {
        public Team team { get; set; }
    }

    public class CreateTeam_Output
    {
        public ResultConfirmation resultConfirmation { get; set; }
        public Team team { get; set; } // This will contain the team and all the players
    }

    // 2. Input Types.  Input type is used for Mutation, it should be included if needed
    public class CreateTeam_InputType : InputObjectGraphType<CreateTeam_Input>
    {
        public CreateTeam_InputType()//MVCDbContext _context)
        {
            Name = "CreateTeam_Input";
            Description = "Insert new team with players";

            Field<TeamInputType>("team",
                description: "The new team.",
                resolve: context => context.Source.team);
        }
    }

    // 3. Output Types. Output type is used for Mutation, it should be included if needed
    public class CreateTeam_OutputType : ObjectGraphType<CreateTeam_Output>
    {
        public CreateTeam_OutputType()
        {
            Name = "CreateTeam_OutputType";
            Description = "Returns the team created and players";

            Field<ResultConfirmationType>("resultConfirmation",
                description: "Result confirmation of the call",
                resolve: context => context.Source.resultConfirmation);
            // Causing an issue?
            Field<TeamType>("team",
                description: "The team and players created",
                resolve: context => context.Source.team); // This won't go to database so fill it out or it will be null
        }
    }

    // 4. Transaction - Logic Controller
    public class CreateTeamTxn
    {
        public CreateTeamTxn()
        {
        }

        public async Task<CreateTeam_Output> Execute(CreateTeam_Input _input, MVCDbContext _contextFather = null, bool _autoCommit = true)
        {
            CreateTeam_Output _output = new CreateTeam_Output();
            _output.resultConfirmation = ResultConfirmation.resultBad(_ResultMessage: "TXN_NOT_STARTED");

            // Error handling
            bool error = false; // To Handle Only One Error

            try
            {
                MVCDbContext _contextMGT = (_contextFather != null) ? _contextFather : new MVCDbContext();
                // An using statement is in reality a try -> finally statement, disposing the element in the finally. So we need to take advance of that to create a DBContext inheritance                
                try
                {
                    // DBContext by convention is a UnitOfWork, track changes and commits when SaveChanges is called
                    // Multithreading issue: so if we use only one DBContext, it will track all entities (_context.Team.Add) and commit them when SaveChanges is called, 
                    // No Matter what transactions or client calls SaveChanges.
                    // Note: Rollback will not remove the entities from the tracker in the context. so better dispose it.

                    //***** 0. Make The Validations - Be careful : Concurrency. Same name can be saved multiple times if called at the exact same time. Better have an alternate database constraint
                    if (_contextMGT.Team.Any(q => q.Name.Equals(_input.team.Name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        error = true;
                        _output.resultConfirmation = ResultConfirmation.resultBad(_ResultMessage: "TEAM_EXISTS", _ResultDetail: _input.team.Name); // If OK                            
                    }

                    if (!error)
                    {
                        //***** 1. Save the Team (Atomic because is same Context DB)
                        _input.team.TeamId = 0; // Just in case they send other thing. This is for the autoincrement                                                        
                        _contextMGT.Team.Add(_input.team);

                        /* Parallelism issue... Analyze the problem and decide:
                         * - What actions can be done before Commit out DB
                         * - What actions can be rolled back
                         * - What actions cannot be rolled back
                         * - What actions can de done later (Delay): i.e. Send confirmation e-mail
                         */

                        //***** 2. Execute events that can be done before the database commits. 
                        //***** If the event fails, return the error, DB automatically rollbacks if SaveChangesAsync is not called

                        /* Call other transactions 

                        // a) Context Inheritance - Commit or not on inner transactions depends on the problem
                        Team inheritedTeam = new Team { TeamId = 0, Name = "FatherTeam" };
                        CreateTeam_Input inheritedInput = new CreateTeam_Input { team = inheritedTeam };
                        CreateTeam_Output inheritedOutput = await CreateTeamTxn(_input: inheritedInput, _contextFather: _contextMGT, _autoCommit: false);

                        // b) Context No Inheritance - Commit or not on inner transactions depends on the problem
                        Team inheritedTeamAlone = new Team { TeamId = 0, Name = "FatherTeamAlone" };
                        CreateTeam_Input inheritedInputAlone = new CreateTeam_Input { team = inheritedTeamAlone };
                        CreateTeam_Output inheritedOutputAlone = await CreateTeamTxn(_input: inheritedInputAlone);

                        */

                        //***** 3. Validate results from events. Define error or success

                        //***** 4. Save and Commit to the Database (Atomic because is same Context DB) 
                        if (!error && _autoCommit)
                        {
                            await _contextMGT.SaveChangesAsync(); // Call it only once so do all other operations first
                        }

                        //***** 5. Execute Send e-mails or other events once the database has been succesfully saved
                        //***** If this task fails, there are options -> 1. Retry multiple times 2. Save the event as Delay, 3.Rollback Database, Re

                        //***** 6. Confirm the Result (Pass | Fail) If gets to here there are not errors then return the new data from database
                        _output.resultConfirmation = ResultConfirmation.resultGood(_ResultMessage: "TEAM_SUCCESSFULLY_SAVED"); // If OK
                        _output.team = _input.team; // The input team also have the Players                            
                    }// if (!error)
                }
                finally
                {
                    // If the context Father is null the context was created on his own, so dispose it
                    if (_contextMGT != null && _contextFather == null)
                    {
                        _contextMGT.Dispose();
                    }
                }
            }
            catch (Exception ex) // Main try 
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                string innerError = (ex.InnerException != null) ? ex.InnerException.Message : "";
                System.Diagnostics.Debug.WriteLine("Error Inner: " + innerError);
                _output = new CreateTeam_Output(); // Restart variable to avoid returning any already saved data
                _output.resultConfirmation = ResultConfirmation.resultBad(_ResultMessage: "EXCEPTION", _ResultDetail: ex.Message);
            }
            finally
            {
                // Save Logs if needed
            }

            return _output;
        }

    }


}
