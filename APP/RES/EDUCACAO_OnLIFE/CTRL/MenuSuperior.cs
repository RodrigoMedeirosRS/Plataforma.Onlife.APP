using Godot;
using System;

public class MenuSuperior : Control
{
    private Node nodo_pagina;
    public override void _Ready()
    {
        nodo_pagina = FindParent("Main").GetNode("Pagina");
    }
    public void _on_TextureRect_pressed()
    {

        nodo_pagina.Call("SetPagina", "Index");
    }

}
