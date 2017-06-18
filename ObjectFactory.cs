using JSONMapper.Elements;
using System;
using System.Collections;

namespace JSONMapper
{
    public class ObjectFactory
    {
        public static Element GetElementFromObject(object model)
        {
            Element element = null;

            if (model == null)
            {
                element = new NullElement();
            }
            else
            {
                var type = model.GetType();

                if (Array.IndexOf(Constants.NumberTypes, type) >= 0)
                {
                    element = new NumberElement();
                }
                else if (Array.IndexOf(Constants.StringTypes, type) >= 0)
                {
                    element = new StringElement();
                }
                else if (Array.IndexOf(Constants.BooleanTypes, type) >= 0)
                {
                    element = new BooleanElement();
                }
                else if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    element = new ArrayElement();
                }
                else
                {
                    element = new ObjectElement();
                }

                element.PopulateElement(model);
            }

            return element;
        }

    }
}
