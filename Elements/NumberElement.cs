using System;
using System.Globalization;
using System.Text;

namespace JSONMapper.Elements
{
    public class NumberElement : Element
    {
        private static readonly char[] AvailableChars = new char[]
            {'-', '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', 'e', 'E' };

        private double value;

        public NumberElement()
            : base(JsonType.Number)
        { }

        public override void PopulateElement(object model)
        {
            value = Convert.ToDouble(model);
        }

        public override void PopulateElement(JStream stream)
        {
            var builder = new StringBuilder();
            while (Array.IndexOf(AvailableChars, stream.Char) >= 0)
            {
                builder.Append(stream.Char);
                stream.Next();
            }

            value = double.Parse(builder.ToString(), CultureInfo.InvariantCulture);
        }

        public override string ToJSON()
        {
            return string.Format(CultureInfo.InvariantCulture,"{0}", value);
        }

        public override object ToObject(Type type)
        {
            if(Array.IndexOf(Constants.NumberTypes, type) < 0)
            {
                throw new Exception(string.Format("Cannot convert number to type [{0}]", type));
            }

            return Convert.ChangeType(value, type);
        }
    }
}
