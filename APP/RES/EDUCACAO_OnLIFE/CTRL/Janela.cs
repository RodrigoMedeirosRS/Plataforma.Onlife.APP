using Godot;
using System;

using DTO;
using CTRL.Interface;

public class Janela : Control, IDisposableCTRL
{
	public RegistroDTO RegistroDTO { get; set; }
	public ConfirmationDialog Alerta { get; set; }
	private bool MousePosicionado { get; set; }
	private Vector2 MouseOffset { get; set; }
	private AcceptDialog Registro { get; set; }
	private FileDialog SaveDialog { get; set; }
	public bool Edicao { get; set; }
	[Signal] public delegate void DadosCarregados();

	public override void _Ready()
	{
		MousePosicionado = false;
		Edicao = false;
		RegistroDTO = new RegistroDTO();
		Alerta = GetNode<ConfirmationDialog>("Conteudo/Alterta");
		Registro = GetNode<AcceptDialog>("./Conteudo/Registro");
		SaveDialog = GetNode<FileDialog>("./Conteudo/SaveDialog");
	}
	public override void _Process(float delta)
	{
		MoverJanela();
	}
	private void MoverJanela()
	{
		try
		{
			if (MousePosicionado && Input.IsActionJustPressed("selecao"))
				MouseOffset = GetLocalMousePosition();
			if (MousePosicionado && Input.IsActionPressed("selecao"))
				this.RectGlobalPosition = GetGlobalMousePosition() - MouseOffset;
		}
		catch{}
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
	public void FecharCTRL()
	{
		GetNode<Cabecalho>("./Cabecalho").FecharCTRL();
	}
	private void _on_Registro_about_to_show()
	{
		Registro.RectGlobalPosition = this.RectGlobalPosition;
	}
	private void _on_Alterta_about_to_show()
	{
		Alerta.RectGlobalPosition = this.RectGlobalPosition;
	}
	private void _on_SaveDialog_confirmed()
	{
		SaveDialog.RectGlobalPosition = this.RectGlobalPosition;
	}
}
