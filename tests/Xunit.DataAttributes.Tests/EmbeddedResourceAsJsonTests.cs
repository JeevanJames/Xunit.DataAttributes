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

using System.Collections.Generic;

using Shouldly;

namespace Xunit.DataAttributes.Tests
{
    public sealed class EmbeddedResourceAsJsonTests
    {
        [Theory]
        [EmbeddedResourceAsJson(Jsons.Collection)]
        public void Can_deserialize_json_array(IList<TelevisionShow> shows)
        {
            shows.Count.ShouldBe(2);
        }

        [Theory]
        [EmbeddedResourceAsJson(Jsons.Object)]
        public void Can_derialize_json_object(TelevisionShow show)
        {
            show.ShouldNotBeNull();
            show.Name.ShouldBe("Daredevil");
            show.Seasons.ShouldBe(3);
            show.ForKids.ShouldBeFalse();
            show.Network.ShouldBe("Netflix");
        }

        [Theory]
        [EmbeddedResourceAsJson(Jsons.Collection)]
        [EmbeddedResourceAsJson(Jsons.Object)]
        public void Can_deserialize_json_collection_and_object(TelevisionShow show)
        {
            show.ShouldNotBeNull();
        }

        [Theory]
        [EmbeddedResourceAsJson(Jsons.PrimitiveCollection)]
        public void Can_deserialize_primitive_collection(int num)
        {
            num.ShouldBeGreaterThanOrEqualTo(1);
            num.ShouldBeLessThanOrEqualTo(7);
        }
    }
}