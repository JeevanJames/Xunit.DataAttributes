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

using Shouldly;

namespace Xunit.DataAttributes.Tests
{
    public sealed class EmbeddedResourceAsJsonDeconstructedArrayTests
    {
        [Theory]
        [ResourceAsJsonDeconstructedArray(Resources.JsonCollection)]
        public void Can_derialize_json_array(TelevisionShow show)
        {
            show.ShouldNotBeNull();
        }

        [Theory]
        [ResourceAsJsonDeconstructedArray(Resources.JsonObject)]
        public void Can_derialize_json_object(TelevisionShow show)
        {
            show.ShouldNotBeNull();
            show.Name.ShouldBe("Daredevil");
            show.Seasons.ShouldBe(3);
            show.ForKids.ShouldBeFalse();
            show.Network.ShouldBe("Netflix");
        }

        [Theory]
        [ResourceAsJsonDeconstructedArray(Resources.JsonCollection)]
        [ResourceAsJsonDeconstructedArray(Resources.JsonObject)]
        public void Can_derialize_json_object_and_array_combo(TelevisionShow show)
        {
            show.ShouldNotBeNull();
        }

        [Theory]
        [ResourceAsJsonDeconstructedArray(Resources.JsonPrimitiveCollection)]
        public void Can_derialize_json_primitive_collection(int num)
        {
            num.ShouldBeGreaterThanOrEqualTo(1);
            num.ShouldBeLessThanOrEqualTo(7);
        }
    }
}