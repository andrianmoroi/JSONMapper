using System;

namespace JSONMapper.Elements
{
    public class BooleanElement : Element
    {
        private const string FalseValue = "false";
        private const string TrueValue = "true";

        private bool value;

        public BooleanElement() 
            : base(JsonType.Boolean)
        { }

        public override void PopulateElement(object model)
        {
            value = (bool)model;
        }

        public override void PopulateElement(JStream stream)
        {
            var posibleValueStr = stream.Char == TrueValue[0] ? TrueValue : FalseValue;

            if (stream.GetSubString(posibleValueStr.Length) == posibleValueStr)
            {
                value = posibleValueStr == TrueValue;
                stream.NextTimes(posibleValueStr.Length);
            }
            else
            {
                throw new Exception(string.Format("Cannot parse boolean value ({0})", stream.GetSubString(20)));
            }
        }

        public override string ToJSON()
        {
            return value ? "true" : "false";
        }

        public override object ToObject(Type type)
        {
            if(Array.IndexOf(Constants.BooleanTypes, type) < 0)
            {
                throw new Exception(string.Format("Cannot convert bool to type [{0}]", type));
            }

            return value;
        }
    }
}
