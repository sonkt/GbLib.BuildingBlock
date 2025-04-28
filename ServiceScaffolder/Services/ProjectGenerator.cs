using System.Text;
using Spectre.Console;

namespace ServiceScaffolder.Services;

public class ProjectGenerator
{
    private const string TemplatesPath = "Templates/CleanArchitecture";

    public async Task GenerateServiceAsync(string serviceName)
    {
        var targetRoot = Path.Combine(Directory.GetCurrentDirectory(), serviceName);
        Directory.CreateDirectory(targetRoot);

        AnsiConsole.MarkupLine($"[yellow]Creating service: {serviceName}[/]");

        await CopyTemplatesRecursivelyAsync(TemplatesPath, targetRoot, serviceName);

        AnsiConsole.MarkupLine($"[bold green]✔ Service {serviceName} created successfully![/]");
    }

    private async Task CopyTemplatesRecursivelyAsync(string sourcePath, string destinationPath, string serviceName)
    {
        // Tạo thư mục
        foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
        }

        // Copy và thay thế {{ServiceName}} trong các file
        foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            var content = await File.ReadAllTextAsync(newPath, Encoding.UTF8);
            content = content.Replace("{{ServiceName}}", serviceName);

            var targetFile = newPath.Replace(sourcePath, destinationPath);
            await File.WriteAllTextAsync(targetFile, content, Encoding.UTF8);
        }
    }
}