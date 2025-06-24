namespace Guinevere;

public partial class LayoutNode
{
    /// <summary>
    /// Handles a click interaction on the current layout node.
    /// </summary>
    /// <returns>
    /// True if the click interaction is successful, otherwise false.
    /// </returns>
    public bool OnClick() => _gui.GetInteractable(this).OnClick();

    /// <summary>
    /// Handles a hover interaction on the current layout node.
    /// </summary>
    /// <returns>
    /// True if the hover interaction is successful; otherwise, false.
    /// </returns>
    public bool OnHover() => _gui.GetInteractable(this).OnHover();
}
