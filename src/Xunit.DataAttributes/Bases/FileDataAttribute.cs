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

namespace Xunit.DataAttributes.Bases
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class FileDataAttribute : ExternalContentDataAttribute
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IReadOnlyList<string> _fileNames;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DirectoryInfo _directory;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _fileMask;

        public FileDataAttribute(params string[] fileNames)
        {
            if (fileNames == null)
                throw new ArgumentNullException(nameof(fileNames));

            _fileNames = fileNames.ToList();

            if (_fileNames.Count == 0)
                throw new ArgumentException("Specify at least one valid file name.", nameof(fileNames));

            string nonexistentFile = _fileNames.FirstOrDefault(fn => !File.Exists(Path.GetFullPath(fn)));
            if (nonexistentFile != null)
                throw new FileNotFoundException($"File {nonexistentFile} not found.", nonexistentFile);
        }

        public FileDataAttribute(string directory, string fileMask)
        {
            _directory = new DirectoryInfo(directory);
            _fileMask = fileMask;
        }

        public sealed override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            Encoding encoding = Encoding;

            var fileLists = new List<List<string>>();
            if (_directory != null)
            {
                IEnumerable<FileInfo> files = _directory.EnumerateFiles(_fileMask);
                fileLists.AddRange(files.Select(f => new List<string> {f.FullName}).ToList());
            }
            else
                fileLists.Add(_fileNames.ToList());

            IReadOnlyList<Type> parameterTypes = testMethod.GetParameters().Select(p => p.ParameterType).ToList();

            foreach (List<string> fileList in fileLists)
            {
                List<string> fileContents = fileList.Select(fname => File.ReadAllText(fname, encoding)).ToList();

                if (fileContents.Count != parameterTypes.Count)
                    throw new Exception("Mismatched number of data vs parameters.");

                var resources = fileContents.Zip(parameterTypes, (fn, type) => (fn, type)).ToList();
                IEnumerable<object[]> data = GetData(resources);
                foreach (object[] items in data)
                    yield return items;
            }
        }
    }
}