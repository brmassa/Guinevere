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
    /// Gets the total scaled time in seconds since the Time instance was created.
    /// This value is affected by <see cref="TimeScale"/>.
    /// </summary>
    public float Elapsed { get; private set; }

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
        // Get current time first
        float currentTime = (float)_stopwatch.Elapsed.TotalSeconds;

        // Calculate delta time before updating elapsed
        DeltaTime = currentTime - _lastFrameTime;

        // Update the other time values
        Elapsed = currentTime;
        _lastFrameTime = currentTime;

        // Update counters
        Frames++;

        // Update smooth FPS calculation
        _smoothingTimer += DeltaTime;
        _frameCountSinceLastSmooth++;
        if (!(_smoothingTimer >= SmoothingInterval)) return;
        SmoothFps = _frameCountSinceLastSmooth / _smoothingTimer;
        _frameCountSinceLastSmooth = 0;
        _smoothingTimer = 0f;
    }
}
