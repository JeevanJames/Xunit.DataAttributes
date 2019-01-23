# Content attributes
Content attributes return the entire text content as the data.

**Classes**
* `EmbeddedResourceContentAttribute`
* `FileContentAttribute`

**Capabilities**
- [x] Allows multiple instances on single test method
- [x] Allows multiple parameters on test method

## Samples
```cs
// Accepts a single resource name that maps to a single method parameter.
[Theory]
[EmbeddedResourceContent("Resource.Name.txt")]
public void Single_parameter_test(string content)
{
}

// Accepts multiple resource names, each of which map to a method parameter.
[Theory]
[EmbeddedResourceContent("Resource1.Name.txt", "Resource2.Name.txt")]
public void Multiple_parameter_test(string content1, string content2)
{
}

// Accepts multiple occurences of the attribute on a test method.
[Theory]
[FileContent("File1.txt", "File2.txt")]
[FileContent("File3.txt", "File4.txt")]
public void Multiple_instances_test(string content1, string content2)
{
}
```