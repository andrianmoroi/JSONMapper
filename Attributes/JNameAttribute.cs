using System;

namespace JSONMapper.Attributes
{
    public class JNameAttribute : Attribute
    {
        public JNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
