using Spectre.Console;
using System.Data.SQLite;
using System.Text.RegularExpressions;
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
                    PrintUsers();
                    break;
                case "Перегляд списку продуктів":
                    AnsiConsole.MarkupLine("[red]Таблиця пуста.[/]");
                    break;
                case "Додавання нового користувача":
                    var firstName = AnsiConsole.Ask<string>("Введіть [green]ім'я[/]:");
                    var lastName = AnsiConsole.Ask<string>("Введіть [green]прізвище[/]:");
                    var email = AnsiConsole.Ask<string>("Введіть [green]email[/]:");

                    if (!IsValidEmail(email))
                    {
                        AnsiConsole.MarkupLine("[red]Невірний формат email. Спробуйте ще раз.[/]");
                        continue;
                    }

                    if (!addUserToDatabase(firstName, lastName, email))
                    {
                        continue;
                    }
                    
                    AnsiConsole.MarkupLine("[green]Користувача успішно додано![/]");
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

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }

    private static bool addUserToDatabase(string firstName, string lastName, string email)
    {
        using (var connection = new SQLiteConnection(GlobalSettings.ConnectionString))
        {
            connection.Open();

            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email;";
            using (var checkCommand = new SQLiteCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@Email", email);
                long count = (long)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    AnsiConsole.MarkupLine($"[red]Користувач з email '{email}' вже існує в базі даних.[/]");
                    return false;
                }
            }

            string insertQuery = @"
                INSERT INTO Users (FirstName, LastName, Email)
                VALUES (@FirstName, @LastName, @Email);";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();
            }
        }
        return true;
    }

    public static void PrintUsers()
    {
        using (var connection = new SQLiteConnection(GlobalSettings.ConnectionString))
        {
            connection.Open();

            string query = "SELECT Id, FirstName, LastName, Email FROM Users;";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                var table = new Table();
                table.Border = TableBorder.Rounded;
                table.AddColumn("[bold]ID[/]");
                table.AddColumn("[bold]Ім'я[/]");
                table.AddColumn("[bold]Прізвище[/]");
                table.AddColumn("[bold]Email[/]");

                while (reader.Read())
                {
                    table.AddRow(
                        reader["Id"].ToString(),
                        reader["FirstName"].ToString(),
                        reader["LastName"].ToString(),
                        reader["Email"].ToString()
                    );
                }

                AnsiConsole.Write(table);
            }
        }
    }
}