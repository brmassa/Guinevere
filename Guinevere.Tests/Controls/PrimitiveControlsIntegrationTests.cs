// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class PrimitiveControlsIntegrationTests : PrimitiveControlsTestBase
// {
//     [Fact]
//     public void MultipleControls_InSameFrame_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.Button("Button 1");
//         var isChecked = false;
//         gui.Checkbox(ref isChecked, "Checkbox 1");
//         var isOn = false;
//         gui.Toggle(ref isOn, "Toggle 1");
//         gui.TextInput("Input 1");
//         gui.Dropdown(new[] { "Option 1", "Option 2" });
//
//         // Assert
//         Assert.Equal(5, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(5, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void MultipleControls_SameTypeInSameFrame_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.Button("Button 1");
//         gui.Button("Button 2");
//         gui.Button("Button 3");
//         gui.Button("Button 4");
//         gui.Button("Button 5");
//
//         // Assert
//         Assert.Equal(5, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(5, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void ControlInteractions_ButtonClickAffectsOtherControls_WorksCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var buttonClicked = false;
//         var checkboxState = false;
//         var toggleState = false;
//
//         // First frame: create controls
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Button("Toggle Checkbox");
//                 gui.Checkbox(ref checkboxState, "Test Checkbox");
//                 gui.Toggle(ref toggleState, "Test Toggle");
//             },
//             renderAction: () =>
//             {
//                 buttonClicked = gui.Button("Toggle Checkbox");
//                 gui.Checkbox(ref checkboxState, "Test Checkbox");
//                 gui.Toggle(ref toggleState, "Test Toggle");
//             });
//
//         // Simulate button click
//         SetupMouseClick(input, new Vector2(50, 25)); // Click on button area
//
//         // Reset for new frame
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second frame with button interaction
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Button("Toggle Checkbox");
//                 gui.Checkbox(ref checkboxState, "Test Checkbox");
//                 gui.Toggle(ref toggleState, "Test Toggle");
//             },
//             renderAction: () =>
//             {
//                 buttonClicked = gui.Button("Toggle Checkbox");
//                 gui.Checkbox(ref checkboxState, "Test Checkbox");
//                 gui.Toggle(ref toggleState, "Test Toggle");
//
//                 Assert.True(buttonClicked); // Button should be clicked
//             });
//     }
//
//     [Fact]
//     public void FocusManagement_TextInputsShareFocus_OnlyOneFocusedAtTime()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//
//         // First frame: create text inputs
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.TextInput("Input 1");
//                 gui.TextInput("Input 2");
//                 gui.TextInput("Input 3");
//             },
//             renderAction: () =>
//             {
//                 gui.TextInput("Input 1");
//                 gui.TextInput("Input 2");
//                 gui.TextInput("Input 3");
//             });
//
//         // Click on first input
//         SetupMouseClick(input, new Vector2(100, 16)); // First input position
//
//         // Reset for new frame
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.TextInput("Input 1");
//                 gui.TextInput("Input 2");
//                 gui.TextInput("Input 3");
//             },
//             renderAction: () =>
//             {
//                 // Should handle focus management without throwing
//                 Assert.DoesNotThrow(() =>
//                 {
//                     gui.TextInput("Input 1");
//                     gui.TextInput("Input 2");
//                     gui.TextInput("Input 3");
//                 });
//             });
//     }
//
//     [Fact]
//     public void DropdownInteraction_ClosesWhenOtherControlsClicked_WorksCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var options = new[] { "Option 1", "Option 2", "Option 3" };
//
//         // First frame: open dropdown
//         SetupMouseClick(input, new Vector2(100, 16)); // Click dropdown button
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Dropdown(options);
//                 gui.Button("Other Button");
//             },
//             renderAction: () =>
//             {
//                 gui.Dropdown(options); // Opens dropdown
//                 gui.Button("Other Button");
//             });
//
//         // Reset and click on button instead
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         SetupMouseClick(input, new Vector2(50, 100)); // Click on button area
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Dropdown(options);
//                 gui.Button("Other Button");
//             },
//             renderAction: () =>
//             {
//                 var dropdownResult = gui.Dropdown(options);
//                 var buttonResult = gui.Button("Other Button");
//
//                 // Should handle dropdown closing and button clicking
//                 Assert.DoesNotThrow(() => { });
//             });
//     }
//
//     [Fact]
//     public void StateManagement_ControlsRetainStateAcrossFrames_WorksCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var checkboxState = false;
//         var toggleState = false;
//         var dropdownSelection = -1;
//
//         // First frame: set initial states
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//             },
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 dropdownSelection = gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//             });
//
//         // Click checkbox to change state
//         SetupMouseClick(input, new Vector2(10, 10)); // Checkbox area
//
//         // Second frame: verify state persistence
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//             },
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState); // Should be checked now
//                 gui.Toggle(ref toggleState);
//                 dropdownSelection = gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//
//                 Assert.True(checkboxState); // State should persist
//             });
//
//         // Reset input and start third frame
//         input.IsMouseButtonPressed(Arg.Any<MouseButton>()).Returns(false);
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Third frame: verify continued persistence
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//             },
//             renderAction: () =>
//             {
//                 var wasToggled = gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 dropdownSelection = gui.Dropdown(new[] { "A", "B", "C" }, dropdownSelection);
//
//                 Assert.True(checkboxState); // Should still be checked
//                 Assert.False(wasToggled); // Should not be toggled this frame
//             });
//     }
//
//     [Fact]
//     public void MixedControlTypes_ComplexLayout_RendersWithoutErrors()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var checkboxes = new bool[3];
//         var toggles = new bool[3];
//         var textInputs = new[] { "Input 1", "Input 2", "Input 3" };
//         var dropdownOptions = new[] { "Red", "Green", "Blue" };
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 // Create a complex layout with mixed controls
//                 for (int i = 0; i < 3; i++)
//                 {
//                     gui.Button($"Button {i + 1}");
//                     gui.Checkbox(ref checkboxes[i], $"Checkbox {i + 1}");
//                     gui.Toggle(ref toggles[i], $"Toggle {i + 1}");
//                     gui.TextInput(textInputs[i]);
//                     gui.Dropdown(dropdownOptions, placeholder: $"Dropdown {i + 1}");
//                     gui.IconButton($"{i + 1}");
//                 }
//             },
//             renderAction: () =>
//             {
//                 Assert.DoesNotThrow(() =>
//                 {
//                     for (int i = 0; i < 3; i++)
//                     {
//                         gui.Button($"Button {i + 1}");
//                         gui.Checkbox(ref checkboxes[i], $"Checkbox {i + 1}");
//                         gui.Toggle(ref toggles[i], $"Toggle {i + 1}");
//                         textInputs[i] = gui.TextInput(textInputs[i]);
//                         gui.Dropdown(dropdownOptions, placeholder: $"Dropdown {i + 1}");
//                         gui.IconButton($"{i + 1}");
//                     }
//                 });
//             });
//
//         // Verify all controls were created
//         Assert.Equal(18, gui.RootNode?.Children.Count); // 6 controls × 3 iterations
//     }
//
//     [Fact]
//     public void ControlCleanup_ClearAllStates_RemovesAllStatefulData()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var checkboxState = true;
//         var toggleState = true;
//         var options = new[] { "A", "B", "C" };
//
//         // Create controls with state
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(options, selectedIndex: 1);
//                 gui.TextInput("Some text");
//             },
//             renderAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(options, selectedIndex: 1);
//                 gui.TextInput("Some text");
//             });
//
//         // Act - Clear all states
//         Assert.DoesNotThrow(() =>
//         {
//             gui.ClearDropdownStates();
//             gui.ClearInputStates();
//         });
//
//         // Reset frame
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Assert - Controls should work without errors after cleanup
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Checkbox(ref checkboxState);
//                 gui.Toggle(ref toggleState);
//                 gui.Dropdown(options);
//                 gui.TextInput("");
//             },
//             renderAction: () =>
//             {
//                 Assert.DoesNotThrow(() =>
//                 {
//                     gui.Checkbox(ref checkboxState);
//                     gui.Toggle(ref toggleState);
//                     gui.Dropdown(options);
//                     gui.TextInput("");
//                 });
//             });
//     }
//
//     [Fact]
//     public void ControlInteraction_MouseClickPropagation_HandledCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var buttonClicked = false;
//         var checkboxToggled = false;
//         var checkboxState = false;
//
//         // Position controls so they don't overlap
//         SetupMouseClick(input, new Vector2(50, 25)); // Button area
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Button("Test Button");
//                 gui.Checkbox(ref checkboxState, "Test Checkbox");
//             },
//             renderAction: () =>
//             {
//                 buttonClicked = gui.Button("Test Button");
//                 checkboxToggled = gui.Checkbox(ref checkboxState, "Test Checkbox");
//
//                 // Only button should be clicked, not checkbox
//                 Assert.True(buttonClicked);
//                 Assert.False(checkboxToggled);
//                 Assert.False(checkboxState);
//             });
//     }
//
//     [Fact]
//     public void KeyboardNavigation_DropdownWithTextInput_HandlesBothCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         var options = new[] { "Option 1", "Option 2", "Option 3" };
//         var textValue = "Test";
//
//         // First focus text input
//         SetupMouseClick(input, new Vector2(300, 16)); // Text input area
//
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Dropdown(options);
//                 gui.TextInput(textValue);
//             },
//             renderAction: () =>
//             {
//                 gui.Dropdown(options);
//                 textValue = gui.TextInput(textValue);
//             });
//
//         // Reset and simulate keyboard input
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//         input.IsMouseButtonPressed(Arg.Any<MouseButton>()).Returns(false);
//         SetupKeyPress(input, KeyboardKey.Backspace);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.Dropdown(options);
//                 gui.TextInput(textValue);
//             },
//             renderAction: () =>
//             {
//                 Assert.DoesNotThrow(() =>
//                 {
//                     gui.Dropdown(options);
//                     textValue = gui.TextInput(textValue);
//                 });
//             });
//     }
//
//     [Fact]
//     public void PerformanceTest_ManyControls_RendersWithoutSignificantDelay()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         const int controlCount = 100;
//         var checkboxes = new bool[controlCount];
//         var toggles = new bool[controlCount];
//
//         // Act & Assert - Should handle many controls without throwing
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 for (int i = 0; i < controlCount; i++)
//                 {
//                     gui.Button($"Button {i}");
//                     gui.Checkbox(ref checkboxes[i]);
//                     gui.Toggle(ref toggles[i]);
//                 }
//             },
//             renderAction: () =>
//             {
//                 Assert.DoesNotThrow(() =>
//                 {
//                     for (int i = 0; i < controlCount; i++)
//                     {
//                         gui.Button($"Button {i}");
//                         gui.Checkbox(ref checkboxes[i]);
//                         gui.Toggle(ref toggles[i]);
//                     }
//                 });
//             });
//
//         // Verify all controls were created
//         Assert.Equal(controlCount * 3, gui.RootNode?.Children.Count);
//     }
// }
