using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lindotnet.Classes.Component.Implementation;
using lindotnet.Classes.Component.Interfaces;

namespace lindotnet
{
    public static class Container
    {
        /// <summary>
        /// Create a new softphone instance
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static ISoftphone CreateNewSoftphone(Account account)
        {
            return new Softphone(account);
        }
    }
}
