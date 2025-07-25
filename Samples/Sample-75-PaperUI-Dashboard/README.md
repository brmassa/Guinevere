# Sample - Prowl.Paper - PaperDemo

PaperDemo.cs
```cs
using FontStashSharp;
using Prowl.PaperUI;
using Prowl.PaperUI.Extras;
using Prowl.Vector;

using System.Reflection;

namespace Shared
{
    public static class PaperDemo
    {
        static FontSystem fontSystem;
        static SpriteFontBase fontSmall;
        static SpriteFontBase fontMedium;
        static SpriteFontBase fontLarge;
        static SpriteFontBase fontTitle;

        // Track state for interactive elements
        static double sliderValue = 0.5f;
        static int selectedTabIndex = 0;
        static Vector2 chartPosition = new Vector2(0, 0);
        static double zoomLevel = 1.0f;
        static bool[] toggleState = { true, false, true, false, true };

        // Sample data for visualization
        static double[] dataPoints = { 0.2f, 0.5f, 0.3f, 0.8f, 0.4f, 0.7f, 0.6f };
        static readonly string[] tabNames = { "Dashboard", "Analytics", "Profile", "Settings", "Windows" };

        //Theme
        static Color backgroundColor;
        static Color cardBackground;
        static Color primaryColor;
        static Color secondaryColor;
        static Color textColor;
        static Color lightTextColor;
        static Color[] colorPalette;
        static bool isDark;

        static double time = 0;

        static string searchText = "";
        static bool searchFocused = false;

        public static void Initialize()
        {
            ToggleTheme();
            fontSystem = new FontSystem();

            // Load fonts with different sizes
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream("Shared.EmbeddedResources.font.ttf"))
            {
                if (stream == null) throw new Exception("Could not load font resource");
                fontSystem.AddFont(stream);
            }
            using (Stream? stream = assembly.GetManifestResourceStream("Shared.EmbeddedResources.fa-regular-400.ttf"))
            {
                if (stream == null) throw new Exception("Could not load font resource");
                fontSystem.AddFont(stream);
            }
            using (Stream? stream = assembly.GetManifestResourceStream("Shared.EmbeddedResources.fa-solid-900.ttf"))
            {
                if (stream == null) throw new Exception("Could not load font resource");
                fontSystem.AddFont(stream);
            }

            fontSmall = fontSystem.GetFont(19);
            fontMedium = fontSystem.GetFont(26);
            fontLarge = fontSystem.GetFont(32);
            fontTitle = fontSystem.GetFont(40);

            DefineStyles();
        }

        private static void DefineStyles()
        {
            // Sidebar styles
            Paper.DefineStyle("sidebar")
                .BackgroundColor(cardBackground)
                .Rounded(8)
                .Width(75);

            // Expanded sidebar
            Paper.DefineStyle("sidebar.expanded")
                .Width(240)
                .BorderColor(primaryColor)
                .BorderWidth(3)
                .Rounded(16);

            // Button
            Paper.DefineStyle("button")
                .Height(40)
                .Rounded(8);

            // Primary button
            Paper.DefineStyle("button.primary")
                .BackgroundColor(primaryColor);

            // Toggle switch
            Paper.DefineStyle("toggle")
                .Width(60)
                .Height(30)
                .Rounded(20);

            // Toggle on
            Paper.DefineStyle("toggle.on", "toggle")
                .BackgroundColor(secondaryColor);

            // Toggle off
            Paper.DefineStyle("toggle.off", "toggle")
                .BackgroundColor(Color.FromArgb(100, lightTextColor));

            // Toggle dot
            Paper.DefineStyle("toggle.dot")
                .Width(24)
                .Height(24)
                .Rounded(20)
                .BackgroundColor(Color.White);

            Paper.DefineStyle("separator")
                .Height(1)
                .Margin(15, 15, 0, 0)
                .BackgroundColor(Color.FromArgb(30, 0, 0, 0));
        }

        public static void RenderUI()
        {
            // Update time for animations
            time += 0.016f; // Assuming ~60fps

            //TestWindows();

            // Main container with light gray background
            using (Paper.Column("MainContainer")
                .BackgroundColor(backgroundColor)
                //.Style(BoxStyle.Solid(backgroundColor))
                .Enter())
            {
                // A stupid simple way to benchmark the performance of the UI (Adds the entire ui multiple times)
                for (int i = 0; i < 1; i++)
                {
                    Paper.PushID((ulong)i);
                    // Top navigation bar
                    RenderTopNavBar();

                    // Main content area
                    using (Paper.Row("ContentArea")
                        .Enter())
                    {
                        // Left sidebar
                        RenderSidebar();

                        // Content area (tabs content)
                        RenderMainContent();
                    }

                    // Footer
                    RenderFooter();
                    Paper.PopID();
                }
            }
        }

        public static bool isWindowAOpen = true;
        public static bool isWindowBOpen = true;

        private static void TestWindows()
        {
            // Window Tests
            WindowManager.SetWindowFont(fontMedium);
            WindowManager.Window("MyTestWindowA", ref isWindowAOpen, "Test Window", () => {
            // Window content rendering
            using (Paper.Column("WindowInnerContent")
                    .Enter())
                {
                    WindowManager.Window("MyTestWindowB", ref isWindowBOpen, "Recursive Window", () => {
                        // Window content rendering
                        using (Paper.Column("WindowInnerContent")
                            .Enter())
                        {
                            using (Paper.Box("Title")
                                .Height(40)
                                .Text(Text.Center("Hello from Window System", fontLarge, textColor))
                                .Enter()) { }

                            using (Paper.Box("Content")
                                .Text(Text.Left("This is content inside the window. You can close, resize, and drag this window.", fontMedium, textColor))
                                .Enter()) { }

                            using (Paper.Box("Button")
                                .PositionType(PositionType.SelfDirected)
                                .Width(200)
                                .Height(200)
                                .Margin(Paper.Stretch(), 0, Paper.Stretch(), 0)
                                .BackgroundColor(primaryColor)
                                //.Style(BoxStyle.SolidRounded(primaryColor, 8f))
                                //.HoverStyle(BoxStyle.SolidRounded(secondaryColor, 12f))
                                //.ActiveStyle(BoxStyle.SolidRounded(primaryColor, 16f))
                                //.FocusedStyle(BoxStyle.SolidRoundedWithBorder(backgroundColor, textColor, 20f, 1f))
                                .Text(Text.Center("Click Me", fontMedium, Color.White))
                                .OnClick((rect) => Console.WriteLine("Button in window clicked!"))
                                .Enter()) { }
                        }
                    });
                }
            });


            //var myWindowB = ImGui.CreateWindow(
            //    fontMedium,
            //    "My OtherWindow",
            //    new Vector2(100, 400),
            //    new Vector2(200, 100),
            //    (window) => {
            //        // Window content rendering
            //        using (ImGui.Column("WindowInnerContent")
            //            .Enter())
            //        {
            //            using (ImGui.LayoutBox("Title")
            //                .Height(40)
            //                .Text(Text.Center("Why Hello There", fontLarge, textColor))
            //                .Enter()) { }
            //        }
            //    }
            //);
        }

        private static void ToggleTheme()
        {
            isDark = !isDark;

            if (isDark)
            {
                //Dark
                backgroundColor = Color.FromArgb(255, 18, 18, 23);
                cardBackground = Color.FromArgb(255, 30, 30, 46);
                primaryColor = Color.FromArgb(255, 94, 104, 202);
                secondaryColor = Color.FromArgb(255, 162, 155, 254);
                textColor = Color.FromArgb(255, 226, 232, 240);
                lightTextColor = Color.FromArgb(255, 148, 163, 184);
                colorPalette = [
                    Color.FromArgb(255, 94, 234, 212),   // Cyan
                    Color.FromArgb(255, 162, 155, 254),  // Purple
                    Color.FromArgb(255, 249, 115, 22),   // Orange
                    Color.FromArgb(255, 248, 113, 113),  // Red
                    Color.FromArgb(255, 250, 204, 21)    // Yellow
                ];
            }
            else
            {

                //Light
                backgroundColor = Color.FromArgb(255, 243, 244, 246);
                cardBackground = Color.FromArgb(255, 255, 255, 255);
                primaryColor = Color.FromArgb(255, 59, 130, 246);
                secondaryColor = Color.FromArgb(255, 16, 185, 129);
                textColor = Color.FromArgb(255, 31, 41, 55);
                lightTextColor = Color.FromArgb(255, 107, 114, 128);
                colorPalette = [
                    Color.FromArgb(255, 59, 130, 246),   // Blue
                    Color.FromArgb(255, 16, 185, 129),   // Teal
                    Color.FromArgb(255, 239, 68, 68),    // Red
                    Color.FromArgb(255, 245, 158, 11),   // Amber
                    Color.FromArgb(255, 139, 92, 246)    // Purple
                ];
            }

            // Redefine styles with new theme colors
            DefineStyles();
        }

        private static void RenderTopNavBar()
        {
            using (Paper.Row("TopNavBar")
                .Height(70)
                .Rounded(8)
                .BackgroundColor(cardBackground)
                .Margin(15, 15, 15, 0)
                .Enter())
            {
                // Logo
                using (Paper.Box("Logo")
                    .Width(180)
                    .Enter())
                {
                    Paper.Box("LogoInner")
                        .Size(50)
                        .Margin(10)
                        .Text(Text.Center(Icons.Newspaper, fontLarge, lightTextColor));

                    Paper.Box("LogoText")
                        .PositionType(PositionType.SelfDirected)
                        .Left(50 + 15)
                        .Text(Text.Left("PaperUI Demo", fontTitle, textColor));
                }

                // Spacer
                using (Paper.Box("Spacer")
                    .Enter()) { }

                // Search bar
                Paper.Box("SearchTextField")
                    .TextField(searchText, fontMedium, newValue => searchText = newValue, "Search...")
                    .SetScroll(Scroll.ScrollX)
                    .Width(300)
                    .Height(40)
                    .Rounded(8)
                    //.Rotate(2)
                    .BackgroundColor(Color.FromArgb(50, 0, 0, 0))
                    .Margin(0, 15, 15, 0);

                // Theme Switch
                using (Paper.Box("LightIcon")
                    .Width(40)
                    .Height(40)
                    //.Style(BoxStyle.SolidRounded(Color.FromArgb(50, 0, 0, 0), 20f))
                    .Margin(0, 10, 15, 0)
                    .Text(Text.Center(Icons.Lightbulb, fontMedium, lightTextColor))
                    .OnClick((rect) => ToggleTheme())
                    .Enter()) { }

                // Notification icon
                using (Paper.Box("NotificationIcon")
                    .Width(40)
                    .Height(40)
                    //.Style(BoxStyle.SolidRounded(Color.FromArgb(50, 0, 0, 0), 20f))
                    .Margin(0, 10, 15, 0)
                    .Text(Text.Center(Icons.CircleExclamation, fontMedium, lightTextColor))
                    .OnClick((rect) => Console.WriteLine("Notifications clicked"))
                    .Enter()) { }

                // User Profile
                using (Paper.Box("UserProfile")
                    .Width(40)
                    .Height(40)
                    .Rounded(40)
                    .BackgroundColor(secondaryColor)
                    //.Style(BoxStyle.SolidRounded(secondaryColor, 20f))
                    .Margin(0, 15, 15, 0)
                    .Text(Text.Center("M", fontMedium, Color.White))
                    .OnClick((rect) => Console.WriteLine("Profile clicked"))
                    .Enter()) { }
            }
        }

        private static void RenderSidebar()
        {
            using (Paper.Column("Sidebar")
                .Style("sidebar")
                .Hovered.Style("sidebar.expanded").End()
                .Transition(GuiProp.Width, 0.25f, Paper.Easing.EaseIn)
                .Transition(GuiProp.BorderColor, 0.75f)
                .Transition(GuiProp.BorderWidth, 0.75f)
                .Transition(GuiProp.Rounded, 0.25f)
                .Margin(15)
                .Enter())
            {
                // Menu header
                Paper.Box("MenuHeader").Height(60).Text(Text.Center("Menu", fontMedium, textColor));

                string[] menuIcons = { Icons.House, Icons.ChartBar, Icons.User, Icons.Gear, Icons.WindowMaximize };
                string[] menuItems = { "Dashboard", "Analytics", "Users", "Settings", "Windows" };

                for (int i = 0; i < menuItems.Length; i++)
                {
                    int index = i;

                    using (Paper.Box($"MenuItemContainer_{i}")
                        .Height(50)
                        .Margin(10, 10, 5, 5)
                        .Rounded(8)
                        .BorderColor(primaryColor)
                        .BorderWidth(selectedTabIndex == index ? 2 : 0)
                        .OnClick((rect) => selectedTabIndex = index)
                        .Hovered
                            .BackgroundColor(Color.FromArgb(20, primaryColor))
                            .BorderWidth(2)
                            .End()
                        //.Transition(GuiProp.BackgroundColor, 0.05f)
                        .Transition(GuiProp.BorderWidth, 0.1f)
                        .Clip()
                        .Enter()
                        )
                    {
                        var icon = Paper.Box($"MenuItemIcon_{i}")
                            .Width(55)
                            .Height(50)
                            .Text(Text.Center(menuIcons[i], fontSmall, textColor));

                        var but = Paper.Box($"MenuItem_{i}")
                            .Width(100)
                            .PositionType(PositionType.SelfDirected)
                            .Left(50 + 15)
                            .Text(Text.Center($"{menuItems[i]}", fontSmall, textColor));
                    }
                }

                // Spacer
                using (Paper.Box("SidebarSpacer")
                    .Enter()) { }

                // Upgrade box
                using (Paper.Box("UpgradeBox")
                    .Margin(15)
                    .Height(Paper.Auto) // Auto height allows the aspect ratio to control it, width will stretch to fit the parent
                    .Rounded(8)
                    .BackgroundColor(primaryColor)
                    .AspectRatio(0.5f)
                    .Enter())
                {
                    using (Paper.Column("UpgradeContent")
                        .Margin(15)
                        .Clip()
                        .Enter())
                    {
                        using (Paper.Box("UpgradeText")
                            .Text(Text.Center("Upgrade to Pro", fontMedium, Color.White))
                            .Enter()) { }

                        using (Paper.Box("UpgradeButton")
                            .Height(30)
                            .BackgroundColor(Color.White)
                            //.Style(BoxStyle.SolidRounded(Color.White, 15f))
                            .Text(Text.Center("Upgrade", fontSmall, primaryColor))
                            .OnClick((rect) => Console.WriteLine("Upgrade clicked"))
                            .Enter()) { }
                    }
                }
            }
        }

        private static void RenderMainContent()
        {
            using (Paper.Column("MainContent")
                .Margin(0, 15, 15, 15)
                .Enter())
            {
                // Tabs navigation
                RenderTabsNavigation();

                // Tab content based on selected tab
                switch (selectedTabIndex)
                {
                    case 0: RenderDashboardTab(); break;
                    case 1: RenderAnalyticsTab(); break;
                    case 2: RenderProfileTab(); break;
                    case 3: RenderSettingsTab(); break;
                    case 4: RenderWindowsTab(); break;
                    default: RenderDashboardTab(); break;
                }
            }
        }

        private static void RenderTabsNavigation()
        {
            using (Paper.Row("TabsNav")
                .Height(60)
                .Rounded(8)
                .BackgroundColor(cardBackground)
                //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                .Enter())
            {
                for (int i = 0; i < tabNames.Length; i++)
                {
                    int index = i;
                    bool isSelected = i == selectedTabIndex;
                    Color tabColor = isSelected ? primaryColor : lightTextColor;

                    // Calculate tab width (dividing space equally)
                    double tabWidth = 1.0f / tabNames.Length;

                    using (Paper.Box($"Tab_{i}")
                        .Width(Paper.Stretch(tabWidth))
                        .Text(Text.Center(tabNames[i], fontMedium, tabColor))
                        .OnClick((rect) => selectedTabIndex = index)
                        .Enter())
                    {
                        // Show indicator line for selected tab
                        if (isSelected)
                        {
                            using (Paper.Box($"TabIndicator_{i}")
                                .Height(4)
                                .BackgroundColor(primaryColor)
                                //.Style(BoxStyle.SolidRounded(primaryColor, 1.5f))
                                .Enter()) { }
                        }
                    }
                }
            }
        }

        private static void RenderDashboardTab()
        {
            using (Paper.Row("DashboardCards")
                .Height(120)
                .Margin(0, 0, 15, 0)
                .Enter())
            {
                // Stat cards
                string[] statNames = { "Total Users", "Revenue", "Projects", "Conversion" };
                string[] statValues = { "3,456", "$12,345", "24", "8.5%" };

                for (int i = 0; i < 4; i++)
                {
                    using (Paper.Box($"StatCard_{i}")
                        .Width(Paper.Stretch(0.25f))
                        .BackgroundColor(cardBackground)
                        .Rounded(8)
                        .Hovered
                            .Rounded(12)
                            .BorderColor(colorPalette[i % colorPalette.Length])
                            .BorderWidth(2)
                            .Scale(1.05f)
                            .End()
                        .Transition(GuiProp.Rounded, 0.2f)
                        .Transition(GuiProp.BorderColor, 0.3f)
                        .Transition(GuiProp.BorderWidth, 0.2f)
                        .Transition(GuiProp.ScaleX, 0.2f)
                        .Transition(GuiProp.ScaleY, 0.2f)
                        .Margin(i == 0 ? 0 : (15 / 2f), i == 3 ? 0 : (15 / 2f), 0, 0)
                        .Enter())
                    {
                        // Card icon
                        Paper.Box($"StatIcon_{i}")
                             .Size(40)
                             .BackgroundColor(Color.FromArgb(150, colorPalette[i % colorPalette.Length]))
                             .Rounded(8)
                             .If(Paper.IsParentHovered)
                                 .Rounded(20)
                                 .End()
                            .Transition(GuiProp.Rounded, 0.3f, Paper.Easing.QuartOut)
                            .Margin(15, 0, 15, 0)
                            .IsNotInteractable();

                        using (Paper.Column($"StatContent_{i}")
                            .Margin(10, 15, 15, 15)
                            .Enter())
                        {
                            using (Paper.Box($"StatLabel_{i}")
                                .Height(Paper.Pixels(25))
                                .Text(Text.Left(statNames[i], fontSmall, lightTextColor))
                                .Enter()) { }

                            using (Paper.Box($"StatValue_{i}")
                                .Text(Text.Left(statValues[i], fontLarge, textColor))
                                .Enter()) { }
                        }
                    }
                }
            }

            // Charts and graphs row
            using (Paper.Row("ChartRow")
                .Margin(0, 0, 15, 0)
                .Enter())
            {
                // Chart area
                using (Paper.Box("ChartArea")
                    .Width(Paper.Stretch(0.7f))
                    .Rounded(8)
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Enter())
                {
                    // Chart header
                    using (Paper.Row("ChartHeader")
                        .Height(60)
                        .Margin(20, 20, 20, 0)
                        .Enter())
                    {
                        using (Paper.Box("ChartTitle")
                            .Text(Text.Left("Performance Overview", fontMedium, textColor))
                            .Enter()) { }

                        using (Paper.Row("ChartControls")
                            .Width(280)
                            .Enter())
                        {
                            string[] periods = { "Day", "Week", "Month", "Year" };
                            foreach (var period in periods)
                            {
                                using (Paper.Box($"Period_{period}")
                                    .Width(60)
                                    .Height(30)
                                    .Rounded(8)
                                    .Margin(5, 5, 0, 0)
                                    .BackgroundColor(period == "Week" ? primaryColor : Color.FromArgb(50, 0, 0, 0))
                                    .Hovered
                                        .BackgroundColor(Color.FromArgb(50, primaryColor))
                                        .End()
                                    .Transition(GuiProp.BackgroundColor, 0.2f)
                                    .Text(Text.Center(period, fontSmall, period == "Week" ? Color.White : lightTextColor))
                                    .OnClick((rect) => Console.WriteLine($"Period {period} clicked"))
                                    .Enter()) { }
                            }
                        }
                    }

                    // Chart content
                    using (Paper.Box("Chart")
                        .Margin(20)
                        .OnDragging((e) => chartPosition += e.Delta)
                        .OnScroll((e) => zoomLevel = Math.Clamp(zoomLevel + e.Delta * 0.1f, 0.5f, 2.0f))
                        .Clip()
                        .Enter())
                    {
                        using (Paper.Box("ChartCanvas")
                            .Translate(chartPosition.x, chartPosition.y)
                            .Scale(zoomLevel)
                            //.TransformSelf((rect) => {
                            //    Transform t = Transform.CreateTranslation(chartPosition) * Transform.CreateScale(zoomLevel);
                            //    //t.RotateWithOrigin(Math.Abs(Math.Sin(time * 0.01f)), rect.Center.X, rect.Center.Y);
                            //    return t;
                            //})
                            .Enter())
                        {
                            // Draw a simple chart with animated data
                            Paper.AddActionElement((vg, rect) => {

                                // Draw grid lines
                                for (int i = 0; i <= 5; i++)
                                {
                                    double y = rect.y + (rect.height / 5) * i;
                                    vg.BeginPath();
                                    vg.MoveTo(rect.x, y);
                                    vg.LineTo(rect.x + rect.width, y);
                                    vg.SetStrokeColor(lightTextColor);
                                    vg.SetStrokeWidth(1);
                                    vg.Stroke();
                                }

                                // Draw animated data points
                                vg.BeginPath();
                                double pointSpacing = rect.width / (dataPoints.Length - 1);
                                double animatedValue;

                                // Draw fill
                                vg.MoveTo(rect.x, rect.y + rect.height);

                                for (int i = 0; i < dataPoints.Length; i++)
                                {
                                    animatedValue = dataPoints[i] + Math.Sin(time * 0.25f + i * 0.5f) * 0.1f;
                                    //animatedValue = Math.Clamp(animatedValue, 0.1f, 0.9f);
                                    animatedValue = Math.Min(Math.Max(animatedValue, 0.1f), 0.9f); // Clamp to [0.1, 0.9]

                                    double x = rect.x + i * pointSpacing;
                                    double y = rect.y + rect.height - (animatedValue * rect.height);

                                    if (i == 0)
                                        vg.MoveTo(x, y);
                                    else
                                        vg.LineTo(x, y);
                                }

                                // Complete the fill path
                                vg.LineTo(rect.x + rect.width, rect.y + rect.height);
                                vg.LineTo(rect.x, rect.y + rect.height);

                                // Fill with gradient
                                //var paint = vg.LinearGradient(
                                //    rect.x, rect.y,
                                //    rect.x, rect.y + rect.height,
                                //    Color.FromArgb(100, primaryColor),
                                //    Color.FromArgb(10, primaryColor));
                                //vg.SetFillPaint(paint);
                                vg.SaveState();
                                vg.SetLinearBrush(rect.x, rect.y, rect.x, rect.y + rect.height, Color.FromArgb(100, primaryColor), Color.FromArgb(10, primaryColor));
                                vg.FillComplex();
                                vg.RestoreState();

                                vg.ClosePath();

                                // Draw the line
                                vg.BeginPath();
                                for (int i = 0; i < dataPoints.Length; i++)
                                {
                                    animatedValue = dataPoints[i] + Math.Sin(time * 0.25f + i * 0.5f) * 0.1f;
                                    //animatedValue = Math.Clamp(animatedValue, 0.1f, 0.9f);
                                    animatedValue = Math.Min(Math.Max(animatedValue, 0.1f), 0.9f); // Clamp to [0.1, 0.9]

                                    double x = rect.x + i * pointSpacing;
                                    double y = rect.y + rect.height - (animatedValue * rect.height);

                                    if (i == 0)
                                        vg.MoveTo(x, y);
                                    else
                                        vg.LineTo(x, y);
                                }

                                vg.SetStrokeColor(primaryColor);
                                vg.SetStrokeWidth(3);
                                vg.Stroke();

                                // Draw points
                                for (int i = 0; i < dataPoints.Length; i++)
                                {
                                    animatedValue = dataPoints[i] + Math.Sin(time * 0.25f + i * 0.5f) * 0.1f;
                                    //animatedValue = Math.Clamp(animatedValue, 0.1f, 0.9f);
                                    animatedValue = Math.Min(Math.Max(animatedValue, 0.1f), 0.9f); // Clamp to [0.1, 0.9]

                                    double x = rect.x + i * pointSpacing;
                                    double y = rect.y + rect.height - (animatedValue * rect.height);

                                    vg.BeginPath();
                                    vg.Circle(x, y, 6);
                                    vg.SetFillColor(Color.White);
                                    vg.Fill();

                                    vg.BeginPath();
                                    vg.Circle(x, y, 4);
                                    vg.SetFillColor(primaryColor);
                                    vg.Fill();
                                }

                                vg.ClosePath();
                            });
                        }
                    }
                }

                // Side panel
                using (Paper.Column("SidePanel")
                    .Width(Paper.Stretch(0.3f))
                    .Margin(15, 0, 0, 0)
                    .Enter())
                {
                    // Activity panel
                    using (Paper.Box("ActivityPanel")
                        .BackgroundColor(cardBackground)
                        //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                        .Rounded(8)
                        .Enter())
                    {
                        // Panel header
                        using (Paper.Box("PanelHeader")
                            .Height(60)
                            .Margin(20, 20, 20, 0)
                            .Text(Text.Left("Recent Activity", fontMedium, textColor))
                            .Enter()) { }

                        // Activity items
                        string[] activities = {
                            "John updated the project",
                            "Alice completed a task",
                            "New user registered",
                            "Project deadline updated",
                            "Team meeting scheduled"
                        };

                        string[] timestamps = {
                            "5m ago", "23m ago", "1h ago", "2h ago", "3h ago"
                        };

                        for (int i = 0; i < activities.Length; i++)
                        {
                            using (Paper.Row($"Activity_{i}")
                                .Height(70)
                                .Margin(15, 15, i == 0 ? 5 : 0, 5)
                                .Enter())
                            {
                                // Activity icon
                                using (Paper.Box($"ActivityIcon_{i}")
                                    .Width(40)
                                    .Height(40)
                                    .Rounded(8)
                                    .Margin(0, 0, 15, 0)
                                    .BackgroundColor(Color.FromArgb(150, colorPalette[i % colorPalette.Length]))
                                    //.Style(BoxStyle.SolidRounded(Color.FromArgb(150, colorPalette[i % colorPalette.Length]), 20f))
                                    .Enter()) { }

                                // Activity content
                                using (Paper.Column($"ActivityContent_{i}")
                                    .Margin(10, 0, 0, 0)
                                    .Enter())
                                {
                                    using (Paper.Box($"ActivityText_{i}")
                                        .Height(Paper.Pixels(20))
                                        .Margin(0, 0, 15, 0)
                                        .Text(Text.Left(activities[i], fontSmall, textColor))
                                        .Enter()) { }

                                    using (Paper.Box($"ActivityTime_{i}")
                                        .Height(Paper.Pixels(20))
                                        .Text(Text.Left(timestamps[i], fontSmall, lightTextColor))
                                        .Enter()) { }
                                }
                            }

                            // Add separator except for the last item
                            if (i < activities.Length - 1)
                            {
                                Paper.Box($"Separator_{i}").Style("seperator");
                            }
                        }
                    }
                }
            }
        }

        private static void RenderAnalyticsTab()
        {
            using (Paper.Row("AnalyticsContent")
                .Enter())
            {
                using (Paper.Box("AnalyticsContent")
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Margin(0, 15 / 2, 15, 0)
                    .Enter())
                {
                    // Analytics header
                    using (Paper.Box("AnalyticsHeader")
                        .Height(80)
                        .Margin(20)
                        .Text(Text.Left("Analytics Dashboard", fontLarge, textColor))
                        .Enter()) { }

                    // Interactive slider as a demo control
                    using (Paper.Column("SliderSection")
                        .Height(100)
                        .Margin(20, 20, 0, 0)
                        .Enter())
                    {
                        using (Paper.Box("SliderLabel")
                            .Height(30)
                            .Text(Text.Left($"Green Amount: {sliderValue:F2}", fontMedium, textColor))
                            .Enter()) { }

                        using (Paper.Box("SliderTrack")
                            .Height(20)
                            .BackgroundColor(Color.FromArgb(30, 0, 0, 0))
                            //.Style(BoxStyle.SolidRounded(Color.FromArgb(30, 0, 0, 0), 10f))
                            .Margin(0, 0, 20, 0)
                            .OnHeld((e) => {
                                double parentWidth = e.ElementRect.width;
                                double pointerX = e.PointerPosition.x - e.ElementRect.x;

                                // Calculate new slider value based on pointer position
                                sliderValue = Math.Clamp(pointerX / parentWidth, 0f, 1f);
                            })
                            .Enter())
                        {
                            // Filled part of slider
                            using (Paper.Box("SliderFill")
                                .Width(Paper.Percent(sliderValue * 100))
                                .BackgroundColor(primaryColor)
                                //.Style(BoxStyle.SolidRounded(primaryColor, 10f))
                                .Enter())
                            {
                                // Slider handle
                                using (Paper.Box("SliderHandle")
                                    .Left(Paper.Percent(100, -10))
                                    .Width(20)
                                    .Height(20)
                                    .BackgroundColor(textColor)
                                    //.Style(BoxStyle.SolidRounded(textColor, 10f))
                                    .PositionType(PositionType.SelfDirected)
                                    .Enter()) { }
                            }
                        }
                    }

                    // "Analysis" mock content
                    using (Paper.Box("AnalyticsVisual")
                        .Margin(20)
                        .Enter())
                    {
                        // Add a simple pie chart visualization
                        Paper.AddActionElement((vg, rect) => {
                            double centerX = rect.x + rect.width / 2;
                            double centerY = rect.y + rect.height / 2;
                            double radius = Math.Min(rect.width, rect.height) * 0.4f;

                            double startAngle = 0;
                            double[] values = { sliderValue, 0.2f, 0.15f, 0.25f, 0.1f };

                            // Normalize Values
                            double total = values.Sum();
                            for (int i = 0; i < values.Length; i++)
                                values[i] /= total;


                            for (int i = 0; i < values.Length; i++)
                            {
                                // Calculate angles
                                double angle = values[i] * Math.PI * 2;
                                double endAngle = startAngle + angle;

                                // Draw pie slice
                                vg.BeginPath();
                                vg.MoveTo(centerX, centerY);
                                vg.Arc(centerX, centerY, radius, startAngle, endAngle);
                                vg.LineTo(centerX, centerY);
                                vg.SetFillColor(colorPalette[i % colorPalette.Length]);
                                vg.Fill();

                                // Draw outline
                                vg.BeginPath();
                                vg.MoveTo(centerX, centerY);
                                vg.Arc(centerX, centerY, radius, startAngle, endAngle);
                                vg.LineTo(centerX, centerY);
                                vg.SetStrokeColor(Color.White);
                                vg.SetStrokeWidth(2);
                                vg.Stroke();

                                // Draw percentage labels
                                double labelAngle = startAngle + angle / 2;
                                double labelRadius = radius * 0.7f;
                                double labelX = centerX + Math.Cos(labelAngle) * labelRadius;
                                double labelY = centerY + Math.Sin(labelAngle) * labelRadius;

                                string label = $"{values[i] * 100:F0}%";
                                vg.SetFillColor(Color.White);
                                //vg.TextAlign(Align.Center | Align.Middle);
                                //vg.FontSize(16);
                                //vg.Text(labelX, labelY, label);
                                vg.DrawText(fontSmall, label, labelX, labelY, Color.White);

                                // Move to next slice
                                startAngle = endAngle;
                            }

                            // Draw center circle
                            vg.BeginPath();
                            vg.Circle(centerX, centerY, radius * 0.4f);
                            vg.SetFillColor(Color.White);
                            vg.Fill();

                            // Draw center text
                            // Draw center text
                            //vg.FillColor(textColor);
                            //vg.TextAlign(NvgSharp.Align.Center | NvgSharp.Align.Middle);
                            //vg.FontSize(20);
                            //vg.Text(centerX, centerY, $"Analytics\n{(sliderValue * 100):F0}%");
                            //vg.Text(fontSmall, $"Analytics\n{(sliderValue * 100):F0}%", centerX, centerY);
                        });
                    }
                }

                using (Paper.Box("ScrollTest")
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Margin(15 / 2, 0, 15, 0)
                    .Enter())
                {
                    // Dynamic content amount based on time
                    int amount = (int)(Math.Abs(Math.Sin(time * 0.25)) * 25) + 10;

                    // Create a grid layout for items
                    using (Paper.Row("GridContainer")
                        .Enter())
                    {
                        // Left column - cards
                        using (Paper.Column("LeftColumn")
                            .Width(Paper.Stretch(0.6))
                            .SetScroll(Scroll.ScrollY)
                            .Enter())
                        {
                            double scrollState = Paper.GetElementStorage<ScrollState>(Paper.CurrentParent, "ScrollState", new ScrollState()).Position.y;

                            for (int i = 0; i < 10; i++)
                            {
                                // Calculate animations based on time and index
                                double hue = (i * 25 + time * 20) % 360;
                                double saturation = 0.7;
                                double value = 0.8;

                                // Convert HSV to RGB
                                double h = hue / 60;
                                int hi = (int)Math.Floor(h) % 6;
                                double f = h - Math.Floor(h);
                                double p = value * (1 - saturation);
                                double q = value * (1 - f * saturation);
                                double t = value * (1 - (1 - f) * saturation);

                                double r, g, b;

                                switch (hi)
                                {
                                    case 0: r = value; g = t; b = p; break;
                                    case 1: r = q; g = value; b = p; break;
                                    case 2: r = p; g = value; b = t; break;
                                    case 3: r = p; g = q; b = value; break;
                                    case 4: r = t; g = p; b = value; break;
                                    default: r = value; g = p; b = q; break;
                                }

                                // Convert to Color
                                Color itemColor = Color.FromArgb(255,
                                    (int)(r * 255),
                                    (int)(g * 255),
                                    (int)(b * 255));

                                // Custom icon for each card
                                string icon = Icons.GetRandomIcon(i);

                                using (Paper.Box($"Card_{i}")
                                    .Height(70)
                                    .Margin(10, 10, 5, 5)
                                    .BackgroundColor(Color.FromArgb(230, itemColor))
                                    .BorderColor(isDark ? Color.FromArgb(50, 255, 255, 255) : Color.FromArgb(50, 0, 0, 0))
                                    .BorderWidth(1)
                                    .Rounded(12)
                                    .Enter())
                                {
                                    using (Paper.Row("CardContent")
                                        .Margin(10)
                                        .Enter())
                                    {
                                        // Icon
                                        using (Paper.Box($"CardIcon_{i}")
                                            .Width(50)
                                            .Height(50)
                                            .Rounded(25)
                                            .BackgroundColor(Color.FromArgb(60, 255, 255, 255))
                                            .Text(Text.Center(icon, fontMedium, textColor))
                                            .Enter()) { }

                                        // Content
                                        using (Paper.Column($"CardTextColumn_{i}")
                                            .Margin(10, 0, 0, 0)
                                            .Enter())
                                        {
                                            using (Paper.Box($"CardTitle_{i}")
                                                .Height(25)
                                                .Text(Text.Left($"Item {i}", fontMedium, textColor))
                                                .Enter()) { }

                                            using (Paper.Box($"CardDescription_{i}")
                                                .Text(Text.Left($"Interactive card with animations", fontSmall,
                                                    Color.FromArgb(200, textColor)))
                                                .Enter()) { }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private static void RenderProfileTab()
        {
            using (Paper.Row("ProfileContent")
                .Margin(0, 0, 15, 0)
                .Enter())
            {
                // Left panel - profile info
                using (Paper.Column("ProfileDetails")
                    .Width(Paper.Stretch(0.4f))
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Enter())
                {
                    // Profile header with avatar
                    using (Paper.Column("ProfileHeader")
                        .Height(250)
                        .Enter())
                    {
                        // Avatar
                        using (Paper.Row("AvatarSpot")
                            .Height(120)
                            .Margin(0, 0, 40, 20)
                            .Enter())
                        {
                            // Spacer to Center Avatar
                            using (Paper.Box("Spacer0").Enter()) { }

                            // Avatar
                            using (Paper.Box("Avatar")
                                .Width(120)
                                .Height(120)
                                .BackgroundColor(secondaryColor)
                                //.Style(BoxStyle.SolidRounded(secondaryColor, 60f))
                                .Text(Text.Center("J", fontTitle, Color.White))
                                .Enter()) { }

                            // Spacer to Center Avatar
                            using (Paper.Box("Spacer1").Enter()) { }
                        }


                        // User name
                        using (Paper.Box("UserName")
                            .Height(40)
                            .Text(Text.Center("John Doe", fontLarge, textColor))
                            .Enter()) { }

                        // User title
                        using (Paper.Box("UserTitle")
                            .Height(30)
                            .Text(Text.Center("Senior Developer", fontMedium, lightTextColor))
                            .Enter()) { }
                    }

                    // User stats
                    using (Paper.Row("UserStats")
                        .Height(80)
                        .Margin(20, 20, 0, 0)
                        .Enter())
                    {
                        string[] statLabels = { "Projects", "Tasks", "Teams" };
                        string[] statValues = { "24", "148", "5" };

                        for (int i = 0; i < statLabels.Length; i++)
                        {
                            using (Paper.Column($"Stat_{i}")
                                .Width(Paper.Stretch(1.0f / statLabels.Length))
                                .Enter())
                            {
                                using (Paper.Box($"StatValue_{i}")
                                    .Height(40)
                                    .Text(Text.Center(statValues[i], fontLarge, primaryColor))
                                    .Enter()) { }

                                using (Paper.Box($"StatLabel_{i}")
                                    .Height(30)
                                    .Text(Text.Center(statLabels[i], fontSmall, lightTextColor))
                                    .Enter()) { }
                            }
                        }
                    }

                    // Contact info
                    using (Paper.Column("ContactInfo")
                        .Margin(20)
                        .Enter())
                    {
                        string[] contactLabels = { "Email", "Phone", "Location", "Department" };
                        string[] contactValues = { "john.doe@example.com", "(555) 123-4567", "San Francisco, CA", "Engineering" };

                        for (int i = 0; i < contactLabels.Length; i++)
                        {
                            using (Paper.Row($"ContactRow_{i}")
                                .Height(50)
                                .Enter())
                            {
                                using (Paper.Box($"ContactLabel_{i}")
                                    .Width(100)
                                    .Text(Text.Left(contactLabels[i] + ":", fontSmall, lightTextColor))
                                    .Enter()) { }

                                using (Paper.Box($"ContactValue_{i}")
                                    .Text(Text.Left(contactValues[i], fontSmall, textColor))
                                    .Enter()) { }
                            }
                        }
                    }
                }

                // Right panel - profile activity
                using (Paper.Column("ProfileActivity")
                    .Width(Paper.Stretch(0.6f))
                    .Margin(15, 0, 0, 0)
                    .Enter())
                {
                    // Activity tracker
                    using (Paper.Box("ActivityTracker")
                        .Height(Paper.Stretch(0.6f))
                        .BackgroundColor(cardBackground)
                        //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                        .Enter())
                    {
                        // Header
                        using (Paper.Box("ActivityHeader")
                            .Height(60)
                            .Margin(20, 20, 0, 0)
                            .Text(Text.Left("Activity Tracker", fontMedium, textColor))
                            .Enter()) { }

                        // Week days
                        using (Paper.Row("WeekDays")
                            .Height(30)
                            .Margin(20, 20, 0, 0)
                            .Enter())
                        {
                            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                            foreach (var day in days)
                            {
                                using (Paper.Box($"Day_{day}")
                                    .Text(Text.Center(day, fontSmall, lightTextColor))
                                    .Enter()) { }
                            }
                        }

                        // Activity grid - contribution calendar
                        using (Paper.Box("ActivityGrid")
                            .Margin(20, 20, 0, 20)
                            .Enter())
                        {
                            // Render contribution graph
                            Paper.AddActionElement((vg, rect) => {
                                int days = 7;
                                int weeks = 4;
                                double cellWidth = rect.width / days;
                                double cellHeight = rect.height / weeks;
                                double cellSize = Math.Min(cellWidth, cellHeight) * 0.8f;
                                double cellMargin = Math.Min(cellWidth, cellHeight) * 0.1f;

                                for (int week = 0; week < days; week++)
                                {
                                    for (int day = 0; day < weeks; day++)
                                    {
                                        // Calculate position
                                        double x = rect.x + week * cellWidth + cellMargin;
                                        double y = rect.y + day * cellHeight + cellMargin;

                                        // Generate intensity based on position and time
                                        double value = Math.Sin(week * 0.4f + day * 0.7f + time) * 0.5f + 0.5f;
                                        value = Math.Pow(value, 1.5f);

                                        // Draw cell
                                        vg.BeginPath();
                                        vg.RoundedRect(x, y, cellSize, cellSize, 3, 3, 3, 3);

                                        // Apply color based on intensity
                                        int alpha = (int)(40 + value * 215);
                                        vg.SetFillColor(Color.FromArgb(alpha, primaryColor));
                                        vg.Fill();
                                    }
                                }
                            });
                        }
                    }

                    // Skills section
                    using (Paper.Box("SkillsSection")
                        .Height(Paper.Stretch(0.4f))
                        .BackgroundColor(cardBackground)
                        //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                        .Margin(0, 0, 15, 0)
                        .Enter())
                    {
                        // Header
                        using (Paper.Box("SkillsHeader")
                            .Height(20)
                            .Margin(20, 20, 20, 0)
                            .Text(Text.MiddleLeft("Skills", fontMedium, textColor))
                            .Enter()) { }

                        // Skill bars
                        string[] skills = { "Programming", "Design", "Communication", "Leadership", "Problem Solving" };
                        double[] skillLevels = { 0.9f, 0.75f, 0.8f, 0.6f, 0.85f };

                        using (Paper.Column("SkillBars")
                            .Margin(20)
                            .Enter())
                        {
                            for (int i = 0; i < skills.Length; i++)
                            {
                                using (Paper.Column($"Skill_{i}")
                                    .Height(Paper.Stretch(1.0f / skills.Length))
                                    .Margin(0, 0, i == 0 ? 0 : 10, 0)
                                    .Enter())
                                {
                                    // Skill label
                                    using (Paper.Box($"SkillLabel_{i}")
                                        .Height(25)
                                        .Text(Text.Left(skills[i], fontSmall, textColor))
                                        .Enter()) { }

                                    // Skill bar
                                    using (Paper.Row($"SkillBarBg_{i}")
                                        .Height(15)
                                        .BackgroundColor(Color.FromArgb(30, 0, 0, 0))
                                        //.Style(BoxStyle.SolidRounded(Color.FromArgb(30, 0, 0, 0), 7.5f))
                                        .Enter())
                                    {
                                        // Animate the skill level with time
                                        double animatedLevel = skillLevels[i];

                                        using (Paper.Box($"SkillBarFg_{i}")
                                            .Width(Paper.Percent(animatedLevel * 100f))
                                            .BackgroundColor(colorPalette[i % colorPalette.Length])
                                            //.Style(BoxStyle.SolidRoundedWithBorder(colorPalette[i % colorPalette.Length], primaryColor, 7.5f, 2))
                                            .Enter()) { }

                                        // Percentage label
                                        using (Paper.Box($"SkillPercent_{i}")
                                            .Width(40)
                                            .Text(Text.Right($"{animatedLevel * 100:F0}%", fontSmall, lightTextColor))
                                            .Enter()) { }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void RenderSettingsTab()
        {
            using (Paper.Row("SettingsContent")
                .Margin(0, 0, 15, 0)
                .Enter())
            {
                // Settings categories sidebar
                using (Paper.Column("SettingsCategories")
                    .Width(200)
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Enter())
                {
                    string[] categories = {
                       "General", "Account", "Appearance",
                       "Notifications", "Privacy", "Security"
                   };

                    for (int i = 0; i < categories.Length; i++)
                    {
                        bool isSelected = i == 0;
                        Color itemBgColor = isSelected ? Color.FromArgb(20, primaryColor) : Color.Transparent;
                        Color itemTextColor = isSelected ? primaryColor : textColor;
                        var index = i;

                        using (Paper.Box($"SettingsCat_{i}")
                            .Height(50)
                            .Margin(10, 10, 5, 5)
                            .BackgroundColor(itemBgColor)
                            //.Style(BoxStyle.SolidRounded(itemBgColor, 8f))
                            .Text(Text.Left($"  {categories[i]}", fontSmall, itemTextColor))
                            .OnClick((rect) => { Console.WriteLine($"Category {categories[index]} clicked"); })
                            .Enter()) { }
                    }
                }

                // Settings content
                using (Paper.Column("SettingsOptions")
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Margin(15, 0, 0, 0)
                    .Enter())
                {
                    // Settings header
                    using (Paper.Box("SettingsHeader")
                        .Height(80)
                        .Margin(20)
                        .Text(Text.Left("General Settings", fontLarge, textColor))
                        .Enter()) { }

                    // Toggle options
                    string[] options = {
                       "Enable notifications",
                       "Dark mode",
                       "Auto-save changes",
                       "Show analytics data",
                       "Email notifications"
                   };

                    for (int i = 0; i < options.Length; i++)
                    {
                        using (Paper.Row($"Setting_{i}")
                            .Height(60)
                            .Margin(20, 20, i == 0 ? 0 : 5, 5)
                            .Enter())
                        {
                            // Option label
                            using (Paper.Box($"SettingLabel_{i}")
                                .Text(Text.Left(options[i], fontMedium, textColor))
                                .Enter()) { }

                            // Toggle switch
                            bool isOn = toggleState[i];

                            int index = i;
                            using (Paper.Box($"ToggleSwitch_{i}")
                                .StyleIf(!isOn, "toggle.off")
                                .StyleIf(isOn, "toggle.on")
                                .Transition(GuiProp.BackgroundColor, 0.25f, Paper.Easing.CubicInOut)
                                .OnClick((rect) => {
                                    toggleState[index] = !toggleState[index];
                                    Console.WriteLine($"Toggle {options[index]}: {!isOn}");
                                })
                                .Enter())
                            {
                                // Toggle dot
                                using (Paper.Box($"ToggleDot_{i}")
                                    .Width(24)
                                    .Height(24)
                                    .Rounded(20)
                                    .BackgroundColor(Color.White)
                                    .Transition(GuiProp.Left, 0.25f, Paper.Easing.CubicInOut)
                                    //.Style(BoxStyle.SolidRounded(Color.White, 12f))
                                    .PositionType(PositionType.SelfDirected)
                                    .Left(Paper.Pixels(isOn ? 32 : 4))
                                    .Top(Paper.Pixels(3))
                                    .Enter()) { }
                            }
                        }

                        // Add separator except for the last item
                        if (i < options.Length - 1)
                        {
                            Paper.Box($"Separator_{i}").Style("seperator");
                        }
                    }

                    // Save button
                    using (Paper.Box("SaveSettings")
                        .Height(50)
                        .Style("button.primary")
                        .Text(Text.Center("Save Changes", fontMedium, Color.White))
                        .Margin(20, 0, 20, 20)
                        .OnClick((rect) => Console.WriteLine("Save settings clicked"))
                        .Enter()) { }
                }
            }
        }
        private static void RenderWindowsTab()
        {
            using (Paper.Row("WindowsContent")
                .Margin(0, 0, 15, 0)
                .Enter())
            {
                // Settings content
                using (Paper.Column("SettingsOptions")
                    .BackgroundColor(cardBackground)
                    //.Style(BoxStyle.SolidRounded(cardBackground, 8f))
                    .Margin(15/2, 0, 0, 0)
                    .Clip()
                    .Enter())
                {
                    // Button to open windows
                    using (Paper.Box("OpenWindowsButton")
                        .Height(50)
                        .Margin(20)
                        .Text(Text.Center("Open Windows", fontMedium, textColor))
                        .Style("button.primary")
                        .OnClick((rect) => OpenWindows())
                        .Enter()) { }

                    TestWindows();
                }
            }
        }

        private static void OpenWindows()
        {
            isWindowAOpen = true;
            isWindowBOpen = true;
        }

        private static void RenderFooter()
        {
            using (Paper.Row("Footer")
                .Height(50)
                .Rounded(8)
                .BackgroundColor(cardBackground)
                //.Style(BoxStyle.SolidRoundedWithBorder(cardBackground, Color.FromArgb(30, 0, 0, 0), 4f, 1f))
                .Margin(15, 15, 0, 15)
                .Enter())
            {
                // Copyright
                using (Paper.Box("Copyright")
                    .Margin(15, 0, 0, 0)
                    .Text(Text.Left("© 2025 PaperUI Demo.", fontSmall, lightTextColor))
                    .Enter()) { }

                // FPS Counter
                Paper.Box("FPS").Text(Text.Left($"FPS: {1f / Paper.DeltaTime:F1}", fontSmall, lightTextColor));
                Paper.Box("NodeCounter").Text(Text.Left($"Nodes: {Paper.CountOfAllElements}", fontSmall, lightTextColor));
                Paper.Box("MS").Text(Text.Left($"Frame ms: {Paper.MillisecondsSpent}", fontSmall, lightTextColor));

                // Footer links
                string[] links = { "Terms", "Privacy", "Contact", "Help" };
                using (Paper.Row("FooterLinks")
                    .Enter())
                {
                    foreach (var link in links)
                    {
                        using (Paper.Box($"Link_{link}")
                            .Width(Paper.Stretch(1f / links.Length))
                            .Text(Text.Center(link, fontSmall, primaryColor))
                            .OnClick((rect) => Console.WriteLine($"Link {link} clicked"))
                            .Enter()) { }
                    }
                }
            }
        }
    }
}

```
Icons.cs

