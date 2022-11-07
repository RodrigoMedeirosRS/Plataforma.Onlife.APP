using Godot;
using System;

public class Janela : Control
{
	private bool MousePosicionado { get; set; }

	public override void _Ready()
	{
		MousePosicionado = false;
	}
	public override void _Process(float delta)
	{
		if(MousePosicionado && Input.IsActionPressed("selecao"))
			this.RectGlobalPosition = GetGlobalMousePosition();
	}
	private void _on_Fechar_button_up()
	{
		this.QueueFree();
	}
	private void _on_BarraSuperior_mouse_entered()
	{
		MousePosicionado = true;
	}
	private void _on_BarraSuperior_mouse_exited()
	{
		MousePosicionado = false;
	}
}
