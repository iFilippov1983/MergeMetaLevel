using System;
using System.Threading.Tasks;

namespace Utils
{
    public static class CallExtensions
    {
        public static T Do<T>(this T self, Action<T> action)
        {
            action.Invoke(self);
            return self;
        }

        public static T Do<T>(this T self, Action<T> action, bool when)
        {
            if (when)
                action.Invoke(self);
            return self;
        }

        public static T Do<T>(this T self, Action<T> action, Func<bool> when)
        {
            if(when())
                action.Invoke(self);
            return self;
        }
        public static async Task Do<T>(this T self, Func<T, Task> action)
        {
            await action.Invoke(self);
        }

        public static async Task Do<T>(this T self, Func<T, Task> action, bool when)
        {
            if (when)
                await action.Invoke(self);
        }

    }
}