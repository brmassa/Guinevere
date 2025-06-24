using System.Numerics;
using Guinevere;
using Guinevere.OpenGL.SilkNET;

namespace Sample_06_Animation;

internal abstract class Program
{
    private static Gui _gui = null!;
    private static bool _showPopup;
    private static bool _showButton1 = true;
    private static bool _showButton2;
    private static bool _showButton3;
    private static float _sliderValue = 0.5f;

    public static void Main()
    {
        _gui = new Gui();
        using var win = new GuiWindow(_gui, 1000, 800, "Animation System Demo");

        win.RunGui(RenderGui);
    }

    private static void RenderGui()
    {
        // Background
        _gui.DrawRect(_gui.ScreenRect, Color.FromArgb(32, 32, 48));

        // Main container
        using (_gui.Node().Expand().Margin(20).Gap(15).Enter())
        {
            // Title section
            using (_gui.Node().Height(60).Enter())
            {
                _gui.DrawText("Animation System Demo", 24, Color.White);
                _gui.DrawText(
                    $"Active Animations: {_gui.ActiveAnimationCount} | Running: {_gui.RunningAnimationCount} | FPS: {_gui.Time.SmoothFps:F1}",
                    12, Color.FromArgb(170, 170, 170));
            }

            // Demo sections
            using (_gui.Node().Expand().Gap(20).Enter())
            {
                DrawPopupDemo();
                DrawButtonAnimationDemo();
                DrawSliderAnimationDemo();
                DrawEasingFunctionsDemo();
            }
        }

        // Render popup if visible
        if (_showPopup)
        {
            DrawAnimatedPopup();
        }
    }

    private static void DrawPopupDemo()
    {
        using (_gui.Node().Height(100).Enter())
        {
            _gui.DrawBackgroundRect(Color.FromArgb(52, 58, 64), 8);

            using (_gui.Node().Expand().Margin(20).Gap(10).Enter())
            {
                _gui.DrawText("Popup Animation Demo", 18, Color.White);

                // Toggle button with hover animation
                using (_gui.Node(120, 35).Enter())
                {
                    var buttonInteractable = _gui.GetInteractable();
                    var buttonHover = _gui.AnimateBool01(buttonInteractable.OnHover(), 0.2f, Easing.SmoothStep);

                    var hoverColor = Color.FromArgb(
                        (int)(255 * (0.3f + buttonHover * 0.3f)),
                        70, 130, 180);

                    _gui.DrawBackgroundRect(hoverColor, 5);
                    _gui.DrawText(_showPopup ? "Hide Popup" : "Show Popup", 12, Color.White);

                    if (buttonInteractable.OnClick())
                    {
                        _showPopup = !_showPopup;
                    }
                }
            }
        }
    }

    private static void DrawButtonAnimationDemo()
    {
        using (_gui.Node().Height(150).Enter())
        {
            _gui.DrawRect(_gui.CurrentNode.Rect, Color.FromArgb(52, 58, 64), 8);

            using (_gui.Node().Expand().Margin(20).Gap(15).Enter())
            {
                _gui.DrawText("Button Animation Demo", 18, Color.White);

                // Toggle buttons row
                using (_gui.Node().Height(35).Direction(Axis.Horizontal).Gap(10).Enter())
                {
                    DrawToggleButton("Toggle 1", ref _showButton1);
                    DrawToggleButton("Toggle 2", ref _showButton2);
                    DrawToggleButton("Toggle 3", ref _showButton3);
                }

                // Animated buttons row
                using (_gui.Node().Height(35).Direction(Axis.Horizontal).Gap(10).Enter())
                {
                    DrawAnimatedButton("Button 1", _showButton1, Color.FromArgb(74, 144, 226), Easing.SmoothStep, 0.3f);
                    DrawAnimatedButton("Button 2", _showButton2, Color.FromArgb(226, 74, 74), Easing.ElasticOut, 0.5f);
                    DrawAnimatedButton("Button 3", _showButton3, Color.FromArgb(74, 226, 74), Easing.BackOut, 0.4f);
                }
            }
        }
    }

    private static void DrawToggleButton(string text, ref bool state)
    {
        using (_gui.Node(80, 30).Enter())
        {
            var buttonInteractable = _gui.GetInteractable();

            _gui.DrawRect(_gui.CurrentNode.Rect, Color.FromArgb(74, 85, 104), 4);
            _gui.DrawText(text, 10, Color.White);

            if (buttonInteractable.OnClick())
            {
                state = !state;
            }
        }
    }

    private static void DrawAnimatedButton(string text, bool visible, Color color, Func<float, float> easing,
        float duration)
    {
        var alpha = _gui.AnimateBool01(visible, duration, easing);

        if (alpha > 0.001f)
        {
            using (_gui.Node(80, 30).Enter())
            {
                var buttonRect = _gui.CurrentNode.Rect;
                var animatedColor = Color.FromArgb((int)(255 * alpha), color.R, color.G, color.B);

                _gui.DrawRect(buttonRect, animatedColor, 4);
                _gui.DrawText(text, 10, Color.FromArgb((int)(255 * alpha), 255, 255, 255));
            }
        }
    }

