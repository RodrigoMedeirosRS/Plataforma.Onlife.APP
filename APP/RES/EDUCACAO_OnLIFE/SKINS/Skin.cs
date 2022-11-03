using Godot;
using System;

public class Skin : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void _on_ButtonFoto_pressed()
    {
        GetNode<Control>("Foto").Visible = true;
        GetNode<Control>("Nome").Visible = false;
        GetNode<Control>("Descricao").Visible = false;
    }

    public void _on_ButtonVoltar_pressed()
    {
        GetNode<Control>("Nome").Visible = true;
        GetNode<Control>("Foto").Visible = false;
        GetNode<Control>("Descricao").Visible = false;
    }
    public void _on_ButtonDescricao_pressed()
    {
        GetNode<Control>("Descricao").Visible = true;
        GetNode<Control>("Nome").Visible = false;
        GetNode<Control>("Foto").Visible = false;
    }
}
