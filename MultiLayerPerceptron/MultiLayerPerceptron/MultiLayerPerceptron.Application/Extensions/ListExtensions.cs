using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiLayerPerceptron.Application.Extensions
{
    public static class ListExtensions
    {
        [ThreadStatic] private static Random _local;

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = ThreadsRandom.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static Random ThreadsRandom =>
            _local ??= new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
    }
}
