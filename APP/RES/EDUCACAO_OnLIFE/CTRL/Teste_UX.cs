using Godot;
using System;

public class Teste_UX : Node2D
{
    public WindowDialog Janela { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Olá mundo");
        //Janela = GetNode<WindowDialog>("./CanvasLayer/WindowDialog");
        //Janela.Show();

    }

    public override void _Process(float delta)
    {

    }


}
