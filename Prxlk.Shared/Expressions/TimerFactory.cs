using System;
using System.Threading;

namespace Prxlk.Shared.Expressions
{
    public class TimerFactory
    {             
        public static Timer CreateTimer(TimerCallback callback, TimeSpan period, TimeSpan delay)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // Don't capture the current ExecutionContext and its AsyncLocals onto the timer
            var restoreFlow = false;
            try
            {
                if (ExecutionContext.IsFlowSuppressed()) 
                    return new Timer(callback, null, delay, period);
                
                ExecutionContext.SuppressFlow();
                restoreFlow = true;

                return new Timer(callback, null, delay, period);
            }
            finally
            {
                // Restore the current ExecutionContext
                if (restoreFlow)
                {
                    ExecutionContext.RestoreFlow();
                }
            }
        } 
    }
}