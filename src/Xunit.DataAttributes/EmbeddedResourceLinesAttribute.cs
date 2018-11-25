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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Xunit.DataAttributes.Bases;

namespace Xunit.DataAttributes
{
    /// <summary>
    ///     Provides a data source for a data theory, with the data coming from one or more assembly
    ///     embedded resources, where each data item is a single line in the embedded resource content.
    /// </summary>
    public sealed class EmbeddedResourceLinesAttribute : EmbeddedResourceDataAttribute
    {
        private readonly Func<string, object> _lineConverter;

        public EmbeddedResourceLinesAttribute(Func<string, object> lineConverter, params string[] resourceNames)
            : base(resourceNames)
        {
            if (lineConverter == null)
                throw new ArgumentNullException(nameof(lineConverter));
            _lineConverter = lineConverter;
        }

        public EmbeddedResourceLinesAttribute(params string[] resourceNames) : base(resourceNames)
        {
        }

        protected override IEnumerable<object[]> GetData(string resourceContent, MethodInfo testMethod)
        {
            string[] lines = Regex.Split(resourceContent, @"\r\n|\r|\n");
            IEnumerable<object> data = _lineConverter != null ? lines.Select(_lineConverter) : lines;
            return data.Select(line => new object[] { line });
        }
    }
}