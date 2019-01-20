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
        private readonly bool _useAsRegex;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Encoding _encoding = Encoding.UTF8;

        public EmbeddedResourceDataAttribute(params string[] resourceNames)
        {
            if (resourceNames == null)
                throw new ArgumentNullException(nameof(resourceNames));
            if (!resourceNames.Any())
                throw new ArgumentException("Specify at least one valid name.", nameof(resourceNames));
            if (resourceNames.Any(name => string.IsNullOrWhiteSpace(name)))
                throw new ArgumentException("Resource names cannot be null or empty.", nameof(resourceNames));

            _resourceNames = resourceNames.ToList();
        }

        public EmbeddedResourceDataAttribute(string resourceName, bool useAsRegex = false)
        {
            if (resourceName == null)
                throw new ArgumentNullException(nameof(resourceName));
            if (resourceName.Trim().Length == 0)
                throw new ArgumentException("Specify a valid resource name.", nameof(resourceName));

            _resourceNames = new List<string> {resourceName};
            _useAsRegex = useAsRegex;
        }

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
            get => _encoding ?? Encoding.UTF8;
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
            Encoding encoding = Encoding;

            var resourceLists = new List<List<string>>();
            if (_useAsRegex)
            {
                IEnumerable<string> matchingResources = GetMatchingResourceNames(Assembly, _resourceNames[0]);
                resourceLists.AddRange(matchingResources.Select(r => new List<string>{r}).ToList());
            }
            else
                resourceLists.Add(_resourceNames.ToList());

            IReadOnlyList<Type> parameterTypes = testMethod.GetParameters().Select(p => p.ParameterType).ToList();

            foreach (List<string> resourceList in resourceLists)
            {
                List<string> resourcesContent = resourceList.Select(rname =>
                {
                    using (Stream stream = assembly.GetManifestResourceStream(rname))
                    using (var reader = new StreamReader(stream, encoding, DetectEncoding))
                    {
                        return reader.ReadToEnd();
                    }
                }).ToList();
                
                if (resourcesContent.Count != parameterTypes.Count)
                    throw new Exception("Mismatched number of data vs parameters.");

                var resources = resourcesContent.Zip(parameterTypes, (res, type) => (res, type)).ToList();
                IEnumerable<object[]> data = GetData(resources);
                foreach (object[] items in data)
                    yield return items;
            }
        }

        protected abstract IEnumerable<object[]> GetData(IReadOnlyList<(string content, Type type)> resources);

        protected Stream ToStream(string str)
        {
            byte[] encoded = Encoding.GetBytes(str);
            return new MemoryStream(encoded);
        }

        protected TextReader ToReader(string str)
        {
            return new StringReader(str);
        }

        private IEnumerable<string> GetMatchingResourceNames(Assembly assembly, string resourceName)
        {
            var regex = new Regex(resourceName);
            return assembly.GetManifestResourceNames()
                .Where(name => regex.IsMatch(name));
        }
    }
}