namespace Guinevere;

public static partial class ControlsExtensions
{
    /// <summary>
    /// Creates a button that returns the clicked state without modifying the input
    /// </summary>
    public static bool Button(this Gui gui, Text text,
        float width = 0, float height = 0,
        Color? backgroundColor = null,
        Color? borderColor = null,
        Color? hoverColor = null,
        Color? pressedColor = null,
        Color? pressedBorderColor = null,
        Color? color = null,
        float fontSize = 16,
        float radius = 4) =>
        ButtonCore(gui, text, width, height, backgroundColor, borderColor, hoverColor,
            pressedColor, pressedBorderColor, color, fontSize, radius);

    /// <summary>
    /// Creates an icon button that can be clicked with internal state management
    /// </summary>
    public static void IconButton(this Gui gui, char? icon, ref bool clicked,
        float size = 32,
        Color? backgroundColor = null,
        Color? hoverColor = null,
        Color? pressedColor = null,
        Color? borderColor = null,
        Color? color = null,
        float fontSize = 16,
        float radius = 4) =>
        clicked = IconButtonCore(gui, icon, size, backgroundColor, hoverColor, pressedColor,
            borderColor, color, fontSize, radius);

    /// <summary>
    /// Creates an icon button that returns the clicked state without modifying the input
    /// </summary>
    public static bool IconButton(this Gui gui, string icon,
        float size = 32,
        Color? backgroundColor = null,
        Color? hoverColor = null,
        Color? pressedColor = null,
        Color? borderColor = null,
        Color? color = null,
        float fontSize = 16,
        float radius = 4) =>
        IconButtonCore(gui, icon, size, backgroundColor, hoverColor, pressedColor,
            borderColor, color, fontSize, radius);

    private static bool ButtonCore(Gui gui, Text text, float width, float height,
        Color? backgroundColor, Color? borderColor, Color? hoverColor, Color? pressedColor,
        Color? pressedBorderColor, Color? color, float fontSize, float radius)
    {
        var (buttonWidth, buttonHeight) = CalculateButtonDimensions(text, width, height, fontSize);

        using (gui.Node(buttonWidth, buttonHeight).Enter())
        {
            if (gui.Pass != Pass.Pass2Render) return false;

            var interactionState = GetButtonInteractionState(gui);
            RenderButtonBackground(gui, interactionState, backgroundColor, hoverColor, pressedColor, radius);
            RenderButtonBorder(gui, interactionState, borderColor, pressedBorderColor);
            RenderButtonText(gui, text, fontSize, color);

            return interactionState.clicked;
        }
    }

    private static bool IconButtonCore(Gui gui, Text? icon, float size, Color? backgroundColor,
        Color? hoverColor, Color? pressedColor, Color? borderColor, Color? iconColor,
        float fontSize, float radius)
    {
        using (gui.Node(size, size).Enter())
        {
            if (gui.Pass != Pass.Pass2Render) return false;

            var interactionState = GetButtonInteractionState(gui);
            RenderIconButtonBackground(gui, interactionState, backgroundColor, hoverColor, pressedColor, radius);
            RenderIconButtonBorder(gui, interactionState, borderColor);
            RenderButtonIcon(gui, icon, fontSize, iconColor);

            return interactionState.clicked;
        }
    }

    private static (float width, float height) CalculateButtonDimensions(Text text, float width, float height,
        float fontSize)
    {
        if (width > 0 && height > 0) return (width, height);

        var font = new SKFont { Size = fontSize };
        font.MeasureText(text.Label ?? "", out var textBounds);

        const float paddingH = 16f;
        const float paddingV = 8f;

        return (
            width > 0 ? width : textBounds.Width + paddingH * 2,
            height > 0 ? height : textBounds.Height + paddingV * 2
        );
    }

    private static (bool hovered, bool pressed, bool clicked) GetButtonInteractionState(Gui gui)
    {
        var interactable = gui.GetInteractable();
        return (interactable.OnHover(), interactable.OnHold(), interactable.OnClick());
    }

    private static void RenderButtonBackground(Gui gui, (bool hovered, bool pressed, bool clicked) state,
        Color? backgroundColor, Color? hoverColor, Color? pressedColor, float radius)
    {
        var buttonColor = GetButtonBackgroundColor(state, backgroundColor, hoverColor, pressedColor);
        gui.DrawBackgroundRect(buttonColor, radius);
    }

