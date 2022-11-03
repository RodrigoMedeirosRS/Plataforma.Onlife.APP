using Godot;
using System;

public class FioVermelho : Line2D
{
    [Export]
    public NodePath ponto_a = null;
    [Export]
    public NodePath ponto_b = null;

    private Vector2 PontaLinha = new Vector2(0.5f, -0.7f);

    public override void _Ready()
    {
        Position = new Vector2(0, 359);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        Atualizar_posicao();
    }
    public void Atualizar_posicao()
    {
        if (GetNodeOrNull(ponto_a) == null || GetNodeOrNull(ponto_b) == null)
        {
            QueueFree();
        }
        else
        if (ponto_a != null && ponto_b != null)
        {
            SetPointPosition(0, (GetNode<Control>(ponto_a).RectGlobalPosition + GetNode<Control>(ponto_a).RectSize * PontaLinha));//  * OffsetNode.RectScale - OffsetNode.RectPosition));
            SetPointPosition(1, (GetNode<Control>(ponto_b).RectGlobalPosition + GetNode<Control>(ponto_b).RectSize * PontaLinha)); // - OffsetNode.RectPosition* OffsetNode.RectScale + new Vector2(1000, 1000)
        }
    }
    public void set_pontos(Node a, Node b)
    {
        ponto_a = a.GetPath();
        ponto_b = b.GetPath();
        Atualizar_posicao();
    }
}
