using System;
using System.Collections.Generic;
using System.Text;

namespace CC_A1
{
    class CodeMapper
    {
       
            [JsonProperty(PropertyName = "function")]
            public string function { get; set; }
            [JsonProperty(PropertyName = "return-type")]
        public string returntype { get; set; }
        [JsonProperty(PropertyName = "parameters")]
            public IEnumerable<Parameters> parameters { get; set; }
        [JsonProperty(PropertyName = "conditions")]
        public IEnumerable<Conditions> conditions { get; set; }

        
    }
}
