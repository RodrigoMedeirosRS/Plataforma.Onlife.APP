using Godot;
using System;

public class ArvoreBrancaCTRL : TextureRect
{
	private TabContainer TabContainer { get; set; }
	public override void _Ready()
	{
		DesativarFuncoesDeAltoProcessamento();
		PopularNodes();
	}
	private void DesativarFuncoesDeAltoProcessamento()
	{
		SetPhysicsProcess(false);
	}
	private void PopularNodes()
	{
		TabContainer = GetParent().GetNode<TabContainer>("./TabContainer");
	}
	public override void _Process(float delta)
	{
		Visible = TabContainer.GetChildCount() == 0;
	}
}
