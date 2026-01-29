# Contributing to SwaggerSloop

Thank you for your interest in contributing to SwaggerSloop! 🎉

## How to Contribute

### Reporting Bugs

1. Check if the bug has already been reported in [Issues](https://github.com/seacode/swagger-sloop/issues)
2. If not, create a new issue with:
   - Clear title and description
   - Steps to reproduce
   - Expected vs actual behavior
   - Environment details (.NET/.Java version, browser, OS)

### Suggesting Features

1. Check existing [Issues](https://github.com/seacode/swagger-sloop/issues) for similar suggestions
2. Open a new issue with the `enhancement` label
3. Describe the feature and its use case

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Test your changes:
   - For .NET: Run the sample project
   - For Java: Run the sample Spring Boot application
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## Development Setup

### .NET

```bash
cd dotnet
dotnet restore
dotnet build
cd samples/SwaggerSloop.Sample
dotnet run
```

### Java

```bash
cd java/swagger-sloop-spring-boot-starter
mvn clean install
cd ../samples/swagger-sloop-sample
mvn spring-boot:run
```

### Frontend Resources

Frontend files (CSS, JS, HTML) are in `shared/resources/`. After modifying:

1. Copy to .NET: `cp -r shared/resources/* dotnet/src/SwaggerSloop/wwwroot/`
2. Copy to Java: `cp -r shared/resources/* java/swagger-sloop-spring-boot-starter/src/main/resources/static/swagger-sloop/`

## Code Style

### .NET

- Follow standard C# naming conventions
- Use meaningful variable names
- Add XML documentation for public APIs

### Java

- Follow standard Java naming conventions
- Add Javadoc for public APIs
- Use `@Override` annotation when overriding methods

### Frontend

- Use meaningful CSS class names with `art-` prefix
- Keep JavaScript in vanilla JS (no frameworks)
- Maintain responsive design

## Questions?

Feel free to open a [Discussion](https://github.com/seacode/swagger-sloop/discussions) or reach out!
