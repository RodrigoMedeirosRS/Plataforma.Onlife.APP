using Godot;
using System;

public class PainelDeFundo : Panel
{
	private Vector2 posicaoAnteriorMouse = new Vector2(0, 0);
	private Control OffsetNode;

	public override void _Ready()
	{
		OffsetNode = GetNode<Control>("Offset");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		mover_mapa();
		zoom();
	}

	private void mover_mapa()
	{
		if (Input.IsActionJustPressed("clique_scroll"))
		{
			posicaoAnteriorMouse = (OffsetNode.GetLocalMousePosition() * OffsetNode.RectScale);
		}
		if (Input.IsActionPressed("clique_scroll"))
		{
			OffsetNode.RectPosition += (OffsetNode.GetLocalMousePosition() * OffsetNode.RectScale - posicaoAnteriorMouse);
		}
	}

	private void zoom()
	{

		if (Input.IsActionJustReleased("scroll_up"))
		{
			//Vector2 vetor = (OffsetNode.RectPivotOffset - (OffsetNode.GetLocalMousePosition() * OffsetNode.RectScale));
			//OffsetNode.RectPosition += vetor;
			//OffsetNode.RectPivotOffset += OffsetNode.GetLocalMousePosition() - OffsetNode.RectPivotOffset;// * OffsetNode.RectScale;//GetLocalMousePosition
			//OffsetNode.RectPosition = new Vector2(0, 0);
			OffsetNode.RectScale *= 1.1f;
			//OffsetNode.RectPosition -= (OffsetNode.GetLocalMousePosition() - OffsetNode.RectPosition * 0.5f);
		}
		if (Input.IsActionJustReleased("scroll_down"))
		{
			//OffsetNode.RectPosition = OffsetNode.GetGlobalMousePosition();// - OffsetNode.RectPosition;
			//OffsetNode.RectPivotOffset += OffsetNode.GetLocalMousePosition() - OffsetNode.RectPivotOffset;//(OffsetNode.RectPivotOffset - OffsetNode.RectPosition);// * OffsetNode.RectScale;

			OffsetNode.RectScale *= 0.9f;
		}
		if (Input.IsActionJustReleased("restaurar_zoom"))
		{
			reset_zoom();
		}
	}
	public void reset_zoom()
	{
		OffsetNode.RectScale = new Vector2(1, 1);
	}
	public void _on_PaginaMapa_visibility_changed()
	{
		reset_zoom();
	}

}
