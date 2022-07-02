using System;
using System.Linq;
using System.Reflection;

// Enumerate all the generated types and constants, printing their names out to STDOUT.

Console.WriteLine("Enumerating types...");
var types = AppDomain.CurrentDomain
    .GetAssemblies()
    .SelectMany(t => t.GetTypes())
    .Where(t => t.IsClass && t.Namespace == "Podimo.ExampleConsoleApp.Generated")
    .ToArray();

Console.WriteLine($"Number of types found: {types.Length}");

foreach (var t in types)
{
    Console.WriteLine(
        $"{t.FullName} ({(t.Attributes & (TypeAttributes.Public | TypeAttributes.NotPublic)).ToString()})"
    );
    var members = t.GetMembers(bindingAttr: BindingFlags.Static | BindingFlags.Public);
    foreach (var member in members)
    {
        Console.WriteLine($"{t.FullName}.{member.Name}");
    }
}