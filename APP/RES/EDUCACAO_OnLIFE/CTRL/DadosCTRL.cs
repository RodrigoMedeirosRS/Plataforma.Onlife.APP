using Godot;
using System;
using System.Threading.Tasks;
using DTO.Dominio;

using DTO;
using BLL;
using CTRL.Interface;
using BLL.Interface;

namespace Onlife.CTRL
{
	public class DadosCTRL : Control, IDisposableCTRL
	{
		private AcceptDialog PopErro { get; set; }
		private HBoxContainer Coluna { get; set; }

		private JanelaBase Pessoa { get; set; }
		private JanelaBase Registro { get; set; }
		private BarraDeBusca BarraBusca { get; set; }
		private Node Sobre { get; set; }
		private Node GPedU { get; set; }
		private Node Equipe { get; set; }
		private Node Contato { get; set; }

		private IConsultarRegistroBLL RegistroBLL { get; set; }
		private IConsultarPessoaBLL PessoaBLL { get; set; }
		private IBuscarBLL BuscarBLL { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
		}
		public void FecharCTRL()
		{
			PopErro.QueueFree();
			Coluna.QueueFree();
			RegistroBLL.Dispose();
			PessoaBLL.Dispose();
			BuscarBLL.Dispose();
			Sobre.QueueFree();
			GPedU.QueueFree();
			Equipe.QueueFree();
			Contato.QueueFree();
			BarraBusca.QueueFree();
			QueueFree();
		}
		private void PopularNodes()
		{
			PopErro = GetNode<AcceptDialog>("./PopErro");
			Coluna = GetNode<HBoxContainer>("./Coluna/Colunas");

			Pessoa = GetNode<JanelaBase>("./JanelaPessoa");
			Registro = GetNode<JanelaBase>("./JanelaRegistro");
			BarraBusca = GetNode<BarraDeBusca>("../BarraDeBusca");

			Sobre = GetNode<Node>("./JanelaSobre");
			GPedU = GetNode<Node>("./JanelaGPedU");
			Equipe = GetNode<Node>("./JanelaEquipe");
			Contato = GetNode<Node>("./JanelaContato");
		}
		private void RealizarInjecaoDeDependencias()
		{
			RegistroBLL = new ConsultarRegistroBLL();
			PessoaBLL = new ConsultarPessoaBLL();
			BuscarBLL = new BuscarBLL(Coluna);
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetProcess(false);
			SetPhysicsProcess(false);
		}
		private void ExibirErro(string mensagem)
		{
			PopErro.DialogText = mensagem;
			PopErro.Popup_();
		}
		private void _on_NovaPistaViva_button_up()
		{
			InstanciarPessoaBox(null, ObterColuna(0), 0);
		}
		private void _on_NovoRegistro_button_up()
		{
			InstanciarPessoaBox(null, ObterColuna(0), 0);
		}
		private void _on_Buscar_toggled(bool button_pressed)
		{
			BarraBusca.Exibir(button_pressed);
		}
		private async void _on_BtnBuscar_button_up()
		{
			var termos = BarraBusca.ObterTermoDeBusca();
			if (BarraBusca.ObterSelecao() == "Pista Viva")
				Task.Run(async () => await RealizarConsultaPessoa(termos[0], termos[1]));
			else
				Task.Run(async () => await RealizarConsultaRegistro(termos[0], termos[1]));
		}
		private void _on_Explorar_button_up()
		{
			// TODO: AQUI VIRÁ O SONAR NO FUTURO
		}
		private void _on_Sobre_button_up()
		{
			InstanciarOutraJanela(Sobre, ObterColuna(0), 0);
		}
		private void _on_Equipes_button_up()
		{
			InstanciarOutraJanela(Equipe, ObterColuna(0), 0);
		}
		private void _on_GPedU_button_up()
		{
			InstanciarOutraJanela(GPedU, ObterColuna(0), 0);
		}
		private void _on_Contato_button_up()
		{
			InstanciarOutraJanela(Contato, ObterColuna(0), 0);
		}
		private async Task RealizarConsultaPessoa(string nome, string sobrenome)
		{
			var resultado = PessoaBLL.RealizarConsulta(new PessoaConsulta()
			{
				Nome = nome
			});
			foreach (var pessoa in resultado)
			{
				CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(0), 0, null);
			}
		}
		public async Task RealizarConsultaRegistro(string nome, string apelido)
		{
			var resultado = RegistroBLL.RealizarConsulta(new RegistroConsulta()
			{
				Nome = nome,
				Apelido = apelido,
				Idioma = "Português"
			});
			foreach (var registro in resultado)
			{
				CallDeferred("InstanciarRegistroBox", new RegistroObject(registro, null), ObterColuna(0), 0, null);
			}
		}
		private void InstanciarOutraJanela(Node janelaInstanciada, VBoxContainer container, int coluna)
		{
			var janela = janelaInstanciada.Duplicate();
			container.AddChild(janela);
			janela._Ready();
		}
		private void InstanciarPessoaBox(PessoaObject pessoaObjct, VBoxContainer container, int coluna, Node boxOrigem = null)
		{
			if (pessoaObjct != null && ValidarPessoaJaInstanciadaNaColuna(pessoaObjct.Pessoa, coluna))
				return;
			var pessoaBox = Pessoa.Duplicate();
			container.AddChild(pessoaBox);
			pessoaBox._Ready();
			var pessoa = pessoaBox.GetNode<JanelaPessoa>("./JanelaPessoa");
			if (pessoaObjct != null)
				pessoa.PopularDados(pessoaObjct.Pessoa);
			pessoa.Dados = this;
			pessoa.Coluna = coluna;
			if (boxOrigem != null)
			{
				criarFio(boxOrigem, pessoaBox);
			}
		}
		private void InstanciarRegistroBox(RegistroObject registroObjct, VBoxContainer container, int coluna, Node boxOrigem = null)
		{
			if (registroObjct != null && ValidarRegistroJaInstanciadoNaColuna(registroObjct.Registro, coluna))
				return;
			var registroBox = Registro.Duplicate();
			container.AddChild(registroBox);
			registroBox._Ready();
			var registro = registroBox.GetNode<JanelaRegistro>("./JanelaRegistro");
			if (registroBox != null)
				registro.PopularDados(registroObjct.Registro);
			registro.Dados = this;
			registro.Coluna = coluna;
			if (boxOrigem != null)
			{
				criarFio(boxOrigem, registroBox);
			}
		}
		private void InstanciarColuna()
		{
			BuscarBLL.InstanciarColuna("res://RES/EDUCACAO_OnLIFE/CENAS/Linha.tscn");
		}
		private bool ValidarPessoaJaInstanciadaNaColuna(PessoaDTO pessoa, int coluna)
		{
			var pessoas = ObterColuna(coluna).GetChildren();
			foreach (var pessoaBox in pessoas)
			{
				if ((pessoaBox as JanelaPessoa)?.CodPessoa == pessoa.Codigo)
					return true;
			}
			return false;
		}
		private bool ValidarRegistroJaInstanciadoNaColuna(RegistroDTO registro, int coluna)
		{
			var registros = ObterColuna(coluna).GetChildren();
			foreach (var registroBox in registros)
			{
				if ((registroBox as JanelaRegistro)?.CodigoRegistro == registro.Codigo)
					return true;
			}
			return false;
		}
		private VBoxContainer ObterColuna(int coluna)
		{
			if (BuscarBLL.ValidarColuna(coluna))
			{
				BuscarBLL.InstanciarColuna("res://RES/EDUCACAO_OnLIFE/CENAS/Linha.tscn");
				System.Threading.Thread.Sleep(100);
			}
			return Coluna.GetChild(coluna).GetNode<VBoxContainer>("./VBoxContainer");
		}
		public void criarFio(Node boxOrigem, Node boxDestino)
		{
			PackedScene fio = GD.Load<PackedScene>("res://RES/EDUCACAO_OnLIFE/CENAS/JANELA_PART/FioVermelho.tscn");
			FioVermelho newFio = (FioVermelho)fio.Instance();
			AddChild(newFio);
			newFio.set_pontos(boxOrigem, boxDestino);
		}
		public async Task BuscarRelacoes(PessoaDTO pessoa, int coluna, Node box)
		{
			try
			{
				var resultado = PessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(pessoa.Codigo));
				var novaColuna = coluna + 1;
				foreach (var relacao in resultado)
				{
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna, box);
				}
			}
			catch (Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		public async Task BuscarRelacoes(RegistroDTO registro, int coluna, Node box)
		{
			try
			{
				var resultado = RegistroBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(registro.Codigo));
				var novaColuna = coluna + 1;
				foreach (var relacao in resultado.Registros)
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna, box);
				foreach (var pessoa in resultado.Pessoas)
					CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(novaColuna), novaColuna, box);
			}
			catch (Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
	}
}
