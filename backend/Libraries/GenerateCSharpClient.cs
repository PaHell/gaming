using backend.Extensions;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using System.Reflection;

namespace backend.Libraries
{
    public static class GenerateCSharpClient
    {
        public static async Task CreateAsync(string input, string output)
        {
            var root = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(root, input);
            var document = await OpenApiDocument.FromFileAsync(fullPath);
            var fileName = Path.GetFileNameWithoutExtension(output);
            var directory = Path.GetDirectoryName(output) ?? string.Empty;
            var settings = new CSharpClientGeneratorSettings
            {
                UseBaseUrl = true,
                ClientBaseClass = null,
                ClassName = fileName,
                GenerateClientInterfaces = true,
                GenerateClientClasses = true,
                DisposeHttpClient = false,
                InjectHttpClient = true,
                UseHttpClientCreationMethod = false,
                UseHttpRequestMessageCreationMethod = false,
                CSharpGeneratorSettings =
                {
                    Namespace = "backend." + directory.PathToNamespace(),
                    GenerateDefaultValues = true
                },
            };

            var generator = new CSharpClientGenerator(document, settings);
            var generatedCode = generator.GenerateFile();

            var file = new FileInfo(output);
            await File.WriteAllTextAsync(file.FullName, generatedCode);
        }
    }
}
