using Godot;
using System;

using DTO;

public class Janela : Control
{
	public RegistroDTO RegistroDTO { get; set; }
	private bool MousePosicionado { get; set; }

	[Signal] public delegate void DadosCarregados();
	[Signal] public delegate void Fechar();
	public override void _Ready()
	{
		MousePosicionado = false;
		RegistroDTO = new RegistroDTO();
	}
	public override void _Process(float delta)
	{
		if(MousePosicionado && Input.IsActionPressed("selecao"))
			this.RectGlobalPosition = GetGlobalMousePosition();
	}
	public void PopularDados(RegistroDTO registro)
	{
		RegistroDTO = registro;
		EmitSignal("DadosCarregados");
	}
	private void _on_Fechar_button_up()
	{
		EmitSignal("Fechar");
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
