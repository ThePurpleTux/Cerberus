using Cerberus.Models.Tasks;

namespace Cerberus.Commands
{
    public class ExecuteAssembly : CerberusCommand
    {
        public override string Name => "execute-assembly";

        public override string Execute(CerberusTask task)
        {
            return Internal.Execute.ExecuteAssembly(task.FileBytes, task.Arguments);
        }
    }
}
