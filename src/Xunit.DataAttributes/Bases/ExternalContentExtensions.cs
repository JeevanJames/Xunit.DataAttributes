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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xunit.DataAttributes.Bases
{
    internal static class ExternalContentExtensions
    {
        internal static IEnumerable<object[]> GetContent(this ExternalContentDataAttribute attribute,
            IReadOnlyList<(string content, Type type)> contents, ContentType contentType)
        {
            foreach (var (content, _) in contents)
            {
                object data;
                if (contentType == ContentType.Stream)
                    data = new MemoryStream(attribute.Encoding.GetBytes(content));
                else if (contentType == ContentType.TextReader)
                    data = new StringReader(content);
                else
                    data = content;
                yield return new object[] {data};
            }
        }

        internal static IEnumerable<object[]> GetLines(this ExternalContentDataAttribute attribute,
            (string content, Type type) content)
        {
            string[] lines = Regex.Split(content.content, @"\r\n|\r|\n");
            return lines.Select(line => new object[] {line});
        }
    }
}