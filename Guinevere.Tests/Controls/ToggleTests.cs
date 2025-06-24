// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class ToggleTests : PrimitiveControlsTestBase
// {
//     [Fact]
//     public void Toggle_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isOn = false;
//
//         // Act
//         gui.Toggle(ref isOn, "Test Toggle");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("ToggleTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void Toggle_WithDefaultSize_CreatesCorrectDimensions()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isOn = false;
//         const float expectedWidth = 50f;
//
//         // Act
//         gui.Toggle(ref isOn);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Equal(expectedWidth, node.Rect.W, 2);
//     }
//
//     [Fact]
//     public void Toggle_WithLabel_CalculatesCorrectWidth()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isOn = false;
//         const float expectedMinWidth = 50f; // Default toggle width
//
//         // Act
//         gui.Toggle(ref isOn, "Test Label");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.True(node.Rect.W > expectedMinWidth); // Should be wider than just the toggle
//     }
//
//     [Fact]
//     public void Toggle_WithoutLabel_UsesOnlyToggleSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isOn = false;
//         const float expectedWidth = 50f; // Default toggle width
//
//         // Act
//         gui.Toggle(ref isOn);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Equal(expectedWidth, node.Rect.W, 2);
//     }
//
//     [Fact]
//     public void Toggle_WithCustomDimensions_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isOn = false;
//         const float customWidth = 80f;
//         const float customHeight = 40f;
//
//         // Act
//         gui.Toggle(ref isOn, width: customWidth, height: customHeight);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.True(node.Rect.W >= customWidth); // Width should be at least the toggle width
//     }
//
//     [Fact]
//     public void Toggle_InitiallyOff_RemainsOff()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 gui.Toggle(ref isOn);
//                 Assert.False(isOn);
//             });
//     }
//
//     [Fact]
//     public void Toggle_InitiallyOn_RemainsOn()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = true;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 gui.Toggle(ref isOn);
//                 Assert.True(isOn);
//             });
//     }
//
//     [Fact]
//     public void Toggle_ClickedWhenOff_TurnsOn()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         SetupMouseClick(input, new Vector2(25, 12)); // Click inside toggle bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 Assert.True(isOn);
//                 Assert.True(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_ClickedWhenOn_TurnsOff()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = true;
//         SetupMouseClick(input, new Vector2(25, 12)); // Click inside toggle bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 Assert.False(isOn);
//                 Assert.True(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_ClickedOutsideBounds_StateUnchanged()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         SetupMouseClick(input, new Vector2(100, 100)); // Click outside toggle bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 Assert.False(isOn);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_HoveredInsideBounds_HandlesProperly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         SetupMouseHover(input, new Vector2(25, 12)); // Hover inside toggle bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 Assert.False(isOn);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Theory]
//     [InlineData(MouseButton.Left)]
//     [InlineData(MouseButton.Right)]
//     [InlineData(MouseButton.Middle)]
//     public void Toggle_ClickedWithDifferentMouseButtons_OnlyLeftClickToggles(MouseButton button)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         SetupMouseClick(input, new Vector2(25, 12), button);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 if (button == MouseButton.Left)
//                 {
//                     Assert.True(isOn);
//                     Assert.True(wasToggled);
//                 }
//                 else
//                 {
//                     Assert.False(isOn);
//                     Assert.False(wasToggled);
//                 }
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithCustomColors_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = true;
//         var onColor = Color.Green;
//         var offColor = Color.Red;
//         var thumbColor = Color.White;
//         var labelColor = Color.Black;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Custom Colors",
//                     onColor: onColor,
//                     offColor: offColor,
//                     thumbColor: thumbColor,
//                     labelColor: labelColor);
//
//                 Assert.True(isOn);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_NonRefOverload_ReturnsUpdatedState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(25, 12)); // Click inside toggle bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(false),
//             renderAction: () =>
//             {
//                 var newState = gui.Toggle(false);
//                 Assert.True(newState); // Should be toggled to true
//             });
//     }
//
//     [Fact]
//     public void Toggle_MultipleToggles_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var toggle1 = false;
//         var toggle2 = false;
//         var toggle3 = false;
//
//         // Act
//         gui.Toggle(ref toggle1, "Toggle 1");
//         gui.Toggle(ref toggle2, "Toggle 2");
//         gui.Toggle(ref toggle3, "Toggle 3");
//
//         // Assert
//         Assert.Equal(3, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(3, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void Toggle_CalledMultipleTimesWithSameParameters_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = true;
//
//         // Act & Assert - First call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn, "Consistent"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Consistent");
//                 Assert.True(isOn);
//                 Assert.False(wasToggled);
//             });
//
//         // Reset for second call
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn, "Consistent"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Consistent");
//                 Assert.True(isOn);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithEmptyLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled1 = gui.Toggle(ref isOn, "");
//                 var wasToggled2 = gui.Toggle(ref isOn, string.Empty);
//                 Assert.False(wasToggled1);
//                 Assert.False(wasToggled2);
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithVeryLongLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//         var longLabel = new string('A', 1000);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, longLabel);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Theory]
//     [InlineData(0f, 0f)] // Zero dimensions
//     [InlineData(-10f, -5f)] // Negative dimensions
//     [InlineData(1000f, 1000f)] // Very large dimensions
//     [InlineData(0.1f, 0.1f)] // Very small dimensions
//     public void Toggle_WithEdgeCaseDimensions_HandlesGracefully(float width, float height)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Test", width: width, height: height);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithCustomFontSizeAndSpacing_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled1 = gui.Toggle(ref isOn, "Custom Font",
//                     fontSize: 18f, spacing: 12f);
//                 var wasToggled2 = gui.Toggle(ref isOn, "Zero Font",
//                     fontSize: 0f, spacing: 0f);
//
//                 Assert.False(wasToggled1);
//                 Assert.False(wasToggled2);
//             });
//     }
//
//     [Fact]
//     public void Toggle_ClickOnLabelArea_TogglesSwitch()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         // Click on the right side where label would be (beyond toggle width)
//         SetupMouseClick(input, new Vector2(80, 12));
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn, "Clickable Label"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Clickable Label");
//                 // The entire control area should be clickable, including label
//                 Assert.True(wasToggled);
//                 Assert.True(isOn);
//             });
//     }
//
//     [Fact]
//     public void Toggle_StateChangesPersistAcrossFrames()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         SetupMouseClick(input, new Vector2(25, 12));
//
//         // Act - First frame with click
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 gui.Toggle(ref isOn);
//                 Assert.True(isOn);
//             });
//
//         // Reset input (no more clicks)
//         ResetInput(input);
//
//         // Start new frame
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second frame without click
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn);
//                 Assert.True(isOn); // Should remain on
//                 Assert.False(wasToggled); // Should not be toggled this frame
//             });
//     }
//
//     [Fact]
//     public void Toggle_HoverStateChangesOnDifferentStates()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOnOff = false;
//         var isOnOn = true;
//         SetupMouseHover(input, new Vector2(25, 12));
//
//         // Act & Assert - Hover when off
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOnOff),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOnOff);
//                 Assert.False(wasToggled);
//             });
//
//         // Reset for second test
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Hover when on
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Toggle(ref isOnOn),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOnOn);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_ThumbPositionChangesBasedOnState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOffState = false;
//         var isOnState = true;
//
//         // Act & Assert - We can't directly test thumb position,
//         // but we can ensure the toggle renders without errors in both states
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled1 = gui.Toggle(ref isOffState, "Off State");
//                 var wasToggled2 = gui.Toggle(ref isOnState, "On State");
//                 Assert.False(wasToggled1);
//                 Assert.False(wasToggled2);
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithNullColors_UsesDefaults()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = true;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, "Default Colors",
//                     onColor: null,
//                     offColor: null,
//                     thumbColor: null,
//                     labelColor: null);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_WithNullLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isOn = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Toggle(ref isOn, null!);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Toggle_RapidToggling_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isOn = false;
//         var toggleCount = 0;
//
//         // Simulate rapid toggling
//         for (int i = 0; i < 5; i++)
//         {
//             SetupMouseClick(input, new Vector2(25, 12));
//
//             SimulateFullControlLifecycle(gui,
//                 buildAction: () => gui.Toggle(ref isOn),
//                 renderAction: () =>
//                 {
//                     var wasToggled = gui.Toggle(ref isOn);
//                     if (wasToggled) toggleCount++;
//                 });
//
//             // Reset frame
//             gui.EndFrame();
//             gui.BeginFrame(CreateTestCanvas());
//             ResetInput(input);
//         }
//
//         // Should have toggled multiple times
//         Assert.True(toggleCount > 0);
//         // Final state should be consistent with odd number of toggles
//         Assert.Equal(toggleCount % 2 == 1, isOn);
//     }
// }
