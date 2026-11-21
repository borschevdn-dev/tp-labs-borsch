using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab3;

/// <summary>
/// Логика работы с текстовыми файлами и кодировками.
/// Никакого вывода в консоль — только возврат результатов и исключения.
/// </summary>
public static class TextFileService
{
    /// <summary>Читает строки с консоли до пустой строки. Возвращает список строк.</summary>
    public static List<string> ReadMultilineText()
    {
        var lines = new List<string>();
        while (true)
        {
            string? line = Console.ReadLine();
            if (line is null || line.Length == 0)
                break;
            lines.Add(line);
        }
        return lines;
    }

    /// <summary>Сохраняет строки в файл в указанной кодировке.</summary>
    public static void SaveWithEncoding(string folder, string fileName, IEnumerable<string> lines, Encoding encoding)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Папка не может быть пустой.", nameof(folder));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла не может быть пустым.", nameof(fileName));
        if (lines is null)
            throw new ArgumentNullException(nameof(lines));
        if (encoding is null)
            throw new ArgumentNullException(nameof(encoding));
        if (!Directory.Exists(folder))
            throw new DirectoryNotFoundException($"Папка не найдена: {folder}");

        string fullPath = Path.Combine(folder, fileName);
        File.WriteAllLines(fullPath, lines, encoding);
    }

    /// <summary>Асинхронная версия сохранения — не блокирует поток на время записи.</summary>
    public static async Task SaveWithEncodingAsync(string folder, string fileName, IEnumerable<string> lines, Encoding encoding)
    {
        if (string.IsNullOrWhiteSpace(folder))
            throw new ArgumentException("Папка не может быть пустой.", nameof(folder));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла не может быть пустым.", nameof(fileName));
        if (lines is null)
            throw new ArgumentNullException(nameof(lines));
        if (encoding is null)
            throw new ArgumentNullException(nameof(encoding));
        if (!Directory.Exists(folder))
            throw new DirectoryNotFoundException($"Папка не найдена: {folder}");

        string fullPath = Path.Combine(folder, fileName);
        await File.WriteAllLinesAsync(fullPath, lines, encoding);
    }

    /// <summary>Возвращает размер текста в байтах в каждой из переданных кодировок.</summary>
    public static Dictionary<string, long> GetSizesInEncodings(
        IEnumerable<string> lines,
        params (string Name, Encoding Encoding)[] encodings)
    {
        if (lines is null) throw new ArgumentNullException(nameof(lines));
        if (encodings is null || encodings.Length == 0)
            throw new ArgumentException("Нужно передать хотя бы одну кодировку.", nameof(encodings));

        var materialized = lines is IList<string> list ? list : new List<string>(lines);

        var result = new Dictionary<string, long>();
        foreach (var (name, enc) in encodings)
        {
            long total = 0;
            foreach (var line in materialized)
            {
                total += enc.GetByteCount(line) + enc.GetByteCount(Environment.NewLine);
            }
            result[name] = total;
        }
        return result;
    }

    /// <summary>Возвращает фактический размер файла в байтах.</summary>
    public static long GetFileSize(string fullPath)
    {
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Файл не найден: {fullPath}", fullPath);
        return new FileInfo(fullPath).Length;
    }
}