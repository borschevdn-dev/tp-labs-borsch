using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lab3;

// Нужно для Windows-1251 и CP866 в .NET 10
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var encodings = new (string Name, Encoding Encoding)[]
{
    ("UTF-8",        new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)),
    ("Windows-1251", Encoding.GetEncoding(1251)),
    ("CP866",        Encoding.GetEncoding(866)),
};

Console.WriteLine("=== Lab3. Сохранение текста с кодировкой ===");
Console.WriteLine("Введите многострочный текст. Пустая строка — конец ввода.");
Console.WriteLine();

List<string> lines;
try
{
    lines = TextFileService.ReadMultilineText();
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка чтения ввода: {ex.Message}");
    return;
}

if (lines.Count == 0)
{
    Console.WriteLine("Текст пуст — сохранять нечего.");
    return;
}

Console.WriteLine();
Console.Write("Введите путь к папке для сохранения: ");
string? folder = Console.ReadLine();
if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
{
    Console.WriteLine("Ошибка: папка не существует.");
    return;
}

Console.Write("Введите имя файла (например, out.txt): ");
string? fileName = Console.ReadLine();
if (string.IsNullOrWhiteSpace(fileName))
{
    Console.WriteLine("Ошибка: имя файла не может быть пустым.");
    return;
}

Console.WriteLine();
Console.WriteLine("Выберите кодировку для сохранения:");
for (int i = 0; i < encodings.Length; i++)
    Console.WriteLine($"  {i + 1} — {encodings[i].Name}");

Console.Write("Ваш выбор (1-3): ");
if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > encodings.Length)
{
    Console.WriteLine("Ошибка: нужно число от 1 до 3.");
    return;
}

var selected = encodings[choice - 1];

try
{
    // Асинхронное сохранение (бонус)
    await TextFileService.SaveWithEncodingAsync(folder, fileName, lines, selected.Encoding);

    string fullPath = Path.Combine(folder, fileName);
    long actualSize = TextFileService.GetFileSize(fullPath);

    Console.WriteLine();
    Console.WriteLine($"Файл сохранён: {fullPath}");
    Console.WriteLine($"Выбранная кодировка: {selected.Name}");
    Console.WriteLine($"Фактический размер файла: {actualSize} байт");
    Console.WriteLine();

    var sizes = TextFileService.GetSizesInEncodings(lines, encodings);
    Console.WriteLine("Размер того же текста в разных кодировках:");
    foreach (var (name, _) in encodings)
        Console.WriteLine($"  {name,-15} {sizes[name],8} байт");
}
catch (DirectoryNotFoundException ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine("Ошибка: нет прав на запись в эту папку.");
}
catch (IOException ex)
{
    Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Ошибка аргумента: {ex.Message}");
}