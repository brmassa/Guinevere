# Guinevere GUI System

Guinevere is a generalized GUI component system designed for modern desktop applications. It provides a clean, hierarchical architecture for building user interfaces with reusable components.

## Overview

The Guinevere system addresses several key challenges in GUI development:

1. **Component Reusability**: All components implement standard interfaces, making them interchangeable and reusable across different applications.

2. **Hierarchical Architecture**: Components are organized in a clear hierarchy with well-defined responsibilities.

3. **Theme Support**: Built-in theming system allows for consistent visual styling across all components.

4. **Configuration Management**: Components can be dynamically configured without rebuilding the UI.

5. **Memory Management**: Proper disposal patterns ensure resources are cleaned up correctly.

## Architecture

### Core Interfaces

#### `IGuiComponent`
Base interface for all GUI components providing:
- Unique identification
- Visibility and enabled state management
- Draw functionality
- Initialization and disposal

#### `IFocusable`
For components that can receive input focus:
- Focus state management
- Focus change notifications

#### `ITextInput`
For text-based input components:
- Content management
- Selection support
- Read-only mode

#### `IUndoable`
For components supporting undo/redo operations:
- Undo/redo state tracking
- Operation execution

#### `IThemeable`
For components that support visual theming:
- Theme application
- Dynamic theme changes

#### `IConfigurable<T>`
For components with configurable behavior:
- Configuration management
- Runtime configuration updates

### Component Structure

```
Guinevere/
├── Core/
│   ├── IGuiComponent.cs       # Base interfaces
│   ├── AppTheme.cs           # Theme system
│   └── GuiComponentBase.cs   # Base implementation
├── Components/
│   ├── Menu/
│   │   ├── HierarchicalMenu.cs    # Hierarchical menu system
│   │   └── ImprovedTopMenu.cs     # Enhanced top menu bar
│   └── TextEditor/
│       └── GuiTextEditor.cs       # Advanced text editor
└── README.md
```

## Component Details

### HierarchicalMenu

A sophisticated menu system supporting:
- **Multi-level hierarchies**: Unlimited nesting of menu items
- **Dynamic positioning**: Smart positioning to stay within screen bounds
- **Rich content**: Icons, shortcuts, separators
- **Keyboard navigation**: Full keyboard accessibility
- **Theme integration**: Consistent visual styling

#### Usage Example

```csharp
var menu = new HierarchicalMenu("main-menu");

var fileMenu = new MenuItem
{
    Id = "file",
    Text = "File",
    Icon = "📁",
    Children = new List<MenuItem>
    {
        new MenuItem { Id = "new", Text = "New", Action = "file-new", Shortcut = "Ctrl+N" },
        new MenuItem { Id = "open", Text = "Open", Action = "file-open", Shortcut = "Ctrl+O" }
    }
};

menu.AddRootItem(fileMenu);
menu.MenuActionRequested += action => HandleMenuAction(action);
menu.Show(x, y);
```

### GuiTextEditor

An advanced text editor component featuring:
- **Syntax highlighting**: Extensible token-based highlighting
- **Multiple selections**: Complex selection operations
- **Undo/Redo**: Full operation history
- **Line numbers**: Optional line numbering
- **Scrolling**: Smooth horizontal and vertical scrolling
- **Configurable**: Extensive configuration options

#### Usage Example

```csharp
var editor = new GuiTextEditor("main-editor");
editor.Content = "Hello, World!";
editor.ContentChanged += content => SaveFile(content);
editor.SetFocus(true);
```

### ImprovedTopMenu

Enhanced top menu bar with:
- **Hamburger menu**: Space-efficient menu access
- **Context display**: Shows current folder/project
- **Window controls**: Integrated window management
- **Responsive design**: Adapts to different window sizes

## Integration with Gaya

The Guinevere system is designed to replace and enhance the existing Gaya components:

### Migration Path

1. **Gradual Replacement**: Components can be migrated one at a time
2. **Interface Compatibility**: New components implement consistent interfaces
3. **Theme Integration**: Uses existing Gaya theme system
4. **Event Compatibility**: Maintains existing event patterns

### Improvements Over Original

1. **Text Editor Issues Fixed**:
   - Proper text positioning and rendering
   - Correct cursor placement
   - Fixed selection highlighting
   - Improved scrolling behavior

2. **Menu System Enhanced**:
   - Hierarchical structure instead of flat menus
   - Better space utilization with hamburger menu
   - Removed non-functional placeholder items
   - Improved visual feedback

3. **Code Organization**:
   - Clear separation of concerns
   - Reusable component architecture
   - Consistent interfaces across components
   - Better memory management

## Configuration Examples

### Text Editor Configuration

```csharp
var config = new TextEditorConfig
{
    ShowLineNumbers = true,
    LineNumberWidth = 60f,
    LineHeight = 20f,
    TabSize = 4,
    SyntaxHighlighting = true,
    WordWrap = false
};

editor.UpdateConfiguration(config);
```

### Menu Configuration

```csharp
var menuConfig = new MenuConfig
{
    MenuWidth = 300f,
    ItemHeight = 32f,
    ShowIcons = true,
    ShowShortcuts = true,
    MaxVisibleItems = 15
};

menu.UpdateConfiguration(menuConfig);
```

## Theming

The Guinevere system supports comprehensive theming:

```csharp
// Apply custom theme
var customTheme = new TextEditorTheme
{
    BackgroundColor = new Color(20, 20, 20, 255),
    TextColor = new Color(220, 220, 220, 255),
    KeywordColor = new Color(86, 156, 214, 255)
};

editor.ApplyTheme(customTheme);
```

## Future Enhancements

1. **Additional Components**:
   - TreeView with virtual scrolling
   - DataGrid with sorting/filtering
   - Dialog system
   - Docking panels

2. **Advanced Features**:
   - Animation system
   - Accessibility support
   - Multi-monitor support
   - High DPI scaling

3. **Performance Optimizations**:
   - Virtual rendering for large datasets
   - Cached drawing operations
   - Background thread processing

## Best Practices

1. **Component Design**:
   - Always implement `IDisposable` for resource cleanup
   - Use events for loose coupling
   - Make components configurable through interfaces
   - Follow consistent naming conventions

2. **Memory Management**:
   - Dispose components when no longer needed
   - Unsubscribe from events in `Dispose()`
   - Use `using` statements for temporary components

3. **Theme Integration**:
   - Always respect the current theme
   - Provide reasonable defaults
   - Support theme changes at runtime

4. **Performance**:
   - Minimize allocations in draw loops
   - Cache expensive calculations
   - Use appropriate data structures

The Guinevere system provides a solid foundation for building modern, maintainable GUI applications with consistent behavior and appearance across all components.