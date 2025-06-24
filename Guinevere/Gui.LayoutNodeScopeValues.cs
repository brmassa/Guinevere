namespace Guinevere;

public partial class Gui
{
    /// <summary>
    /// Sets the text color for the current node and its children.
    /// The color is automatically restored when exiting the node scope.
    /// </summary>
    public void SetTextColor(Color color, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.TextColor = color;
    }

    /// <summary>
    /// Sets the text size for the current node and its children.
    /// The size is automatically restored when exiting the node scope.
    /// </summary>
    public void SetTextSize(float size, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.TextSize = size;
    }

    /// <summary>
    /// Sets the text font for the current node and its children using the Font wrapper.
    /// The font is automatically restored when exiting the node scope.
    /// </summary>
    public void SetTextFont(Font font, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.TextFont = font;
    }

    /// <summary>
    /// Sets the icon font for the current node and its children using the Font wrapper.
    /// The font is automatically restored when exiting the node scope.
    /// </summary>
    public void SetIconFont(Font font, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.IconFont = font;
    }

    /// <summary>
    /// Sets the icon font for the current node and its children using the Font wrapper.
    /// The font is automatically restored when exiting the node scope.
    /// </summary>
    public void SetZIndex(int index, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.ZIndex = index;
    }


    /// <summary>
    /// Gets the effective text color for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public Color GetEffectiveTextColor(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.TextColor, Color.Black);

    /// <summary>
    /// Gets the effective text size for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public float GetEffectiveTextSize(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.TextSize, 20f);

    /// <summary>
    /// Gets the effective text font for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public Font GetEffectiveTextFont(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.TextFont, new Font());

    /// <summary>
    /// Gets the effective icon font for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public Font GetEffectiveIconFont(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.IconFont, new Font());

    /// <summary>
    /// Gets the effective icon font for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public int GetEffectiveZIndex(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.ZIndex, 0);

    /// <summary>
    /// Gets the effective scroll container ID for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public string? GetEffectiveScrollContainerId(LayoutNodeScope? scope = null)
    {
        var currentScope = scope ?? CurrentNodeScope;
        while (currentScope != null)
        {
            if (currentScope.ScrollContainerId != null)
                return currentScope.ScrollContainerId;
            currentScope = currentScope.Node.Parent?.Scope;
        }

        return null;
    }

    /// <summary>
    /// Gets the effective clipping state for the current scope, inheriting from parent scopes if not set.
    /// </summary>
    public bool GetEffectiveIsClipped(LayoutNodeScope? scope = null) =>
        GetEffectiveValue(scope ?? CurrentNodeScope, s => s.IsClipped, false);

    /// <summary>
    /// Sets the scroll container ID for the current scope, marking it as a scrollable container.
    /// </summary>
    public void SetScrollContainer(string containerId, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.ScrollContainerId = containerId;
    }

    /// <summary>
    /// Gets the effective cumulative scroll offset for the current scope, inheriting from parent scopes.
    /// </summary>
    public Vector2 GetEffectiveCumulativeScrollOffset(LayoutNodeScope? scope = null)
    {
        var currentScope = scope ?? CurrentNodeScope;
        while (currentScope != null)
        {
            if (currentScope.CumulativeScrollOffset != null)
                return currentScope.CumulativeScrollOffset.Value;
            currentScope = GetParentScope(currentScope);
        }

        return Vector2.Zero;
    }

    /// <summary>
    /// Sets the cumulative scroll offset for the current scope.
    /// </summary>
    public void SetCumulativeScrollOffset(Vector2 offset, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.CumulativeScrollOffset = offset;
    }

    /// <summary>
    /// Marks the current scope as a scrollable container.
    /// </summary>
    public void SetIsScrollContainer(bool isScrollContainer, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.IsScrollContainer = isScrollContainer;
    }

    /// <summary>
    /// Gets whether the current scope is a scrollable container.
    /// </summary>
    public bool GetEffectiveIsScrollContainer(LayoutNodeScope? scope = null)
    {
        var currentScope = scope ?? CurrentNodeScope;
        while (currentScope != null)
        {
            if (currentScope.IsScrollContainer != null)
                return currentScope.IsScrollContainer.Value;
            currentScope = GetParentScope(currentScope);
        }

        return false;
    }

    /// <summary>
    /// Sets the local scroll offset for the current scope.
    /// </summary>
    public void SetLocalScrollOffset(Vector2 offset, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.LocalScrollOffset = offset;
    }

    /// <summary>
    /// Gets the effective local scroll offset for the current scope.
    /// </summary>
    public Vector2 GetEffectiveLocalScrollOffset(LayoutNodeScope? scope = null)
    {
        var currentScope = scope ?? CurrentNodeScope;
        while (currentScope != null)
        {
            if (currentScope.LocalScrollOffset != null)
                return currentScope.LocalScrollOffset.Value;
            currentScope = GetParentScope(currentScope);
        }

        return Vector2.Zero;
    }

    /// <summary>
    /// Gets the parent scope of the given scope by traversing the layout node hierarchy.
    /// </summary>
    private LayoutNodeScope? GetParentScope(LayoutNodeScope scope)
    {
        var parentNode = scope.Node.Parent;
        if (parentNode == null) return null;

        // Find the scope in the stack that corresponds to the parent node
        var stackArray = LayoutNodeScopeStack.ToArray();
        for (var i = stackArray.Length - 1; i >= 0; i--)
        {
            if (stackArray[i].Node == parentNode)
                return stackArray[i];
        }

        return null;
    }

    /// <summary>
    /// Sets the clipping state for the current scope.
    /// </summary>
    public void SetClipped(bool isClipped, LayoutNodeScope? scope = null)
    {
        scope ??= CurrentNodeScope;
        scope.IsClipped = isClipped;
    }


    private T GetEffectiveValue<T>(LayoutNodeScope? scope, Func<LayoutNodeScope, T?> selector, T defaultValue)
        where T : struct
    {
        while (scope != null)
        {
            var val = selector(scope);
            if (val.HasValue)
                return val.Value;

            scope = scope.Node.Parent?.Scope;
        }

        return defaultValue;
    }

    private T GetEffectiveValue<T>(LayoutNodeScope? scope, Func<LayoutNodeScope, T?> selector, T defaultValue)
        where T : class
    {
        while (scope != null)
        {
            var val = selector(scope);
            if (val != null)
                return val;

            scope = scope.Node.Parent?.Scope;
        }

        return defaultValue;
    }
}
