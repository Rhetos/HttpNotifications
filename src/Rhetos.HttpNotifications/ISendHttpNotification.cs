using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhetos.HttpNotifications
{
    // TODO: Remove this after refactoring Rhetos.Jobs to support any job executer instead of DSL Actions only.
    public interface ISendHttpNotification
    {
        string Url { get; set; }
        string Payload { get; set; }
    }
}
