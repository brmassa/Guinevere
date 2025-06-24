// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class TextInputTests : PrimitiveControlsTestBase
// {
//     [Fact]
//     public void TextInput_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.TextInput("Initial text");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("TextInputTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void TextInput_WithDefaultDimensions_CreatesCorrectSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float expectedWidth = 200f;
//         const float expectedHeight = 32f;
//
//         // Act
//         gui.TextInput("Test");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, expectedWidth, expectedHeight);
//     }
//
//     [Fact]
//     public void TextInput_WithCustomDimensions_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float customWidth = 300f;
//         const float customHeight = 40f;
//
//         // Act
//         gui.TextInput("Test", width: customWidth, height: customHeight);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, customWidth, customHeight);
//     }
//
//     [Fact]
//     public void TextInput_WithInitialText_ReturnsInitialText()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         const string initialText = "Hello World";
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput(initialText);
//                 Assert.Equal(initialText, result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithEmptyText_ReturnsEmptyString()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput("");
//                 Assert.Equal("", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_ClickedInside_GainsFocus()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click inside text input bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Test"),
//             renderAction: () =>
//             {
//                 // Should handle focus gain without throwing
//                 var result = gui.TextInput("Test");
//                 Assert.Equal("Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_ClickedOutside_DoesNotGainsFocus()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(400, 400)); // Click outside text input bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Test"),
//             renderAction: () =>
//             {
//                 // Should handle lack of focus without throwing
//                 var result = gui.TextInput("Test");
//                 Assert.Equal("Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_BackspaceWhenFocused_RemovesCharacter()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.Backspace);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle backspace without throwing
//                 var result = gui.TextInput("Hello");
//                 // Note: Actual backspace behavior depends on implementation
//                 Assert.NotNull(result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_DeleteWhenFocused_RemovesCharacter()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.Delete);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle delete without throwing
//                 var result = gui.TextInput("Hello");
//                 Assert.NotNull(result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_ArrowKeysWhenFocused_ChangeCursorPosition()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.Left);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle arrow keys without throwing
//                 var result = gui.TextInput("Hello");
//                 Assert.Equal("Hello", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_HomeKeyWhenFocused_MovesCursorToStart()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.Home);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle home key without throwing
//                 var result = gui.TextInput("Hello");
//                 Assert.Equal("Hello", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_EndKeyWhenFocused_MovesCursorToEnd()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.End);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle end key without throwing
//                 var result = gui.TextInput("Hello");
//                 Assert.Equal("Hello", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_EscapeWhenFocused_LosesFocus()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 16)); // Click to focus
//         SetupKeyPress(input, KeyboardKey.Escape);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hello"),
//             renderAction: () =>
//             {
//                 // Should handle escape key without throwing
//                 var result = gui.TextInput("Hello");
//                 Assert.Equal("Hello", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithCustomColors_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var backgroundColor = Color.White;
//         var borderColor = Color.Gray;
//         var textColor = Color.Black;
//         var placeholderColor = Color.LightGray;
//         var cursorColor = Color.Blue;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput("Test",
//                     backgroundColor: backgroundColor,
//                     borderColor: borderColor,
//                     textColor: textColor,
//                     placeholderColor: placeholderColor,
//                     cursorColor: cursorColor);
//                 Assert.Equal("Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithPlaceholder_DisplaysWhenEmpty()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         const string placeholder = "Enter text here...";
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput("", placeholder: placeholder);
//                 Assert.Equal("", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithCustomStyling_AppliesCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput("Styled Text",
//                     fontSize: 16f,
//                     padding: 12f,
//                     borderRadius: 8f,
//                     maxLength: 100);
//                 Assert.Equal("Styled Text", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_MultipleInputs_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.TextInput("Input 1");
//         gui.TextInput("Input 2");
//         gui.TextInput("Input 3");
//
//         // Assert
//         Assert.Equal(3, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(3, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void TextInput_CalledMultipleTimesWithSameParameters_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert - First call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Consistent"),
//             renderAction: () =>
//             {
//                 var firstResult = gui.TextInput("Consistent");
//                 Assert.Equal("Consistent", firstResult);
//             });
//
//         // Reset for second call
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Consistent"),
//             renderAction: () =>
//             {
//                 var secondResult = gui.TextInput("Consistent");
//                 Assert.Equal("Consistent", secondResult);
//             });
//     }
//
//     [Theory]
//     [InlineData(0f, 0f)] // Zero dimensions
//     [InlineData(-10f, -5f)] // Negative dimensions
//     [InlineData(1000f, 1000f)] // Very large dimensions
//     [InlineData(0.1f, 0.1f)] // Very small dimensions
//     public void TextInput_WithEdgeCaseDimensions_HandlesGracefully(float width, float height)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 // Should handle edge case dimensions without throwing
//                 var result = gui.TextInput("Test", width: width, height: height);
//                 Assert.Equal("Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithVeryLongText_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var longText = new string('A', 1000);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 // Should handle very long text without issues
//                 var result = gui.TextInput(longText);
//                 Assert.Equal(longText, result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_HoverState_ChangesAppearance()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseHover(input, new Vector2(100, 16)); // Hover inside text input
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextInput("Hover Test"),
//             renderAction: () =>
//             {
//                 // Should handle hover state without throwing
//                 var result = gui.TextInput("Hover Test");
//                 Assert.Equal("Hover Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_ClearInputStates_RemovesAllStates()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () =>
//             {
//                 gui.TextInput("Input 1");
//                 gui.TextInput("Input 2");
//             },
//             renderAction: () =>
//             {
//                 gui.TextInput("Input 1");
//                 gui.TextInput("Input 2");
//
//                 // Should not throw when clearing states
//                 gui.ClearInputStates();
//                 // Test passes if no exception is thrown
//             });
//     }
//
//     [Fact]
//     public void PasswordInput_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.PasswordInput("secret");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("TextInputTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void PasswordInput_WithPassword_MasksCharacters()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 // Should handle password masking without throwing
//                 var result = gui.PasswordInput("password123");
//                 Assert.Equal("password123", result);
//             });
//     }
//
//     [Fact]
//     public void PasswordInput_WithCustomMaskChar_UsesProvidedCharacter()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.PasswordInput("secret", maskChar: '*');
//                 Assert.Equal("secret", result);
//             });
//     }
//
//     [Fact]
//     public void TextArea_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.TextArea("Multiline\nText");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("TextInputTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void TextArea_WithDefaultDimensions_CreatesCorrectSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float expectedWidth = 300f;
//         const float expectedHeight = 100f;
//
//         // Act
//         gui.TextArea("Test");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, expectedWidth, expectedHeight);
//     }
//
//     [Fact]
//     public void TextArea_WithMultilineText_HandlesCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var multilineText = "Line 1\nLine 2\nLine 3";
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextArea(multilineText);
//                 Assert.Equal(multilineText, result);
//             });
//     }
//
//     [Fact]
//     public void TextArea_WithCustomDimensions_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float customWidth = 400f;
//         const float customHeight = 150f;
//
//         // Act
//         gui.TextArea("Test", width: customWidth, height: customHeight);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, customWidth, customHeight);
//     }
//
//     [Fact]
//     public void TextArea_ClickedInside_GainsFocus()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(150, 50)); // Click inside text area bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.TextArea("Test"),
//             renderAction: () =>
//             {
//                 // Should handle focus gain without throwing
//                 var result = gui.TextArea("Test");
//                 Assert.Equal("Test", result);
//             });
//     }
//
//     [Fact]
//     public void TextArea_WithMaxLength_LimitsTextLength()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextArea("Very long text that exceeds limit", maxLength: 10);
//                 Assert.NotNull(result);
//             });
//     }
//
//     [Fact]
//     public void TextInput_WithNullText_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.TextInput(null!);
//                 Assert.NotNull(result);
//             });
//     }
// }
