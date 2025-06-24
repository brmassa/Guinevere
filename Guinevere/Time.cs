using System.Diagnostics;

namespace Guinevere;

/// <summary>
/// Provides time measurement and frame rate calculation utilities for game loops.
/// </summary>
public class Time
{
    private const float SmoothingInterval = 0.1f;
    private float _smoothingTimer;
    private int _frameCountSinceLastSmooth;

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private float _lastFrameTime;

    /// <summary>
    /// Gets the time in seconds that elapsed since the last frame update.
    /// This value is affected by <see cref="TimeScale"/>.
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    /// Gets the total time in seconds elapsed since the application's start.
    /// This is the real (unscaled) time and unaffected by <see cref="TimeScale"/>.
    /// </summary>
    public float UnscaledElapsed { get; private set; }

    /// <summary>
    /// Gets the total scaled time in seconds since the Time instance was created.
    /// This value is affected by <see cref="TimeScale"/>.
    /// </summary>
    public float Elapsed { get; private set; }

    /// <summary>
    /// Gets or sets the scale at which time progresses.
    /// 1.0 is normal speed, 0.5 is half speed, 2.0 is double speed.
    /// </summary>
    public float TimeScale { get; set; } = 1f;

    /// <summary>
    /// Gets the total number of frames that have been processed since creation.
    /// </summary>
    public ulong Frames { get; private set; }

    /// <summary>
    /// Gets the instantaneous frames per second (FPS) based on the last frame's delta time.
    /// Returns 0 if delta time is 0 (to avoid division by zero).
    /// </summary>
    public float Fps => DeltaTime > 0 ? 1f / DeltaTime : 0f;

    /// <summary>
    /// Gets the smoothed frames per second (FPS) value, updated every second.
    /// This provides a more stable FPS reading than the instantaneous <see cref="Fps"/> value.
    /// </summary>
    public float SmoothFps { get; private set; }

    /// <summary>
    /// Updates the time measurements. Called once per frame.
    /// </summary>
    public void Update()
    {
        // Calculate raw frame time
        var currentTime = (float)_stopwatch.Elapsed.TotalSeconds;
        var unscaledDelta = currentTime - _lastFrameTime;
        _lastFrameTime = currentTime;

        // Update counters
        Frames++;
        UnscaledElapsed += unscaledDelta;
        DeltaTime = unscaledDelta * TimeScale;
        Elapsed += DeltaTime;

        // Update smooth FPS calculation
        _smoothingTimer += unscaledDelta;
        _frameCountSinceLastSmooth++;

        if (!(_smoothingTimer >= SmoothingInterval)) return;
        SmoothFps = _frameCountSinceLastSmooth / _smoothingTimer;
        _frameCountSinceLastSmooth = 0;
        _smoothingTimer = 0f;
    }
}
