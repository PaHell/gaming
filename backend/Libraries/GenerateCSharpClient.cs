using backend.Extensions;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace backend.Libraries
{
    public static class GenerateCSharpClient
    {
        public static async Task CreateAsync(string path, string className)
        {
            var root = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(root, $"{path}/swagger.json");
            var document = await OpenApiDocument.FromFileAsync(fullPath);
            var settings = new CSharpClientGeneratorSettings
            {
                UseBaseUrl = true,
                ClassName = className,
                GenerateClientInterfaces = true,
                GenerateClientClasses = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "backend." + path.PathToNamespace(),
                },
            };

            var generator = new CSharpClientGenerator(document, settings);
            var generatedCode = generator.GenerateFile();

            var file = new FileInfo(path + "/Clients.cs");
            await File.WriteAllTextAsync(file.FullName, generatedCode);
        }
    }
}
