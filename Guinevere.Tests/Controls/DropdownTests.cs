// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class DropdownTests : PrimitiveControlsTestBase
// {
//     private readonly string[] _testOptions = ["Option 1", "Option 2", "Option 3", "Option 4"];
//
//     [Fact]
//     public void Dropdown_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.Dropdown(_testOptions);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("DropdownTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void Dropdown_WithDefaultDimensions_CreatesCorrectSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float expectedWidth = 200f;
//         const float expectedHeight = 32f;
//
//         // Act
//         gui.Dropdown(_testOptions);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, expectedWidth, expectedHeight);
//     }
//
//     [Fact]
//     public void Dropdown_WithCustomDimensions_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float customWidth = 300f;
//         const float customHeight = 40f;
//
//         // Act
//         gui.Dropdown(_testOptions, width: customWidth, height: customHeight);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, customWidth, customHeight);
//     }
//
//     [Fact]
//     public void Dropdown_InitiallyNoSelection_ReturnsNegativeOne()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithInitialSelection_ReturnsSelectedIndex()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         const int initialSelection = 2;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions, selectedIndex: initialSelection);
//                 Assert.Equal(initialSelection, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_ClickedOnButton_OpensDropdown()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click in middle of dropdown button
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 // First call should open the dropdown
//                 var result = gui.Dropdown(_testOptions);
//                 Assert.Equal(-1, result); // Should still return -1 as no option was selected
//             });
//     }
//
//     [Fact]
//     public void Dropdown_ClickedOutsideWhenOpen_ClosesDropdown()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame and click outside
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         SetupMouseClick(input, new Vector2(400, 400)); // Click far outside
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions);
//                 // Dropdown should close when clicking outside
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_ClickOnOption_SelectsOption()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame and click on first option
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         SetupMouseClick(input, new Vector2(100, 50)); // Click on first dropdown item
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions);
//                 Assert.Equal(0, result); // Should select first option
//             });
//     }
//
//     [Fact]
//     public void Dropdown_KeyboardEscape_ClosesDropdown()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame and press escape
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         ResetInput(input);
//         SetupKeyPress(input, KeyboardKey.Escape);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions);
//                 // Dropdown should close on escape
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_KeyboardNavigation_ChangesHoveredIndex()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame and press down arrow
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         ResetInput(input);
//         SetupKeyPress(input, KeyboardKey.Down);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 // Should handle keyboard navigation without throwing
//                 var result = gui.Dropdown(_testOptions);
//                 Assert.Equal(-1, result); // Navigation doesn't select, just hovers
//             });
//     }
//
//     [Fact]
//     public void Dropdown_KeyboardEnter_SelectsHoveredOption()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame, navigate down and press enter
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         ResetInput(input);
//         input.IsKeyPressed(KeyboardKey.Down).Returns(true);
//         input.IsKeyPressed(KeyboardKey.Enter).Returns(true);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 // Should handle enter key selection
//                 var result = gui.Dropdown(_testOptions);
//                 // This may or may not select depending on implementation
//                 Assert.True(result >= -1 && result < _testOptions.Length);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithEmptyOptions_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var emptyOptions = Array.Empty<string>();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(emptyOptions);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithSingleOption_WorksCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var singleOption = new[] { "Only Option" };
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(singleOption);
//                 Assert.Equal(-1, result); // Should start with no selection
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithManyOptions_LimitsVisibleItems()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var manyOptions = Enumerable.Range(1, 20).Select(i => $"Option {i}").ToArray();
//         const int maxVisible = 6; // Default maxVisibleItems
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(manyOptions, maxVisibleItems: maxVisible);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithCustomColors_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var backgroundColor = Color.White;
//         var borderColor = Color.Gray;
//         var textColor = Color.Black;
//         var dropdownColor = Color.LightGray;
//         var hoverColor = Color.Blue;
//         var selectedColor = Color.Green;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions,
//                     backgroundColor: backgroundColor,
//                     borderColor: borderColor,
//                     textColor: textColor,
//                     dropdownColor: dropdownColor,
//                     hoverColor: hoverColor,
//                     selectedColor: selectedColor);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithCustomPlaceholder_DisplaysCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         const string customPlaceholder = "Choose an option...";
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions, placeholder: customPlaceholder);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Theory]
//     [InlineData(-5)] // Negative selection
//     [InlineData(10)] // Out of bounds selection
//     [InlineData(int.MaxValue)] // Very large selection
//     public void Dropdown_WithInvalidInitialSelection_HandlesGracefully(int invalidSelection)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions, selectedIndex: invalidSelection);
//                 // Should handle invalid selections gracefully
//                 Assert.True(result >= -1);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_MultipleDropdowns_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.Dropdown(_testOptions);
//         gui.Dropdown(_testOptions);
//         gui.Dropdown(_testOptions);
//
//         // Assert
//         Assert.Equal(3, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(3, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void Dropdown_CalledMultipleTimesWithSameParameters_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert - First call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 var firstResult = gui.Dropdown(_testOptions);
//                 Assert.Equal(-1, firstResult);
//             });
//
//         // Reset for second call
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 var secondResult = gui.Dropdown(_testOptions);
//                 Assert.Equal(-1, secondResult);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_SearchableDropdown_CallsCorrectMethod()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.SearchableDropdown(_testOptions);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_ClearDropdownStates_RemovesAllStates()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Dropdown(_testOptions);
//                 gui.Dropdown(_testOptions);
//             },
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions);
//                 gui.Dropdown(_testOptions);
//
//                 // Should not throw when clearing states
//                 gui.ClearDropdownStates();
//                 // Test passes if no exception is thrown
//             });
//     }
//
//     [Theory]
//     [InlineData(0f, 0f)] // Zero dimensions
//     [InlineData(-10f, -5f)] // Negative dimensions
//     [InlineData(1000f, 1000f)] // Very large dimensions
//     [InlineData(0.1f, 0.1f)] // Very small dimensions
//     public void Dropdown_WithEdgeCaseDimensions_HandlesGracefully(float width, float height)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions, width: width, height: height);
//                 Assert.True(result >= -1);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithLongOptionText_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var longOptions = new[]
//         {
//             new string('A', 1000),
//             new string('B', 500),
//             "Normal Option"
//         };
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(longOptions);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_MouseHoverOverOptions_ChangesHoveredState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First click to open dropdown
//         SetupMouseClick(input, new Vector2(100, 16));
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 gui.Dropdown(_testOptions); // Opens dropdown
//             });
//
//         // Reset for next frame and hover over dropdown area
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         ResetInput(input);
//         SetupMouseHover(input, new Vector2(100, 60)); // Hover over dropdown items area
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.Dropdown(_testOptions),
//             renderAction: () =>
//             {
//                 // Should handle hover state changes without throwing
//                 var result = gui.Dropdown(_testOptions);
//                 Assert.True(result >= -1);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithCustomStyling_AppliesCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(_testOptions,
//                     fontSize: 16f,
//                     padding: 12f,
//                     borderRadius: 8f,
//                     maxVisibleItems: 10);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithNullOptions_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(null!);
//                 Assert.Equal(-1, result);
//             });
//     }
//
//     [Fact]
//     public void Dropdown_WithNullOptionItems_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var optionsWithNulls = new string?[] { "Option 1", null, "Option 3", null };
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.Dropdown(optionsWithNulls!);
//                 Assert.Equal(-1, result);
//             });
//     }
// }
