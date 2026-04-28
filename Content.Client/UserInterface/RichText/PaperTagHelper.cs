using Content.Client.Paper.UI;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.RichText;

/// <summary>
/// Shared helper methods for paper form tag handlers ([form], [signature], [check]).
/// </summary>
public static class PaperTagHelper
{
    public static float FontLineHeight { get; set; } = 16.0f;
    public static readonly string[] CheckSymbols = ["☐", "✔", "✖"];

    public static PaperWindow? FindPaperWindow(Control control)
    {
        var parent = control.Parent;
        while (parent != null && parent is not PaperWindow)
            parent = parent.Parent;
        return parent as PaperWindow;
    }

    public static int CountButtonsBefore(Control target, Func<Button, bool> predicate)
    {
        var root = target;
        while (root.Parent != null)
            root = root.Parent;

        var count = 0;
        var found = false;
        CountButtonsRecursive(root, target, predicate, ref count, ref found);
        return found ? count : 0;
    }

    private static void CountButtonsRecursive(Control control, Control target, Func<Button, bool> predicate, ref int count, ref bool found)
    {
        if (found)
            return;

        if (control is Button btn && predicate(btn))
        {
            if (control == target)
            {
                found = true;
                return;
            }
            count++;
        }

        foreach (var child in control.Children)
            CountButtonsRecursive(child, target, predicate, ref count, ref found);
    }
}
