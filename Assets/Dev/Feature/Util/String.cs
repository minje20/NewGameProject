using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StringExtension
{
    public static string FormatWithPlaceholder(this string str, params (string, object)[] strs)
    {
        StringBuilder builder = new StringBuilder(str);
        
        foreach (var s in strs)
        {
            builder.Replace("{" + s.Item1 + "}", s.Item2.ToString());
        }

        return builder.ToString();
    }
}