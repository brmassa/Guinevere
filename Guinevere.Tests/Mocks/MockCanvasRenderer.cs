namespace Guinevere.Tests.Mocks;

public class MockCanvasRenderer : ICanvasRenderer
{
    private SKSurface? _surface;
    private SKCanvas? _canvas;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public SKCanvas Canvas => _canvas ?? throw new InvalidOperationException("Canvas not initialized");

    public void Initialize(int width, int height)
    {
        Width = width;
        Height = height;
        CreateSurface(width, height);
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;
        _surface?.Dispose();
        _canvas?.Dispose();
        CreateSurface(width, height);
    }

    public void Render(Action<SKCanvas> draw)
    {
        if (_canvas == null)
            throw new InvalidOperationException("Canvas not initialized");
        
        draw(_canvas);
    }

    private void CreateSurface(int width, int height)
    {
        var imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        _surface = SKSurface.Create(imageInfo);
        _canvas = _surface.Canvas;
        
        // Clear any existing clip and set exact bounds
        _canvas.RestoreToCount(0);
        _canvas.Save();
        _canvas.ClipRect(new SKRect(0, 0, width, height));
    }

    public void Dispose()
    {
        _canvas?.Dispose();
        _surface?.Dispose();
    }
}