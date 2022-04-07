using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class Async
    {
        public static async void DoAsync(this Task task)
        {
            await task;
        }
        
        public static async Task DoDelay(this Func<Task> cbTask, int delay)
        {
            await Delay(delay);
            await cbTask();    
        }
        
        public static async void DelayedCall(int delay, Action cb)
        {
            await Delay(delay);
            cb();
        }
        
        public static Task Delay(int milliseconds)
        {
            var timeScale = Time.timeScale != 0 ? Time.timeScale : 0.001f;
            return Task.Delay((int) (milliseconds / timeScale));
        }
        
        // RunTask(TaskUtil.RefreshToken(ref _cancellationTokenSource));
        public static CancellationToken CancelToken(ref CancellationTokenSource tokenSource) {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

    }
}