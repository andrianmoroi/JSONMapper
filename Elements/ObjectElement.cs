using JSONMapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONMapper.Elements
{
    public class ObjectElement : Element
    {
        private IDictionary<string, Element> value;

        public ObjectElement()
            : base(JsonType.Object)
        {
            value = new Dictionary<string, Element>();
        }

        public override void PopulateElement(object model)
        {
            var properties = model.GetType().GetProperties().Where(m => m.CanRead);
            value.Clear();

            foreach (var item in properties)
            {
                var ignored = item.GetCustomAttributes(typeof(JIgnoreAttribute), false).Any();

                if (!ignored)
                {
                    var nameAttrib = item.GetCustomAttributes(typeof(JNameAttribute), false).FirstOrDefault() as JNameAttribute;
                    var name = nameAttrib != null ? nameAttrib.Name : item.Name;
                    var element = ObjectFactory.GetElementFromObject(item.GetValue(model, null));

                    value.Add(name, element);
                }
            }
        }

        public override void PopulateElement(JStream stream)
        {
            if (stream.Char == '{')
            {
                stream.Next();
            }
            else
            {
                throw new Exception(string.Format("Cannot parse Object first character '{0}'", stream.Char));
            }

            value.Clear();

            stream.SkipSpaces();
            var first = true;
            while (stream.Char != '}')
            {
                if (!first)
                {
                    if (stream.Char != ',') throw new Exception("Cannot parse object, no ,(comma) found");

                    stream.Next();
                    stream.SkipSpaces();
                }

                value.Add(ParseKeyValuePair(stream));

                stream.SkipSpaces();

                first = false;
            }

            stream.Next();
        }

        public override string ToJSON()
        {
            var result = new StringBuilder();

            result.Append("{");

            var first = true;
            foreach (var item in value)
            {
                if (!first)
                {
                    result.Append(",");
                }

                result.AppendFormat(@"""{0}"":{1}", item.Key, item.Value.ToJSON());

                first = false;
            }

            result.Append("}");

            return result.ToString();
        }

        public override object ToObject(Type type)
        {
            var properties = type.GetProperties().Where(m => m.CanWrite);

            var result = Activator.CreateInstance(type);
            foreach (var item in properties)
            {
                var ignored = item.GetCustomAttributes(typeof(JIgnoreAttribute), false).Any();

                if (!ignored)
                {
                    var nameAttrib = item.GetCustomAttributes(typeof(JNameAttribute), false).FirstOrDefault() as JNameAttribute;
                    var elementName = nameAttrib != null ? nameAttrib.Name : item.Name;
                    var propertyValue = value[elementName].ToObject(item.PropertyType);

                    item.SetValue(result, propertyValue, null);
                }
            }

            return result;
        }

        private KeyValuePair<string, Element> ParseKeyValuePair(JStream stream)
        {
            //TODO: just temporary
            var stringKey = new StringElement();
            stringKey.PopulateElement(stream);

            stream.SkipSpaces();

            if (stream.Char != ':') throw new Exception("Cannot parse object, not found :");

            stream.Next();

            stream.SkipSpaces();

            var key = stringKey.Value;
            var element = StringFactory.GetJElementFromJSON(stream);

            return new KeyValuePair<string, Element>(key, element);
        }
    }
}
