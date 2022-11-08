using Godot;
using System;

using DTO;
using CTRL.Interface;
public class ResultadoRelacao : Control, IDisposableCTRL
{
	private RelacaoDTO Relacao { get; set; }
	public bool Relacionado { get; set; }
	private Label Texto { get; set; }
	private CheckButton RelacaoBTN { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		Relacao = null;
		Relacionado = false;
		RelacaoBTN = GetNode<CheckButton>("./Button");
		Texto = GetNode<Label>("./Label");
	}
	public void DefinirDados(RegistroDTO registro, bool relacionado)
	{
		Relacao = new RelacaoDTO()
		{
			RegistroPessoaID = null,
			RelacaoID = registro.Codigo,
			TipoRelacao = "Autor"
		};
		RelacaoBTN.Pressed = relacionado;
		Texto.Text = registro.Nome;
		Texto.HintTooltip = registro.Descricao;
	}
	private void _on_Button_toggled(bool button_pressed)
	{
		Relacionado = button_pressed;
	}
	public void FecharCTRL()
	{
		QueueFree();
	}
}
