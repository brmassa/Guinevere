using System.Numerics;
using Guinevere;
using Guinevere.OpenGL.SilkNET;

namespace TestScrollFixes;

public static class Program
{
    public static void Main()
    {
        var gui = new Gui();
        using var win = new GuiWindow(gui, 800, 600, "Scroll Fixes Test");
        var demo = new ScrollFixesDemo(gui);
        win.RunGui(demo.Draw);
    }
}

public class ScrollFixesDemo(Gui gui)
{
    private int _selectedTest = 0;
    private readonly string[] _testNames =
    [
        "Unified Scroll Methods Test",
        "Dynamic Parent Clipping Test",
        "Mixed Scroll Container Test"
    ];

    public void Draw()
    {
        gui.DrawRect(gui.ScreenRect, Color.FromArgb(255, 35, 35, 35));
        gui.SetTextColor(Color.White);

        using (gui.Node().Expand().Padding(20).Direction(Axis.Vertical).Gap(10).Enter())
        {
            gui.DrawText("Scroll Fixes Test Suite", size: 24);
            gui.DrawText("Testing unified scroll methods and dynamic parent clipping", size: 14, color: Color.Gray);

            gui.Node(0, 20); // Spacing

            // Test selector
            using (gui.Node().Expand().Height(40).Direction(Axis.Horizontal).Gap(10).Enter())
            {
                for (int i = 0; i < _testNames.Length; i++)
                {
                    var bgColor = i == _selectedTest ? Color.FromArgb(255, 0, 120, 215) : Color.FromArgb(255, 60, 60, 60);
                    if (gui.Button(_testNames[i], backgroundColor: bgColor))
                    {
                        _selectedTest = i;
                    }
                }
            }

            gui.Node(0, 20); // Spacing

            // Run selected test
            switch (_selectedTest)
            {
                case 0:
                    BuildUnifiedScrollMethodsTest();
                    break;
                case 1:
                    BuildDynamicParentClippingTest();
                    break;
                case 2:
                    BuildMixedScrollContainerTest();
                    break;
            }
        }
    }

    private void BuildUnifiedScrollMethodsTest()
    {
        gui.DrawText("Unified Scroll Methods Test", size: 18);
        gui.DrawText("All scroll methods (ScrollX, ScrollY, Scroll) now use ScrollContainer internally", size: 12, color: Color.Gray);

        using (gui.Node().Expand().Direction(Axis.Horizontal).Gap(20).Enter())
        {
            // ScrollX Test
            using (gui.Node().Expand().Width(200).Padding(10).Enter())
            {
                gui.DrawText("ScrollX Only", size: 14);
                gui.DrawBackgroundRect(Color.FromArgb(255, 40, 40, 40), 3);

                using (gui.Node().Expand().Height(100).Padding(5).Enter())
                {
                    gui.ScrollX();
                    using (gui.Node().Direction(Axis.Horizontal).Gap(5).Enter())
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            using (gui.Node(80, 80).Padding(5).Enter())
                            {
                                gui.DrawBackgroundRect(Color.FromArgb(255, 100, 150, 200), 2);
                                gui.DrawText($"H{i}", size: 12);
                            }
                        }
                    }
                }
            }

