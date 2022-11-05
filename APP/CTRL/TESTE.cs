using Godot;
using System;

public class TESTE : Control
{
	public override void _Ready()
	{
		GetNode<ConfirmationDialog>("./CanvasLayer/CadastroDePessoa").Popup_();
	}
}
