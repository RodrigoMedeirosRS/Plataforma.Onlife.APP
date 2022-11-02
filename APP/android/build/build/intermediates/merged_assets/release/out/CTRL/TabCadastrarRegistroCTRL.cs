using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BibliotecaViva.BLL;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;
using BibliotecaViva.DTO;
using BibliotecaViva.DTO.Utils;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.DTO.Dominio;

namespace BibliotecaViva.CTRL
{
	public class TabCadastrarRegistroCTRL : Tabs, IDisposableCTRL
	{
		public int CodigoRegistro { get; set; }
		private IConsultarTipoBLL BLLTipo { get; set; }
		private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
		private IConsultarRegistroBLL ConsultarRegistroBLL { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Apelido { get; set; }
		private LineEdit LatLong { get ; set; }
		private Label Erro { get; set; }
		private Label Sucesso { get; set; }
		private OptionButton Idioma { get ; set; }
		private OptionButton Tipo { get; set; }
		private List<TipoDTO> Tipos { get; set; }
		private TipoDTO TipoSelecionado { get; set; }
		private TextEdit Descricao { get; set; }
		private TextEdit ConteudoASCII { get; set; }
		private Button ConteudoBIN { get; set; }
		private LineEdit CaminhoBIN { get; set; }
		private FileDialog ModalDeBusca { get; set; }
		private LineEdit NomeBusca { get; set; }
		private LinhaRelacaoCTRL LinhaRelacao { get; set; }
		private OptionButton IdiomaBusca { get; set; }
		private VBoxContainer ContainerRelacao { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
			PopularDropDowns();
		}
		private void RealizarInjecaoDeDependencias()
		{
			CadastrarRegistroBLL = new CadastrarRegistroBLL();
			ConsultarRegistroBLL = new ConsultarRegistroBLL();
			BLLTipo = new ConsultarTipoBLL();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}		
		private void PopularNodes()
		{
			Nome = GetNode<LineEdit>("./Inputs/Nome");
			Apelido = GetNode<LineEdit>("./Inputs/Apelido");
			LatLong = GetNode<LineEdit>("./Inputs/LatLong");
			Idioma = GetNode<OptionButton>("./Inputs/Idioma");
			Tipo = GetNode<OptionButton>("./Inputs/Tipo");
			Erro = GetNode<Label>("./Inputs/Erro");
			Sucesso = GetNode<Label>("./Inputs/Sucesso");
			Descricao = GetNode<TextEdit>("./Inputs/Descricao");
			ConteudoASCII = GetNode<TextEdit>("./Inputs/Conteudo/ConteudoASCII");
			ConteudoBIN = GetNode<Button>("./Inputs/Conteudo/ConteudoBIN");
			CaminhoBIN = GetNode<LineEdit>("./Inputs/Conteudo/ConteudoBIN/CaminhoBIN");
			ModalDeBusca = GetNode<FileDialog>("./ModalDeBusca");
			LinhaRelacao = GetNode<LinhaRelacaoCTRL>("./LinhaRelacao");
			NomeBusca = GetNode<LineEdit>("./BuscaRelacoes/Nome");
			IdiomaBusca = GetNode<OptionButton>("./BuscaRelacoes/Idioma");
			ContainerRelacao = GetNode<VBoxContainer>("./Inputs/ScrollContainer/VBoxContainer");
			CodigoRegistro = 0;
		}
		private void _on_SalvarAlteracoes_button_up()
		{
			Task.Run(async () => await RelizarEnvioRegistro());
		}
		public async Task RelizarEnvioRegistro()
		{
			try
			{
				var registro = new RegistroDTO();
				if (TipoSelecionado.Binario)
					registro = CadastrarRegistroBLL.PopularRegistro(Nome.Text, Apelido.Text, LatLong.Text, Descricao.Text, CarregarArquivoBinario(), TipoSelecionado, Idioma, CodigoRegistro, ObterListaDeRelacoes());
				else
					registro = CadastrarRegistroBLL.PopularRegistro(Nome.Text, Apelido.Text, LatLong.Text, Descricao.Text, ConteudoASCII.Text, TipoSelecionado, Idioma, CodigoRegistro, ObterListaDeRelacoes());
				LimparPreenchimento();
				LimparItensNaoRelacionados(true);
				var retorno = CadastrarRegistroBLL.CadastrarRegistro(registro);
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
				{
					var tipoRelacao = (relacao as LinhaRelacaoCTRL).ObterTipoRelacao();
					lista.Add(new RelacaoDTO()
					{
						RelacaoID = (relacao as LinhaRelacaoCTRL).CodigoRelacao,
						TipoRelacao = tipoRelacao != null ? tipoRelacao.Nome : string.Empty
					});
				}
		
			return lista;
		}
		private string CarregarArquivoBinario()
		{
			return ImportadorDeBinariosUtil.ObterBase64(CaminhoBIN.Text);
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			Sucesso.Text = sucesso ? mensagem : string.Empty;
			Erro.Text = sucesso ? string.Empty : mensagem;
		}
		public void PopularDropDowns()
		{
			BLLTipo.PopularDropDownIdioma(Idioma);
			BLLTipo.PopularDropDownIdioma(IdiomaBusca);
			Tipos = BLLTipo.PopularDropDownTipo(Tipo);
			ObterDadosExtensao(Tipo.GetItemText(0));
			AtualizarCampoPreenchimento(0);
		}
		private void _on_Tipo_item_selected(int index)
		{
			AtualizarCampoPreenchimento(index);
		}
		private void _on_Buscar_button_up()
		{
			Task.Run(async () => await BuscarRegistros(true));
		}
		private void _on_Limpar_button_up()
		{
			LimparItensNaoRelacionados(false);
		}
		private async Task BuscarRegistros(bool novaConsulta, RegistroDTO registro = null)
		{
			try
			{
				LimparItensNaoRelacionados(false);
				var resultado = novaConsulta ? RealizarConsultaDeRegistros() : RealizarConsultaDeRegistrosJaRelacionados(registro);
				foreach (var registroRelacionado in resultado)
				{
					CallDeferred("InstanciarRelacao", registroRelacionado);
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

			var consulta = ConsultarRegistroBLL.RealizarConsulta(new RegistroConsulta()
			{
					Nome = NomeBusca.Text,
					Apelido = string.Empty,
					Idioma = IdiomaBusca.GetItemText(IdiomaBusca.Selected)
			});

			foreach (var registro in consulta)
			{
				resultado.Add(new RegistroObject(registro, null));
			}
			
			return resultado;
		}
		private List<RegistroObject> RealizarConsultaDeRegistrosJaRelacionados(RegistroDTO registro)
		{
			var resultado = new List<RegistroObject>();

			var consulta = ConsultarRegistroBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta ()
			{
				CodRegistro = CodigoRegistro
			})?.Registros;

			consulta = consulta.OrderBy(x => x.Codigo).ToList();
			var relacoes = registro.Referencias.OrderBy(x => x.RelacaoID).ToList();

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
			(linhaRelacao as LinhaRelacaoCTRL).PopularRelacoes(new List<TipoRelacaoDTO>());
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
		private void LimparItensNaoRelacionados(bool limparTudo)
		{
			foreach(var relacao in ContainerRelacao.GetChildren())
				if (!(relacao as LinhaRelacaoCTRL).ObterRelacao() || limparTudo)
					(relacao as LinhaRelacaoCTRL).RemoverLinha();
		}
		private void AtualizarCampoPreenchimento(int index)
		{
			ObterDadosExtensao(Tipo.GetItemText(index));
			AlternarVisibulidadeCampoConteudo();
		}
		private void ObterDadosExtensao(string nomeTipo)
		{
			TipoSelecionado = (from tipo in Tipos where tipo.Nome == nomeTipo select tipo).FirstOrDefault();
		}
		private void AlternarVisibulidadeCampoConteudo()
		{
			ConteudoBIN.Visible = TipoSelecionado.Binario;
			ConteudoASCII.Visible = !TipoSelecionado.Binario;
		}
		public void PopularPreenchiento(RegistroDTO registro)
		{
			CodigoRegistro = registro.Codigo;
			Nome.Text = registro.Nome;
			Apelido.Text = registro.Apelido;
			if (!string.IsNullOrEmpty(registro.Latitude) && !string.IsNullOrEmpty(registro.Longitude))
				LatLong.Text = registro.Latitude + ", " + registro.Longitude;
			Descricao.Text = registro.Descricao;
			if (!ObterDetalhesTipo(registro.Tipo).Binario)
				ConteudoASCII.Text = registro.Conteudo;
			else
			{
				var caminho = "./TEMP/" + registro.Nome + ObterDetalhesTipo(registro.Tipo).Extensao;
				ImportadorDeBinariosUtil.SalvarBase64(caminho, registro.Conteudo);
				CaminhoBIN.Text = caminho;				
			}
			Idioma.Select(BuscarOpcao(registro.Idioma, Idioma));

			var tipoIndex = BuscarOpcao(registro.Tipo, Tipo);
			Tipo.Select(tipoIndex);

			AtualizarCampoPreenchimento(tipoIndex);
			Task.Run(async () => await BuscarRegistros(false, registro));
		}
		private int BuscarOpcao(string nome, OptionButton dropdown)
		{
			for (int i = 0; i < dropdown.GetItemCount(); i++)
			{
				if (dropdown.GetItemText(i) == nome)
					return i;
			}
			return 0;
		}
		public TipoDTO ObterDetalhesTipo(string nomeTipo)
		{
			return (from tipo in Tipos
				where
					tipo.Nome == nomeTipo
				select 
					tipo).FirstOrDefault();
		}
		private void LimparPreenchimento()
		{
			CodigoRegistro = 0;
			Nome.Text = string.Empty;
			Apelido.Text = string.Empty; 
			LatLong.Text = string.Empty; 
			Descricao.Text = string.Empty; 
			ConteudoASCII.Text = string.Empty;
			if (CaminhoBIN.Text.Contains("./TEMP/"))
				ImportadorDeBinariosUtil.DeletarBinario(CaminhoBIN.Text);
			CaminhoBIN.Text = string.Empty;

		}
		private void _on_CaminhoBIN_text_changed(String new_text)
		{
			ValidarArquivoBinario(new_text);
		}
		private void _on_ModalDeBusca_file_selected(String path)
		{
			CaminhoBIN.Text = path;
		}
		private void ValidarArquivoBinario(string new_text)
		{
			try
			{
				CadastrarRegistroBLL.ValidarConteudoBinario(new_text, TipoSelecionado.Extensao);
				Erro.Text = string.Empty;
			}
			catch (Exception ex)
			{
				Feedback(ex.Message, false);
			}
		}
		private void _on_ConteudoBIN_button_up()
		{
			ModalDeBusca.Popup_();
		}
		public void FecharCTRL()
		{
			if (CaminhoBIN.Text.Contains("./TEMP/"))
				ImportadorDeBinariosUtil.DeletarBinario(CaminhoBIN.Text);
			CadastrarRegistroBLL.Dispose();
			ConsultarRegistroBLL.Dispose();
			BLLTipo.Dispose();
			TipoSelecionado.Dispose();
			Nome.QueueFree();
			Apelido.QueueFree();
			LatLong.QueueFree();
			Idioma.QueueFree();
			Tipo.QueueFree();
			Erro.QueueFree();
			Sucesso.QueueFree();
			Descricao.QueueFree();
			ConteudoASCII.QueueFree();
			ConteudoBIN.QueueFree();
			CaminhoBIN.QueueFree();
			ModalDeBusca.QueueFree();
			Desalocador.DesalocarLista<TipoDTO>(Tipos);
			foreach(var linha in ContainerRelacao.GetChildren())
				(linha as LinhaRelacaoCTRL).RemoverLinha();
			LinhaRelacao.RemoverLinha();
			NomeBusca.QueueFree();
			IdiomaBusca.QueueFree();
			QueueFree();
		}
	}
}
