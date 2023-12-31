﻿// LFInteractive LLC. 2021-2024
﻿using System.Text;

namespace cclip;

/// <summary>
/// Object for managing and creating options
/// </summary>
public class OptionsManager
{
    internal List<Option> options;

    /// <summary>
    /// The application name or context
    /// </summary>
    public string Context { get; }

    /// <summary>
    /// Creates an options manager object with the application context and an array of options
    /// </summary>
    /// <param name="context">the application name or context</param>
    /// <param name="options">an array of options</param>
    public OptionsManager(string context, params Option[] options)
    {
        this.options = options.ToList();
        Context = context;
        Add(new() { ShortName = "h", LongName = "help", HasArgument = false, Required = false, Description = "displays the help screen" });
    }

    /// <summary>
    /// Adds an option to watch.
    /// </summary>
    /// <param name="option"></param>
    /// <returns>the option added</returns>
    public Option Add(Option option)
    {
        options.Add(option);
        return option;
    }

    /// <summary>
    /// Parses arguments from array
    /// </summary>
    /// <param name="args">arguments</param>
    /// <returns></returns>
    public OptionsParser Parse(string[] args) => new(this, args);

    /// <summary>
    /// Parses the arguments from the command-line
    /// </summary>
    /// <returns></returns>
    public OptionsParser Parse() => Parse(Environment.GetCommandLineArgs());

    /// <summary>
    /// Prints the help screen.
    /// </summary>
    public void PrintHelp()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Context} - Help:");
        int shortLength, longLength, descriptionLength;
        shortLength = longLength = descriptionLength = 0;

        foreach (Option option in options)
        {
            if (option.ShortName.Length > shortLength)
            {
                shortLength = option.ShortName.Length;
            }
            if (option.LongName.Length > longLength)
            {
                longLength = option.LongName.Length;
                longLength += 8;
            }
            if (option.Description.Length > descriptionLength)
            {
                descriptionLength = option.Description.Length;
            }
        }

        foreach (Option option in options)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"-{option.ShortName}{new string(' ', shortLength - option.ShortName.Length)}");
            Console.ResetColor();
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" --{option.LongName}");
            Console.ResetColor();
            if (option.HasArgument)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" [<arg>]");
                Console.ResetColor();
                Console.Write(new string(' ', longLength - option.LongName.Length));
            }
            else
            {
                Console.Write(new string(' ', longLength - option.LongName.Length + 8));
            }
            Console.Write(" | ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(option.Description);
            Console.Write(new string(' ', descriptionLength - option.Description.Length));
            if (option.Required)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" (*)");
            }
            Console.Write("\n");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("* - required arguments");

        Console.ResetColor();
    }

    /// <summary>
    /// Returns the text from the help screen without decoration
    /// </summary>
    /// <returns>help text</returns>
    public string GetHelpText()
    {
        StringBuilder builder = new();

        builder.AppendLine($"{Context} - Help:");

        foreach (Option option in options)
        {
            builder.Append($"-{option.ShortName}");
            builder.Append($" | ");
            builder.Append($"--{option.LongName}");
            builder.Append($" | ");
            if (option.HasArgument)
            {
                builder.Append("[<arg>] | ");
            }
            builder.Append(option.Description);

            if (option.Required)
            {
                builder.Append(" (*)");
            }
            builder.Append('\n');
        }

        builder.AppendLine("* - required arguments");

        return builder.ToString();
    }
}