using Godot;
using System;
using System.Collections.Generic;

namespace MenuControl.addons.MenuControl.Helpers;
public static class AddNodes
{
    public static void AddTitleNode(Control parentNode, string text, string color = "gray", int size = 10)
    {
        RichTextLabel title = new()
        {
            Text = $"[font_size={size}][color={color}]{text}[/color][/font_size]",
            BbcodeEnabled = true,
            FitContent = true,
        };
        parentNode.AddChild(title);
    }

    public static OptionButton AddOptionsDropdownNode(
        Control parentNode,
        IEnumerable<string> contentItems,
        Func<string, bool> showItemPredicate = null,
        Func<string, bool> selectItemPredicate = null)
    {
        OptionButton optionButton = new();
        foreach (string item in contentItems)
        {
            if (showItemPredicate(item) == false)
            {
                continue;
            }
            optionButton.AddItem(item);
            if (selectItemPredicate(item))
            {
                optionButton.Select(optionButton.ItemCount - 1);
            }
        }
        parentNode.AddChild(optionButton);
        return optionButton;
    }

    public static OptionButton AddOptionsDropdownNode(
        Control parentNode,
        IEnumerable<NodePath> contentItems,
        Func<NodePath, bool> showItemPredicate = null,
        Func<NodePath, bool> selectItemPredicate = null)
    {
        OptionButton optionButton = new();
        foreach (NodePath item in contentItems)
        {
            if (showItemPredicate(item) == false)
            {
                continue;
            }
            optionButton.AddItem(item);
            if (selectItemPredicate(item))
            {
                optionButton.Select(optionButton.ItemCount - 1);
            }
        }
        parentNode.AddChild(optionButton);
        return optionButton;
    }

    public static CheckBox AddCheckboxNode(string text, bool isChecked) => new()
    {
        Text = text,
        ButtonPressed = isChecked,
    };
}