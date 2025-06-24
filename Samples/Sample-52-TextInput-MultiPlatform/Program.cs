using Guinevere;

namespace Sample_52_TextInput_MultiPlatform;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var integration = args.Length > 0 ? args[0].ToLower() : "silknet-opengl";

        Console.WriteLine("Guinevere Multi-Platform Text Input Sample");
        Console.WriteLine("Available integrations: opentk, silknet-opengl, silknet-vulkan, raylib");
        Console.WriteLine($"Using integration: {integration}");
        Console.WriteLine();

        var gui = new Gui();

        // Test data
        var textInput = "Hello World!";
        var passwordInput = "";
        var multilineText = "Multi-line text\nLine 2\nLine 3\nLine 4\nLine 5\nLine 6\nEdit me!";
        var numberInput = "12345";
        var testInput = "";

        switch (integration)
        {
            case "opentk":
                RunWithOpenTk(gui,
                    () => DrawTextInputDemo(gui, ref textInput, ref passwordInput, ref multilineText, ref numberInput,
                        ref testInput));
                break;
            case "silknet-opengl":
            case "silknet":
                RunWithSilkNetOpenGl(gui,
                    () => DrawTextInputDemo(gui, ref textInput, ref passwordInput, ref multilineText, ref numberInput,
                        ref testInput));
                break;
            case "silknet-vulkan":
            case "vulkan":
                RunWithSilkNetVulkan(gui,
                    () => DrawTextInputDemo(gui, ref textInput, ref passwordInput, ref multilineText, ref numberInput,
                        ref testInput));
                break;
            case "raylib":
                RunWithRaylib(gui,
                    () => DrawTextInputDemo(gui, ref textInput, ref passwordInput, ref multilineText, ref numberInput,
                        ref testInput));
                break;
            default:
                Console.WriteLine($"Unknown integration: {integration}");
                Console.WriteLine("Available integrations: opentk, silknet-opengl, silknet-vulkan, raylib");
                return;
        }
    }

    private static void RunWithOpenTk(Gui gui, Action drawCallback)
    {
        try
        {
            var win = new Guinevere.OpenGL.OpenTK.GuiWindow(gui, 1000);
            win.RunGui(drawCallback);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running OpenTK integration: {ex.Message}");
        }
    }

    private static void RunWithSilkNetOpenGl(Gui gui, Action drawCallback)
    {
        try
        {
            var win = new Guinevere.OpenGL.SilkNET.GuiWindow(gui, 1000);
            win.RunGui(drawCallback);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running SilkNET OpenGL integration: {ex.Message}");
        }
    }

    private static void RunWithSilkNetVulkan(Gui gui, Action drawCallback)
    {
        try
        {
            var win = new Guinevere.Vulkan.SilkNET.GuiWindow(gui, 1000);
            win.RunGui(drawCallback);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running SilkNET Vulkan integration: {ex.Message}");
        }
    }

    private static void RunWithRaylib(Gui gui, Action drawCallback)
    {
        try
        {
            var win = new Guinevere.OpenGL.Raylib.GuiWindow(gui, 1000);
            win.RunGui(drawCallback);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running Raylib integration: {ex.Message}");
        }
    }

    private static void DrawTextInputDemo(Gui gui, ref string textInput, ref string passwordInput,
        ref string multilineText, ref string numberInput, ref string testInput)
    {
        gui.DrawRect(gui.ScreenRect, Color.DarkGray);
        gui.DrawWindowTitlebar();

        using (gui.Node().Expand().Enter())
        {
            // Title
            gui.DrawText("Multi-Platform Text Input Test", 24);
            gui.DrawText("Test text input across different integrations", 14);
            using (gui.Node().Expand().Direction(Axis.Horizontal).Gap(20).Padding(20).Enter())
            {
                gui.SetTextSize(14);
                // Test inputs section
                using (gui.Node().Expand().Direction(Axis.Vertical).Gap(10).Enter())
                {
                    gui.DrawText("Text Input Controls:", 18);

                    // Basic text input
                    gui.DrawText("Basic Text Input:");
                    gui.TextInput(ref textInput, width: 400, placeholder: "Type here...");

                    // Password input
                    gui.DrawText("Password Input:");
                    gui.PasswordInput(ref passwordInput, width: 400, placeholder: "Enter password...");

                    // Number input
                    gui.DrawText("Number Input:");
                    gui.TextInput(ref numberInput, width: 200, placeholder: "Numbers only...");

                    // Test input for experimentation
                    gui.DrawText("Test Input (experiment here):");
                    gui.TextInput(ref testInput, width: 500,
                        placeholder: "Test various characters, copy/paste, etc...");

                    // Multi-line text area
                    gui.DrawText("Multi-line Text Area:");
                    gui.TextArea(ref multilineText, width: 600, height: 120,
                        placeholder: "Multi-line text...\nPress Enter for new lines");
                }

                // Display values
                using (gui.Node().Width(300).ExpandHeight().Direction(Axis.Vertical).Enter())
                {
                    gui.DrawText("Current Values:", 18);
                    gui.SetTextSize(12);

                    gui.Node(10, 10);

                    gui.DrawText($"Text Input: '{textInput}' ({textInput.Length} chars)");
                    gui.DrawText(
                        $"Password: {passwordInput} ({passwordInput.Length} chars)");
                    gui.DrawText($"Number: '{numberInput}'");
                    gui.DrawText($"Test Input: '{testInput}' ({testInput.Length} chars)");

                    var multilinePreview = multilineText.Replace('\n', '↵');
                    if (multilinePreview.Length > 60)
                        multilinePreview = multilinePreview.Substring(0, 60) + "...";
                    gui.DrawText($"Multiline: '{multilinePreview}' ({multilineText.Split('\n').Length} lines)");

                    // Instructions
                    using (gui.Node().Expand().Direction(Axis.Vertical).Enter())
                    {
                        gui.DrawText("Instructions:", 16);
                        gui.DrawText("• Click on any input field to focus it");
                        gui.DrawText("• Type to enter text, use Backspace/Delete to remove");
                        gui.DrawText("• Use arrow keys, Home, End to navigate cursor");
                        gui.DrawText("• Use Ctrl+C to copy, Ctrl+V to paste (if supported)");
                        gui.DrawText("• Press Enter in text areas to add new lines");
                        gui.DrawText("• Press Escape to unfocus any input field");
                        gui.DrawText("• Test special characters: çáããüaáéíóú, 中文, 🌟, !@#$%^&*()");

                        using (gui.Node(10, 10).Enter())
                        {
                        } // Spacer

                        gui.DrawText("Run with different integrations:", 14);
                        gui.DrawText("dotnet run opentk");
                        gui.DrawText("dotnet run silknet-opengl");
                        gui.DrawText("dotnet run silknet-vulkan");
                        gui.DrawText("dotnet run raylib");
                    }
                }
            }
        }
    }
}
