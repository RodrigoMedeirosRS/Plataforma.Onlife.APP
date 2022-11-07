using Godot;
using System;

public class MenuSuperior : Control
{
    private Node nodo_pagina { get; set; }
    private WindowDialog ModalSobre { get; set; }
    private WindowDialog ModalEquipes { get; set; }
    private WindowDialog ModalContato { get; set; }


    public override void _Ready()
    {
        //nodo_pagina = FindParent("Main").GetNode("Pagina");
        ModalSobre = GetNode<WindowDialog>("Modais/ModalSobre");
        ModalEquipes = GetNode<WindowDialog>("Modais/ModalEquipe");
        ModalContato = GetNode<WindowDialog>("Modais/ModalContato");
    }
    public void _on_TextureRect_pressed()
    {
        nodo_pagina.Call("SetPagina", "Index");
    }
    private void _on_Sobre_pressed()
    {
        ModalSobre.PopupCentered();
    }
    private void _on_Equipes_pressed()
    {
        ModalEquipes.PopupCentered();
    }
    private void _on_GPedU_pressed()
    {
        OS.ShellOpen("https://gpedu.com.br/");
    }
    private void _on_Contato_pressed()
    {
        ModalContato.PopupCentered();
    }
}
