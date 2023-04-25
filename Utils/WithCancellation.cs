using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    public static class WithCancellationExpand
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => tcs.TrySetResult(true)))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new TaskCanceledException(task);
                }
            }
            return await task;
        }

        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => tcs.TrySetResult(true)))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new TaskCanceledException(task);
                }
            }
            await task;
        }

    }
}
