using System;

namespace JSONMapper
{
    public abstract class Element
    {
        protected Element(JsonType type)
        {
            Type = type;
        }

        public JsonType Type { get; private set; }

        public abstract void PopulateElement(JStream stream);

        public abstract void PopulateElement(object model);

        public abstract string ToJSON();

        public abstract object ToObject(Type type);

        protected T GetNewInstance<T>()
        {
            var type = typeof(T);
            return (T)Activator.CreateInstance(type);
        }
    }
}
