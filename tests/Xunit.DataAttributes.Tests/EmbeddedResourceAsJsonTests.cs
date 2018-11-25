using Shouldly;

namespace Xunit.DataAttributes.Tests
{
    public sealed class EmbeddedResourceAsJsonTests
    {
        [Theory]
        [EmbeddedResourceAsJson("Xunit.DataAttributes.Tests.Data.Collection.json")]
        public void Can_deserialize_json_array(TelevisionShow show)
        {
            show.ShouldNotBeNull();
        }

        [Theory]
        [EmbeddedResourceAsJson("Xunit.DataAttributes.Tests.Data.Object.json")]
        public void Can_derialize_json_object(TelevisionShow show)
        {
            show.ShouldNotBeNull();
            show.Name.ShouldBe("Daredevil");
            show.Seasons.ShouldBe(3);
            show.ForKids.ShouldBeFalse();
            show.Network.ShouldBe("Netflix");
        }

        [Theory]
        [EmbeddedResourceAsJson("Xunit.DataAttributes.Tests.Data.Collection.json")]
        [EmbeddedResourceAsJson("Xunit.DataAttributes.Tests.Data.Object.json")]
        public void Can_deserialize_json_collection_and_object(TelevisionShow show)
        {
            show.ShouldNotBeNull();
        }
    }
}