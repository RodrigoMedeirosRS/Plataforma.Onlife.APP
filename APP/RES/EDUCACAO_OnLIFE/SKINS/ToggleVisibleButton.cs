using Godot;
using System;

public class ToggleVisibleButton : TextureButton
{
    [Export]
    public NodePath alvo = null;
    public Control alvo_object = null;


    public override void _Ready()
    {
        if (alvo != null)
        {
            alvo_object = GetNodeOrNull<Control>(alvo);
        }
        if (alvo_object == null)
        {
            alvo_object = GetChildOrNull<Control>(0);
        }
    }

    public void _on_ToggleVisibleButton_pressed()
    {
        if (alvo_object != null)
        {
            alvo_object.Visible = !alvo_object.Visible;
        }
    }

    public void _on_ToggleVisibleButton_visibility_changed()
    {
        if (alvo_object != null)
        {
            alvo_object.Visible = false;
        }
    }
}
