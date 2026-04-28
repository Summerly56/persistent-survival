using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// Converts [check] tags into clickable buttons that toggle between ✔ and ✖.
/// </summary>
public sealed class CheckTagHandler : IMarkupTagHandler
{
    public string Name => "check";

    private static int _checkCounter;

    public static void ResetCheckCounter() => _checkCounter = 0;

    public void PushDrawContext(MarkupNode node, MarkupDrawingContext context) { }
    public void PopDrawContext(MarkupNode node, MarkupDrawingContext context) { }
    public string TextBefore(MarkupNode node) => "";
    public string TextAfter(MarkupNode node) => "";

    public bool TryCreateControl(MarkupNode node, [NotNullWhen(true)] out Control? control)
    {
        var btn = new Button
        {
            Text = "☐",
            MinSize = new Vector2(PaperTagHelper.FontLineHeight + 2, PaperTagHelper.FontLineHeight + 2),
            MaxSize = new Vector2(PaperTagHelper.FontLineHeight + 2, PaperTagHelper.FontLineHeight + 2),
            Margin = new Thickness(1, 0, 1, 0),
            StyleClasses = { "ButtonSquare" },
            TextAlign = Label.AlignMode.Center,
            Name = $"check_{_checkCounter++}"
        };

        btn.OnPressed += _ =>
        {
            if (PaperTagHelper.FindPaperWindow(btn) is { } paperWindow)
            {
                var buttonIndex = PaperTagHelper.CountButtonsBefore(btn, b => Array.Exists(PaperTagHelper.CheckSymbols, s => s == b.Text));
                paperWindow.OpenCheckDialog(buttonIndex);
            }
        };

        control = btn;
        return true;
    }
}