            // ScrollY Test
            using (gui.Node().Expand().Width(200).Padding(10).Enter())
            {
                gui.DrawText("ScrollY Only", size: 14);
                gui.DrawBackgroundRect(Color.FromArgb(255, 40, 40, 40), 3);

                using (gui.Node().Expand().Height(200).Padding(5).Enter())
                {
                    gui.ScrollY();
                    using (gui.Node().Direction(Axis.Vertical).Gap(5).Enter())
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            using (gui.Node().Expand().Height(30).Padding(5).Enter())
                            {
                                gui.DrawBackgroundRect(Color.FromArgb(255, 200, 150, 100), 2);
                                gui.DrawText($"Vertical Item {i}", size: 12);
                            }
                        }
                    }
                }
            }

            // Scroll (both) Test
            using (gui.Node().Expand().Width(200).Padding(10).Enter())
            {
                gui.DrawText("Scroll Both", size: 14);
                gui.DrawBackgroundRect(Color.FromArgb(255, 40, 40, 40), 3);

                using (gui.Node().Expand().Height(200).Padding(5).Enter())
                {
                    gui.Scroll();
                    using (gui.Node().Direction(Axis.Vertical).Gap(5).Enter())
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            using (gui.Node().Direction(Axis.Horizontal).Gap(5).Enter())
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    using (gui.Node(60, 60).Padding(3).Enter())
                                    {
                                        gui.DrawBackgroundRect(Color.FromArgb(255, 150, 100, 200), 2);
                                        gui.DrawText($"{i},{j}", size: 10);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildDynamicParentClippingTest()
    {
        gui.DrawText("Dynamic Parent Clipping Test", size: 18);
        gui.DrawText("Parent container with no fixed size - clipping should work properly", size: 12, color: Color.Gray);

        // Dynamic parent container (no fixed size)
        using (gui.Node().Expand().Padding(20).Enter())
        {
            gui.DrawBackgroundRect(Color.FromArgb(255, 50, 50, 50), 3);

            // This container has no fixed dimensions but should still clip properly
            using (gui.Node().Expand().Padding(10).Enter())
            {
                gui.DrawBackgroundRect(Color.FromArgb(255, 30, 30, 30), 3);

                using (gui.Node().Expand().Padding(5).Enter())
                {
                    gui.ScrollContainer(scrollY: true);

                    // Content that extends beyond the container
                    using (gui.Node().Direction(Axis.Vertical).Gap(5).Enter())
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            using (gui.Node().Expand().Height(40).Padding(8).Enter())
                            {
                                var color = Color.FromArgb(255,
                                    (byte)(100 + (i * 5) % 155),
                                    (byte)(50 + (i * 7) % 155),
                                    (byte)(150 + (i * 3) % 105));
                                gui.DrawBackgroundRect(color, 2);
                                gui.DrawText($"Dynamic Item {i} - This should be clipped properly", size: 12);

                                if (i == 15)
                                {
                                    gui.DrawText(" [MIDDLE ITEM]", color: Color.Yellow);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void BuildMixedScrollContainerTest()
    {
        gui.DrawText("Mixed Scroll Container Test", size: 18);
        gui.DrawText("Nested scrollable containers with different configurations", size: 12, color: Color.Gray);

        using (gui.Node().Expand().Direction(Axis.Horizontal).Gap(20).Enter())
        {
            // Left side - Vertical scroll containing horizontal scrolls
            using (gui.Node().Expand().Width(350).Padding(10).Enter())
            {
                gui.DrawText("Vertical with Horizontal Nested", size: 14);
                gui.DrawBackgroundRect(Color.FromArgb(255, 40, 40, 40), 3);

                using (gui.Node().Expand().Height(300).Padding(10).Enter())
                {
                    gui.ScrollContainer(scrollY: true);

                    using (gui.Node().Direction(Axis.Vertical).Gap(10).Enter())
                    {
                        for (int section = 0; section < 8; section++)
                        {
                            using (gui.Node().Expand().Height(80).Padding(5).Enter())
                            {
                                gui.DrawBackgroundRect(Color.FromArgb(255, 60, 60, 60), 2);
                                gui.DrawText($"Section {section}", size: 12);

                                using (gui.Node().Expand().Height(50).Padding(3).Enter())
                                {
                                    gui.ScrollContainer(scrollX: true);

                                    using (gui.Node().Direction(Axis.Horizontal).Gap(3).Enter())
                                    {
                                        for (int item = 0; item < 15; item++)
                                        {
                                            using (gui.Node(60, 40).Padding(2).Enter())
                                            {
                                                gui.DrawBackgroundRect(Color.FromArgb(255, 80, 120, 160), 1);
                                                gui.DrawText($"{section}.{item}", size: 10);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Right side - Both directions scroll
            using (gui.Node().Expand().Padding(10).Enter())
            {
                gui.DrawText("Both Directions Scroll", size: 14);
                gui.DrawBackgroundRect(Color.FromArgb(255, 40, 40, 40), 3);

                using (gui.Node().Expand().Padding(10).Enter())
                {
                    gui.ScrollContainer(scrollX: true, scrollY: true);

                    using (gui.Node().Direction(Axis.Vertical).Gap(5).Enter())
                    {
                        for (int row = 0; row < 20; row++)
                        {
                            using (gui.Node().Direction(Axis.Horizontal).Gap(5).Enter())
                            {
                                for (int col = 0; col < 20; col++)
                                {
                                    using (gui.Node(80, 50).Padding(5).Enter())
                                    {
                                        var hue = (row * 20 + col) * 10 % 360;
                                        var color = Color.FromArgb(255,
                                            (byte)(128 + Math.Sin(hue * Math.PI / 180) * 127),
                                            (byte)(128 + Math.Sin((hue + 120) * Math.PI / 180) * 127),
                                            (byte)(128 + Math.Sin((hue + 240) * Math.PI / 180) * 127));
                                        gui.DrawBackgroundRect(color, 2);
                                        gui.DrawText($"({row},{col})", size: 9);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
