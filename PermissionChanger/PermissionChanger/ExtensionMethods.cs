using System;

namespace PermissionChanger
{
    public static class ExtensionMethods
    {
        public static string Truncate(this string item, int length = 40, bool fromLeft = true, bool fillWithThreeDots = true)
        {
            if (fillWithThreeDots) length -= 3;
            if (string.IsNullOrEmpty(item) || item.Length <= length) return item;

            string returnString = (fillWithThreeDots) ? "..." : "";

            if (fromLeft)
            {
                returnString += item.Substring(item.Length - length);
            }
            else
            {
                returnString = item.Remove(length) + returnString;
            }

            return returnString;
        }
    }
}
