# Podimo.ConstEmbed

This project is a [Source Generator](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview) which generates constant strings from files at compile-time.

## Using

We use project files to control the generation of constants.
You can see how these are used in [Podimo.ExampleConsoleApp](examples/Podimo.ExampleConsoleApp/Podimo.ExampleConsoleApp.csproj).

## Publish to Nuget

Publishing to Nuget requires a tag to be attached to a given commit in the `main` branch.
GitHub Actions will build the package, publish to Nuget and create a GitHub release as well.


## License

See [LICENSE-APACHE](LICENSE-APACHE), [LICENSE-MIT](LICENSE-MIT).
