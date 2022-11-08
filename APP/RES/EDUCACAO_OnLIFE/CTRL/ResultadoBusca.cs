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
	public void DefinirDados(RegistroDTO registro)
	{
		if(PistaViva != null)
		{
			PistaViva.Dispose();
			PistaViva = null;
		}
		Registro = registro;
		Texto.Text = registro.Nome;
		Texto.HintTooltip = registro.Descricao;
	}
	public void DefinirDados(PessoaDTO pistaViva)
	{
		if(Registro != null)
		{
			Registro.Dispose();
			Registro = null;
		}
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
		GetParent().GetParent().GetParent().GetParent<AcceptDialog>().Hide();
	}
	public void FecharCTRL()
	{
		QueueFree();
	}
}
