# Guinevere.Excalibur

A ready-to-use set of default controls for the [Guinevere](https://github.com/MASS4ORG/Guinevere)
immediate-mode GUI library. Everything here is an extension method on `Gui` living in the `Guinevere`
namespace, so it drops into an existing Guinevere app with a single package reference and no setup.

## Install

```
dotnet add package MASS4.Guinevere.Excalibur
```

## Quick Start

```csharp
using Guinevere;

public partial class App
{
    private bool _isEnabled = true;
    private string _name = "";
    private float _volume = 0.5f;
    private int _activeTab;

    public void Draw(Gui gui)
    {
        if (gui.Button("Run")) { /* clicked */ }
        gui.Checkbox(ref _isEnabled, "Enable feature");
        gui.TextInput(ref _name, "Your name");
        gui.Slider(ref _volume, 0f, 1f);

        gui.Tabs(ref _activeTab, tabs =>
        {
            tabs.Tab("Home", () => gui.DrawText("Home content", 12, Color.Gray));
            tabs.Tab("Settings", () => gui.DrawText("Settings content", 12, Color.Gray));
        });
    }
}
```

## Controls

| Control | Description |
|---------|-------------|
| `Button`, `IconButton` | Filled or icon buttons with hover/press states, focus and keyboard activation |
| `Checkbox`, `Toggle` | on/off selection |
| `RadioButton`, `RadioGroup` | exclusive selection |
| `Dropdown` | options list that opens over the rest of the frame |
| `TextInput`, `PasswordInput`, `TextArea` | single-line, masked, and multi-line text editing |
| `NumberField` | numeric input with Unity-style scrub editing |
| `ObjectField` | a labelled field for compact object rows |
| `Slider`, `Splitter` | numeric scrub under drag, and draggable split of two panes |
| `ProgressBar` | determinate and indeterminate progress |
| `Tabs`, `TabBar`, `TabStrip`, `PillTabs`, `VerticalTabs` | tab variants; tabs close only when marked `closable: true` |
| `Breadcrumb` | navigable trail with icons and child menus |
| `TreeView` | virtualised tree — thousands of rows cost the same as a screenful |
| `MenuBar`, `Flyout`, `ContextMenu` | menu bars and flyouts with submenus, separators, shortcuts, disabled and checkable items |
| `Popup`, `ModalPopup`, `Tooltip` | anchored and modal overlays, delayed tooltips |
| `Toast`, `Toasts` | animated toast/snackbar panels stacked in a corner |
| `DockSpace`, `DockLayout` | docking — split, tab, float, tear-off and close with layout persistence |

## Menu Bar

```csharp
gui.MenuBar(menus =>
{
    menus.Menu("File", file =>
    {
        file.Item("New", () => NewFile(), "Ctrl+N");
        file.Separator();
        file.CheckItem("Autosave", () => _autosave, v => _autosave = v);
        file.Submenu("Export", export =>
        {
            export.Item("PNG", () => Export("png"));
            export.Item("SVG", () => Export("svg"));
        });
    });
    menus.Menu("Help", help => help.Item("About", () => ShowAbout()));
});
```

## Tree View

```csharp
var state = new TreeViewState();
var items = new List<TreeItem>
{
    new("src", "src", 0, HasChildren: true),
    new("src/main", "main.cs", 1),
    new("assets", "assets", 0, HasChildren: true),
};

gui.TreeView(state, items, onClick: e => SelectedPath = e.Item.Label);
```

## Docking

```csharp
var layout = new DockLayout
{
    Root = new DockSplit(Axis.Horizontal, new DockLeaf("files"), new DockLeaf("editor"), 0.4f)
};

gui.DockSpace(layout,
    panelInfo: id => new DockPanelInfo("Files"),
    renderPanel: (id, g) => g.DrawText(id));
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.