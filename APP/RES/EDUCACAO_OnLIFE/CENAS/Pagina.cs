using Godot;
using System;

public class Pagina : CanvasLayer
{
    public override void _Ready()
    {
        SetPagina("Index");
    }

    public void SetPagina(string pagina)
    {
        if (pagina == "Index")
        {
            GetNode<Panel>("PaginaIndex").Visible = true;
            GetNode<Panel>("PaginaMapa").Visible = false;
        }
        else if (pagina == "Mapa")
        {
            GetNode<Panel>("PaginaIndex").Visible = false;
            GetNode<Panel>("PaginaMapa").Visible = true;
        }
    }
}
