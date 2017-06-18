using System;

namespace JSONMapper.Elements
{
    public class NullElement : Element
    {
        public NullElement()
            : base(JsonType.Null)
        { }

        public override void PopulateElement(object model)
        {
            //nothing to do
        }

        public override void PopulateElement(JStream stream)
        {
            if (stream.GetSubString(Constants.Null.Length) == Constants.Null)
            {
                stream.NextTimes(Constants.Null.Length);
            }
            else
            {
                throw new ArgumentException("Cannot parse null value");
            }
        }

        public override string ToJSON()
        {
            return Constants.Null;
        }

        public override object ToObject(Type type)
        {
            return null;
        }
    }
}
