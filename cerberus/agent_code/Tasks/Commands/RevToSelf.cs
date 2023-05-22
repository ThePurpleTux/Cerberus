using Models.Tasks;

namespace Tasks.Commands
{
    public class RevToSelf : CerberusCommand
    {
        public override string Name => "rev2self";

        public override string Execute(MythicTask task)
        {
            if (Native.Advapi.RevertToSelf())
            {
                return "Reverted to self";
            }

            return "Failed to revert";
        }
    }
}
