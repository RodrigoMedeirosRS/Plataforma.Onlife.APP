using Godot;
using System;
using System.Threading.Tasks;

using BibliotecaViva.DTO;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class PessoaBoxCTRL : Panel, IDisposableCTRL
	{
		public int Coluna { get; set; }
		public TabBuscarCTRL TabBuscar { get; set; }
		public TabSonarCTRL TabSonar { get; set; }
		public PessoaDTO Pessoa { get; set; }
		private Label NomeCompleto { get; set; }
		private Label Genero { get; set; }
		private Label Apelido { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularNodes()
		{
			NomeCompleto = GetNode<Label>("./Conteudo/NomeCompleto/Conteudo");
			Genero = GetNode<Label>("./Conteudo/VBoxContainer/Genero/Conteudo");
			Apelido = GetNode<Label>("./Conteudo/VBoxContainer/Apelido/Conteudo");
		}
		public void Preencher(PessoaDTO pessoaDTO, Vector2 posicao)
		{
			RectPosition = posicao;
			Pessoa = pessoaDTO;
			NomeCompleto.Text = Pessoa.Nome + " " + Pessoa.Sobrenome;
			PopularCampoOpcional(Genero, Pessoa.Genero);
			PopularCampoOpcional(Apelido, Pessoa.Apelido);
		}
		private void PopularCampoOpcional(Label campo, string conteudo)
		{
			(campo.GetParent() as Control).Visible = (!string.IsNullOrEmpty(conteudo) && conteudo != "Prefiro não declarar");
			campo.Text = conteudo;
		}
		private void _on_Editar_button_up()
		{
			MainCTRL.EditarPessoa(Pessoa);
		}
		private void _on_Button_button_up()
		{
			if (TabBuscar != null)
				Task.Run(async () => await TabBuscar.BuscarRelacoes(Pessoa, Coluna, this));
			else
				Task.Run(async () => await TabSonar.BuscarRelacoes(Pessoa, Coluna, this));
		}
		public void FecharCTRL()
		{
			NomeCompleto.QueueFree();
			Genero.QueueFree();
			Apelido.QueueFree();
			if (Pessoa != null)
				Pessoa.Dispose();
			QueueFree();
		}
	}
}
