using Godot;
using System;

using DTO;
using CTRL.Interface;

public class ResultadoBusca : Control, IDisposableCTRL
{
	private RegistroDTO Registro { get; set; }
	private PessoaDTO PistaViva { get; set; }
	private Label Texto { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		Registro = null;
		PistaViva = null;
		Texto = GetNode<Label>("./Label");
	}
	private void DefinirDados(RegistroDTO registro)
	{
		if(PistaViva != null)
			PistaViva.Dispose();
		Registro = registro;
		Texto.Text = registro.Nome;
		Texto.HintTooltip = registro.Descricao;
	}
	private void DefinirDados(PessoaDTO pistaViva)
	{
		if(Registro != null)
			Registro.Dispose();
		PistaViva = pistaViva;
		Texto.Text = pistaViva.Nome;
		Texto.HintTooltip = string.Empty;
	}
	private void _on_Busca_button_up()
	{
		if (PistaViva != null)
			Main.InstanciarPrimeiraPessoa(PistaViva);
		if (Registro != null)
			Main.InstanciarPrimeiroRegisro(Registro);
	}
	public void FecharCTRL()
	{
		if(Registro != null)
			Registro.Dispose();
		if(PistaViva != null)
			PistaViva.Dispose();
		QueueFree();
	}
}
