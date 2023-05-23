using System;
using System.Security.Principal;
using Models.Managers;

namespace Managers
{


    public class IdentityManager : IManager
    {
        public string Name => "IdentityManager";

        public WindowsIdentity OriginalIdentity { get; set; }
        public WindowsIdentity CurrentIdentity { get; set; }
        public WindowsIdentity ImpersonationIdentity { get; set; }

        public void Init()
        {
            OriginalIdentity = WindowsIdentity.GetCurrent();
            CurrentIdentity = WindowsIdentity.GetCurrent();
            ImpersonationIdentity = WindowsIdentity.GetCurrent();
        }

        public bool SetCurrentWindowsIdentity(WindowsIdentity identity)
        {
            try
            {
                CurrentIdentity = identity;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetImpersonationIdentity(WindowsIdentity identity)
        {
            try
            {
                ImpersonationIdentity = identity;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RevertToOriginalIdentity()
        {
            try
            {
                ImpersonationIdentity = OriginalIdentity;
                CurrentIdentity = OriginalIdentity;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RevertCurrentIdentity()
        {
            try
            {
                CurrentIdentity = OriginalIdentity;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RevertImpersonationIdentity()
        {
            try
            {
                ImpersonationIdentity = OriginalIdentity;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
