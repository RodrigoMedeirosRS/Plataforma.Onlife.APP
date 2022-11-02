using Godot;
using System;

public class InterfaceSobreposta : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void _on_ExpandirButton_pressed()
    {
        GetNode<AnimationPlayer>("Anim").Play("Expandir");
    }

    public void _on_CloseButton_pressed()
    {
        GetNode<AnimationPlayer>("Anim").Play("Retrair");
    }
}
