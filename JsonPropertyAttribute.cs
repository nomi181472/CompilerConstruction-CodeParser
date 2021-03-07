using System;

namespace CC_A1
{
    internal class JsonPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}