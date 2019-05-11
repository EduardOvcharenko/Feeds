using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds
{
    public interface ICacheRefresher
    {
        void Refresh();
    }
}
