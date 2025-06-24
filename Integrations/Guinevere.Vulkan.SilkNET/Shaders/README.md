# Vulkan Shaders for Guinevere

This directory contains the GLSL shader sources for the Vulkan integration of Guinevere.

## Shader Files

- `shader.vert` - Vertex shader that passes through position and texture coordinates
- `shader.frag` - Fragment shader that samples from a texture

## Compiling Shaders

To use these shaders with Vulkan, they need to be compiled to SPIR-V bytecode. You can compile them using the Vulkan SDK's `glslc` compiler:

```bash
# Compile vertex shader
 glslc shader.vert -o vert.spv

# Compile fragment shader
glslc shader.frag -o frag.spv
```

Alternatively, you can use `glslangValidator`:

```bash
# Compile vertex shader
glslangValidator -V shader.vert -o vert.spv

# Compile fragment shader
glslangValidator -V shader.frag -o frag.spv
```

## Current Implementation

The current Vulkan CanvasRenderer implementation includes embedded SPIR-V bytecode for these shaders to avoid requiring external shader compilation at runtime. If you modify the shaders, you'll need to:

1. Compile them to SPIR-V
2. Convert the bytecode to C# byte arrays
3. Update the `GetVertexShaderSpirv()` and `GetFragmentShaderSpirv()` methods in `CanvasRenderer.cs`

## Alternative Implementation

For a more flexible approach, you could modify the CanvasRenderer to load compiled SPIR-V files at runtime:

```csharp
private byte[] LoadShaderSpirv(string filename)
{
    return File.ReadAllBytes(Path.Combine("Shaders", filename));
}
```

This would require distributing the compiled `.spv` files with your application.
