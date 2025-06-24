using Guinevere;
using Guinevere.OpenGL.SilkNET;

namespace Sample_70_PanGui_HelloWorld;

public abstract class Program
{
    public static void Main()
    {
        var gui = new Gui();
        using var win = new GuiWindow(gui);

        win.RunGui(() =>
        {
            gui.DrawRect(gui.ScreenRect, Color.FromArgb(255, 29, 29, 29));
            gui.DrawText("Hello, world!");
        });
    }
}
