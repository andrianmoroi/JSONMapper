using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JSONMapper.Elements
{
    public class ArrayElement : Element
    {
        private List<Element> value;

        public ArrayElement()
            : base(JsonType.Array)
        {
            value = new List<Element>();
        }

        public override void PopulateElement(object model)
        {
            value.Clear();

            var enumerable = model as IEnumerable;

            if (enumerable != null)
            {
                foreach (var item in enumerable)
                {
                    value.Add(ObjectFactory.GetElementFromObject(item));
                }
            }
        }

        public override void PopulateElement(JStream stream)
        {
            if (stream.Char == '[')
            {
                stream.Next();
            }
            else
            {
                throw new Exception(string.Format("Cannot parse Array first character '{0}'", stream.Char));
            }

            value.Clear();

            var first = true;

            stream.SkipSpaces();
            while (stream.Char != ']')
            {
                if (!first)
                {
                    if (stream.Char != ',') throw new Exception("Cannot parse array, no , (comma) found");

                    stream.Next();
                    stream.SkipSpaces();
                }

                var element = StringFactory.GetJElementFromJSON(stream);

                value.Add(element);

                stream.SkipSpaces();

                first = false;
            }

            stream.Next();
        }

        public override string ToJSON()
        {
            var result = new StringBuilder();

            result.Append("[");

            var first = true;
            foreach (var item in value)
            {
                if (!first)
                {
                    result.Append(",");
                }

                result.Append(item.ToJSON());

                first = false;
            }

            result.Append("]");

            return result.ToString();
        }

        public override object ToObject(Type type)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(type))
            {
                throw new Exception(string.Format("Cannot convert Array to type [{0}]", type));
            }

            return type.IsArray ? ToArray(type) : ToList(type);
        }

        private object ToArray(Type type)
        {
            var elementType = type.GetElementType();

            var result = Array.CreateInstance(elementType, value.Count);

            for (int i = 0; i < value.Count; i++)
            {
                var val = value[i].ToObject(elementType);

                result.SetValue(val, i);
            }

            return result;
        }

        private object ToList(Type type)
        {
            var genericTypes = type.GetGenericArguments();
            var elementType = genericTypes.Length > 0 ? genericTypes[0] : typeof(object);

            var listType = typeof(List<>).MakeGenericType(elementType);
            var result = Activator.CreateInstance(listType);

            for (int i = 0; i < value.Count; i++)
            {
                var val = value[i].ToObject(elementType);

                listType.GetMethod("Add").Invoke(result, new object[] { val });
            }

            return result;
        }
    }
}
