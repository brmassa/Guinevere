namespace Guinevere.Tests.Mocks;

public class TestableGui : Gui
{
    private Rect _testScreenRect = null!;
    private bool _useTestScreenRect;

    public override Rect ScreenRect => _useTestScreenRect ? _testScreenRect : base.ScreenRect;

    public void SetScreenRect(float width, float height)
    {
        _testScreenRect = new Rect(0, 0, width, height);
        _useTestScreenRect = true;
    }

    public void SetScreenRect(Rect rect)
    {
        _testScreenRect = rect;
        _useTestScreenRect = true;
    }

    public void UseRealCanvas()
    {
        _useTestScreenRect = false;
    }
}
