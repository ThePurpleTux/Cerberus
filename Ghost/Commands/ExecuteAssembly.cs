using Ghost.Models.Tasks;

namespace Ghost.Commands
{
    public class ExecuteAssembly : GhostCommand
    {
        public override string Name => "execute-assembly";

        public override string Execute(GhostTask task)
        {
            return Internal.Execute.ExecuteAssembly(task.FileBytes, task.Arguments);
        }
    }
}
