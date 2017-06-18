using System;

namespace JSONMapper
{
    public class Constants
    {
        public const string Null = "null";

        public static readonly Type[] NumberTypes = new Type[]
        { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
                typeof(long), typeof(ulong), typeof(double), typeof(decimal), typeof(float), typeof(object)};

        public static readonly Type[] StringTypes = new Type[] { typeof(string), typeof(object) };

        public static readonly Type[] BooleanTypes = new Type[] { typeof(bool), typeof(object) };
    }
}