    private static void DrawSliderAnimationDemo()
    {
        using (_gui.Node().Height(120).Enter())
        {
            _gui.DrawRect(_gui.CurrentNode.Rect, Color.FromArgb(52, 58, 64), 8);

            using (_gui.Node().Expand().Margin(20).Gap(10).Enter())
            {
                _gui.DrawText("Slider Animation Demo", 18, Color.White);
                _gui.DrawText($"Value: {_sliderValue:F2}", 14, Color.FromArgb(170, 170, 170));

                // Animated progress bar
                using (_gui.Node().Height(20).Enter())
                {
                    var barRect = _gui.CurrentNode.Rect;
                    var sliderInteractable = _gui.GetInteractable();

                    // Handle slider interaction
                    if (sliderInteractable.OnHold())
                    {
                        var mouseX = _gui.Input.MousePosition.X;
                        var relativeX = mouseX - barRect.X;
                        _sliderValue = Math.Clamp(relativeX / barRect.W, 0f, 1f);
                    }

                    // Animated fill
                    var animatedValue = _gui.GetAnimationFloat(_sliderValue);
                    animatedValue.AnimateTo(_sliderValue, 0.5f, Easing.SmoothStep);
                    var fillWidth = barRect.W * animatedValue.GetValue();

                    // Background
                    _gui.DrawRect(barRect, Color.FromArgb(74, 85, 104), 10);

                    // Fill
                    if (fillWidth > 0)
                    {
                        var fillRect = new Rect(barRect.X, barRect.Y, fillWidth, barRect.H);
                        _gui.DrawRect(fillRect, Color.FromArgb(74, 144, 226), 10);
                    }
                }
            }
        }
    }

    private static void DrawEasingFunctionsDemo()
    {
        using (_gui.Node().Expand().Enter())
        {
            _gui.DrawRect(_gui.CurrentNode.Rect, Color.FromArgb(52, 58, 64), 8);

            using (_gui.Node().Expand().Margin(20).Gap(10).Enter())
            {
                _gui.DrawText("Easing Functions Comparison", 18, Color.White);

                // Create a looping animation
                var time = (_gui.Time.Elapsed % 3.0f) / 3.0f; // 3-second loop

                var easingFunctions = new[]
                {
                    ("Linear", (Func<float, float>)Easing.Linear), ("EaseIn", Easing.EaseIn),
                    ("EaseOut", Easing.EaseOut), ("SmoothStep", Easing.SmoothStep), ("BackOut", Easing.BackOut),
                    ("ElasticOut", Easing.ElasticOut)
                };

                using (_gui.Node().Expand().Gap(8).Enter())
                {
                    foreach (var (name, easingFunc) in easingFunctions)
                    {
                        using (_gui.Node().Height(25).Direction(Axis.Horizontal).Gap(15).Enter())
                        {
                            // Label
                            using (_gui.Node(80, 25).AlignContent(0.0f).Enter())
                            {
                                _gui.DrawText($"{name}:", 12, Color.FromArgb(200, 200, 200));
                            }

                            // Progress bar
                            using (_gui.Node(200, 15).AlignSelf(0.5f).Enter())
                            {
                                var barRect = _gui.CurrentNode.Rect;
                                var easedValue = easingFunc(time);
                                var fillWidth = barRect.W * easedValue;

                                // Background
                                _gui.DrawRect(barRect, Color.FromArgb(74, 85, 104), 3);

                                // Fill
                                if (fillWidth > 0)
                                {
                                    var fillRect = new Rect(barRect.X, barRect.Y, fillWidth, barRect.H);
                                    _gui.DrawRect(fillRect, Color.FromArgb(74, 144, 226), 3);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static void DrawAnimatedPopup()
    {
        // Create popup structure matching user's example
        var popup = new Popup { Visibility = _gui.GetAnimationFloat() };

        if (_showPopup)
        {
            popup.Show();
        }
        else
        {
            popup.Hide();
        }

        if (!popup.IsVisible())
            return;

        var tVisibility01 = popup.Visibility.GetValue();

        // Calculate popup scaling for animation effects
        var scalePopupIn = Vector2.Lerp(new Vector2(0.8f, 0.8f), Vector2.One, tVisibility01);

        // Semi-transparent background overlay
        var overlayColor = Color.FromArgb((int)(128 * tVisibility01), 0, 0, 0);
        _gui.DrawRect(_gui.ScreenRect, overlayColor);

        // Popup box with scaling
        var popupSize = new Vector2(400, 300);
        var scaledSize = popupSize * scalePopupIn;
        var popupRect = new Rect(
            (_gui.ScreenRect.W - scaledSize.X) / 2,
            (_gui.ScreenRect.H - scaledSize.Y) / 2,
            scaledSize.X,
            scaledSize.Y
        );

        using (_gui.Node().Margin(30).Gap(20).Enter())
        {
            _gui.DrawRect(popupRect, Color.FromArgb(45, 45, 65), 12);

            _gui.DrawText("Animated Popup!", 20, Color.White);
            _gui.DrawText("This popup smoothly animates in and out using the", 14, Color.FromArgb(200, 200, 200));
            _gui.DrawText("AnimationFloat system with SmoothStep easing.", 14, Color.FromArgb(200, 200, 200));

            // Close button
            using (_gui.Node(100, 35).Enter())
            {
                var buttonInteractable = _gui.GetInteractable();

                _gui.DrawRect(_gui.CurrentNode.Rect, Color.FromArgb(74, 85, 104), 5);
                _gui.DrawText("Close", 12, Color.White);

                if (buttonInteractable.OnClick())
                {
                    _showPopup = false;
                }
            }
        }
    }

    // Popup structure matching the user's example
    private struct Popup
    {
        public AnimationFloat Visibility;
        // public LayoutNodeScope HeaderContainer;
        // public LayoutNodeScope BodyContainer;
        // public LayoutNodeScope FooterContainer;

        public void Show()
        {
            Visibility.AnimateTo(1, 0.3f, Easing.SmoothStep);
        }

        public void Hide()
        {
            Visibility.AnimateTo(0, 0.3f, Easing.SmoothStep);
        }

        public bool IsVisible()
        {
            return Visibility > 0.001f;
        }
    }
}
