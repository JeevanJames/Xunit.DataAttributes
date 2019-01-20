#region --- License & Copyright Notice ---
/*
xUnit custom data attributes
Copyright (c) 2018 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json.Linq;

using Xunit.DataAttributes.Bases;

namespace Xunit.DataAttributes
{
    /// <summary>
    ///     Provides a data source for a data theory, with the data coming from one or more assembly
    ///     embedded resources, where each resource is a JSON structure that can be deserialized
    ///     into the specified type.
    /// </summary>
    public sealed class EmbeddedResourceAsJsonAttribute : EmbeddedResourceDataAttribute
    {
        public EmbeddedResourceAsJsonAttribute(params string[] resourceNames) : base(resourceNames)
        {
        }

        protected override IEnumerable<object> GetData(IReadOnlyList<(string content, Type type)> resources)
        {
            foreach (var (content, type) in resources)
            {
                var allData = JToken.Parse(content);
                if (allData is JArray arr)
                {
                    Type listType = typeof(List<>).MakeGenericType(type);
                    var data = (IList)arr.ToObject(listType);
                    yield return data;
                }
                else if (allData is JObject obj)
                {
                    object data = obj.ToObject(type);
                    yield return data;
                }
            }
        }
    }
}