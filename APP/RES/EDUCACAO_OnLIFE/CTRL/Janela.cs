using Godot;
using System;


public class Janela : Control
{
    private bool EhPodeArrastar = false;
    private bool EhArrastando = false;
    private Vector2 mouse_offset = new Vector2(0, 0);
    
    [Export]
    private string Titulo = "Janela sem título";
    private Control NodoJanelas = null;

    public override void _Ready()
    {
        //set_titulo(Titulo);
        NodoJanelas = GetParent<Control>();
        set_titulo(Titulo);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("clique_esquerdo") && EhPodeArrastar)
        {
            EhArrastando = true;
            mouse_offset = GetLocalMousePosition();
            NodoJanelas.MoveChild(this, NodoJanelas.GetChildCount());
        }
        else if (EhArrastando && Input.IsActionJustReleased("clique_esquerdo"))
        {
            EhArrastando = false;
        }

        if (EhArrastando)
        {
            RectPosition = GetParentOrNull<Control>().GetLocalMousePosition() - mouse_offset;
        }
    }


    public void _on_BarraDeTituloJanela_mouse_entered()
    {
        EhPodeArrastar = true;
    }

    public void _on_BarraDeTituloJanela_mouse_exited()
    {
        EhPodeArrastar = false;
    }


    public void _on_CloseButton_pressed()
    {
        QueueFree();
    }


    public void set_titulo(string novo_titulo)
    {
        GetNode<Label>("BarraDeTituloJanela/MarginContainer/Titulo").Text = Titulo;
    }

}
