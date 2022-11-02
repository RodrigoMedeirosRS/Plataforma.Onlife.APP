using Godot;
using System;
using System.Threading.Tasks;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class TabBuscarCTRL : Tabs, IDisposableCTRL
	{
		private AcceptDialog PopErro { get; set; }
		private PessoaBoxCTRL PessoaBox { get; set; }
		private RegistroBoxCTRL RegistroBox { get; set; }
		private PesquisaCTRL Pesquisa { get; set; }
		private IConsultarRegistroBLL RegistroBLL { get; set; }
		private IConsultarPessoaBLL PessoaBLL { get; set; }
		private IBuscarBLL BuscarBLL { get; set; }
		private HBoxContainer Coluna { get; set; }
		public override void _Ready()
		{
			PoularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			InstanciarColuna();
		}
		private void PoularNodes()
		{
			Pesquisa = GetNode<PesquisaCTRL>("./Pesquisa");
			PopErro = GetNode<AcceptDialog>("./PopErro");
			PessoaBox = GetNode<PessoaBoxCTRL>("./PessoaBox");
			RegistroBox = GetNode<RegistroBoxCTRL>("./RegistroBox");
			Coluna = GetNode<HBoxContainer>("./Dados/ScrollContainer/Colunas");
		}
		private void RealizarInjecaoDeDependencias()
		{
			RegistroBLL = new ConsultarRegistroBLL();
			PessoaBLL = new ConsultarPessoaBLL();
			BuscarBLL = new BuscarBLL(Coluna);
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void _on_Pesquisar_button_up()
		{
			Task.Run(async () => await RealizarBusca());
		}
		private async Task RealizarBusca()
		{
			try
			{
				if (Pesquisa.ConsultaPessoa())
					RealizarConsultaPessoa(0);
				else
					RealizarConsultaRegistro(0);
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		private void ExibirErro(string mensagem)
		{
			PopErro.DialogText = mensagem;
			PopErro.Popup_();
		}
		private void RealizarConsultaPessoa(int coluna)
		{
			var resultado = PessoaBLL.RealizarConsulta(new PessoaConsulta()
			{
				Nome = Pesquisa.Nome.Text,
				Sobrenome = Pesquisa.Sobrenome.Text,
				Apelido = Pesquisa.Apelido.Text
			});
			foreach (var pessoa in resultado)
			{
				CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(coluna), coluna);
			}
		}
		private void InstanciarPessoaBox(PessoaObject pessoaObjct, VBoxContainer container, int coluna)
		{
			if (ValidarPessoaJaInstanciadaNaColuna(pessoaObjct.Pessoa, coluna))
				return;
			var pessoaBox = PessoaBox.Duplicate();
			container.AddChild(pessoaBox);
			pessoaBox._Ready();
			(pessoaBox as PessoaBoxCTRL).Preencher(pessoaObjct.Pessoa, new Vector2(0, 0));
			(pessoaBox as PessoaBoxCTRL).TabBuscar = this;
			(pessoaBox as PessoaBoxCTRL).Coluna = coluna;
		}
		private void RealizarConsultaRegistro(int coluna)
		{
			var resultado = RegistroBLL.RealizarConsulta(new RegistroConsulta()
			{
				Nome = Pesquisa.Nome.Text,
				Apelido = Pesquisa.Apelido.Text,
				Idioma = Pesquisa.Idioma.GetItemText(Pesquisa.Idioma.GetSelectedId())
			});
			foreach (var registro in resultado)
			{
				CallDeferred("InstanciarRegistroBox", new RegistroObject(registro, null), ObterColuna(coluna), coluna);
			}
		}
		private void InstanciarRegistroBox(RegistroObject registroObjct, VBoxContainer container, int coluna)
		{
			if (ValidarRegistroJaInstanciadoNaColuna(registroObjct.Registro, coluna))
				return;
			var registroBox = RegistroBox.Duplicate();
			container.AddChild(registroBox);
			registroBox._Ready();
			(registroBox as RegistroBoxCTRL).Preencher(registroObjct.Registro, new Vector2(0, 0));
			(registroBox as RegistroBoxCTRL).TabBuscar = this;
			(registroBox as RegistroBoxCTRL).Coluna = coluna;
		}
		private void InstanciarColuna()
		{
			BuscarBLL.InstanciarColuna();
		}
		private bool ValidarPessoaJaInstanciadaNaColuna(PessoaDTO pessoa, int coluna)
		{
			var pessoas = ObterColuna(coluna).GetChildren();
			foreach (var pessoaBox in pessoas)
			{
				if ((pessoaBox as PessoaBoxCTRL)?.Pessoa.Codigo == pessoa.Codigo)
					return true;
			}
			return false;
		}
		private bool ValidarRegistroJaInstanciadoNaColuna(RegistroDTO registro, int coluna)
		{
			var registros = ObterColuna(coluna).GetChildren();
			foreach (var registroBox in registros)
			{
				if ((registroBox as RegistroBoxCTRL)?.Registro.Codigo == registro.Codigo)
					return true;
			}
			return false;
		}
		private VBoxContainer ObterColuna(int coluna)
		{
			if (BuscarBLL.ValidarColuna(coluna))
			{
				BuscarBLL.InstanciarColuna();
				System.Threading.Thread.Sleep(100);
			}
			return Coluna.GetChild(coluna).GetChild<VBoxContainer>(0);
		}
		public async Task BuscarRelacoes(PessoaDTO pessoa, int coluna, Node box)
		{
			try
			{
				var resultado = PessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(pessoa.Codigo));
				var novaColuna = coluna + 1;			
				foreach (var relacao in resultado)
				{
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				}
			}
			catch(Exception ex)
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
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				foreach (var pessoa in resultado.Pessoas)
					CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(novaColuna), novaColuna);
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		public void FecharCTRL()
		{
			PopErro.QueueFree();
			Pesquisa.FecharCTRL();
			RegistroBLL.Dispose();
			PessoaBLL.Dispose();
			foreach(var coluna in Coluna.GetChildren())
			{
				foreach(var linha in (coluna as Node).GetChild(0).GetChildren())
					(linha as IDisposableCTRL).FecharCTRL();
				BuscarBLL.RemoverColuna((coluna as Node));
			}
			Coluna.QueueFree();
			RegistroBox.FecharCTRL();
			PessoaBox.FecharCTRL();
			QueueFree();
		}
	}
}
