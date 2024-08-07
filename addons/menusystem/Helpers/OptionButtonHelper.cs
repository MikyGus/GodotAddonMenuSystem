using Godot;

namespace MenuControl.addons.MenuControl.Helpers;
internal static class OptionButtonHelper
{
    public static string GetSelectedItemText(this OptionButton button) => button.GetItemText(button.Selected);

    public static void SelectText(this OptionButton optionButton, string itemToSelect)
    {
        for (int i = 0; i < optionButton.ItemCount; i++)
        {
            if (optionButton.GetItemText(i) == itemToSelect)
            {
                optionButton.Select(i);
                return;
            }
        }
    }
}
