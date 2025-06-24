using Guinevere;
using Guinevere.OpenGL.SilkNET;
using Sample_01;

namespace Sample_01_OpenGL_SilkNet;

public abstract class Program
{
    public static void Main()
    {
        var gui = new Gui();
        using var win = new GuiWindow(gui);
        Shared shared = new(gui);
        win.RunGui(shared.Draw);
    }
}
