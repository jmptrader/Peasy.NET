﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peasy.Extensions
{
    public static class IntExtensions
    {
        public static void Times(this int value, Action action)
        {
            for (var counter = 0; counter < value; counter++)
            {
                action();
            }
        }

        public static void Times(this int value, Action<int> action)
        {
            for (var counter = 0; counter < value; counter++)
            {
                action(counter);
            }
        }

        public static IEnumerable<T> Times<T>(this int value, Func<int, T> func)
        {
            for (var counter = 0; counter < value; counter++)
            {
                yield return func(counter);
            }
        }
    }

}