    private static void RenderIconButtonBackground(Gui gui, (bool hovered, bool pressed, bool clicked) state,
        Color? backgroundColor, Color? hoverColor, Color? pressedColor, float radius)
    {
        if (!ShouldDrawIconButtonBackground(state, backgroundColor)) return;

        var buttonColor = GetIconButtonBackgroundColor(state, backgroundColor, hoverColor, pressedColor);
        gui.DrawBackgroundRect(buttonColor, radius);
    }

    private static void RenderButtonBorder(Gui gui, (bool hovered, bool pressed, bool clicked) state,
        Color? borderColor, Color? pressedBorderColor)
    {
        if (!state.hovered && !state.pressed) return;

        var rect = gui.CurrentNode.Rect;
        var borderColorFinal = GetButtonBorderColor(state, borderColor, pressedBorderColor);
        gui.DrawRectBorder(rect.Position, rect.Size, borderColorFinal, 1f, 4f);
    }

    private static void RenderIconButtonBorder(Gui gui, (bool hovered, bool pressed, bool clicked) state,
        Color? borderColor)
    {
        if (!state.hovered && !state.pressed) return;
        if (!borderColor.HasValue) return;

        var rect = gui.CurrentNode.Rect;
        gui.DrawRectBorder(rect.Position, rect.Size, borderColor.Value, 1f, 4f);
    }

    private static void RenderButtonText(Gui gui, Text? text, float fontSize, Color? color)
    {
        var rect = gui.CurrentNode.Rect;
        var textColorFinal = color ?? Color.White;
        var font = new SKFont { Size = fontSize };
        var paint = new SKPaint { Color = textColorFinal, IsAntialias = true };

        font.MeasureText(text?.Label ?? string.Empty, out var textBounds);
        var textPos = CalculateTextCenterPosition(rect, textBounds);

        var textShape = new Text(text?.Label ?? string.Empty, textPos, font, paint);
        gui.CurrentNode.DrawList.Add(textShape);
    }

    private static void RenderButtonIcon(Gui gui, Text? icon, float fontSize, Color? iconColor)
    {
        var rect = gui.CurrentNode.Rect;
        var iconColorFinal = iconColor ?? Color.White;
        var font = new SKFont { Size = fontSize };
        var paint = new SKPaint { Color = iconColorFinal, IsAntialias = true };

        font.MeasureText(icon?.Label ?? string.Empty, out var iconBounds);
        var iconPos = CalculateTextCenterPosition(rect, iconBounds);

        var iconShape = new Text(icon?.Label ?? string.Empty, iconPos, font, paint);
        gui.CurrentNode.DrawList.Add(iconShape);
    }

    // Color calculation helpers
    private static Color GetButtonBackgroundColor((bool hovered, bool pressed, bool clicked) state,
        Color? backgroundColor, Color? hoverColor, Color? pressedColor) =>
        state switch
        {
            { pressed: true } when pressedColor.HasValue => pressedColor.Value,
            { hovered: true } when hoverColor.HasValue => hoverColor.Value,
            _ => backgroundColor ?? Color.FromArgb(255, 100, 149, 237)
        };

    private static Color GetIconButtonBackgroundColor((bool hovered, bool pressed, bool clicked) state,
        Color? backgroundColor, Color? hoverColor, Color? pressedColor) =>
        state switch
        {
            { pressed: true } when pressedColor.HasValue => pressedColor.Value,
            { hovered: true } when hoverColor.HasValue => hoverColor.Value,
            _ => backgroundColor ?? Color.FromArgb(50, 128, 128, 128)
        };

    private static Color GetButtonBorderColor((bool hovered, bool pressed, bool clicked) state,
        Color? borderColor, Color? pressedBorderColor) =>
        state switch
        {
            { pressed: true } => pressedBorderColor ?? Color.FromArgb(255, 60, 109, 197),
            _ => borderColor ?? Color.FromArgb(255, 120, 169, 255)
        };

    private static bool ShouldDrawIconButtonBackground((bool hovered, bool pressed, bool clicked) state,
        Color? backgroundColor) =>
        backgroundColor.HasValue || state.hovered || state.pressed;

    private static Vector2 CalculateTextCenterPosition(Rect rect, SKRect textBounds) =>
        new(
            rect.X + (rect.W - textBounds.Width) * 0.5f,
            rect.Y + (rect.H + textBounds.Height) * 0.5f
        );
}
