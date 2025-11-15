namespace backend.Extensions
{
    public static class BaseExtensions
    {
        public static string PathToNamespace(this string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            // Normalize slashes
            var normalized = path.Replace("\\", "/").Trim();

            // Remove file extension
            normalized = Path.ChangeExtension(normalized, null) ?? normalized;

            // Replace slashes with dots
            normalized = normalized.Replace("/", ".");

            // Remove invalid namespace chars from each segment
            var parts = normalized
                .Split('.', StringSplitOptions.RemoveEmptyEntries) // <-- fixes leading+trailing dots
                .Select(p =>
                {
                    var cleaned = new string(p.Where(char.IsLetterOrDigit).ToArray());

                    if (string.IsNullOrEmpty(cleaned))
                        return null;

                    return cleaned;
                })
                .Where(p => p != null);

            // Join with dots
            return string.Join(".", parts);
        }
    }
}
