﻿#region --- License & Copyright Notice ---
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

using Shouldly;

namespace Xunit.DataAttributes.Tests
{
    public sealed class EmbeddedResourceLinesTests
    {
        [Theory]
        [EmbeddedResourceLines(Resources.TextLines)]
        public void Can_deserialize_lines_from_text(string line)
        {
            line.ShouldNotBeNull();
            line.ShouldEndWith("day");
        }
    }
}