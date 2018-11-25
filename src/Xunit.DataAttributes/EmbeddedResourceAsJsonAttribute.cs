#region --- License & Copyright Notice ---
/*
IniFile Library for .NET
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

        protected override IEnumerable<object[]> GetData(string resourceContent, MethodInfo testMethod)
        {
            ParameterInfo[] testMethodParams = testMethod.GetParameters();
            if (testMethodParams.Length != 1)
                throw new InvalidOperationException($"The test method {testMethod.Name} should have only a single parameter.");

            Type dataType = testMethodParams[0].ParameterType;

            var allData = JToken.Parse(resourceContent);
            if (allData is JArray arr)
            {
                Type listType = typeof(List<>).MakeGenericType(dataType);
                var data = (IList)arr.ToObject(listType);
                foreach (object dataItem in data)
                    yield return new object[] { dataItem };
            }
            else if (allData is JObject obj)
            {
                object data = obj.ToObject(dataType);
                yield return new object[] { data };
            }
        }
    }
}