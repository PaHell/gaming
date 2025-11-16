using backend.Extensions;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace backend.Libraries
{
    public static class GenerateCSharpClient
    {
        public static async Task CreateAsync(string input, string output)
        {
            var root = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(root, input);
            var document = await OpenApiDocument.FromFileAsync(fullPath);
            var fileName = Path.GetFileNameWithoutExtension(input);
            var directory = Path.GetDirectoryName(output) ?? string.Empty;
            var settings = new CSharpClientGeneratorSettings
            {
                UseBaseUrl = true,
                ClassName = fileName,
                GenerateClientInterfaces = true,
                GenerateClientClasses = true,
                CSharpGeneratorSettings =
                {
                    Namespace = "backend." + directory.PathToNamespace(),
                },
            };

            var generator = new CSharpClientGenerator(document, settings);
            var generatedCode = generator.GenerateFile();

            var file = new FileInfo(output);
            await File.WriteAllTextAsync(file.FullName, generatedCode);
        }
    }
}
