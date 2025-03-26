using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YSHFrame.Singleton;

namespace GameServerApp
{
    public class RoleMgr : Singleton<RoleMgr>
    {
        public List<Role> AllRoles { get; private set; } = new List<Role>();
    }
}
