using JSONMapper.Elements;
using System;

namespace JSONMapper
{
    public static class StringFactory
    {
        private static readonly char[] NumberFirstCharacters = new char[]
            { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        
        public static Element GetJElementFromJSON(JStream stream)
        {
            var result = GetNewElementForCharacter(stream.Char);

            result.PopulateElement(stream);
            
            return result;
        }
        
        private static Element GetNewElementForCharacter(char c)
        {
            if (c == 'n') return new NullElement();
            if (c == '"') return new StringElement();
            if (c == 't' || c == 'f') return new BooleanElement();
            if (Array.IndexOf(NumberFirstCharacters, c) >= 0) return new NumberElement();
            
            if (c == '[') return new ArrayElement();
            if (c == '{') return new ObjectElement();


            throw new ArgumentException(string.Format("Unknown to deserialize cahracter '{0}'", c));
        }

    }
}
