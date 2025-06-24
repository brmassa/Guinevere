// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class CheckboxTests : PrimitiveControlsTestBase
// {
//     [Fact]
//     public void Checkbox_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isChecked = false;
//
//         // Act
//         gui.Checkbox(ref isChecked, "Test Checkbox");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("CheckboxTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void Checkbox_WithLabel_CalculatesCorrectWidth()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isChecked = false;
//         const float expectedMinWidth = 20f; // Default checkbox size
//
//         // Act
//         gui.Checkbox(ref isChecked, "Test Label");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.True(node.Rect.W > expectedMinWidth); // Should be wider than just the checkbox
//     }
//
//     [Fact]
//     public void Checkbox_WithoutLabel_UsesOnlyCheckboxSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isChecked = false;
//         const float expectedSize = 20f; // Default checkbox size
//
//         // Act
//         gui.Checkbox(ref isChecked);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Equal(expectedSize, node.Rect.W, 2);
//     }
//
//     [Fact]
//     public void Checkbox_WithCustomSize_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var isChecked = false;
//         const float customSize = 30f;
//
//         // Act
//         gui.Checkbox(ref isChecked, size: customSize);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.True(node.Rect.W >= customSize); // Width should be at least the checkbox size
//     }
//
//     [Fact]
//     public void Checkbox_InitiallyUnchecked_RemainsUnchecked()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref isChecked);
//                 Assert.False(isChecked);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_InitiallyChecked_RemainsChecked()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = true;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref isChecked);
//                 Assert.True(isChecked);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_ClickedWhenUnchecked_BecomesChecked()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         SetupMouseClick(input, new Vector2(10, 10)); // Click inside checkbox bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 Assert.True(isChecked);
//                 Assert.True(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_ClickedWhenChecked_BecomesUnchecked()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = true;
//         SetupMouseClick(input, new Vector2(10, 10)); // Click inside checkbox bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 Assert.False(isChecked);
//                 Assert.True(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_ClickedOutsideBounds_StateUnchanged()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         SetupMouseClick(input, new Vector2(100, 100)); // Click outside checkbox bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 Assert.False(isChecked);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_HoveredInsideBounds_HandlesProperly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         SetupMouseHover(input, new Vector2(10, 10)); // Hover inside checkbox bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 // Should handle hover state without throwing
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 Assert.False(isChecked);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Theory]
//     [InlineData(MouseButton.Left)]
//     [InlineData(MouseButton.Right)]
//     [InlineData(MouseButton.Middle)]
//     public void Checkbox_ClickedWithDifferentMouseButtons_OnlyLeftClickToggles(MouseButton button)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         SetupMouseClick(input, new Vector2(10, 10), button);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 if (button == MouseButton.Left)
//                 {
//                     Assert.True(isChecked);
//                     Assert.True(wasToggled);
//                 }
//                 else
//                 {
//                     Assert.False(isChecked);
//                     Assert.False(wasToggled);
//                 }
//             });
//     }
//
//     [Fact]
//     public void Checkbox_WithCustomColors_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = true;
//         var backgroundColor = Color.LightGray;
//         var checkColor = Color.Green;
//         var borderColor = Color.DarkGray;
//         var labelColor = Color.Black;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, "Custom Colors",
//                     backgroundColor: backgroundColor,
//                     checkColor: checkColor,
//                     borderColor: borderColor,
//                     labelColor: labelColor);
//
//                 Assert.True(isChecked);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_NonRefOverload_ReturnsUpdatedState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(10, 10)); // Click inside checkbox bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(false),
//             renderAction: () =>
//             {
//                 var newState = gui.Checkbox(false);
//                 Assert.True(newState); // Should be toggled to true
//             });
//     }
//
//     [Fact]
//     public void Checkbox_MultipleCheckboxes_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         var check1 = false;
//         var check2 = false;
//         var check3 = false;
//
//         // Act
//         gui.Checkbox(ref check1, "Checkbox 1");
//         gui.Checkbox(ref check2, "Checkbox 2");
//         gui.Checkbox(ref check3, "Checkbox 3");
//
//         // Assert
//         Assert.Equal(3, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(3, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void Checkbox_CalledMultipleTimesWithSameParameters_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = true;
//
//         // Act & Assert - First call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked, "Consistent"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, "Consistent");
//                 Assert.True(isChecked);
//                 Assert.False(wasToggled);
//             });
//
//         // Reset for second call
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked, "Consistent"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, "Consistent");
//                 Assert.True(isChecked);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_WithEmptyLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled1 = gui.Checkbox(ref isChecked, "");
//                 var wasToggled2 = gui.Checkbox(ref isChecked, string.Empty);
//                 Assert.False(wasToggled1);
//                 Assert.False(wasToggled2);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_WithVeryLongLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//         var longLabel = new string('A', 1000);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, longLabel);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Theory]
//     [InlineData(0f)] // Zero size
//     [InlineData(-10f)] // Negative size
//     [InlineData(1000f)] // Very large size
//     [InlineData(0.1f)] // Very small size
//     public void Checkbox_WithEdgeCaseSizes_HandlesGracefully(float size)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, "Test", size: size);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_WithCustomFontSizeAndSpacing_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled1 = gui.Checkbox(ref isChecked, "Custom Font",
//                     fontSize: 18f, spacing: 12f);
//                 var wasToggled2 = gui.Checkbox(ref isChecked, "Zero Font",
//                     fontSize: 0f, spacing: 0f);
//
//                 Assert.False(wasToggled1);
//                 Assert.False(wasToggled2);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_ClickOnLabelArea_TogglesCheckbox()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         // Click on the right side where label would be (beyond checkbox size)
//         SetupMouseClick(input, new Vector2(50, 10));
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked, "Clickable Label"),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, "Clickable Label");
//                 // The entire control area should be clickable, including label
//                 Assert.True(wasToggled);
//                 Assert.True(isChecked);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_StateChangesPersistAcrossFrames()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         SetupMouseClick(input, new Vector2(10, 10));
//
//         // Act - First frame with click
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref isChecked);
//                 Assert.True(isChecked);
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
//             buildAction: () => gui.Checkbox(ref isChecked),
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked);
//                 Assert.True(isChecked); // Should remain checked
//                 Assert.False(wasToggled); // Should not be toggled this frame
//             });
//     }
//
//     [Fact]
//     public void Checkbox_WithNullLabel_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var isChecked = false;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref isChecked, null!);
//                 Assert.False(wasToggled);
//             });
//     }
//
//     [Fact]
//     public void Checkbox_RapidToggling_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var isChecked = false;
//         var toggleCount = 0;
//
//         // Simulate rapid toggling
//         for (int i = 0; i < 5; i++)
//         {
//             SetupMouseClick(input, new Vector2(10, 10));
//
//             SimulateFullControlLifecycle(gui,
//                 buildAction: () => gui.Checkbox(ref isChecked),
//                 renderAction: () =>
//                 {
//                     var wasToggled = gui.Checkbox(ref isChecked);
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
//         Assert.Equal(toggleCount % 2 == 1, isChecked);
//     }
// }
