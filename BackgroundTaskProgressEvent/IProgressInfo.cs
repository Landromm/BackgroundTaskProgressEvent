using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskProgressEvent
{
    public interface IProgressInfo
    {
        bool IsCompleted { get; }
    }

    

}
