dotnet test --blame .\tests\Xunit.DataAttributes.Tests\Xunit.DataAttributes.Tests.csproj /p:CollectCoverage=true /p:Exclude=\"[xunit.*]*\" /p:CoverletOutputFormat=opencover
reportgenerator -reports:.\tests\Xunit.DataAttributes.Tests\coverage.opencover.xml -targetdir:.\coverage
