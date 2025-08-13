# Archer.Extension - .NET NuGet Package Library

Archer.Extension is a .NET Standard 2.0 NuGet package that provides utility helpers and extensions for .NET applications. It includes SecurityHelper (encryption/decryption), JwtHelper (JWT token management), DatabaseHelper (database connections), WatermarkHelper (image watermarking), and various extension methods.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Initial Setup and Build
- **NEVER CANCEL**: All build commands take less than 30 seconds but set timeouts to 60+ seconds for safety.
- `dotnet restore` - Restore NuGet packages (takes ~30 seconds). NEVER CANCEL: Set timeout to 60+ seconds.
- `dotnet build` - Build the entire solution (takes ~2-7 seconds). NEVER CANCEL: Set timeout to 60+ seconds.
- `dotnet build --configuration Release` - Build in Release mode, automatically generates NuGet package (takes ~2-7 seconds). NEVER CANCEL: Set timeout to 60+ seconds.

### Running the Test Application
- The `Archer.Extension.Testing` project is a console application that demonstrates the library functionality.
- **REQUIREMENT**: The testing project targets .NET 6.0 but system has .NET 8.0. Either install .NET 6.0 or temporarily update `Archer.Extension.Testing.csproj` to target `net8.0` instead of `net6.0`.
- **REQUIREMENT**: Create `appsettings.json` in `Archer.Extension.Testing/` directory with the following content:
```json
{
  "JwtSettings": {
    "Issuer": "https://example.com",
    "Audience": "https://example.com",
    "SignKey": "this-is-a-very-long-secret-key-for-jwt-token-signing-purpose-only",
    "ExpireInMinutes": 60
  },
  "UBOL_API": "https://test-api.example.com"
}
```
- Run with: `dotnet run --project Archer.Extension.Testing`
- **Expected behavior**: Application tests JWT generation/validation and extension methods, then fails on database connection (expected due to invalid connection string "EEE").

### Package Generation
- NuGet packages are automatically generated during Release builds
- Generated package: `Archer.Extension/bin/Release/Archer.Extension.1.1.5.nupkg`
- Package targets .NET Standard 2.0 for maximum compatibility

## Validation

### Build Validation
- Always run `dotnet restore` before building if dependencies may have changed.
- Always run `dotnet build --configuration Release` to ensure full compilation and NuGet package generation.
- Check for build errors - warnings are acceptable (mainly nullability warnings in test project).

### Code Quality Validation
- Always run `dotnet format --verify-no-changes` to ensure code formatting compliance (takes ~10 seconds). NEVER CANCEL: Set timeout to 30+ seconds.
- The formatting check should pass with exit code 0.

### Manual Testing Scenarios
- **JWT Functionality**: After making changes to JwtHelper, always run the test application to verify JWT token generation and validation work correctly.
- **Security Helper**: When modifying SecurityHelper, test encryption/decryption by running the test application.
- **Extension Methods**: When adding new extension methods, add test cases in the test application and verify they execute properly.
- **Database Helper**: Database connection creation will fail in test environment (expected behavior).

### Testing Limitations
- No formal unit test framework exists - validation is done through the console test application.
- Database functionality cannot be fully tested without proper connection strings.
- Image watermarking functionality requires actual image files for testing.

## Project Structure

### Main Library Project (`Archer.Extension/`)
- **Targets**: .NET Standard 2.0
- **Main File**: `Archer.Extension.cs` - Contains extension methods
- **Key Components**:
  - `SecurityHelper/` - Encryption/decryption utilities using BouncyCastle
  - `JwtHelper/` - JWT token generation and validation
  - `DatabaseHelper/` - Database connection factory (SQL Server, SQLite)
  - `Images/` - Image watermarking functionality
  - `Models/` - Data transfer objects (TokenModel, etc.)

### Test Project (`Archer.Extension.Testing/`)
- **Targets**: .NET 6.0 (may need to be updated to .NET 8.0 for compatibility)
- **Purpose**: Console application demonstrating library functionality
- **Requirements**: Needs `appsettings.json` for configuration

## Key Dependencies
- Microsoft.AspNetCore.Http (2.2.2)
- Microsoft.Data.SqlClient (5.2.0)
- Microsoft.Data.Sqlite.Core (7.0.8)
- Microsoft.IdentityModel.Tokens (7.5.1)
- System.IdentityModel.Tokens.Jwt (7.5.1)
- Portable.BouncyCastle (1.9.0)
- System.Drawing.Common (8.0.7)

## Build Times and Timeouts
- `dotnet restore`: ~30 seconds - NEVER CANCEL: Set timeout to 60+ seconds minimum
- `dotnet build`: ~2-7 seconds - NEVER CANCEL: Set timeout to 60+ seconds minimum
- `dotnet format --verify-no-changes`: ~10 seconds - NEVER CANCEL: Set timeout to 30+ seconds minimum
- `dotnet run --project Archer.Extension.Testing`: ~3 seconds - NEVER CANCEL: Set timeout to 60+ seconds minimum

## Common Tasks

### Adding New Extension Methods
1. Add the new method to `Archer.Extension.cs`
2. Add test code to `Program.cs` in the testing project
3. Run `dotnet build --configuration Release`
4. Run `dotnet format --verify-no-changes`
5. Test functionality with `dotnet run --project Archer.Extension.Testing`

### Modifying Helper Classes
1. Make changes to the appropriate helper class (SecurityHelper, JwtHelper, etc.)
2. Update test scenarios in `Program.cs` if needed
3. Ensure `appsettings.json` contains required configuration
4. Run `dotnet build --configuration Release`
5. Run `dotnet format --verify-no-changes`
6. Manually test with `dotnet run --project Archer.Extension.Testing`

### Package Version Updates
- Update `<Version>` in `Archer.Extension/Archer.Extension.csproj`
- Rebuild to generate new package version
- New package will be in `bin/Release/Archer.Extension.{version}.nupkg`

## Repository Info
- **Root**: Contains `Archer.Extension.sln` solution file
- **Main Project**: `Archer.Extension/` - The NuGet library
- **Test Project**: `Archer.Extension.Testing/` - Console test application
- **Documentation**: `README.md` contains usage examples and API documentation
- **License**: MIT License specified in project file