# Package Icon Placeholder

This directory is intended to contain the package icon for Guinevere NuGet packages.

## Icon Requirements

For NuGet packages, the icon should be:
- 128x128 pixels (recommended)
- PNG format
- Named `icon.png`
- Placed in this `assets` directory

## Creating the Icon

To create a proper icon for the Guinevere packages:

1. Create a 128x128 PNG image
2. The icon should represent the Guinevere GUI system
3. Consider using elements that suggest:
   - GUI/interface design
   - GPU acceleration (perhaps with subtle gradients or effects)
   - Modern technology
   - The color scheme should be professional

## Temporary Solution

Until a proper icon is created, the NuGet packages will use the default NuGet icon.

## Usage in Projects

Once created, add the icon to each project file:

```xml
<PropertyGroup>
    <PackageIcon>icon.png</PackageIcon>
</PropertyGroup>

<ItemGroup>
    <None Include="..\..\assets\icon.png" Pack="true" PackagePath="\" />
</ItemGroup>
```
