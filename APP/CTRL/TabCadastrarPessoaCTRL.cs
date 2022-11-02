using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.DTO.Utils;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class TabCadastrarPessoaCTRL : Tabs, IDisposableCTRL
	{
		public int CodigoPessoa { get; set; }
		private ICadastrarPessoaBLL CadastroPessoaBLL { get; set; }
		private IConsultarPessoaBLL ConsultarPessoaBLL { get; set; }
		private IConsultarTipoBLL TipoBLL { get; set; }
		private IConsultarRegistroBLL RegistroBLL { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Sobrenome { get; set; }
		private LineEdit NomeBusca { get; set; }
		private Label TesteID { get; set; } /////REMOVER
		private OptionButton Genero { get; set; }
		private LineEdit Apelido { get; set; }
		private Label Erro { get; set; }
		private Label Sucesso { get; set; }
		private VBoxContainer ContainerRelacao { get; set; }
		private OptionButton DropdownIdioma { get ; set; }
		private List<TipoRelacaoDTO> TiposRelacao { get; set; }
		private List<IdiomaDTO> Idiomas { get; set; }
		private LinhaRelacaoCTRL LinhaRelacao { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void RealizarInjecaoDeDependencias()
		{
			CadastroPessoaBLL = new CadastrarPessoaBLL();
			ConsultarPessoaBLL = new ConsultarPessoaBLL();
			TipoBLL = new ConsultarTipoBLL();
			RegistroBLL = new ConsultarRegistroBLL();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularNodes()
		{
			Nome = GetNode<LineEdit>("./Inputs/Nome");
			TesteID = GetNode<Label>("./Inputs/Nome/TesteID");
			NomeBusca = GetNode<LineEdit>("./BuscaRelacoes/Nome");
			Sobrenome = GetNode<LineEdit>("./Inputs/Sobrenome");
			Genero = GetNode<OptionButton>("./Inputs/Genero");
			Apelido = GetNode<LineEdit>("./Inputs/Apelido");
			Sucesso = GetNode<Label>("./Sucesso");
			Erro = GetNode<Label>("./Erro");
			DropdownIdioma = GetNode<OptionButton>("./BuscaRelacoes/Idioma");
			ContainerRelacao = GetNode<VBoxContainer>("./Inputs/ScrollContainer/VBoxContainer");
			LinhaRelacao = GetNode<LinhaRelacaoCTRL>("./LinhaRelacao");
			Genero = GetNode<OptionButton>("./Inputs/Genero");
			TiposRelacao = TipoBLL.ConsultarTiposRelacao();
			Idiomas = TipoBLL.PopularDropDownIdioma(DropdownIdioma);
			TipoBLL.PopularDropDownGenero(Genero);
			CodigoPessoa = 0;
		}
		private void _on_SalvarAlteracoes_button_up()
		{
			Task.Run(async () => await RelizarEnvioRegistro());
		}
		private void _on_Buscar_button_up()
		{
			Task.Run(async () => await BuscarRegistros(true));
		}
		private void _on_Limpar_button_up()
		{
			NomeBusca.Text = string.Empty;
			LimparItensNaoRelacionados(false);
		}
		private void LimparItensNaoRelacionados(bool limparTudo)
		{
			foreach(var relacao in ContainerRelacao.GetChildren())
				if (!(relacao as LinhaRelacaoCTRL).ObterRelacao() || limparTudo)
					(relacao as LinhaRelacaoCTRL).RemoverLinha();
		}
		private async Task BuscarRegistros(bool novaConsulta, PessoaDTO pessoa = null)
		{
			try
			{
				LimparItensNaoRelacionados(false);
				var resultado = novaConsulta ? RealizarConsultaDeRegistros() : RealizarConsultaDeRegistrosJaRelacionados(pessoa);
				foreach (var registro in resultado)
				{
					CallDeferred("InstanciarRelacao", registro);
				}
			}
			catch(Exception ex)
			{
				Feedback(ex.Message, false);
			}
		}
		private List<RegistroObject> RealizarConsultaDeRegistros()
		{
			var resultado = new List<RegistroObject>();

			var consulta = RegistroBLL.RealizarConsulta(new RegistroConsulta()
			{
					Nome = NomeBusca.Text,
					Apelido = string.Empty,
					Idioma = DropdownIdioma.GetItemText(DropdownIdioma.Selected)
			});

			foreach (var registro in consulta)
			{
				resultado.Add(new RegistroObject(registro, null));
			}
			
			return resultado;
		}
		private List<RegistroObject> RealizarConsultaDeRegistrosJaRelacionados(PessoaDTO pessoa)
		{
			var resultado = new List<RegistroObject>();

			var consulta = ConsultarPessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta ()
			{
				CodRegistro = CodigoPessoa
			});

			consulta = consulta.OrderBy(x => x.Codigo).ToList();
			var relacoes = pessoa.Relacoes.OrderBy(x => x.RelacaoID).ToList();

			for (int i = 0; i < consulta.Count; i ++)
			{
				resultado.Add(new RegistroObject(consulta[i], relacoes[i]));
			}

			return resultado;
		}
		private void InstanciarRelacao(RegistroObject registro)
		{
			var linhaRelacao = LinhaRelacao.Duplicate();
			ContainerRelacao.AddChild(linhaRelacao);
			linhaRelacao._Ready();
			(linhaRelacao as LinhaRelacaoCTRL).PopularRelacoes(TiposRelacao);
			(linhaRelacao as LinhaRelacaoCTRL).Nome.Text = registro.Registro.Nome;
			(linhaRelacao as LinhaRelacaoCTRL).CodigoRelacao = registro.Registro.Codigo;
			if (registro.Relacao != null)
			{
				(linhaRelacao as LinhaRelacaoCTRL).SelecionarTipoRelecao(registro.Relacao.TipoRelacao);
				(linhaRelacao as LinhaRelacaoCTRL).BotaoRelacionar.Disabled = false;
				(linhaRelacao as LinhaRelacaoCTRL).BotaoRelacionar.Pressed = true;
				(linhaRelacao as LinhaRelacaoCTRL).DefinirRelacao(true);
			}
		}
		public async Task RelizarEnvioRegistro()
		{
			try
			{
				var pessoa = CadastroPessoaBLL.PopularPessoa(Nome.Text, Sobrenome.Text, Genero.GetItemText(Genero.Selected), Apelido.Text, CodigoPessoa, ObterListaDeRelacoes());
				LimparPreenchimento();
				LimparItensNaoRelacionados(true);
				var retorno = CadastroPessoaBLL.CadastrarPessoa(pessoa);
				CallDeferred("Feedback", retorno, true);
			}
			catch(Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private List<RelacaoDTO> ObterListaDeRelacoes()
		{
			var lista = new List<RelacaoDTO>();

			foreach(var relacao in ContainerRelacao.GetChildren())
				if ((relacao as LinhaRelacaoCTRL).ObterRelacao())
					lista.Add(new RelacaoDTO()
					{
						RelacaoID = (relacao as LinhaRelacaoCTRL).CodigoRelacao,
						TipoRelacao = (relacao as LinhaRelacaoCTRL).ObterTipoRelacao().Nome
					});
		
			return lista;
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			Sucesso.Text = sucesso ? mensagem : string.Empty;
			Erro.Text = sucesso ? string.Empty : mensagem;
		}
		private void SetarDropdownDeGenero(string genero)
		{
			switch(genero)
			{
				case "Feminino":
					Genero.Selected = 1;
					break;
				case "Masculino":
					Genero.Selected = 2;
					break;
				default:
					Genero.Selected = 3;
					break;					
			}
		}
		public void PopularPreenchiento(PessoaDTO pessoa)
		{
			CodigoPessoa = pessoa.Codigo;
			TesteID.Text = "Codigo: " + CodigoPessoa.ToString();
			Nome.Text = pessoa.Nome;
			Sobrenome.Text = pessoa.Sobrenome;
			Apelido.Text = pessoa.Apelido;
			SetarDropdownDeGenero(pessoa.Genero);
			Task.Run(async () => await BuscarRegistros(false, pessoa));
		}
		private void LimparPreenchimento()
		{
			CodigoPessoa = 0;
			Nome.Text = string.Empty;
			Sobrenome.Text = string.Empty;
			Apelido.Text = string.Empty;
			NomeBusca.Text = string.Empty;
			Genero.Selected = 0;
		}
		public void FecharCTRL()
		{
			CadastroPessoaBLL.Dispose();
			RegistroBLL.Dispose();
			ConsultarPessoaBLL.Dispose();
			TipoBLL.Dispose();
			Nome.QueueFree();
			NomeBusca.QueueFree();
			Sobrenome.QueueFree();
			Genero.QueueFree();
			Apelido.QueueFree();
			Sucesso.QueueFree();
			Erro.QueueFree();
			foreach(var linha in ContainerRelacao.GetChildren())
				(linha as LinhaRelacaoCTRL).RemoverLinha();
			LinhaRelacao.RemoverLinha();
			ContainerRelacao.QueueFree();
			DropdownIdioma.QueueFree();
			Desalocador.DesalocarLista<TipoRelacaoDTO>(TiposRelacao);
			Desalocador.DesalocarLista<IdiomaDTO>(Idiomas);
			QueueFree();
		}
	}
}
