using Models.Tasks;

using System.Security.Principal;

namespace Tasks.Commands
{
    public class WhoAmI : CerberusCommand
    {
        public override string Name => "whoami";

        public override string Execute(MythicTask task)
        {
            var identity = WindowsIdentity.GetCurrent();
            return identity.Name;
        }
    }
}
