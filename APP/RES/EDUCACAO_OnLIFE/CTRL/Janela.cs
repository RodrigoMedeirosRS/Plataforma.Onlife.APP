using Godot;
using System;

using DTO;

public class Janela : Control
{
	public RegistroDTO RegistroDTO { get; set; }
	public ConfirmationDialog Alerta { get; set; }
	private bool MousePosicionado { get; set; }
	public bool Edicao { get; set; }
	[Signal] public delegate void DadosCarregados();

	public override void _Ready()
	{
		MousePosicionado = false;
		Edicao = false;
		RegistroDTO = new RegistroDTO();
		Alerta = GetNode<ConfirmationDialog>("Conteudo/Alterta");
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
		Edicao = false;
		Alerta.Popup_();
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
