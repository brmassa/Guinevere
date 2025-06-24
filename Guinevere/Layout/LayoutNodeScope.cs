namespace Guinevere;

/// <summary>
/// Represents a scope management class for layout nodes, which facilitates entering, exiting,
/// and managing specific properties of a layout node in a 2D or UI rendering context.
/// </summary>
public class LayoutNodeScope(ILayoutNodeEnterExit? nodeManager, LayoutNode node) : IDisposable
{
    /// <summary>
    /// Provides access to the current layout node within a specific scope.
    /// The property represents the layout node that is managed during the
    /// lifetime of the associated <see cref="LayoutNodeScope"/> instance.
    /// </summary>
    public LayoutNode Node { get; } = node;

    /// <summary>
    /// Gets or sets the text color for the current layout node within the scope.
    /// The value is used for rendering text and can be inherited from parent scopes if not explicitly set.
    /// </summary>
    public Color? TextColor { get; set; }

    /// <summary>
    /// Gets or sets the size of the text for rendering within the current layout node.
    /// This property applies to the node and its children, inheriting the value from parent nodes
    /// if it is not explicitly specified in the current scope.
    /// </summary>
    public float? TextSize { get; set; }

    /// <summary>
    /// Gets or sets the font used for rendering text within the current layout node.
    /// This property affects the node and its children, inheriting the value from parent nodes
    /// if it is not explicitly defined in the current scope.
    /// </summary>
    public Font? TextFont { get; set; }

    /// <summary>
    /// Gets or sets the font used for rendering icons in the current layout node.
    /// This property applies to the node and its children, and the value is inherited
    /// from parent nodes if not explicitly set.
    /// </summary>
    public Font? IconFont { get; set; }

    /// <summary>
    /// Gets or sets the Z-index of the layout node, which determines its rendering order
    /// in relation to sibling nodes. Nodes with higher Z-index values will be rendered
    /// in front of nodes with lower Z-index values.
    /// </summary>
    public int? ZIndex { get; set; }

    /// <summary>
    /// Gets or sets the node ID of the scrollable container that affects this node.
    /// This is used to cascade scroll transforms to child nodes.
    /// </summary>
    public string? ScrollContainerId { get; set; }

    /// <summary>
    /// Gets or sets whether this node should be clipped to its parent container bounds.
    /// This cascades to child nodes unless explicitly overridden.
    /// </summary>
    public bool? IsClipped { get; set; }

    /// <summary>
    /// Gets or sets the cumulative scroll offset applied to this node and its children.
    /// This represents the total scroll offset from all scrollable parent containers.
    /// </summary>
    public Vector2? CumulativeScrollOffset { get; set; }

    /// <summary>
    /// Gets or sets whether this node is a scrollable container.
    /// If true, this node can contribute its own scroll offset to child nodes.
    /// </summary>
    public bool? IsScrollContainer { get; set; }

    /// <summary>
    /// Gets or sets the local scroll offset for this node (if it's a scroll container).
    /// This is the scroll offset that this specific node contributes.
    /// </summary>
    public Vector2? LocalScrollOffset { get; set; }

    /// <summary>
    /// Releases all resources used by the <see cref="LayoutNodeScope"/> instance and exits the current layout node context.
    /// </summary>
    /// <remarks>
    /// This method ensures that any resources associated with the current layout node scope are properly released,
    /// and that the <see cref="ILayoutNodeEnterExit"/> manager, if provided, is notified to exit the node context.
    /// </remarks>
    public void Dispose()
    {
        nodeManager?.Exit();
    }

    /// <summary>
    /// Enters the current layout node context, typically used to prepare and set up the environment for drawing operations or related activities.
    /// </summary>
    /// <returns>Returns the current <see cref="LayoutNodeScope"/> after entering the layout node context.</returns>
    public LayoutNodeScope Enter()
    {
        nodeManager?.Enter(Node);
        return this;
    }

    /// <summary>
    /// Exits the current layout node context, performing necessary cleanup operations.
    /// </summary>
    public void Exit()
    {
        nodeManager?.Exit();
    }

    /// <summary>
    /// Sets the Z-index value for the layout node, determining its stacking order.
    /// </summary>
    /// <param name="index">The Z-index value to assign to the layout node.</param>
    /// <returns>Returns the current <see cref="LayoutNodeScope"/> instance for method chaining.</returns>
    public LayoutNodeScope SetZIndex(int index)
    {
        ZIndex = index;
        return this;
    }
}
