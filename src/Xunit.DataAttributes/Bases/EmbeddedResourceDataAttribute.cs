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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Xunit.Sdk;

namespace Xunit.DataAttributes.Bases
{
    /// <summary>
    ///     Base class for xUnit data attributes that extract data from one or more embedded
    ///     resources in assemblies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class EmbeddedResourceDataAttribute : DataAttribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IReadOnlyList<string> _resourceNames;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Encoding _encoding = Encoding.UTF8;

        public EmbeddedResourceDataAttribute(params string[] resourceNames)
        {
            if (resourceNames == null)
                throw new ArgumentNullException(nameof(resourceNames));
            _resourceNames = resourceNames.Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
            if (_resourceNames.Count == 0)
                throw new ArgumentException("Specify at least one valid name.", nameof(resourceNames));
        }

        /// <summary>
        ///     Indicates whether the specified resource names are regular expressions.
        /// </summary>
        public bool UseAsRegex { get; set; }

        /// <summary>
        ///     The assembly to load the resources from. If not specified, this defaults to the
        ///     currently executing assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        ///     Gets or sets the character encoding to use when loading the resource data.
        /// </summary>
        public Encoding Encoding
        {
            get => _encoding;
            set => _encoding = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     Gets or sets whether to automatically detect the character encoding when loading
        ///     the resource data.
        /// </summary>
        public bool DetectEncoding { get; set; } = false;

        public sealed override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            Assembly assembly = Assembly ?? testMethod.DeclaringType.Assembly;
            Encoding encoding = Encoding ?? Encoding.UTF8;

            var matchingResources = new HashSet<string>(StringComparer.Ordinal);

            // Figure out all the resources to load. Avoid duplicate resource names.
            foreach (string resourceName in _resourceNames)
            {
                IEnumerable<string> resolvedResources = GetResourceNames(assembly, resourceName);
                foreach (string resolvedResource in resolvedResources)
                    matchingResources.Add(resolvedResource);
            }

            foreach (string matchingResource in matchingResources)
            {
                using (Stream stream = assembly.GetManifestResourceStream(matchingResource))
                using (var reader = new StreamReader(stream, encoding, DetectEncoding))
                {
                    string content = reader.ReadToEnd();
                    IEnumerable<object[]> data = GetData(content, testMethod);
                    foreach (object[] dataItem in data)
                        yield return dataItem;
                }
            }
        }

        /// <summary>
        ///     Given the resource content, deriving classes should override this method to extract
        ///     the required data and return as an <c>IEnumerable&lt;object[]&gt;</c>
        /// </summary>
        /// <param name="resourceContent">The content of the resource, as a string.</param>
        /// <param name="testMethod">The test method.</param>
        /// <returns>A collection of extracted data.</returns>
        protected abstract IEnumerable<object[]> GetData(string resourceContent, MethodInfo testMethod);

        private IEnumerable<string> GetResourceNames(Assembly assembly, string resourceName)
        {
            if (!UseAsRegex)
                return new[] { resourceName };

            var regex = new Regex(resourceName);
            return assembly.GetManifestResourceNames()
                .Where(name => regex.IsMatch(name));
        }
    }
}