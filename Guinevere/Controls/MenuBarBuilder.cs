namespace Guinevere;

/// <summary>
/// Builder for creating menu bars
/// </summary>
public class MenuBarBuilder
{
    private readonly Gui _gui;
    private readonly float _height;
    private readonly Color? _textColor;
    private readonly Color? _hoverColor;
    private readonly float _fontSize;
    private readonly float _padding;
    private readonly Dictionary<string, bool> _menuStates = new();

    internal MenuBarBuilder(Gui gui, float height, Color? textColor, Color? hoverColor, float fontSize, float padding)
    {
        _gui = gui;
        _height = height;
        _textColor = textColor;
        _hoverColor = hoverColor;
        _fontSize = fontSize;
        _padding = padding;
    }

    /// <summary>
    /// Adds a menu to the menu bar
    /// </summary>
    public MenuBarBuilder Menu(string text, Action<FlyoutBuilder> buildMenu)
    {
        // Get or create menu state
        var menuId = $"menubar_{text}";
        var isOpen = _menuStates.TryGetValue(menuId, out var state) && state;

        using (_gui.Node().Height(_height).Padding(_padding, 0).Enter())
        {
            bool isHovered;
            bool isClicked;

            if (_gui.Pass == Pass.Pass2Render)
            {
                var interactable = _gui.GetInteractable();
                isHovered = interactable.OnHover();
                isClicked = interactable.OnClick();

                if (isHovered || isOpen)
                {
                    var hoverColorFinal = _hoverColor ?? Color.FromArgb(255, 230, 230, 230);
                    _gui.DrawBackgroundRect(hoverColorFinal);
                }

                if (isClicked)
                {
                    isOpen = !isOpen;
                    _menuStates[menuId] = isOpen;
                }
            }

            _gui.DrawText(text, _fontSize, _textColor ?? Color.Black, centerInRect: false);
        }

        // Render flyout if open
        if (isOpen)
        {
            var rect = _gui.CurrentNode.Rect;
            var menuPosition = new Vector2(rect.X, rect.Y + rect.H);

            _gui.Flyout(ref isOpen, menuPosition, buildMenu);
            _menuStates[menuId] = isOpen; // Update state after flyout handling
        }

        return this;
    }
}
