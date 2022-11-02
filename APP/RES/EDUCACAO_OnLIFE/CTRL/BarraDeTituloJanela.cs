using Godot;
using System;

public class BarraDeTituloJanela : Panel
{
    private bool IsMouseOver = false;
    private bool EhArrastando = false;
    private Vector2 mouse_offset = new Vector2(0, 0);
    private Vector2 BarraDeTitulo_offset = new Vector2(0, 70);
    private Control NodoJanelas = null;
    private Control Janela = null;
    [Export]
    private string Titulo = "Janela sem título";


    public override void _Ready()
    {
        Janela = GetParent<Control>();
        NodoJanelas = Janela.GetParent<Control>();
        set_titulo(Titulo);
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("clique_esquerdo") && IsMouseOver)
        {
            EhArrastando = true;
            mouse_offset = GetLocalMousePosition();
            NodoJanelas.MoveChild(Janela, NodoJanelas.GetChildCount());
        }
        else if (EhArrastando && Input.IsActionJustReleased("clique_esquerdo"))
        {
            EhArrastando = false;
        }

        if (EhArrastando)
        {
            GetParent<Control>().RectPosition = GetGlobalMousePosition() - mouse_offset + BarraDeTitulo_offset;
        }
    }

    public void _on_BarraDeTituloJanela_mouse_entered()
    {
        IsMouseOver = true;
    }

    public void _on_BarraDeTituloJanela_mouse_exited()
    {
        IsMouseOver = false;
    }


    public void _on_CloseButton_pressed()
    {
        Janela.QueueFree();
    }

    public void set_titulo(string novo_titulo)
    {
        GetNode<Label>("MarginContainer/Titulo").Text = Titulo;
    }


}
