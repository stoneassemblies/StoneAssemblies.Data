string NuGetVersionV2 = "";
string SolutionFileName = "src/StoneAssemblies.Data.sln";

string[] DockerFiles = System.Array.Empty<string>();

string[] OutputImages = System.Array.Empty<string>();

string[] ComponentProjects  = new [] {
	"./src/StoneAssemblies.Data/StoneAssemblies.Data.csproj",
};

string TestProject = "src/StoneAssemblies.Data.Tests/StoneAssemblies.Data.Tests.csproj";

string SonarProjectKey = "StoneAssemblies.Data";
string SonarOrganization = "stoneassemblies";