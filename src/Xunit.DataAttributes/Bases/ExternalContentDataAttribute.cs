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
using System.Text;

using Xunit.Sdk;

namespace Xunit.DataAttributes.Bases
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class ExternalContentDataAttribute : DataAttribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Encoding _encoding = Encoding.UTF8;

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

        protected abstract IEnumerable<object[]> GetData(IReadOnlyList<(string content, Type type)> contents);
    }
}