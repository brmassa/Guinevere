# Guinevere

[![CI](https://github.com/brmassa/guinevere/actions/workflows/ci.yml/badge.svg)](https://github.com/brmassa/guinevere/actions/workflows/ci.yml)
[![Release](https://github.com/brmassa/guinevere/actions/workflows/release.yml/badge.svg)](https://github.com/brmassa/guinevere/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/org.mass4.Guinevere.svg)](https://www.nuget.org/packages/org.mass4.Guinevere/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A **GPU accelerated immediate mode GUI system** built on SkiaSharp, designed for high-performance applications with modern graphics APIs support. You can use it to create rich and beautiful apps.

## Features

- **Immediate Mode**
- **GPU Accelerated Rendering**
- **Cross-Platform**
- **Latest .NET**
- **Multiple Graphics API Support**
- **Multiple Framework Integrations**

| Package            | Graphics API | C# Framework | Use Case                                    |
|--------------------|--------------|--------------|---------------------------------------------|
| **OpenGl.SilkNET** | OpenGL       | Silk.NET     | High-performance applications (Recommended) |
| **OpenGl.OpenTK**  | OpenGL       | OpenTK 4.x   | Game development, tools                     |
| **OpenGl.Raylib**  | OpenGL       | Raylib-cs    | Simple games, prototypes                    |
| **Vulkan.SilkNET** | Vulkan       | Silk.NET     | Maximum performance, modern graphics        |

## Quick Start

### Basic Usage

```csharp
using Guinevere;
using Guinevere.OpenGL.SilkNET;

namespace Sample_70_PanGui_HelloWorld;

public abstract class Program
{
    public static void Main()
    {
        var gui = new Gui();
        using var win = new GuiWindow(gui);

        win.RunGui(() =>
        {
            gui.DrawRect(gui.ScreenRect, Color.FromArgb(255, 29, 29, 29));
            gui.DrawText("Hello, world!");
        });
    }
}
```

## Samples

The repository includes comprehensive samples demonstrating various features.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [SkiaSharp](https://github.com/mono/SkiaSharp): The foundation of our rendering system
- [OpenTK](https://github.com/opentk/opentk): OpenGL bindings for .NET
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs): C# bindings
- [Silk.NET](https://github.com/dotnet/Silk.NET): Modern .NET bindings for graphics APIs
- [NUKE](https://nuke.build): Build automation system
