using Godot;
using System;

public class BotaoAbrirMapa : Button
{
    private Node nodo_pagina;

    public override void _Ready()
    {
        nodo_pagina = FindParent("Pagina");
    }

    public void _on_Balloom_pressed()
    {
        nodo_pagina.Call("SetPagina", "Mapa");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
