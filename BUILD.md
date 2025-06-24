# Guinevere Build System Documentation

This document describes the NUKE-based build system for the Guinevere project, including all available targets, configuration options, and deployment processes.

## Overview

The Guinevere project uses [NUKE](https://nuke.build/) as its build automation system. NUKE provides a strongly-typed, cross-platform build system that integrates seamlessly with .NET projects.

## Build System Architecture

The build system is modularized into several partial classes:

- **Build.cs**: Main build file with entry point and summary targets
- **Build.Changelog.cs**: Changelog management and versioning
- **Build.Compile.cs**: Compilation of libraries and integrations
- **Build.NuGet.cs**: NuGet package creation and publishing
- **Build.Publish.cs**: Publishing of library packages
- **Build.Release.cs**: GitHub release management and asset uploading
- **Build.Samples.cs**: Sample application building and packaging
- **Build.Solution.cs**: Solution-wide variables and configuration
- **Build.Test.cs**: Testing and coverage reporting
- **Build.Version.cs**: Version management using GitVersion

## Quick Start

### Prerequisites

- .NET 9.0 SDK
- Git (with GitVersion support)
- GitHub CLI (optional, for releases)

### Basic Commands

```bash
# Build everything
./build.sh

# Run tests
./build.sh Test

# Create NuGet packages
./build.sh PackNuGet

# Build samples
./build.sh BuildSamples

# Complete release build
./build.sh PublishAll
```

## Available Build Targets

### Core Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `Clean` | Removes build outputs | None |
| `Restore` | Restores NuGet packages | `Clean` |
| `Compile` | Builds all library projects | `Restore` |
| `Test` | Runs unit tests with coverage | `Compile` |
| `TestReport` | Generates coverage reports | `Test` |

### NuGet Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `PackNuGet` | Creates NuGet packages | `Compile` |
| `PublishNuGet` | Publishes to NuGet.org | `PackNuGet` |

### Sample Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `BuildSamples` | Builds all sample apps | `Compile` |
| `PublishSamples` | Publishes samples for distribution | `BuildSamples` |
| `PackageSamples` | Creates sample distribution archives | `PublishSamples` |
| `CleanSamples` | Cleans sample build outputs | None |

### Release Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `UpdateChangelog` | Updates CHANGELOG.md | `CheckNewCommits` |
| `CreateTag` | Creates and pushes Git tag | `CreateCommit` |
| `CreateGitHubRelease` | Creates GitHub release with assets | `PackNuGet`, `PackageSamples` |
| `PublishAll` | Complete release pipeline | All release targets |

### Versioning Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `ShowCurrentVersion` | Displays version information | None |
| `CheckNewCommits` | Checks for changes since last tag | `ShowCurrentVersion` |
| `UpdateProjectVersions` | Updates project version numbers | `CheckNewCommits` |
| `CreateCommit` | Creates version bump commit | `UpdateProjectVersions` |

### Summary Targets

| Target | Description | Dependencies |
|--------|-------------|--------------|
| `CI` | Complete CI pipeline | `Clean`, `Restore`, `Compile`, `Test` |
| `Release` | Complete release build | `CI`, `PackNuGet`, `PublishSamples`, `PackageSamples` |
| `BuildAll` | Build all deliverables | `Compile`, `BuildSamples`, `PackNuGet`, `PackageSamples` |

## Configuration Parameters

### Global Parameters

| Parameter | Description | Default | Example |
|-----------|-------------|---------|---------|
| `Configuration` | Build configuration | Debug (local), Release (CI) | `Release` |
| `RuntimeIdentifier` | Target runtime | `linux-x64` | `win-x64` |

### NuGet Parameters

| Parameter | Description | Default | Example |
|-----------|-------------|---------|---------|
| `NuGetApiKey` | API key for NuGet publishing | None | `oy2abc...` |
| `NuGetSource` | NuGet server URL | `https://api.nuget.org/v3/index.json` | Custom server |
| `SkipSymbols` | Skip symbol package creation | `false` | `true` |

### Sample Parameters

| Parameter | Description | Default | Example |
|-----------|-------------|---------|---------|
| `SampleRuntimes` | Target runtimes for samples | `win-x64,linux-x64` | `win-x64,linux-x64,osx-x64` |
| `SamplesOutputDirectory` | Sample output directory | `./samples-output` | `/custom/path` |

### GitHub Release Parameters

| Parameter | Description | Default | Example |
|-----------|-------------|---------|---------|
| `GitHubToken` | GitHub API token | None | `ghp_abc123...` |
| `GitHubOwner` | Repository owner | `brmassa` | `your-org` |
| `GitHubRepository` | Repository name | `guinevere` | `your-repo` |
| `SkipGitHubRelease` | Skip release creation | `false` | `true` |

### Testing Parameters

| Parameter | Description | Default | Example |
|-----------|-------------|---------|---------|
| `CoverageThreshold` | Minimum coverage percentage | `80` | `90` |

## Package Configuration

### Core Packages

The build system creates the following NuGet packages:

1. **org.mass4.Guinevere**: Core GUI library
2. **org.mass4.Guinevere.OpenGL.OpenTK**: OpenTK integration
3. **org.mass4.Guinevere.OpenGL.Raylib**: Raylib integration
4. **org.mass4.Guinevere.OpenGL.SilkNET**: Silk.NET OpenGL integration
5. **org.mass4.Guinevere.Vulkan.SilkNET**: Silk.NET Vulkan integration

### Package Metadata

All packages include:
- Consistent versioning using GitVersion
- MIT license
- Project URL and repository information
- README.md file
- XML documentation
- Symbol packages (.snupkg)

## Versioning Strategy

### GitVersion Configuration

The project uses GitVersion with the following strategy:
- **MainlineDevelopment**: Simple workflow with main branch
- **Semantic Versioning**: MAJOR.MINOR.PATCH format
- **Automatic incrementing**: Based on commit messages and branch patterns

### Version Sources

- **MAJOR**: Breaking changes (manual bump)
- **MINOR**: New features (conventional commits with `feat:`)
- **PATCH**: Bug fixes (conventional commits with `fix:`)
- **BUILD**: Commit count since last version

### Conventional Commits

The build system recognizes conventional commit formats:
```
feat: add new button animation system
fix: resolve OpenGL texture binding issue
docs: update API documentation
```

## Sample Applications

### Included Samples

The build system automatically builds and packages all samples:

- **Basic Samples**: Core functionality demonstrations
- **Integration Samples**: Graphics API specific examples
- **Advanced Samples**: Complex UI demonstrations
- **Showcase Samples**: Real-world application examples

### Sample Packaging

Samples are packaged as:
- Platform-specific archives (ZIP)
- Self-contained executables
- README with descriptions and usage instructions
- Cross-platform compatibility matrix

## CI/CD Integration

### GitHub Actions

The project includes two main workflows:

#### CI Workflow (`.github/workflows/ci.yml`)
- **Trigger**: Push to main/develop, Pull Requests
- **Platforms**: Ubuntu Latest, Windows Latest
- **Actions**: Build, Test, Upload Artifacts
- **Artifacts**: Test results, build outputs

#### Release Workflow (`.github/workflows/release.yml`)
- **Trigger**: Weekly schedule (Thursdays), Manual dispatch
- **Actions**: Full release pipeline
- **Deliverables**: NuGet packages, GitHub release, sample archives
- **Publishing**: Automatic NuGet.org publishing

### Environment Variables

Required for CI/CD:
```bash
# GitHub Actions Secrets
GITHUB_TOKEN=<github_token>
NUGET_API_KEY=<nuget_api_key>
```

## Local Development

### Building Locally

```bash
# Clone and setup
git clone https://github.com/brmassa/guinevere.git
cd guinevere

# Restore packages
dotnet restore

# Build everything
./build.sh BuildAll

# Run tests with coverage
./build.sh TestReport

# Create packages (without publishing)
./build.sh PackNuGet PackageSamples
```

### Testing Individual Components

```bash
# Test specific project
dotnet test Guinevere.Tests/

# Build specific integration
dotnet build Integrations/Guinevere.OpenGL.OpenTK/

# Run specific sample
dotnet run --project Samples/Sample-01-OpenGL-OpenTK/
```

### Custom Configuration

Create a local configuration file:
```bash
# .nuke/parameters.json
{
  "Configuration": "Release",
  "RuntimeIdentifier": "win-x64",
  "CoverageThreshold": 85
}
```

## Release Process

### Automatic Releases

Every Thursday (if changes exist):
1. Check for new commits since last tag
2. Run full test suite
3. Update project versions
4. Update CHANGELOG.md
5. Create Git tag
6. Build and package all deliverables
7. Create GitHub release with assets
8. Publish NuGet packages

### Manual Releases

For urgent fixes or major releases:
```bash
# Force release creation
./build.sh PublishAll

# Or trigger via GitHub Actions
gh workflow run release.yml
```

### Release Assets

Each release includes:
- **NuGet Packages**: All library packages with symbols
- **Sample Archives**: Platform-specific sample applications
- **Source Code**: Automatic GitHub source archives
- **Release Notes**: Generated from CHANGELOG.md

## Troubleshooting

### Common Issues

1. **GitVersion not found**:
   ```bash
   dotnet tool install --global GitVersion.Tool
   ```

2. **Package restore fails**:
   ```bash
   dotnet nuget locals all --clear
   dotnet restore --force
   ```

3. **Tests fail in CI**:
   - Check test project references
   - Verify test runner compatibility
   - Review coverage threshold settings

4. **NuGet publish fails**:
   - Verify API key is valid
   - Check package ID conflicts
   - Ensure version is higher than existing

### Debug Mode

Run build with verbose logging:
```bash
./build.sh --verbosity Verbose BuildAll
```

### Build System Updates

To update the build system:
1. Modify the appropriate `Build.*.cs` file
2. Test locally with `./build.sh`
3. Update this documentation
4. Commit changes and test in CI

## Performance Optimization

### Build Caching

The build system uses several caching strategies:
- **NuGet package caching**: Reduces restore time
- **Incremental builds**: Only rebuilds changed projects
- **Artifact caching**: Reuses previous build outputs

### Parallel Builds

Enable parallel building:
```bash
./build.sh --parallel BuildAll
```

### Resource Management

For large builds:
- Use `--max-cpu-count` to limit CPU usage
- Monitor memory usage during sample builds
- Clean intermediate files regularly

## Contributing to Build System

### Adding New Targets

1. Create or modify appropriate `Build.*.cs` file
2. Follow existing patterns and naming conventions
3. Add dependencies and documentation
4. Test thoroughly before committing

### Adding New Packages

1. Create project with proper metadata
2. Add to `PackageableProjects` list in `Build.NuGet.cs`
3. Add package descriptions in helper methods
4. Update version management if needed

### Build System Architecture

The modular design allows for:
- **Easy maintenance**: Changes isolated to specific files
- **Clear separation**: Each concern has its own partial class
- **Reusability**: Common patterns across all targets
- **Extensibility**: New functionality easily added

For questions or issues with the build system, please:
1. Check this documentation
2. Review existing build logs
3. Create an issue with build system label
4. Include relevant error messages and context