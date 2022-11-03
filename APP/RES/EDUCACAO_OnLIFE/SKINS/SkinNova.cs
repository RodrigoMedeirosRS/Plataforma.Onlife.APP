using Godot;
using System;

public class SkinNova : Control
{
    public override void _Ready()
    {

    }
    public void _on_ButtonFoto_pressed()
    {
        GetNode<Control>("Foto").Visible = !GetNode<Control>("Foto").Visible;
    }

    public void _on_ButtonDescricao_pressed()
    {
        GetNode<Control>("Descricao").Visible = !GetNode<Control>("Descricao").Visible;
    }
    public void _on_ButtonDescricao2_pressed()
    {
        GetNode<Control>("Descricao2").Visible = !GetNode<Control>("Descricao2").Visible;
    }
    public void _on_ButtonDescricao3_pressed()
    {
        GetNode<Control>("Descricao3").Visible = !GetNode<Control>("Descricao3").Visible;
    }
}
