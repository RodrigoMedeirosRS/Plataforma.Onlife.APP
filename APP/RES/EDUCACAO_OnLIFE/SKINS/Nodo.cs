using Godot;
using System;

public class Nodo : TextureButton
{
    public override void _Ready()
    {

    }
    public void _on_Nodo_pressed()
    {
        Visible = false;
    }
}
