# Sample - PanGui - HelloWorld

https://pangui.io/

Program.cs
```cs
using PanGui;

public class Program
{
    public static void Main()
    {
        var gui = new Gui();
        var win = new GuiWindow(gui);

        win.RunGui(() =>
        {
            gui.DrawRect(gui.ScreenRect, 0x292929FF);
            gui.DrawText("Hello, world!");
        });
    }
}
```
