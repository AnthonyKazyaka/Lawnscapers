using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lawnscapers.Extensions
{
    public static class ExtensionMethods
    {
        public static List<string> Split(this string input, string splitter)
        {
            var array = new[] { splitter };
            return input.Split(array, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> Split(this string input, string splitter, StringSplitOptions option)
        {
            var array = new[] { splitter };
            return input.Split(array, option).ToList();
        }

        public static List<string> RemoveEmptyEntries(this IEnumerable<string> list)
        {
            return list.Where(x => !string.IsNullOrEmpty(x)).ToList();
        }
    }
}