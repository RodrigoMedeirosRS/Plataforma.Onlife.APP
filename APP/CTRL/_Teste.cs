using Godot;
using System;

public class _Teste : Sprite
{
    private Control OffsetNode;

    public override void _Ready()
    {
        OffsetNode = GetParent<Control>();
    }

    private void mover_mapa()
    {
        if (Input.IsActionJustPressed("clique_direito"))
        {
            Position = OffsetNode.GetLocalMousePosition();
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        mover_mapa();
    }
}
