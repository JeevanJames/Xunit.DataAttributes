# Lines attributes
Lines attributes return each line from the text content as the data.

**Classes**
* `EmbeddedResourceLinesAttribute`
* `FileLinesAttribute`

**Capabilities**
- [x] Allows multiple instances on single text method
- [ ] Allows multiple parameters on test method

## Samples

```cs
// Accepts a single file name.
// The test method is called for each line in the file content.
[Theory]
[FileLines("File1.txt")]
public void Single_file_lines_test(string line)
{
}

// You can also specify multiple files, one per attribute usage.
// The test method is called for each line in both files combined.
[Theory]
[FileLines("File1.txt")]
[FileLines("File2.txt")]
public void Multiple_files_test(string line)
{
}
```
