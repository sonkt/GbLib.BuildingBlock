using Spectre.Console;
using ServiceScaffolder.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // CLI yêu cầu tên service từ người dùng
        var serviceName = AnsiConsole.Ask<string>("Nhập [green]tên Service[/]:");

        // Tạo Project mới dựa vào template
        var generator = new ProjectGenerator();
        await generator.GenerateServiceAsync(serviceName);
    }
}