```cs
// Generated by https://github.com/juliettef/IconFontCppHeaders script GenerateIconFontCppHeaders.py for language C#
// from https://github.com/FortAwesome/Font-Awesome/raw/6.x/metadata/icons.yml
// for use with https://github.com/FortAwesome/Font-Awesome/blob/6.x/webfonts/fa-regular-400.ttf, https://github.com/FortAwesome/Font-Awesome/blob/6.x/webfonts/fa-solid-900.ttf

using System.Reflection;

namespace Shared
{
    public class Icons
    {
        public const string FontIconFileNameFAR = "fa-regular-400.ttf";
        public const string FontIconFileNameFAS = "fa-solid-900.ttf";

        private static readonly Dictionary<int, string> _iconCache = new Dictionary<int, string>();

        internal static string GetRandomIcon(int seed)
        {
            // Check if result for this seed is already in cache
            if (_iconCache.TryGetValue(seed, out string cachedIcon))
                return cachedIcon;

            var random = new Random(seed);

            // Use Reflection to get all public static fields
            var fields = typeof(Icons).GetFields(BindingFlags.Public | BindingFlags.Static)
                // Filter only unicode character fields (icons)
                .Where(field =>
                    field.FieldType == typeof(string) &&
                    !field.Name.StartsWith("FontIcon") &&
                    !field.Name.StartsWith("IconMin") &&
                    !field.Name.StartsWith("IconMax"))
                .ToArray();

            if (fields.Length == 0)
            {
                return string.Empty;
            }

            var randomField = fields[random.Next(fields.Length)];
            var randomIcon = randomField.GetValue(null)?.ToString();

            return randomIcon ?? string.Empty;
        }

        public const int IconMin = 0xe005;
        public const int IconMax16 = 0xf8ff;
        public const int IconMax = 0xf8ff;
        public const string Num0 = "\u0030";
        public const string Num1 = "\u0031";
        public const string Num2 = "\u0032";
        public const string Num3 = "\u0033";
        public const string Num4 = "\u0034";
        public const string Num5 = "\u0035";
        public const string Num6 = "\u0036";
        public const string Num7 = "\u0037";
        public const string Num8 = "\u0038";
        public const string Num9 = "\u0039";
        public const string A = "\u0041";
        public const string AddressBook = "\uf2b9";
        public const string AddressCard = "\uf2bb";
        public const string AlignCenter = "\uf037";
        public const string AlignJustify = "\uf039";
        public const string AlignLeft = "\uf036";
        public const string AlignRight = "\uf038";
        public const string Anchor = "\uf13d";
        public const string AnchorCircleCheck = "\ue4aa";
        public const string AnchorCircleExclamation = "\ue4ab";
        public const string AnchorCircleXmark = "\ue4ac";
        public const string AnchorLock = "\ue4ad";
        public const string AngleDown = "\uf107";
        public const string AngleLeft = "\uf104";
        public const string AngleRight = "\uf105";
        public const string AngleUp = "\uf106";
        public const string AnglesDown = "\uf103";
        public const string AnglesLeft = "\uf100";
        public const string AnglesRight = "\uf101";
        public const string AnglesUp = "\uf102";
        public const string Ankh = "\uf644";
        public const string AppleWhole = "\uf5d1";
        public const string Archway = "\uf557";
        public const string ArrowDown = "\uf063";
        public const string ArrowDown19 = "\uf162";
        public const string ArrowDown91 = "\uf886";
        public const string ArrowDownAZ = "\uf15d";
        public const string ArrowDownLong = "\uf175";
        public const string ArrowDownShortWide = "\uf884";
        public const string ArrowDownUpAcrossLine = "\ue4af";
        public const string ArrowDownUpLock = "\ue4b0";
        public const string ArrowDownWideShort = "\uf160";
        public const string ArrowDownZA = "\uf881";
        public const string ArrowLeft = "\uf060";
        public const string ArrowLeftLong = "\uf177";
        public const string ArrowPointer = "\uf245";
        public const string ArrowRight = "\uf061";
        public const string ArrowRightArrowLeft = "\uf0ec";
        public const string ArrowRightFromBracket = "\uf08b";
        public const string ArrowRightLong = "\uf178";
        public const string ArrowRightToBracket = "\uf090";
        public const string ArrowRightToCity = "\ue4b3";
        public const string ArrowRotateLeft = "\uf0e2";
        public const string ArrowRotateRight = "\uf01e";
        public const string ArrowTrendDown = "\ue097";
        public const string ArrowTrendUp = "\ue098";
        public const string ArrowTurnDown = "\uf149";
        public const string ArrowTurnUp = "\uf148";
        public const string ArrowUp = "\uf062";
        public const string ArrowUp19 = "\uf163";
        public const string ArrowUp91 = "\uf887";
        public const string ArrowUpAZ = "\uf15e";
        public const string ArrowUpFromBracket = "\ue09a";
        public const string ArrowUpFromGroundWater = "\ue4b5";
        public const string ArrowUpFromWaterPump = "\ue4b6";
        public const string ArrowUpLong = "\uf176";
        public const string ArrowUpRightDots = "\ue4b7";
        public const string ArrowUpRightFromSquare = "\uf08e";
        public const string ArrowUpShortWide = "\uf885";
        public const string ArrowUpWideShort = "\uf161";
        public const string ArrowUpZA = "\uf882";
        public const string ArrowsDownToLine = "\ue4b8";
        public const string ArrowsDownToPeople = "\ue4b9";
        public const string ArrowsLeftRight = "\uf07e";
        public const string ArrowsLeftRightToLine = "\ue4ba";
        public const string ArrowsRotate = "\uf021";
        public const string ArrowsSpin = "\ue4bb";
        public const string ArrowsSplitUpAndLeft = "\ue4bc";
        public const string ArrowsToCircle = "\ue4bd";
        public const string ArrowsToDot = "\ue4be";
        public const string ArrowsToEye = "\ue4bf";
        public const string ArrowsTurnRight = "\ue4c0";
        public const string ArrowsTurnToDots = "\ue4c1";
        public const string ArrowsUpDown = "\uf07d";
        public const string ArrowsUpDownLeftRight = "\uf047";
        public const string ArrowsUpToLine = "\ue4c2";
        public const string Asterisk = "\u002a";
        public const string At = "\u0040";
        public const string Atom = "\uf5d2";
        public const string AudioDescription = "\uf29e";
        public const string AustralSign = "\ue0a9";
        public const string Award = "\uf559";
        public const string B = "\u0042";
        public const string Baby = "\uf77c";
        public const string BabyCarriage = "\uf77d";
        public const string Backward = "\uf04a";
        public const string BackwardFast = "\uf049";
        public const string BackwardStep = "\uf048";
        public const string Bacon = "\uf7e5";
        public const string Bacteria = "\ue059";
        public const string Bacterium = "\ue05a";
        public const string BagShopping = "\uf290";
        public const string Bahai = "\uf666";
        public const string BahtSign = "\ue0ac";
        public const string Ban = "\uf05e";
        public const string BanSmoking = "\uf54d";
        public const string Bandage = "\uf462";
        public const string BangladeshiTakaSign = "\ue2e6";
        public const string Barcode = "\uf02a";
        public const string Bars = "\uf0c9";
        public const string BarsProgress = "\uf828";
        public const string BarsStaggered = "\uf550";
        public const string Baseball = "\uf433";
        public const string BaseballBatBall = "\uf432";
        public const string BasketShopping = "\uf291";
        public const string Basketball = "\uf434";
        public const string Bath = "\uf2cd";
        public const string BatteryEmpty = "\uf244";
        public const string BatteryFull = "\uf240";
        public const string BatteryHalf = "\uf242";
        public const string BatteryQuarter = "\uf243";
        public const string BatteryThreeQuarters = "\uf241";
        public const string Bed = "\uf236";
        public const string BedPulse = "\uf487";
        public const string BeerMugEmpty = "\uf0fc";
        public const string Bell = "\uf0f3";
        public const string BellConcierge = "\uf562";
        public const string BellSlash = "\uf1f6";
        public const string BezierCurve = "\uf55b";
        public const string Bicycle = "\uf206";
        public const string Binoculars = "\uf1e5";
        public const string Biohazard = "\uf780";
        public const string BitcoinSign = "\ue0b4";
        public const string Blender = "\uf517";
        public const string BlenderPhone = "\uf6b6";
        public const string Blog = "\uf781";
        public const string Bold = "\uf032";
        public const string Bolt = "\uf0e7";
        public const string BoltLightning = "\ue0b7";
        public const string Bomb = "\uf1e2";
        public const string Bone = "\uf5d7";
        public const string Bong = "\uf55c";
        public const string Book = "\uf02d";
        public const string BookAtlas = "\uf558";
        public const string BookBible = "\uf647";
        public const string BookBookmark = "\ue0bb";
        public const string BookJournalWhills = "\uf66a";
        public const string BookMedical = "\uf7e6";
        public const string BookOpen = "\uf518";
        public const string BookOpenReader = "\uf5da";
        public const string BookQuran = "\uf687";
        public const string BookSkull = "\uf6b7";
        public const string BookTanakh = "\uf827";
        public const string Bookmark = "\uf02e";
        public const string BorderAll = "\uf84c";
        public const string BorderNone = "\uf850";
        public const string BorderTopLeft = "\uf853";
        public const string BoreHole = "\ue4c3";
        public const string BottleDroplet = "\ue4c4";
        public const string BottleWater = "\ue4c5";
        public const string BowlFood = "\ue4c6";
        public const string BowlRice = "\ue2eb";
        public const string BowlingBall = "\uf436";
        public const string Box = "\uf466";
        public const string BoxArchive = "\uf187";
        public const string BoxOpen = "\uf49e";
        public const string BoxTissue = "\ue05b";
        public const string BoxesPacking = "\ue4c7";
        public const string BoxesStacked = "\uf468";
        public const string Braille = "\uf2a1";
        public const string Brain = "\uf5dc";
        public const string BrazilianRealSign = "\ue46c";
        public const string BreadSlice = "\uf7ec";
        public const string Bridge = "\ue4c8";
        public const string BridgeCircleCheck = "\ue4c9";
        public const string BridgeCircleExclamation = "\ue4ca";
        public const string BridgeCircleXmark = "\ue4cb";
        public const string BridgeLock = "\ue4cc";
        public const string BridgeWater = "\ue4ce";
        public const string Briefcase = "\uf0b1";
        public const string BriefcaseMedical = "\uf469";
        public const string Broom = "\uf51a";
        public const string BroomBall = "\uf458";
        public const string Brush = "\uf55d";
        public const string Bucket = "\ue4cf";
        public const string Bug = "\uf188";
        public const string BugSlash = "\ue490";
        public const string Bugs = "\ue4d0";
        public const string Building = "\uf1ad";
        public const string BuildingCircleArrowRight = "\ue4d1";
        public const string BuildingCircleCheck = "\ue4d2";
        public const string BuildingCircleExclamation = "\ue4d3";
        public const string BuildingCircleXmark = "\ue4d4";
        public const string BuildingColumns = "\uf19c";
        public const string BuildingFlag = "\ue4d5";
        public const string BuildingLock = "\ue4d6";
        public const string BuildingNgo = "\ue4d7";
        public const string BuildingShield = "\ue4d8";
        public const string BuildingUn = "\ue4d9";
        public const string BuildingUser = "\ue4da";
        public const string BuildingWheat = "\ue4db";
        public const string Bullhorn = "\uf0a1";
        public const string Bullseye = "\uf140";
        public const string Burger = "\uf805";
        public const string Burst = "\ue4dc";
        public const string Bus = "\uf207";
        public const string BusSimple = "\uf55e";
        public const string BusinessTime = "\uf64a";
        public const string C = "\u0043";
        public const string CableCar = "\uf7da";
        public const string CakeCandles = "\uf1fd";
        public const string Calculator = "\uf1ec";
        public const string Calendar = "\uf133";
        public const string CalendarCheck = "\uf274";
        public const string CalendarDay = "\uf783";
        public const string CalendarDays = "\uf073";
        public const string CalendarMinus = "\uf272";
        public const string CalendarPlus = "\uf271";
        public const string CalendarWeek = "\uf784";
        public const string CalendarXmark = "\uf273";
        public const string Camera = "\uf030";
        public const string CameraRetro = "\uf083";
        public const string CameraRotate = "\ue0d8";
        public const string Campground = "\uf6bb";
        public const string CandyCane = "\uf786";
        public const string Cannabis = "\uf55f";
        public const string Capsules = "\uf46b";
        public const string Car = "\uf1b9";
        public const string CarBattery = "\uf5df";
        public const string CarBurst = "\uf5e1";
        public const string CarOn = "\ue4dd";
        public const string CarRear = "\uf5de";
        public const string CarSide = "\uf5e4";
        public const string CarTunnel = "\ue4de";
        public const string Caravan = "\uf8ff";
        public const string CaretDown = "\uf0d7";
        public const string CaretLeft = "\uf0d9";
        public const string CaretRight = "\uf0da";
        public const string CaretUp = "\uf0d8";
        public const string Carrot = "\uf787";
        public const string CartArrowDown = "\uf218";
        public const string CartFlatbed = "\uf474";
        public const string CartFlatbedSuitcase = "\uf59d";
        public const string CartPlus = "\uf217";
        public const string CartShopping = "\uf07a";
        public const string CashRegister = "\uf788";
        public const string Cat = "\uf6be";
        public const string CediSign = "\ue0df";
        public const string CentSign = "\ue3f5";
        public const string Certificate = "\uf0a3";
        public const string Chair = "\uf6c0";
        public const string Chalkboard = "\uf51b";
        public const string ChalkboardUser = "\uf51c";
        public const string ChampagneGlasses = "\uf79f";
        public const string ChargingStation = "\uf5e7";
        public const string ChartArea = "\uf1fe";
        public const string ChartBar = "\uf080";
        public const string ChartColumn = "\ue0e3";
        public const string ChartGantt = "\ue0e4";
        public const string ChartLine = "\uf201";
        public const string ChartPie = "\uf200";
        public const string ChartSimple = "\ue473";
        public const string Check = "\uf00c";
        public const string CheckDouble = "\uf560";
        public const string CheckToSlot = "\uf772";
        public const string Cheese = "\uf7ef";
        public const string Chess = "\uf439";
        public const string ChessBishop = "\uf43a";
        public const string ChessBoard = "\uf43c";
        public const string ChessKing = "\uf43f";
        public const string ChessKnight = "\uf441";
        public const string ChessPawn = "\uf443";
        public const string ChessQueen = "\uf445";
        public const string ChessRook = "\uf447";
        public const string ChevronDown = "\uf078";
        public const string ChevronLeft = "\uf053";
        public const string ChevronRight = "\uf054";
        public const string ChevronUp = "\uf077";
        public const string Child = "\uf1ae";
        public const string ChildCombatant = "\ue4e0";
        public const string ChildDress = "\ue59c";
        public const string ChildReaching = "\ue59d";
        public const string Children = "\ue4e1";
        public const string Church = "\uf51d";
        public const string Circle = "\uf111";
        public const string CircleArrowDown = "\uf0ab";
        public const string CircleArrowLeft = "\uf0a8";
        public const string CircleArrowRight = "\uf0a9";
        public const string CircleArrowUp = "\uf0aa";
        public const string CircleCheck = "\uf058";
        public const string CircleChevronDown = "\uf13a";
        public const string CircleChevronLeft = "\uf137";
        public const string CircleChevronRight = "\uf138";
        public const string CircleChevronUp = "\uf139";
        public const string CircleDollarToSlot = "\uf4b9";
        public const string CircleDot = "\uf192";
        public const string CircleDown = "\uf358";
        public const string CircleExclamation = "\uf06a";
        public const string CircleH = "\uf47e";
        public const string CircleHalfStroke = "\uf042";
        public const string CircleInfo = "\uf05a";
        public const string CircleLeft = "\uf359";
        public const string CircleMinus = "\uf056";
        public const string CircleNodes = "\ue4e2";
        public const string CircleNotch = "\uf1ce";
        public const string CirclePause = "\uf28b";
        public const string CirclePlay = "\uf144";
        public const string CirclePlus = "\uf055";
        public const string CircleQuestion = "\uf059";
        public const string CircleRadiation = "\uf7ba";
        public const string CircleRight = "\uf35a";
        public const string CircleStop = "\uf28d";
        public const string CircleUp = "\uf35b";
        public const string CircleUser = "\uf2bd";
        public const string CircleXmark = "\uf057";
        public const string City = "\uf64f";
        public const string Clapperboard = "\ue131";
        public const string Clipboard = "\uf328";
        public const string ClipboardCheck = "\uf46c";
        public const string ClipboardList = "\uf46d";
        public const string ClipboardQuestion = "\ue4e3";
        public const string ClipboardUser = "\uf7f3";
        public const string Clock = "\uf017";
        public const string ClockRotateLeft = "\uf1da";
        public const string Clone = "\uf24d";
        public const string ClosedCaptioning = "\uf20a";
        public const string Cloud = "\uf0c2";
        public const string CloudArrowDown = "\uf0ed";
        public const string CloudArrowUp = "\uf0ee";
        public const string CloudBolt = "\uf76c";
        public const string CloudMeatball = "\uf73b";
        public const string CloudMoon = "\uf6c3";
        public const string CloudMoonRain = "\uf73c";
        public const string CloudRain = "\uf73d";
        public const string CloudShowersHeavy = "\uf740";
        public const string CloudShowersWater = "\ue4e4";
        public const string CloudSun = "\uf6c4";
        public const string CloudSunRain = "\uf743";
        public const string Clover = "\ue139";
        public const string Code = "\uf121";
        public const string CodeBranch = "\uf126";
        public const string CodeCommit = "\uf386";
        public const string CodeCompare = "\ue13a";
        public const string CodeFork = "\ue13b";
        public const string CodeMerge = "\uf387";
        public const string CodePullRequest = "\ue13c";
        public const string Coins = "\uf51e";
        public const string ColonSign = "\ue140";
        public const string Comment = "\uf075";
        public const string CommentDollar = "\uf651";
        public const string CommentDots = "\uf4ad";
        public const string CommentMedical = "\uf7f5";
        public const string CommentSlash = "\uf4b3";
        public const string CommentSms = "\uf7cd";
        public const string Comments = "\uf086";
        public const string CommentsDollar = "\uf653";
        public const string CompactDisc = "\uf51f";
        public const string Compass = "\uf14e";
        public const string CompassDrafting = "\uf568";
        public const string Compress = "\uf066";
        public const string Computer = "\ue4e5";
        public const string ComputerMouse = "\uf8cc";
        public const string Cookie = "\uf563";
        public const string CookieBite = "\uf564";
        public const string Copy = "\uf0c5";
        public const string Copyright = "\uf1f9";
        public const string Couch = "\uf4b8";
        public const string Cow = "\uf6c8";
        public const string CreditCard = "\uf09d";
        public const string Crop = "\uf125";
        public const string CropSimple = "\uf565";
        public const string Cross = "\uf654";
        public const string Crosshairs = "\uf05b";
        public const string Crow = "\uf520";
        public const string Crown = "\uf521";
        public const string Crutch = "\uf7f7";
        public const string CruzeiroSign = "\ue152";
        public const string Cube = "\uf1b2";
        public const string Cubes = "\uf1b3";
        public const string CubesStacked = "\ue4e6";
        public const string D = "\u0044";
        public const string Database = "\uf1c0";
        public const string DeleteLeft = "\uf55a";
        public const string Democrat = "\uf747";
        public const string Desktop = "\uf390";
        public const string Dharmachakra = "\uf655";
        public const string DiagramNext = "\ue476";
        public const string DiagramPredecessor = "\ue477";
        public const string DiagramProject = "\uf542";
        public const string DiagramSuccessor = "\ue47a";
        public const string Diamond = "\uf219";
        public const string DiamondTurnRight = "\uf5eb";
        public const string Dice = "\uf522";
        public const string DiceD20 = "\uf6cf";
        public const string DiceD6 = "\uf6d1";
        public const string DiceFive = "\uf523";
        public const string DiceFour = "\uf524";
        public const string DiceOne = "\uf525";
        public const string DiceSix = "\uf526";
        public const string DiceThree = "\uf527";
        public const string DiceTwo = "\uf528";
        public const string Disease = "\uf7fa";
        public const string Display = "\ue163";
        public const string Divide = "\uf529";
        public const string Dna = "\uf471";
        public const string Dog = "\uf6d3";
        public const string DollarSign = "\u0024";
        public const string Dolly = "\uf472";
        public const string DongSign = "\ue169";
        public const string DoorClosed = "\uf52a";
        public const string DoorOpen = "\uf52b";
        public const string Dove = "\uf4ba";
        public const string DownLeftAndUpRightToCenter = "\uf422";
        public const string DownLong = "\uf309";
        public const string Download = "\uf019";
        public const string Dragon = "\uf6d5";
        public const string DrawPolygon = "\uf5ee";
        public const string Droplet = "\uf043";
        public const string DropletSlash = "\uf5c7";
        public const string Drum = "\uf569";
        public const string DrumSteelpan = "\uf56a";
        public const string DrumstickBite = "\uf6d7";
        public const string Dumbbell = "\uf44b";
        public const string Dumpster = "\uf793";
        public const string DumpsterFire = "\uf794";
        public const string Dungeon = "\uf6d9";
        public const string E = "\u0045";
        public const string EarDeaf = "\uf2a4";
        public const string EarListen = "\uf2a2";
        public const string EarthAfrica = "\uf57c";
        public const string EarthAmericas = "\uf57d";
        public const string EarthAsia = "\uf57e";
        public const string EarthEurope = "\uf7a2";
        public const string EarthOceania = "\ue47b";
        public const string Egg = "\uf7fb";
        public const string Eject = "\uf052";
        public const string Elevator = "\ue16d";
        public const string Ellipsis = "\uf141";
        public const string EllipsisVertical = "\uf142";
        public const string Envelope = "\uf0e0";
        public const string EnvelopeCircleCheck = "\ue4e8";
        public const string EnvelopeOpen = "\uf2b6";
        public const string EnvelopeOpenText = "\uf658";
        public const string EnvelopesBulk = "\uf674";
        public new const string Equals = "\u003d";
        public const string Eraser = "\uf12d";
        public const string Ethernet = "\uf796";
        public const string EuroSign = "\uf153";
        public const string Exclamation = "\u0021";
        public const string Expand = "\uf065";
        public const string Explosion = "\ue4e9";
        public const string Eye = "\uf06e";
        public const string EyeDropper = "\uf1fb";
        public const string EyeLowVision = "\uf2a8";
        public const string EyeSlash = "\uf070";
        public const string F = "\u0046";
        public const string FaceAngry = "\uf556";
        public const string FaceDizzy = "\uf567";
        public const string FaceFlushed = "\uf579";
        public const string FaceFrown = "\uf119";
        public const string FaceFrownOpen = "\uf57a";
        public const string FaceGrimace = "\uf57f";
        public const string FaceGrin = "\uf580";
        public const string FaceGrinBeam = "\uf582";
        public const string FaceGrinBeamSweat = "\uf583";
        public const string FaceGrinHearts = "\uf584";
        public const string FaceGrinSquint = "\uf585";
        public const string FaceGrinSquintTears = "\uf586";
        public const string FaceGrinStars = "\uf587";
        public const string FaceGrinTears = "\uf588";
        public const string FaceGrinTongue = "\uf589";
        public const string FaceGrinTongueSquint = "\uf58a";
        public const string FaceGrinTongueWink = "\uf58b";
        public const string FaceGrinWide = "\uf581";
        public const string FaceGrinWink = "\uf58c";
        public const string FaceKiss = "\uf596";
        public const string FaceKissBeam = "\uf597";
        public const string FaceKissWinkHeart = "\uf598";
        public const string FaceLaugh = "\uf599";
        public const string FaceLaughBeam = "\uf59a";
        public const string FaceLaughSquint = "\uf59b";
        public const string FaceLaughWink = "\uf59c";
        public const string FaceMeh = "\uf11a";
        public const string FaceMehBlank = "\uf5a4";
        public const string FaceRollingEyes = "\uf5a5";
        public const string FaceSadCry = "\uf5b3";
        public const string FaceSadTear = "\uf5b4";
        public const string FaceSmile = "\uf118";
        public const string FaceSmileBeam = "\uf5b8";
        public const string FaceSmileWink = "\uf4da";
        public const string FaceSurprise = "\uf5c2";
        public const string FaceTired = "\uf5c8";
        public const string Fan = "\uf863";
        public const string Faucet = "\ue005";
        public const string FaucetDrip = "\ue006";
        public const string Fax = "\uf1ac";
        public const string Feather = "\uf52d";
        public const string FeatherPointed = "\uf56b";
        public const string Ferry = "\ue4ea";
        public const string File = "\uf15b";
        public const string FileArrowDown = "\uf56d";
        public const string FileArrowUp = "\uf574";
        public const string FileAudio = "\uf1c7";
        public const string FileCircleCheck = "\ue5a0";
        public const string FileCircleExclamation = "\ue4eb";
        public const string FileCircleMinus = "\ue4ed";
        public const string FileCirclePlus = "\ue494";
        public const string FileCircleQuestion = "\ue4ef";
        public const string FileCircleXmark = "\ue5a1";
        public const string FileCode = "\uf1c9";
        public const string FileContract = "\uf56c";
        public const string FileCsv = "\uf6dd";
        public const string FileExcel = "\uf1c3";
        public const string FileExport = "\uf56e";
        public const string FileImage = "\uf1c5";
        public const string FileImport = "\uf56f";
        public const string FileInvoice = "\uf570";
        public const string FileInvoiceDollar = "\uf571";
        public const string FileLines = "\uf15c";
        public const string FileMedical = "\uf477";
        public const string FilePdf = "\uf1c1";
        public const string FilePen = "\uf31c";
        public const string FilePowerpoint = "\uf1c4";
        public const string FilePrescription = "\uf572";
        public const string FileShield = "\ue4f0";
        public const string FileSignature = "\uf573";
        public const string FileVideo = "\uf1c8";
        public const string FileWaveform = "\uf478";
        public const string FileWord = "\uf1c2";
        public const string FileZipper = "\uf1c6";
        public const string Fill = "\uf575";
        public const string FillDrip = "\uf576";
        public const string Film = "\uf008";
        public const string Filter = "\uf0b0";
        public const string FilterCircleDollar = "\uf662";
        public const string FilterCircleXmark = "\ue17b";
        public const string Fingerprint = "\uf577";
        public const string Fire = "\uf06d";
        public const string FireBurner = "\ue4f1";
        public const string FireExtinguisher = "\uf134";
        public const string FireFlameCurved = "\uf7e4";
        public const string FireFlameSimple = "\uf46a";
        public const string Fish = "\uf578";
        public const string FishFins = "\ue4f2";
        public const string Flag = "\uf024";
        public const string FlagCheckered = "\uf11e";
        public const string FlagUsa = "\uf74d";
        public const string Flask = "\uf0c3";
        public const string FlaskVial = "\ue4f3";
        public const string FloppyDisk = "\uf0c7";
        public const string FlorinSign = "\ue184";
        public const string Folder = "\uf07b";
        public const string FolderClosed = "\ue185";
        public const string FolderMinus = "\uf65d";
        public const string FolderOpen = "\uf07c";
        public const string FolderPlus = "\uf65e";
        public const string FolderTree = "\uf802";
        public const string Font = "\uf031";
        public const string FontAwesome = "\uf2b4";
        public const string Football = "\uf44e";
        public const string Forward = "\uf04e";
        public const string ForwardFast = "\uf050";
        public const string ForwardStep = "\uf051";
        public const string FrancSign = "\ue18f";
        public const string Frog = "\uf52e";
        public const string Futbol = "\uf1e3";
        public const string G = "\u0047";
        public const string Gamepad = "\uf11b";
        public const string GasPump = "\uf52f";
        public const string Gauge = "\uf624";
        public const string GaugeHigh = "\uf625";
        public const string GaugeSimple = "\uf629";
        public const string GaugeSimpleHigh = "\uf62a";
        public const string Gavel = "\uf0e3";
        public const string Gear = "\uf013";
        public const string Gears = "\uf085";
        public const string Gem = "\uf3a5";
        public const string Genderless = "\uf22d";
        public const string Ghost = "\uf6e2";
        public const string Gift = "\uf06b";
        public const string Gifts = "\uf79c";
        public const string GlassWater = "\ue4f4";
        public const string GlassWaterDroplet = "\ue4f5";
        public const string Glasses = "\uf530";
        public const string Globe = "\uf0ac";
        public const string GolfBallTee = "\uf450";
        public const string Gopuram = "\uf664";
        public const string GraduationCap = "\uf19d";
        public const string GreaterThan = "\u003e";
        public const string GreaterThanEqual = "\uf532";
        public const string Grip = "\uf58d";
        public const string GripLines = "\uf7a4";
        public const string GripLinesVertical = "\uf7a5";
        public const string GripVertical = "\uf58e";
        public const string GroupArrowsRotate = "\ue4f6";
        public const string GuaraniSign = "\ue19a";
        public const string Guitar = "\uf7a6";
        public const string Gun = "\ue19b";
        public const string H = "\u0048";
        public const string Hammer = "\uf6e3";
        public const string Hamsa = "\uf665";
        public const string Hand = "\uf256";
        public const string HandBackFist = "\uf255";
        public const string HandDots = "\uf461";
        public const string HandFist = "\uf6de";
        public const string HandHolding = "\uf4bd";
        public const string HandHoldingDollar = "\uf4c0";
        public const string HandHoldingDroplet = "\uf4c1";
        public const string HandHoldingHand = "\ue4f7";
        public const string HandHoldingHeart = "\uf4be";
        public const string HandHoldingMedical = "\ue05c";
        public const string HandLizard = "\uf258";
        public const string HandMiddleFinger = "\uf806";
        public const string HandPeace = "\uf25b";
        public const string HandPointDown = "\uf0a7";
        public const string HandPointLeft = "\uf0a5";
        public const string HandPointRight = "\uf0a4";
        public const string HandPointUp = "\uf0a6";
        public const string HandPointer = "\uf25a";
        public const string HandScissors = "\uf257";
        public const string HandSparkles = "\ue05d";
        public const string HandSpock = "\uf259";
        public const string Handcuffs = "\ue4f8";
        public const string Hands = "\uf2a7";
        public const string HandsAslInterpreting = "\uf2a3";
        public const string HandsBound = "\ue4f9";
        public const string HandsBubbles = "\ue05e";
        public const string HandsClapping = "\ue1a8";
        public const string HandsHolding = "\uf4c2";
        public const string HandsHoldingChild = "\ue4fa";
        public const string HandsHoldingCircle = "\ue4fb";
        public const string HandsPraying = "\uf684";
        public const string Handshake = "\uf2b5";
        public const string HandshakeAngle = "\uf4c4";
        public const string HandshakeSimple = "\uf4c6";
        public const string HandshakeSimpleSlash = "\ue05f";
        public const string HandshakeSlash = "\ue060";
        public const string Hanukiah = "\uf6e6";
        public const string HardDrive = "\uf0a0";
        public const string Hashtag = "\u0023";
        public const string HatCowboy = "\uf8c0";
        public const string HatCowboySide = "\uf8c1";
        public const string HatWizard = "\uf6e8";
        public const string HeadSideCough = "\ue061";
        public const string HeadSideCoughSlash = "\ue062";
        public const string HeadSideMask = "\ue063";
        public const string HeadSideVirus = "\ue064";
        public const string Heading = "\uf1dc";
        public const string Headphones = "\uf025";
        public const string HeadphonesSimple = "\uf58f";
        public const string Headset = "\uf590";
        public const string Heart = "\uf004";
        public const string HeartCircleBolt = "\ue4fc";
        public const string HeartCircleCheck = "\ue4fd";
        public const string HeartCircleExclamation = "\ue4fe";
        public const string HeartCircleMinus = "\ue4ff";
        public const string HeartCirclePlus = "\ue500";
        public const string HeartCircleXmark = "\ue501";
        public const string HeartCrack = "\uf7a9";
        public const string HeartPulse = "\uf21e";
        public const string Helicopter = "\uf533";
        public const string HelicopterSymbol = "\ue502";
        public const string HelmetSafety = "\uf807";
        public const string HelmetUn = "\ue503";
        public const string Highlighter = "\uf591";
        public const string HillAvalanche = "\ue507";
        public const string HillRockslide = "\ue508";
        public const string Hippo = "\uf6ed";
        public const string HockeyPuck = "\uf453";
        public const string HollyBerry = "\uf7aa";
        public const string Horse = "\uf6f0";
        public const string HorseHead = "\uf7ab";
        public const string Hospital = "\uf0f8";
        public const string HospitalUser = "\uf80d";
        public const string HotTubPerson = "\uf593";
        public const string Hotdog = "\uf80f";
        public const string Hotel = "\uf594";
        public const string Hourglass = "\uf254";
        public const string HourglassEnd = "\uf253";
        public const string HourglassHalf = "\uf252";
        public const string HourglassStart = "\uf251";
        public const string House = "\uf015";
        public const string HouseChimney = "\ue3af";
        public const string HouseChimneyCrack = "\uf6f1";
        public const string HouseChimneyMedical = "\uf7f2";
        public const string HouseChimneyUser = "\ue065";
        public const string HouseChimneyWindow = "\ue00d";
        public const string HouseCircleCheck = "\ue509";
        public const string HouseCircleExclamation = "\ue50a";
        public const string HouseCircleXmark = "\ue50b";
        public const string HouseCrack = "\ue3b1";
        public const string HouseFire = "\ue50c";
        public const string HouseFlag = "\ue50d";
        public const string HouseFloodWater = "\ue50e";
        public const string HouseFloodWaterCircleArrowRight = "\ue50f";
        public const string HouseLaptop = "\ue066";
        public const string HouseLock = "\ue510";
        public const string HouseMedical = "\ue3b2";
        public const string HouseMedicalCircleCheck = "\ue511";
        public const string HouseMedicalCircleExclamation = "\ue512";
        public const string HouseMedicalCircleXmark = "\ue513";
        public const string HouseMedicalFlag = "\ue514";
        public const string HouseSignal = "\ue012";
        public const string HouseTsunami = "\ue515";
        public const string HouseUser = "\ue1b0";
        public const string HryvniaSign = "\uf6f2";
        public const string Hurricane = "\uf751";
        public const string I = "\u0049";
        public const string ICursor = "\uf246";
        public const string IceCream = "\uf810";
        public const string Icicles = "\uf7ad";
        public const string _Icons = "\uf86d";
        public const string IdBadge = "\uf2c1";
        public const string IdCard = "\uf2c2";
        public const string IdCardClip = "\uf47f";
        public const string Igloo = "\uf7ae";
        public const string Image = "\uf03e";
        public const string ImagePortrait = "\uf3e0";
        public const string Images = "\uf302";
        public const string Inbox = "\uf01c";
        public const string Indent = "\uf03c";
        public const string IndianRupeeSign = "\ue1bc";
        public const string Industry = "\uf275";
        public const string Infinity = "\uf534";
        public const string Info = "\uf129";
        public const string Italic = "\uf033";
        public const string J = "\u004a";
        public const string Jar = "\ue516";
        public const string JarWheat = "\ue517";
        public const string Jedi = "\uf669";
        public const string JetFighter = "\uf0fb";
        public const string JetFighterUp = "\ue518";
        public const string Joint = "\uf595";
        public const string JugDetergent = "\ue519";
        public const string K = "\u004b";
        public const string Kaaba = "\uf66b";
        public const string Key = "\uf084";
        public const string Keyboard = "\uf11c";
        public const string Khanda = "\uf66d";
        public const string KipSign = "\ue1c4";
        public const string KitMedical = "\uf479";
        public const string KitchenSet = "\ue51a";
        public const string KiwiBird = "\uf535";
        public const string L = "\u004c";
        public const string LandMineOn = "\ue51b";
        public const string Landmark = "\uf66f";
        public const string LandmarkDome = "\uf752";
        public const string LandmarkFlag = "\ue51c";
        public const string Language = "\uf1ab";
        public const string Laptop = "\uf109";
        public const string LaptopCode = "\uf5fc";
        public const string LaptopFile = "\ue51d";
        public const string LaptopMedical = "\uf812";
        public const string LariSign = "\ue1c8";
        public const string LayerGroup = "\uf5fd";
        public const string Leaf = "\uf06c";
        public const string LeftLong = "\uf30a";
        public const string LeftRight = "\uf337";
        public const string Lemon = "\uf094";
        public const string LessThan = "\u003c";
        public const string LessThanEqual = "\uf537";
        public const string LifeRing = "\uf1cd";
        public const string Lightbulb = "\uf0eb";
        public const string LinesLeaning = "\ue51e";
        public const string Link = "\uf0c1";
        public const string LinkSlash = "\uf127";
        public const string LiraSign = "\uf195";
        public const string List = "\uf03a";
        public const string ListCheck = "\uf0ae";
        public const string ListOl = "\uf0cb";
        public const string ListUl = "\uf0ca";
        public const string LitecoinSign = "\ue1d3";
        public const string LocationArrow = "\uf124";
        public const string LocationCrosshairs = "\uf601";
        public const string LocationDot = "\uf3c5";
        public const string LocationPin = "\uf041";
        public const string LocationPinLock = "\ue51f";
        public const string Lock = "\uf023";
        public const string LockOpen = "\uf3c1";
        public const string Locust = "\ue520";
        public const string Lungs = "\uf604";
        public const string LungsVirus = "\ue067";
        public const string M = "\u004d";
        public const string Magnet = "\uf076";
        public const string MagnifyingGlass = "\uf002";
        public const string MagnifyingGlassArrowRight = "\ue521";
        public const string MagnifyingGlassChart = "\ue522";
        public const string MagnifyingGlassDollar = "\uf688";
        public const string MagnifyingGlassLocation = "\uf689";
        public const string MagnifyingGlassMinus = "\uf010";
        public const string MagnifyingGlassPlus = "\uf00e";
        public const string ManatSign = "\ue1d5";
        public const string Map = "\uf279";
        public const string MapLocation = "\uf59f";
        public const string MapLocationDot = "\uf5a0";
        public const string MapPin = "\uf276";
        public const string Marker = "\uf5a1";
        public const string Mars = "\uf222";
        public const string MarsAndVenus = "\uf224";
        public const string MarsAndVenusBurst = "\ue523";
        public const string MarsDouble = "\uf227";
        public const string MarsStroke = "\uf229";
        public const string MarsStrokeRight = "\uf22b";
        public const string MarsStrokeUp = "\uf22a";
        public const string MartiniGlass = "\uf57b";
        public const string MartiniGlassCitrus = "\uf561";
        public const string MartiniGlassEmpty = "\uf000";
        public const string Mask = "\uf6fa";
        public const string MaskFace = "\ue1d7";
        public const string MaskVentilator = "\ue524";
        public const string MasksTheater = "\uf630";
        public const string MattressPillow = "\ue525";
        public const string Maximize = "\uf31e";
        public const string Medal = "\uf5a2";
        public const string Memory = "\uf538";
        public const string Menorah = "\uf676";
        public const string Mercury = "\uf223";
        public const string Message = "\uf27a";
        public const string Meteor = "\uf753";
        public const string Microchip = "\uf2db";
        public const string Microphone = "\uf130";
        public const string MicrophoneLines = "\uf3c9";
        public const string MicrophoneLinesSlash = "\uf539";
        public const string MicrophoneSlash = "\uf131";
        public const string Microscope = "\uf610";
        public const string MillSign = "\ue1ed";
        public const string Minimize = "\uf78c";
        public const string Minus = "\uf068";
        public const string Mitten = "\uf7b5";
        public const string Mobile = "\uf3ce";
        public const string MobileButton = "\uf10b";
        public const string MobileRetro = "\ue527";
        public const string MobileScreen = "\uf3cf";
        public const string MobileScreenButton = "\uf3cd";
        public const string MoneyBill = "\uf0d6";
        public const string MoneyBill1 = "\uf3d1";
        public const string MoneyBill1Wave = "\uf53b";
        public const string MoneyBillTransfer = "\ue528";
        public const string MoneyBillTrendUp = "\ue529";
        public const string MoneyBillWave = "\uf53a";
        public const string MoneyBillWheat = "\ue52a";
        public const string MoneyBills = "\ue1f3";
        public const string MoneyCheck = "\uf53c";
        public const string MoneyCheckDollar = "\uf53d";
        public const string Monument = "\uf5a6";
        public const string Moon = "\uf186";
        public const string MortarPestle = "\uf5a7";
        public const string Mosque = "\uf678";
        public const string Mosquito = "\ue52b";
        public const string MosquitoNet = "\ue52c";
        public const string Motorcycle = "\uf21c";
        public const string Mound = "\ue52d";
        public const string Mountain = "\uf6fc";
        public const string MountainCity = "\ue52e";
        public const string MountainSun = "\ue52f";
        public const string MugHot = "\uf7b6";
        public const string MugSaucer = "\uf0f4";
        public const string Music = "\uf001";
        public const string N = "\u004e";
        public const string NairaSign = "\ue1f6";
        public const string NetworkWired = "\uf6ff";
        public const string Neuter = "\uf22c";
        public const string Newspaper = "\uf1ea";
        public const string NotEqual = "\uf53e";
        public const string Notdef = "\ue1fe";
        public const string NoteSticky = "\uf249";
        public const string NotesMedical = "\uf481";
        public const string O = "\u004f";
        public const string ObjectGroup = "\uf247";
        public const string ObjectUngroup = "\uf248";
        public const string OilCan = "\uf613";
        public const string OilWell = "\ue532";
        public const string Om = "\uf679";
        public const string Otter = "\uf700";
        public const string Outdent = "\uf03b";
        public const string P = "\u0050";
        public const string Pager = "\uf815";
        public const string PaintRoller = "\uf5aa";
        public const string Paintbrush = "\uf1fc";
        public const string Palette = "\uf53f";
        public const string Pallet = "\uf482";
        public const string Panorama = "\ue209";
        public const string PaperPlane = "\uf1d8";
        public const string Paperclip = "\uf0c6";
        public const string ParachuteBox = "\uf4cd";
        public const string Paragraph = "\uf1dd";
        public const string Passport = "\uf5ab";
        public const string Paste = "\uf0ea";
        public const string Pause = "\uf04c";
        public const string Paw = "\uf1b0";
        public const string Peace = "\uf67c";
        public const string Pen = "\uf304";
        public const string PenClip = "\uf305";
        public const string PenFancy = "\uf5ac";
        public const string PenNib = "\uf5ad";
        public const string PenRuler = "\uf5ae";
        public const string PenToSquare = "\uf044";
        public const string Pencil = "\uf303";
        public const string PeopleArrows = "\ue068";
        public const string PeopleCarryBox = "\uf4ce";
        public const string PeopleGroup = "\ue533";
        public const string PeopleLine = "\ue534";
        public const string PeoplePulling = "\ue535";
        public const string PeopleRobbery = "\ue536";
        public const string PeopleRoof = "\ue537";
        public const string PepperHot = "\uf816";
        public const string Percent = "\u0025";
        public const string Person = "\uf183";
        public const string PersonArrowDownToLine = "\ue538";
        public const string PersonArrowUpFromLine = "\ue539";
        public const string PersonBiking = "\uf84a";
        public const string PersonBooth = "\uf756";
        public const string PersonBreastfeeding = "\ue53a";
        public const string PersonBurst = "\ue53b";
        public const string PersonCane = "\ue53c";
        public const string PersonChalkboard = "\ue53d";
        public const string PersonCircleCheck = "\ue53e";
        public const string PersonCircleExclamation = "\ue53f";
        public const string PersonCircleMinus = "\ue540";
        public const string PersonCirclePlus = "\ue541";
        public const string PersonCircleQuestion = "\ue542";
        public const string PersonCircleXmark = "\ue543";
        public const string PersonDigging = "\uf85e";
        public const string PersonDotsFromLine = "\uf470";
        public const string PersonDress = "\uf182";
        public const string PersonDressBurst = "\ue544";
        public const string PersonDrowning = "\ue545";
        public const string PersonFalling = "\ue546";
        public const string PersonFallingBurst = "\ue547";
        public const string PersonHalfDress = "\ue548";
        public const string PersonHarassing = "\ue549";
        public const string PersonHiking = "\uf6ec";
        public const string PersonMilitaryPointing = "\ue54a";
        public const string PersonMilitaryRifle = "\ue54b";
        public const string PersonMilitaryToPerson = "\ue54c";
        public const string PersonPraying = "\uf683";
        public const string PersonPregnant = "\ue31e";
        public const string PersonRays = "\ue54d";
        public const string PersonRifle = "\ue54e";
        public const string PersonRunning = "\uf70c";
        public const string PersonShelter = "\ue54f";
        public const string PersonSkating = "\uf7c5";
        public const string PersonSkiing = "\uf7c9";
        public const string PersonSkiingNordic = "\uf7ca";
        public const string PersonSnowboarding = "\uf7ce";
        public const string PersonSwimming = "\uf5c4";
        public const string PersonThroughWindow = "\ue5a9";
        public const string PersonWalking = "\uf554";
        public const string PersonWalkingArrowLoopLeft = "\ue551";
        public const string PersonWalkingArrowRight = "\ue552";
        public const string PersonWalkingDashedLineArrowRight = "\ue553";
        public const string PersonWalkingLuggage = "\ue554";
        public const string PersonWalkingWithCane = "\uf29d";
        public const string PesetaSign = "\ue221";
        public const string PesoSign = "\ue222";
        public const string Phone = "\uf095";
        public const string PhoneFlip = "\uf879";
        public const string PhoneSlash = "\uf3dd";
        public const string PhoneVolume = "\uf2a0";
        public const string PhotoFilm = "\uf87c";
        public const string PiggyBank = "\uf4d3";
        public const string Pills = "\uf484";
        public const string PizzaSlice = "\uf818";
        public const string PlaceOfWorship = "\uf67f";
        public const string Plane = "\uf072";
        public const string PlaneArrival = "\uf5af";
        public const string PlaneCircleCheck = "\ue555";
        public const string PlaneCircleExclamation = "\ue556";
        public const string PlaneCircleXmark = "\ue557";
        public const string PlaneDeparture = "\uf5b0";
        public const string PlaneLock = "\ue558";
        public const string PlaneSlash = "\ue069";
        public const string PlaneUp = "\ue22d";
        public const string PlantWilt = "\ue5aa";
        public const string PlateWheat = "\ue55a";
        public const string Play = "\uf04b";
        public const string Plug = "\uf1e6";
        public const string PlugCircleBolt = "\ue55b";
        public const string PlugCircleCheck = "\ue55c";
        public const string PlugCircleExclamation = "\ue55d";
        public const string PlugCircleMinus = "\ue55e";
        public const string PlugCirclePlus = "\ue55f";
        public const string PlugCircleXmark = "\ue560";
        public const string Plus = "\u002b";
        public const string PlusMinus = "\ue43c";
        public const string Podcast = "\uf2ce";
        public const string Poo = "\uf2fe";
        public const string PooStorm = "\uf75a";
        public const string Poop = "\uf619";
        public const string PowerOff = "\uf011";
        public const string Prescription = "\uf5b1";
        public const string PrescriptionBottle = "\uf485";
        public const string PrescriptionBottleMedical = "\uf486";
        public const string Print = "\uf02f";
        public const string PumpMedical = "\ue06a";
        public const string PumpSoap = "\ue06b";
        public const string PuzzlePiece = "\uf12e";
        public const string Q = "\u0051";
        public const string Qrcode = "\uf029";
        public const string Question = "\u003f";
        public const string QuoteLeft = "\uf10d";
        public const string QuoteRight = "\uf10e";
        public const string R = "\u0052";
        public const string Radiation = "\uf7b9";
        public const string Radio = "\uf8d7";
        public const string Rainbow = "\uf75b";
        public const string RankingStar = "\ue561";
        public const string Receipt = "\uf543";
        public const string RecordVinyl = "\uf8d9";
        public const string RectangleAd = "\uf641";
        public const string RectangleList = "\uf022";
        public const string RectangleXmark = "\uf410";
        public const string Recycle = "\uf1b8";
        public const string Registered = "\uf25d";
        public const string Repeat = "\uf363";
        public const string Reply = "\uf3e5";
        public const string ReplyAll = "\uf122";
        public const string Republican = "\uf75e";
        public const string Restroom = "\uf7bd";
        public const string Retweet = "\uf079";
        public const string Ribbon = "\uf4d6";
        public const string RightFromBracket = "\uf2f5";
        public const string RightLeft = "\uf362";
        public const string RightLong = "\uf30b";
        public const string RightToBracket = "\uf2f6";
        public const string Ring = "\uf70b";
        public const string Road = "\uf018";
        public const string RoadBarrier = "\ue562";
        public const string RoadBridge = "\ue563";
        public const string RoadCircleCheck = "\ue564";
        public const string RoadCircleExclamation = "\ue565";
        public const string RoadCircleXmark = "\ue566";
        public const string RoadLock = "\ue567";
        public const string RoadSpikes = "\ue568";
        public const string Robot = "\uf544";
        public const string Rocket = "\uf135";
        public const string Rotate = "\uf2f1";
        public const string RotateLeft = "\uf2ea";
        public const string RotateRight = "\uf2f9";
        public const string Route = "\uf4d7";
        public const string Rss = "\uf09e";
        public const string RubleSign = "\uf158";
        public const string Rug = "\ue569";
        public const string Ruler = "\uf545";
        public const string RulerCombined = "\uf546";
        public const string RulerHorizontal = "\uf547";
        public const string RulerVertical = "\uf548";
        public const string RupeeSign = "\uf156";
        public const string RupiahSign = "\ue23d";
        public const string S = "\u0053";
        public const string SackDollar = "\uf81d";
        public const string SackXmark = "\ue56a";
        public const string Sailboat = "\ue445";
        public const string Satellite = "\uf7bf";
        public const string SatelliteDish = "\uf7c0";
        public const string ScaleBalanced = "\uf24e";
        public const string ScaleUnbalanced = "\uf515";
        public const string ScaleUnbalancedFlip = "\uf516";
        public const string School = "\uf549";
        public const string SchoolCircleCheck = "\ue56b";
        public const string SchoolCircleExclamation = "\ue56c";
        public const string SchoolCircleXmark = "\ue56d";
        public const string SchoolFlag = "\ue56e";
        public const string SchoolLock = "\ue56f";
        public const string Scissors = "\uf0c4";
        public const string Screwdriver = "\uf54a";
        public const string ScrewdriverWrench = "\uf7d9";
        public const string Scroll = "\uf70e";
        public const string ScrollTorah = "\uf6a0";
        public const string SdCard = "\uf7c2";
        public const string Section = "\ue447";
        public const string Seedling = "\uf4d8";
        public const string Server = "\uf233";
        public const string Shapes = "\uf61f";
        public const string Share = "\uf064";
        public const string ShareFromSquare = "\uf14d";
        public const string ShareNodes = "\uf1e0";
        public const string SheetPlastic = "\ue571";
        public const string ShekelSign = "\uf20b";
        public const string Shield = "\uf132";
        public const string ShieldCat = "\ue572";
        public const string ShieldDog = "\ue573";
        public const string ShieldHalved = "\uf3ed";
        public const string ShieldHeart = "\ue574";
        public const string ShieldVirus = "\ue06c";
        public const string Ship = "\uf21a";
        public const string Shirt = "\uf553";
        public const string ShoePrints = "\uf54b";
        public const string Shop = "\uf54f";
        public const string ShopLock = "\ue4a5";
        public const string ShopSlash = "\ue070";
        public const string Shower = "\uf2cc";
        public const string Shrimp = "\ue448";
        public const string Shuffle = "\uf074";
        public const string ShuttleSpace = "\uf197";
        public const string SignHanging = "\uf4d9";
        public const string Signal = "\uf012";
        public const string Signature = "\uf5b7";
        public const string SignsPost = "\uf277";
        public const string SimCard = "\uf7c4";
        public const string Sink = "\ue06d";
        public const string Sitemap = "\uf0e8";
        public const string Skull = "\uf54c";
        public const string SkullCrossbones = "\uf714";
        public const string Slash = "\uf715";
        public const string Sleigh = "\uf7cc";
        public const string Sliders = "\uf1de";
        public const string Smog = "\uf75f";
        public const string Smoking = "\uf48d";
        public const string Snowflake = "\uf2dc";
        public const string Snowman = "\uf7d0";
        public const string Snowplow = "\uf7d2";
        public const string Soap = "\ue06e";
        public const string Socks = "\uf696";
        public const string SolarPanel = "\uf5ba";
        public const string Sort = "\uf0dc";
        public const string SortDown = "\uf0dd";
        public const string SortUp = "\uf0de";
        public const string Spa = "\uf5bb";
        public const string SpaghettiMonsterFlying = "\uf67b";
        public const string SpellCheck = "\uf891";
        public const string Spider = "\uf717";
        public const string Spinner = "\uf110";
        public const string Splotch = "\uf5bc";
        public const string Spoon = "\uf2e5";
        public const string SprayCan = "\uf5bd";
        public const string SprayCanSparkles = "\uf5d0";
        public const string Square = "\uf0c8";
        public const string SquareArrowUpRight = "\uf14c";
        public const string SquareCaretDown = "\uf150";
        public const string SquareCaretLeft = "\uf191";
        public const string SquareCaretRight = "\uf152";
        public const string SquareCaretUp = "\uf151";
        public const string SquareCheck = "\uf14a";
        public const string SquareEnvelope = "\uf199";
        public const string SquareFull = "\uf45c";
        public const string SquareH = "\uf0fd";
        public const string SquareMinus = "\uf146";
        public const string SquareNfi = "\ue576";
        public const string SquareParking = "\uf540";
        public const string SquarePen = "\uf14b";
        public const string SquarePersonConfined = "\ue577";
        public const string SquarePhone = "\uf098";
        public const string SquarePhoneFlip = "\uf87b";
        public const string SquarePlus = "\uf0fe";
        public const string SquarePollHorizontal = "\uf682";
        public const string SquarePollVertical = "\uf681";
        public const string SquareRootVariable = "\uf698";
        public const string SquareRss = "\uf143";
        public const string SquareShareNodes = "\uf1e1";
        public const string SquareUpRight = "\uf360";
        public const string SquareVirus = "\ue578";
        public const string SquareXmark = "\uf2d3";
        public const string StaffSnake = "\ue579";
        public const string Stairs = "\ue289";
        public const string Stamp = "\uf5bf";
        public const string Stapler = "\ue5af";
        public const string Star = "\uf005";
        public const string StarAndCrescent = "\uf699";
        public const string StarHalf = "\uf089";
        public const string StarHalfStroke = "\uf5c0";
        public const string StarOfDavid = "\uf69a";
        public const string StarOfLife = "\uf621";
        public const string SterlingSign = "\uf154";
        public const string Stethoscope = "\uf0f1";
        public const string Stop = "\uf04d";
        public const string Stopwatch = "\uf2f2";
        public const string Stopwatch20 = "\ue06f";
        public const string Store = "\uf54e";
        public const string StoreSlash = "\ue071";
        public const string StreetView = "\uf21d";
        public const string Strikethrough = "\uf0cc";
        public const string Stroopwafel = "\uf551";
        public const string Subscript = "\uf12c";
        public const string Suitcase = "\uf0f2";
        public const string SuitcaseMedical = "\uf0fa";
        public const string SuitcaseRolling = "\uf5c1";
        public const string Sun = "\uf185";
        public const string SunPlantWilt = "\ue57a";
        public const string Superscript = "\uf12b";
        public const string Swatchbook = "\uf5c3";
        public const string Synagogue = "\uf69b";
        public const string Syringe = "\uf48e";
        public const string T = "\u0054";
        public const string Table = "\uf0ce";
        public const string TableCells = "\uf00a";
        public const string TableCellsLarge = "\uf009";
        public const string TableColumns = "\uf0db";
        public const string TableList = "\uf00b";
        public const string TableTennisPaddleBall = "\uf45d";
        public const string Tablet = "\uf3fb";
        public const string TabletButton = "\uf10a";
        public const string TabletScreenButton = "\uf3fa";
        public const string Tablets = "\uf490";
        public const string TachographDigital = "\uf566";
        public const string Tag = "\uf02b";
        public const string Tags = "\uf02c";
        public const string Tape = "\uf4db";
        public const string Tarp = "\ue57b";
        public const string TarpDroplet = "\ue57c";
        public const string Taxi = "\uf1ba";
        public const string Teeth = "\uf62e";
        public const string TeethOpen = "\uf62f";
        public const string TemperatureArrowDown = "\ue03f";
        public const string TemperatureArrowUp = "\ue040";
        public const string TemperatureEmpty = "\uf2cb";
        public const string TemperatureFull = "\uf2c7";
        public const string TemperatureHalf = "\uf2c9";
        public const string TemperatureHigh = "\uf769";
        public const string TemperatureLow = "\uf76b";
        public const string TemperatureQuarter = "\uf2ca";
        public const string TemperatureThreeQuarters = "\uf2c8";
        public const string TengeSign = "\uf7d7";
        public const string Tent = "\ue57d";
        public const string TentArrowDownToLine = "\ue57e";
        public const string TentArrowLeftRight = "\ue57f";
        public const string TentArrowTurnLeft = "\ue580";
        public const string TentArrowsDown = "\ue581";
        public const string Tents = "\ue582";
        public const string Terminal = "\uf120";
        public const string TextHeight = "\uf034";
        public const string TextSlash = "\uf87d";
        public const string TextWidth = "\uf035";
        public const string Thermometer = "\uf491";
        public const string ThumbsDown = "\uf165";
        public const string ThumbsUp = "\uf164";
        public const string Thumbtack = "\uf08d";
        public const string Ticket = "\uf145";
        public const string TicketSimple = "\uf3ff";
        public const string Timeline = "\ue29c";
        public const string ToggleOff = "\uf204";
        public const string ToggleOn = "\uf205";
        public const string Toilet = "\uf7d8";
        public const string ToiletPaper = "\uf71e";
        public const string ToiletPaperSlash = "\ue072";
        public const string ToiletPortable = "\ue583";
        public const string ToiletsPortable = "\ue584";
        public const string Toolbox = "\uf552";
        public const string Tooth = "\uf5c9";
        public const string ToriiGate = "\uf6a1";
        public const string Tornado = "\uf76f";
        public const string TowerBroadcast = "\uf519";
        public const string TowerCell = "\ue585";
        public const string TowerObservation = "\ue586";
        public const string Tractor = "\uf722";
        public const string Trademark = "\uf25c";
        public const string TrafficLight = "\uf637";
        public const string Trailer = "\ue041";
        public const string Train = "\uf238";
        public const string TrainSubway = "\uf239";
        public const string TrainTram = "\ue5b4";
        public const string Transgender = "\uf225";
        public const string Trash = "\uf1f8";
        public const string TrashArrowUp = "\uf829";
        public const string TrashCan = "\uf2ed";
        public const string TrashCanArrowUp = "\uf82a";
        public const string Tree = "\uf1bb";
        public const string TreeCity = "\ue587";
        public const string TriangleExclamation = "\uf071";
        public const string Trophy = "\uf091";
        public const string Trowel = "\ue589";
        public const string TrowelBricks = "\ue58a";
        public const string Truck = "\uf0d1";
        public const string TruckArrowRight = "\ue58b";
        public const string TruckDroplet = "\ue58c";
        public const string TruckFast = "\uf48b";
        public const string TruckField = "\ue58d";
        public const string TruckFieldUn = "\ue58e";
        public const string TruckFront = "\ue2b7";
        public const string TruckMedical = "\uf0f9";
        public const string TruckMonster = "\uf63b";
        public const string TruckMoving = "\uf4df";
        public const string TruckPickup = "\uf63c";
        public const string TruckPlane = "\ue58f";
        public const string TruckRampBox = "\uf4de";
        public const string Tty = "\uf1e4";
        public const string TurkishLiraSign = "\ue2bb";
        public const string TurnDown = "\uf3be";
        public const string TurnUp = "\uf3bf";
        public const string Tv = "\uf26c";
        public const string U = "\u0055";
        public const string Umbrella = "\uf0e9";
        public const string UmbrellaBeach = "\uf5ca";
        public const string Underline = "\uf0cd";
        public const string UniversalAccess = "\uf29a";
        public const string Unlock = "\uf09c";
        public const string UnlockKeyhole = "\uf13e";
        public const string UpDown = "\uf338";
        public const string UpDownLeftRight = "\uf0b2";
        public const string UpLong = "\uf30c";
        public const string UpRightAndDownLeftFromCenter = "\uf424";
        public const string UpRightFromSquare = "\uf35d";
        public const string Upload = "\uf093";
        public const string User = "\uf007";
        public const string UserAstronaut = "\uf4fb";
        public const string UserCheck = "\uf4fc";
        public const string UserClock = "\uf4fd";
        public const string UserDoctor = "\uf0f0";
        public const string UserGear = "\uf4fe";
        public const string UserGraduate = "\uf501";
        public const string UserGroup = "\uf500";
        public const string UserInjured = "\uf728";
        public const string UserLarge = "\uf406";
        public const string UserLargeSlash = "\uf4fa";
        public const string UserLock = "\uf502";
        public const string UserMinus = "\uf503";
        public const string UserNinja = "\uf504";
        public const string UserNurse = "\uf82f";
        public const string UserPen = "\uf4ff";
        public const string UserPlus = "\uf234";
        public const string UserSecret = "\uf21b";
        public const string UserShield = "\uf505";
        public const string UserSlash = "\uf506";
        public const string UserTag = "\uf507";
        public const string UserTie = "\uf508";
        public const string UserXmark = "\uf235";
        public const string Users = "\uf0c0";
        public const string UsersBetweenLines = "\ue591";
        public const string UsersGear = "\uf509";
        public const string UsersLine = "\ue592";
        public const string UsersRays = "\ue593";
        public const string UsersRectangle = "\ue594";
        public const string UsersSlash = "\ue073";
        public const string UsersViewfinder = "\ue595";
        public const string Utensils = "\uf2e7";
        public const string V = "\u0056";
        public const string VanShuttle = "\uf5b6";
        public const string Vault = "\ue2c5";
        public const string VectorSquare = "\uf5cb";
        public const string Venus = "\uf221";
        public const string VenusDouble = "\uf226";
        public const string VenusMars = "\uf228";
        public const string Vest = "\ue085";
        public const string VestPatches = "\ue086";
        public const string Vial = "\uf492";
        public const string VialCircleCheck = "\ue596";
        public const string VialVirus = "\ue597";
        public const string Vials = "\uf493";
        public const string Video = "\uf03d";
        public const string VideoSlash = "\uf4e2";
        public const string Vihara = "\uf6a7";
        public const string Virus = "\ue074";
        public const string VirusCovid = "\ue4a8";
        public const string VirusCovidSlash = "\ue4a9";
        public const string VirusSlash = "\ue075";
        public const string Viruses = "\ue076";
        public const string Voicemail = "\uf897";
        public const string Volcano = "\uf770";
        public const string Volleyball = "\uf45f";
        public const string VolumeHigh = "\uf028";
        public const string VolumeLow = "\uf027";
        public const string VolumeOff = "\uf026";
        public const string VolumeXmark = "\uf6a9";
        public const string VrCardboard = "\uf729";
        public const string W = "\u0057";
        public const string WalkieTalkie = "\uf8ef";
        public const string Wallet = "\uf555";
        public const string WandMagic = "\uf0d0";
        public const string WandMagicSparkles = "\ue2ca";
        public const string WandSparkles = "\uf72b";
        public const string Warehouse = "\uf494";
        public const string Water = "\uf773";
        public const string WaterLadder = "\uf5c5";
        public const string WaveSquare = "\uf83e";
        public const string WeightHanging = "\uf5cd";
        public const string WeightScale = "\uf496";
        public const string WheatAwn = "\ue2cd";
        public const string WheatAwnCircleExclamation = "\ue598";
        public const string Wheelchair = "\uf193";
        public const string WheelchairMove = "\ue2ce";
        public const string WhiskeyGlass = "\uf7a0";
        public const string Wifi = "\uf1eb";
        public const string Wind = "\uf72e";
        public const string WindowMaximize = "\uf2d0";
        public const string WindowMinimize = "\uf2d1";
        public const string WindowRestore = "\uf2d2";
        public const string WineBottle = "\uf72f";
        public const string WineGlass = "\uf4e3";
        public const string WineGlassEmpty = "\uf5ce";
        public const string WonSign = "\uf159";
        public const string Worm = "\ue599";
        public const string Wrench = "\uf0ad";
        public const string X = "\u0058";
        public const string XRay = "\uf497";
        public const string Xmark = "\uf00d";
        public const string XmarksLines = "\ue59a";
        public const string Y = "\u0059";
        public const string YenSign = "\uf157";
        public const string YinYang = "\uf6ad";
        public const string Z = "\u005a";
    }
}
```
