namespace Guinevere;

public partial class Gui
{
    /// <summary>
    /// Draws text as a layout node: in Pass1Build phase, creates a node sized to the text; in Pass2Render phase, draws the text in the node's rect.
    /// Returns the node for layout chaining.
    /// </summary>
    public LayoutNode DrawText(
        string text,
        float size = 0,
        Color? color = null,
        Font? font = null,
        float wrapWidth = 0,
        bool centerInRect = true,
        bool clip = false)
        => DrawTextOrGlyph(new(
            text,
            font ?? GetEffectiveTextFont(),
            size, color, centerInRect, clip, wrapWidth));

    /// <summary>
    /// Draws a glyph (icon) as a layout node: in Pass1Build phase, creates a node sized to the glyph; in Pass2Render phase, draws the glyph in the node's rect.
    /// Returns the node for layout chaining.
    /// </summary>
    public LayoutNode DrawGlyph(
        char iconCode,
        float size = 0,
        Color? color = null,
        Font? font = null,
        bool centerInRect = true,
        bool clip = false)
        => DrawTextOrGlyph(new(
            iconCode.ToString(),
            font ?? GetEffectiveIconFont(),
            size, color, centerInRect, clip, 0));

    private record struct DrawConfig(
        string Text,
        Font Font,
        float Size,
        Color? Color,
        bool Center,
        bool Clip,
        float WrapWidth);

    private LayoutNode DrawTextOrGlyph(DrawConfig cfg)
    {
        var size = cfg.Size > 0 ? cfg.Size : GetEffectiveTextSize();
        var color = cfg.Color ?? GetEffectiveTextColor();

        var usedFont = new SKFont(cfg.Font.SkFont.Typeface, size);

        // Handle text wrapping if wrapWidth is specified
        var lines = cfg.WrapWidth > 0 ? WrapText(cfg.Text, usedFont, cfg.WrapWidth) : cfg.Text.Split('\n');

        var maxWidth = 0f;
        var lineHeight = usedFont.Size * 1.2f; // Add some line spacing

        foreach (var line in lines)
        {
            usedFont.MeasureText(line, out var lineBounds);
            maxWidth = Math.Max(maxWidth, lineBounds.Width);
        }

        // If wrapping is enabled, use the wrap width as max width
        if (cfg.WrapWidth > 0)
        {
            maxWidth = Math.Min(maxWidth, cfg.WrapWidth);
        }

        var totalHeight = lines.Length * lineHeight;

        var node = Node(maxWidth, totalHeight);

        if (Pass != Pass.Pass2Render)
            return node;

        var paint = new SKPaint { IsAntialias = true, Color = color };


        // Draw each line
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            usedFont.MeasureText(line, out var lineBounds);

            var pos = node.InnerRect.Position;
            pos.Y += (i + 1) * lineHeight; // Move down for each line

            if (cfg.Center)
            {
                pos.X += Math.Max((node.InnerRect.W - lineBounds.Width) * 0.5f, 0f);
            }

            AddDraw(new Text(line, pos, usedFont, paint), cfg.Clip, node);
        }


        return node;
    }

    private string[] WrapText(string text, SKFont font, float maxWidth)
    {
        var lines = new List<string>();
        var paragraphs = text.Split('\n');

        foreach (var paragraph in paragraphs)
        {
            if (string.IsNullOrEmpty(paragraph))
            {
                lines.Add("");
                continue;
            }

            var words = paragraph.Split(' ');
            var currentLine = "";

            foreach (var word in words)
            {
                var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                font.MeasureText(testLine, out var bounds);

                if (bounds.Width <= maxWidth)
                {
                    currentLine = testLine;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                    {
                        // Single word is too long, add it anyway
                        lines.Add(word);
                    }
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }
        }

        return lines.ToArray();
    }
}
