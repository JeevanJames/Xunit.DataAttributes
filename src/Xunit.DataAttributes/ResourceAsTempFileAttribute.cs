#region --- License & Copyright Notice ---
/*
xUnit custom data attributes
Copyright (c) 2018-19 Jeevan James
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
using System.IO;
using System.Reflection;
using Xunit.DataAttributes.Bases;
using Xunit.Sdk;

namespace Xunit.DataAttributes
{
    public sealed class ResourceAsTempFileAttribute : DataAttribute
    {
        public ResourceAsTempFileAttribute(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException(Errors.NullOrEmptyResource, nameof(resourceName));
            ResourceName = resourceName;
        }

        public string ResourceName { get; }

        public Assembly Assembly { get; set; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod is null)
                throw new ArgumentNullException(nameof(testMethod));

            Assembly assembly = Assembly ?? testMethod.DeclaringType.Assembly;
            string content = assembly.ReadResource(ResourceName);

            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, content);

            return new object[][]
            {
                new object[] { tempFile }
            };
        }
    }
}
