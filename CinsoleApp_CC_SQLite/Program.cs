using Spectre.Console;
using System.Data.SQLite;
using CinsoleApp_CC_SQLite;

public static class Program
{
    public static void Main(string[] args)
    {
        DatabaseInitializer.CreateDatabase();
        
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Вас вітає консольна програма для роботи з SQLite оберіть [green]дію[/]:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Перегляд списку користувачів",
                        "Перегляд списку продуктів",
                        "Додавання нового користувача",
                        "Вихід із програми"
                    }));

            switch (option)
            {
                case "Перегляд списку користувачів":
                    
                    break;
                case "Перегляд списку продуктів":
                    
                    break;
                case "Додавання нового користувача":
                    
                    break;
                case "Вихід із програми":
                    AnsiConsole.MarkupLine("[red]До побачення![/]");
                    return;
            }

            AnsiConsole.MarkupLine("[grey]Натисніть будь-яку клавішу для продовження...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();
        }
    }
}