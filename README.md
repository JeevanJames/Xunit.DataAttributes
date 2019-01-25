# xUnit custom data attributes

[![Build status](https://img.shields.io/appveyor/ci/JeevanJames/xunit-dataattributes.svg)](https://ci.appveyor.com/project/JeevanJames/xunit-dataattributes/branch/master) [![Test status](https://img.shields.io/appveyor/tests/JeevanJames/xunit-dataattributes.svg)](https://ci.appveyor.com/project/JeevanJames/xunit-dataattributes/branch/master) [![codecov](https://codecov.io/gh/JeevanJames/Xunit.DataAttributes/branch/master/graph/badge.svg)](https://codecov.io/gh/JeevanJames/Xunit.DataAttributes) [![NuGet Version](http://img.shields.io/nuget/v/Xunit.DataAttributes.svg?style=flat)](https://www.nuget.org/packages/Xunit.DataAttributes/) [![NuGet Downloads](https://img.shields.io/nuget/dt/Xunit.DataAttributes.svg)](https://www.nuget.org/packages/Xunit.DataAttributes/)

Custom data attributes for xUnit, including attributes that provide various types of data from embedded resource and files.

## Text content data attributes
Content data attributes provide various types of data from different text sources. Currently, two sources are supported - embedded resources in .NET assemblies and files.

|Type of data|Source: Assembly embedded resource|Source: File on file system|
|------------|----------------------------------|---------------------------|
|Whole text content as the data.|[`EmbeddedResourceContentAttribute`](docs/content-attribute.md)|[`FileContentAttribute`](docs/content-attribute.md)|
|Lines from the text content as the data.|[`EmbeddedResourceLinesAttribute`](docs/lines-attribute.md)|[`FileLinesAttribute`](docs/lines-attribute.md)|
|Text content as JSON, where the whole content is deserialized to a .NET type.|`EmbeddedResourceAsJsonAttribute`|`FileAsJsonAttribute`|
|Text content as JSON, where JSON arrays are deconstructed and each item is a data item.|`EmbeddedResourceAsJsonDeconstructedAttribute`|`FileAsJsonDeconstructedAttribute`|
