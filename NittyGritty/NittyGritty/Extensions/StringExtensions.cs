﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NittyGritty.Extensions
{
    public static class StringExtensions
    {

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrWhitespace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsBetweenLength(this string value, int minLength, int maxLength)
        {
            if(minLength < 0 || maxLength < 0)
            {
                throw new ArgumentException("String length should not be less than 0");
            }

            if(minLength > maxLength)
            {
                throw new ArgumentException("Minimum length should be less than Maximum length");
            }

            if(value.IsNotNullOrEmpty())
            {
                return minLength == 0 ? true : false;
            }
            else
            {
                return value.Length >= minLength && value.Length <= maxLength ? true : false;
            }
        }

        public static bool IsExactLength(this string value, int exactLength)
        {
            return value.IsBetweenLength(exactLength, exactLength);
        }

        public static bool IsMinLength(this string value, int minLength)
        {
            return value.IsBetweenLength(minLength, int.MaxValue);
        }

        public static bool IsMaxLength(this string value, int maxLength)
        {
            return value.IsBetweenLength(0, maxLength);
        }

        public static bool IsRegex(this string value, string regex)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            return new Regex(regex, RegexOptions.IgnoreCase).IsMatch(value);
        }

        /// <summary>
        /// Returns true if <paramref name="path"/> starts with the path <paramref name="basePath"/>.
        /// The comparison is case-insensitive, handles / and \ slashes as folder separators and
        /// only matches if the base dir folder name is matched exactly ("c:\foobar\file.txt" is not a sub path of "c:\foo").
        /// </summary>
        public static bool IsSubPathOf(this string path, string basePath)
        {
            string normalizedPath = path.NormalizeDirectory();
            string normalizedBasePath = basePath.NormalizeDirectory();

            return normalizedPath.StartsWith(normalizedBasePath, StringComparison.OrdinalIgnoreCase);
        }

        public static string NormalizeDirectory(this string path)
        {
            return Path.GetFullPath(path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                .WithEnding(Path.DirectorySeparatorChar.ToString()));
        }

        /// <summary>
        /// Returns <paramref name="str"/> with the minimal concatenation of <paramref name="ending"/> (starting from end) that
        /// results in satisfying .EndsWith(ending).
        /// </summary>
        /// <example>"hel".WithEnding("llo") returns "hello", which is the result of "hel" + "lo".</example>
        public static string WithEnding(this string str, string ending)
        {
            if (str == null)
                return ending;

            string result = str;

            // Right() is 1-indexed, so include these cases
            // * Append no characters
            // * Append up to N characters, where N is ending length
            for (int i = 0; i <= ending.Length; i++)
            {
                string tmp = result + ending.Right(i);
                if (tmp.EndsWith(ending))
                    return tmp;
            }

            return result;
        }

        /// <summary>Gets the rightmost <paramref name="length" /> characters from a string.</summary>
        /// <param name="value">The string to retrieve the substring from.</param>
        /// <param name="length">The number of characters to retrieve.</param>
        /// <returns>The substring.</returns>
        public static string Right(this string value, int length)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "Length is less than zero");
            }

            return (length < value.Length) ? value.Substring(value.Length - length) : value;
        }
    }
}
