using System;
using System.Text;

namespace JSONMapper.Elements
{
    public class StringElement : Element
    {
        private string value = null;

        public StringElement()
            : base(JsonType.String)
        { }

        public string Value {  get { return value; } }

        public override void PopulateElement(object model)
        {
            value = model == null ? null : model.ToString();
        }

        public override void PopulateElement(JStream stream)
        {
            if (stream.Char == '"')
            {
                stream.Next();
            }
            else
            {
                throw new Exception(string.Format("String cannot start with '{0}' character", stream.Char));
            }

            var builder = new StringBuilder();
            while(stream.Char != '"')
            {
                if(stream.Char == '\\')
                {
                    builder.Append(stream.Char);
                    stream.Next();
                }
                
                builder.Append(stream.Char);
                stream.Next();
            }

            value = builder.ToString();
            stream.Next();
        }

        public override string ToJSON()
        {
            return value == null ? Constants.Null : string.Format(@"""{0}""", value);
        }

        public override object ToObject(Type type)
        {
            if (Array.IndexOf(Constants.StringTypes, type) < 0)
            {
                throw new Exception(string.Format("Cannot convert from string to type [{0}]", type));
            }

            return value;
        }
    }
}
