using System.Drawing;
using Guinevere;
using Guinevere.OpenGL.OpenTK;

namespace Sample_02_SimpleLayout;

public abstract class Program
{
    public static void Main()
    {
        var gui = new Gui();
        var win = new GuiWindow(gui);

        win.RunGui(() =>
        {
            // 1. Test absolute positioning with fixed rectangles and FPS display
            gui.DrawRect(new(0, 0, 150, 150), Color.DeepPink);
            gui.DrawRect(new(50, 50, 100, 100), Color.BlueViolet, radius: 10);

            // 2. Test the horizontal layout container with margins, and content alignment
            using (gui.Node().Expand()
                       .Margin(100)
                       .Direction(Axis.Horizontal)
                       .Gap(10)
                       .AlignContent(.5f)
                       .Enter())
            {
                gui.DrawBackgroundRect(Color.Crimson, radius: 20, Corner.Bottom);

                // 3. Test nested node with margin/padding combinations
                using (gui
                           .Node(50, 50)
                           .Margin(10, 20)
                           .Padding(10)
                           .Enter())
                {
                    gui.DrawBackgroundRect(Color.Green);

                    if (gui.Pass == Pass.Pass2Render)
                    {
                        Console.WriteLine(gui.CurrentNode.Rect);
                    }
                }

                // 4. Test conditional rendering based on input
                if (gui.Input.IsKeyDown(KeyboardKey.A))
                {
                    using (gui
                               .Node(100, 100)
                               .Enter())
                    {
                        gui.DrawBackgroundRect(Color.White, radius: 15);
                    }
                }

                // 5. Test hover/click interactions with state-based styling
                using (gui
                           .Node(150, 150)
                           .Enter())
                {
                    if (gui.GetInteractable().OnHover())
                    {
                        gui.DrawBackgroundRect(Color.DarkRed, radius: 10);
                    }
                    else
                    {
                        gui.DrawBackgroundRect(Color.Orange, radius: 2);
                    }

                    gui.GetInteractable().OnClick();

                    gui.SetTextColor(Color.Aqua);
                    gui.DrawText($"{(gui.Time.SmoothFps):N0}");
                }

                // 6. Test vertical layout nesting within the horizontal parent
                using (gui
                           .Node(200, 200)
                           .Margin(10, 20)
                           .Direction(Axis.Vertical)
                           .AlignContent(.5f)
                           .Enter())
                {
                    gui.DrawBackgroundRect(Color.Purple, radius: 15, corners: Corner.Bottom);
                    gui.DrawText("Press 'A'");

                    if (gui.GetInteractable().OnHover())
                    {
                        gui.DrawBackgroundRect(Color.DeepPink, radius: 10);
                    }

                    // 7. Test 3rd level nesting with margin/padding
                    using (gui
                               .Node(50, 50)
                               .Margin(10, 20)
                               // .Padding(10)
                               .Enter())
                    {
                        gui.DrawBackgroundRect(Color.Green);
                    }
                }
                // 8. Test negative z-index (should be hidden behind the parent container)
                using (gui
                           .Node(100, 100)
                           .SetZIndex(-1)
                           .Enter())
                {
                    gui.DrawBackgroundRect(Color.Blue);
                }
            }
        });
    }
}
