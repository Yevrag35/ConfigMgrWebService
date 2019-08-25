using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;

namespace MG.Sccm.Api
{
    public interface IOutputToJson
    {
        string ToJson();
        string ToJson(IDictionary parameters);
    }
}
