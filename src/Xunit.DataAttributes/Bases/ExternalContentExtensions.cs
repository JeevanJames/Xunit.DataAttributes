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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

namespace Xunit.DataAttributes.Bases
{
    internal static class ExternalContentExtensions
    {
        internal static string ReadResource(this Assembly assembly, string resourceName, Encoding encoding = null, bool detectEncoding = false)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream, encoding ?? Encoding.UTF8, detectEncoding))
            {
                return reader.ReadToEnd();
            }
        }

        internal static IEnumerable<object[]> GetContent(this ExternalContentDataAttribute attribute,
            IReadOnlyList<(string content, Type type)> contents, ContentType contentType)
        {
            foreach (var (content, _) in contents)
            {
                if (contentType == ContentType.Stream)
                    yield return new object[] { new MemoryStream(attribute.Encoding.GetBytes(content)) };
                else if (contentType == ContentType.TextReader)
                    yield return new object[] { new StringReader(content) };
                else
                    yield return new object[] { content };
            }
        }

        internal static IEnumerable<object[]> GetLines(this ExternalContentDataAttribute attribute,
            (string content, Type type) content)
        {
            string[] lines = Regex.Split(content.content, @"\r\n|\r|\n");
            return lines.Select(line => new object[] {line});
        }

        internal static IEnumerable<object[]> GetAsJson(this ExternalContentDataAttribute attribute,
            IReadOnlyList<(string content, Type type)> contents)
        {
            var result = new object[contents.Count];
            for (int i = 0; i < contents.Count; i++)
            {
                var (content, type) = contents[i];
                JToken allData = JToken.Parse(content);
                result[i] = allData.ToObject(type);
            }
            yield return result;
        }

        internal static IEnumerable<object[]> GetAsJsonDeconstructedArray(this ExternalContentDataAttribute attribute,
            (string content, Type type) content)
        {
            var allData = JToken.Parse(content.content);
            if (allData is JObject obj)
                yield return new object[] { obj.ToObject(content.type) };
            else if (allData is JArray arr)
            {
                foreach (JToken item in arr)
                    yield return new object[] { item.ToObject(content.type) };
            }
        }
    }
}