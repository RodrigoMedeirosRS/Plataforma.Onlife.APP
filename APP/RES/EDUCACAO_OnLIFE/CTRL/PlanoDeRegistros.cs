using Godot;
using System;

public class PlanoDeRegistros : Node
{
	public Control Container { get; set; }
	private ColorRect Veu { get; set; }
	private Vector2 MouseOffset { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		MouseOffset = new Vector2();
		Container = GetNode<Control>("./CanvasLayer/Container");
		Veu = GetNode<ColorRect>("./CanvasLayer/Veu");
	}
	private void AlternarVisiblidade()
	{
		Veu.Visible = Container.GetChildCount() > 0;
		if (!Veu.Visible)
			Container.RectPosition = new Vector2(960, 540);
	}
	public override void _Process(float delta)
	{
		AlternarVisiblidade();
		Mover();
	}
	public void Mover()
	{
		if (Veu.Visible)
		{
			if (Input.IsActionJustPressed("meio"))
				MouseOffset = Container.GetLocalMousePosition();
			if (Input.IsActionPressed("meio"))
				Container.RectGlobalPosition = Container.GetGlobalMousePosition() - MouseOffset;
		}
	}
}
