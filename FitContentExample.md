# Fit-Content Layout Behavior

This document explains the new fit-content layout behavior in Guinevere, which makes nodes adapt to their children's sizes by default, similar to HTML.

## What Changed

Previously, nodes with no explicit width or height would either fill available space or use hardcoded fallback values. Now, nodes automatically size themselves to fit their content, just like HTML elements.

### Default Values

- **Width**: Default is `-1` (indicates fit-content)
- **Height**: Default is `-1` (indicates fit-content)
- **Explicit sizes**: Use `0` or positive values for fixed sizes
- **Expansion**: Use `ExpandWidth()` or `ExpandHeight()` to fill available space

## How It Works

### Fit-Content Calculation

When width or height is `-1`, the layout engine calculates the required size based on:

1. **Horizontal Layout (Direction.Horizontal)**:
   - Width: Sum of all children widths + gaps + margins
   - Height: Tallest child height + its margins

2. **Vertical Layout (Direction.Vertical)**:
   - Width: Widest child width + its margins  
   - Height: Sum of all children heights + gaps + margins

3. **Leaf Nodes (no children)**:
   - Use existing rect size if available
   - Otherwise use minimal size (10px)

### Examples

#### Basic Fit-Content
```csharp
// Container will size itself to fit the three children
using (gui.Node().Direction(Axis.Horizontal).Gap(10).Enter())
{
    gui.DrawBackgroundRect(Color.Blue); // Background shows actual size
    
    using (gui.Node(50, 30).Enter()) { /* Child 1 */ }
    using (gui.Node(80, 40).Enter()) { /* Child 2 */ }
    using (gui.Node(60, 35).Enter()) { /* Child 3 */ }
}
// Container will be: Width = 50+80+60+20(gaps) = 210px, Height = 40px (tallest child)
```

#### Nested Fit-Content
```csharp
// Outer container fits inner container, which fits its children
using (gui.Node().Direction(Axis.Vertical).Gap(5).Enter())
{
    gui.DrawBackgroundRect(Color.Green);
    
    using (gui.Node().Direction(Axis.Horizontal).Gap(10).Enter())
    {
        gui.DrawBackgroundRect(Color.Yellow);
        using (gui.Node(40, 40).Enter()) { /* Child A */ }
        using (gui.Node(60, 30).Enter()) { /* Child B */ }
    }
}
```

#### Explicit Sizes vs Fit-Content
```csharp
// Explicit width, fit-content height
using (gui.Node(200).Direction(Axis.Vertical).Enter())
{
    // Width is fixed at 200px, height adapts to children
}

// Both dimensions fit content
using (gui.Node().Direction(Axis.Horizontal).Enter())
{
    // Both width and height adapt to children
}

// Expand to fill available space
using (gui.Node().Expand().Enter())
{
    // Fills available width and height
}
```

## Migration Guide

### Before (Old Behavior)
```csharp
// These would fill available space or use hardcoded sizes
using (gui.Node().Enter()) { /* Would be large */ }
using (gui.Node().Direction(Axis.Horizontal).Enter()) { /* Would use default 60px height */ }
```

### After (New Behavior)
```csharp
// These now fit their content
using (gui.Node().Enter()) { /* Sizes to fit children or minimal size */ }
using (gui.Node().Direction(Axis.Horizontal).Enter()) { /* Height fits tallest child */ }

// To get old behavior, explicitly expand:
using (gui.Node().Expand().Enter()) { /* Fills available space */ }
using (gui.Node().ExpandHeight().Enter()) { /* Fits width, expands height */ }
```

## Benefits

1. **HTML-like Behavior**: Nodes naturally size to their content
2. **Better Default Layouts**: Less need for explicit sizing
3. **Responsive Design**: Content determines layout, not arbitrary sizes
4. **Cleaner Code**: Fewer explicit size specifications needed

## Performance Notes

- Fit-content calculation is recursive but cached
- For large nested hierarchies, consider using explicit sizes for performance-critical sections
- The calculation happens during layout phase, not during rendering

## Backward Compatibility

- Existing code with explicit sizes (`Node(width, height)`) works unchanged
- Existing expansion behaviors (`Expand()`, `ExpandWidth()`, etc.) work unchanged
- Only default sizing behavior changed from "fill space" to "fit content"