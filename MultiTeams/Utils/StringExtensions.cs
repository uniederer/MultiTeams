//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;

namespace MultiTeams.Utils
{
    public static class StringExtensions
    {
        public static string MakePathSafe(this string Path)
        {
            foreach (var c in System.IO.Path.GetInvalidPathChars())
            {
                Path = Path.Replace(c, '-');
            }
            Path = Path.Replace(":", "-");
            Path = Path.Replace("\\", "-");
            Path = Path.Replace("\"", "-");

            return Path;
        }
    }
}
