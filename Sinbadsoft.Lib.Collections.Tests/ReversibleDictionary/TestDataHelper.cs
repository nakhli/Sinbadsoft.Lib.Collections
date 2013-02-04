// <copyright file="TestDataHelper.cs" company="Sinbadsoft">
//        Copyright (c) Sinbadsoft 2009-2013.
//        This file is released under the terms of the
//        GNU Library General Public License (LGPL) version 2.1
//        Please refer to the "License.txt" accompanying this file.
// </copyright>
// <author>Chaker NAKHLI</author>
// <email>Chaker.Nakhli@Sinbadsoft.com</email>
// <date>2009/04/17</date>

namespace Sinbadsoft.Lib.Collections.Tests.ReversibleDictionary
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Helper class for <see cref="IReversibleDictionary{TKey,TValue}"/> test data generation.
    /// </summary>
    internal class TestDataHelper
    {
        public static KeyValuePair<int, int>[] Int2Int(int lower, int upper)
        {
            var result = new KeyValuePair<int, int>[upper - lower];
            for (int i = lower; i < upper; ++i)
            {
                result[i - lower] = new KeyValuePair<int, int>(i, upper + lower - i - 1);
            }

            return result;
        }

        public static KeyValuePair<string, string>[] String2String(int lower, int upper)
        {
            var result = new KeyValuePair<string, string>[upper - lower];

            for (int i = lower; i < upper; ++i)
            {
                string key = string.Format(CultureInfo.InvariantCulture, "Key {0}", i);
                string val = string.Format(CultureInfo.InvariantCulture, "Val {0}", i);

                result[i - lower] = new KeyValuePair<string, string>(key, val);
            }

            return result;
        }

        public static KeyValuePair<int, string>[] Int2String(int lower, int upper)
        {
            var result = new KeyValuePair<int, string>[upper - lower];

            for (int i = lower; i < upper; ++i)
            {
                var val = string.Format(CultureInfo.InvariantCulture, "Val {0}", i);
                result[i - lower] = new KeyValuePair<int, string>(i, val);
            }

            return result;
        }
    }
}
