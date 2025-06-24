using Guinevere.Tests.Mocks;

namespace Guinevere.Tests;

public abstract class LayoutNodeTestBase
{
    private static uint _globalOrderTemp;

    protected TestableGui CreateTestGui(int width = 800, int height = 600)
    {
        var gui = new TestableGui();
        gui.SetScreenRect(width, height);
        // Still need a canvas for BeginFrame, but we'll override ScreenRect
        var canvas = CreateTestCanvas();
        gui.BeginFrame(canvas);
        return gui;
    }

    private SKCanvas CreateTestCanvas()
    {
        var surface = SKSurface.Create(new SKImageInfo(800, 600));
        return surface.Canvas;
    }

    protected LayoutNode CreateTestLayoutNode(Gui? gui = null, LayoutNode? parent = null, float? width = null,
        float? height = null)
    {
        gui ??= CreateTestGui();
        return new LayoutNode($"{_globalOrderTemp++}", gui, parent, width, height);
    }

    protected void AssertMarginValues(LayoutNode node, float expectedTop, float expectedRight, float expectedBottom,
        float expectedLeft)
    {
        Assert.Equal(expectedTop, node.Style.MarginTop);
        Assert.Equal(expectedRight, node.Style.MarginRight);
        Assert.Equal(expectedBottom, node.Style.MarginBottom);
        Assert.Equal(expectedLeft, node.Style.MarginLeft);
    }

    protected void AssertPaddingValues(LayoutNode node, float expectedTop, float expectedRight, float expectedBottom,
        float expectedLeft)
    {
        Assert.Equal(expectedTop, node.Style.PaddingTop);
        Assert.Equal(expectedRight, node.Style.PaddingRight);
        Assert.Equal(expectedBottom, node.Style.PaddingBottom);
        Assert.Equal(expectedLeft, node.Style.PaddingLeft);
    }

    protected void AssertRectValues(Rect rect, float expectedX, float expectedY, float expectedW, float expectedH)
    {
        Assert.Equal(expectedX, rect.X, 2);
        Assert.Equal(expectedY, rect.Y, 2);
        Assert.Equal(expectedW, rect.W, 2);
        Assert.Equal(expectedH, rect.H, 2);
    }

    protected void SetGuiStage(Gui gui, Pass pass)
    {
        gui.SetStage(pass);
    }

    protected LayoutNode CreateNodeWithBuildStage()
    {
        var gui = CreateTestGui();
        SetGuiStage(gui, Pass.Pass1Build);
        return CreateTestLayoutNode(gui);
    }

    protected LayoutNode CreateNodeWithRenderStage()
    {
        var gui = CreateTestGui();
        SetGuiStage(gui, Pass.Pass2Render);
        return CreateTestLayoutNode(gui);
    }

    protected void VerifyFluentReturn<T>(T original, T returned) where T : LayoutNode
    {
        Assert.Same(original, returned);
    }

    protected T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field == null)
            throw new ArgumentException($"Field '{fieldName}' not found on type '{obj.GetType().Name}'");

        var value = field.GetValue(obj);
        return value == null ? default(T)! : (T)value;
    }

    protected T GetStyleProperty<T>(LayoutNode node, string propertyName)
    {
        var styleField = node.GetType().GetField("Style", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (styleField == null)
            throw new ArgumentException("_style field not found on LayoutNode");

        var style = styleField.GetValue(node);
        if (style == null)
            throw new ArgumentException("_style is null");

        var property = style.GetType().GetProperty(propertyName);
        if (property == null)
            throw new ArgumentException($"Property '{propertyName}' not found on LayoutStyle");

        var value = property.GetValue(style);

        // Special handling for nullable float types - convert -1f to null
        if (typeof(T) == typeof(float?) && value is -1f)
        {
            return default(T)!;
        }

        return value == null ? default(T)! : (T)value;
    }
}
