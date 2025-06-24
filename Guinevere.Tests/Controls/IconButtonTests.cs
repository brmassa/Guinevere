// using NSubstitute;
// using Xunit;
//
// namespace Guinevere.Tests.Controls;
//
// public class IconButtonTests : PrimitiveControlsTestBase
// {
//     [Fact]
//     public void IconButton_BuildStage_CreatesNodeWithCorrectId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.IconButton("🏠");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         Assert.Contains("IconButtonTests.cs", node.Id);
//     }
//
//     [Fact]
//     public void IconButton_WithDefaultSize_CreatesSquareButton()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float expectedSize = 32f;
//
//         // Act
//         gui.IconButton("⚙️");
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, expectedSize, expectedSize);
//     }
//
//     [Fact]
//     public void IconButton_WithCustomSize_UsesProvidedSize()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//         const float expectedSize = 64f;
//
//         // Act
//         gui.IconButton("📁", size: expectedSize);
//
//         // Assert
//         var node = gui.RootNode?.Children.FirstOrDefault();
//         Assert.NotNull(node);
//         AssertNodeHasSize(node, expectedSize, expectedSize);
//     }
//
//     [Fact]
//     public void IconButton_NotClicked_ReturnsFalse()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("❌");
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_ClickedInsideBounds_ReturnsTrue()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(16, 16)); // Click inside center of default 32x32 button
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("✓"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("✓");
//                 Assert.True(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_ClickedOutsideBounds_ReturnsFalse()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(100, 100)); // Click outside button bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("✓"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("✓");
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_HoveredInsideBounds_HandlesProperly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseHover(input, new Vector2(16, 16)); // Hover inside button bounds
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🔍"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🔍");
//                 Assert.False(result); // No click, just hover
//             });
//     }
//
//     [Theory]
//     [InlineData(MouseButton.Left)]
//     [InlineData(MouseButton.Right)]
//     [InlineData(MouseButton.Middle)]
//     public void IconButton_ClickedWithDifferentMouseButtons_OnlyLeftClickReturnsTrue(MouseButton button)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(16, 16), button);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🎯"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🎯");
//                 Assert.Equal(button == MouseButton.Left, result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithCustomColors_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var backgroundColor = Color.Blue;
//         var hoverColor = Color.LightBlue;
//         var iconColor = Color.White;
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🎨",
//                     backgroundColor: backgroundColor,
//                     hoverColor: hoverColor,
//                     iconColor: iconColor);
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithoutBackgroundColor_HandlesTransparency()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("👻", backgroundColor: null);
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_MultipleIconButtons_EachHasUniqueId()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         SimulateBuildStage(gui);
//
//         // Act
//         gui.IconButton("🏠");
//         gui.IconButton("⚙️");
//         gui.IconButton("📁");
//
//         // Assert
//         Assert.Equal(3, gui.RootNode?.Children.Count);
//         var ids = gui.RootNode?.Children.Select(c => c.Id).ToArray();
//         Assert.NotNull(ids);
//         Assert.Equal(3, ids.Distinct().Count()); // All IDs should be unique
//     }
//
//     [Fact]
//     public void IconButton_CalledMultipleTimesWithSameParameters_MaintainsConsistentState()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert - First call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🔄"),
//             renderAction: () =>
//             {
//                 var firstResult = gui.IconButton("🔄");
//                 Assert.False(firstResult);
//             });
//
//         // Reset for second call
//         gui.EndFrame();
//         gui.BeginFrame(CreateTestCanvas());
//
//         // Act & Assert - Second call
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🔄"),
//             renderAction: () =>
//             {
//                 var secondResult = gui.IconButton("🔄");
//                 Assert.False(secondResult);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithEmptyIcon_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result1 = gui.IconButton("");
//                 var result2 = gui.IconButton(string.Empty);
//                 Assert.False(result1);
//                 Assert.False(result2);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithTextIcon_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result1 = gui.IconButton("X");
//                 var result2 = gui.IconButton("OK");
//                 var result3 = gui.IconButton("123");
//                 Assert.False(result1);
//                 Assert.False(result2);
//                 Assert.False(result3);
//             });
//     }
//
//     [Theory]
//     [InlineData(0f)] // Zero size
//     [InlineData(-10f)] // Negative size
//     [InlineData(1000f)] // Very large size
//     [InlineData(0.1f)] // Very small size
//     public void IconButton_WithEdgeCaseSizes_HandlesGracefully(float size)
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("📐", size: size);
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithCustomFontSize_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result1 = gui.IconButton("🔤", fontSize: 8f);
//                 var result2 = gui.IconButton("🔤", fontSize: 24f);
//                 var result3 = gui.IconButton("🔤", fontSize: 0f);
//                 Assert.False(result1);
//                 Assert.False(result2);
//                 Assert.False(result3);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithCustomRadius_RendersCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result1 = gui.IconButton("🔲", radius: 0f);
//                 var result2 = gui.IconButton("🔲", radius: 16f);
//                 var result3 = gui.IconButton("🔲", radius: -5f);
//                 Assert.False(result1);
//                 Assert.False(result2);
//                 Assert.False(result3);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithUnicodeEmojis_HandlesCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result1 = gui.IconButton("😀");
//                 var result2 = gui.IconButton("🎉");
//                 var result3 = gui.IconButton("🌟");
//                 var result4 = gui.IconButton("🚀");
//                 var result5 = gui.IconButton("💫");
//                 Assert.False(result1);
//                 Assert.False(result2);
//                 Assert.False(result3);
//                 Assert.False(result4);
//                 Assert.False(result5);
//             });
//     }
//
//     [Fact]
//     public void IconButton_SimultaneousHoverAndClick_PrioritizesClick()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseClick(input, new Vector2(16, 16)); // Both hover and click
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🎪"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🎪");
//                 Assert.True(result); // Click should be detected
//             });
//     }
//
//     [Fact]
//     public void IconButton_LargeButtonWithSmallClick_DetectsClickCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         const float largeSize = 100f;
//         SetupMouseClick(input, new Vector2(50, 50)); // Click in center of large button
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🎯", size: largeSize),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🎯", size: largeSize);
//                 Assert.True(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_WithNullIcon_HandlesGracefully()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => { },
//             renderAction: () =>
//             {
//                 var result = gui.IconButton(null!);
//                 Assert.False(result);
//             });
//     }
//
//     [Fact]
//     public void IconButton_PressedState_DetectedCorrectly()
//     {
//         // Arrange
//         var gui = CreateTestGui();
//         var input = gui.Input;
//         SetupMouseHover(input, new Vector2(16, 16));
//         SetupMouseDown(input, new Vector2(16, 16), MouseButton.Left);
//
//         // Act & Assert
//         SimulateFullControlLifecycle(gui,
//             buildAction: () => gui.IconButton("🔥"),
//             renderAction: () =>
//             {
//                 var result = gui.IconButton("🔥");
//                 // Note: pressed is different from clicked
//                 Assert.False(result); // No click event, just held down
//             });
//     }
// }
