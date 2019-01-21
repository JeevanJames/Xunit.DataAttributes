﻿#region --- License & Copyright Notice ---
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
using System.Collections.Generic;

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

        public EmbeddedResourceAsJsonAttribute(string resourceName, bool useAsRegex = false) : base(resourceName, useAsRegex)
        {
        }

        /// <inheritdoc/>
        protected override IEnumerable<object[]> GetData(IReadOnlyList<(string content, Type type)> resources)
        {
            var result = new object[resources.Count];
            for (int i = 0; i < resources.Count; i++)
            {
                var (content, type) = resources[i];
                JToken allData = JToken.Parse(content);
                result[i] = allData.ToObject(type);
            }
            yield return result;
        }
    }
}