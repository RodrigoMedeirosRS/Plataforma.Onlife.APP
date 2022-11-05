using Godot;
using System;
using RandomGen;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using DTO;
using BLL;
using DTO.Utils;
using DTO.Dominio;
using BLL.Interface;
using CTRL.Interface;
using BLL.Utils;

namespace Onlife.CTRL
{
	public class JanelaPessoa : MarginContainer, IDisposableCTRL
	{
		public int Coluna { get; set; }
		public DadosCTRL Dados { get; set; }
		public int CodPessoa { get; set; }
		public PessoaDTO Pessoa { get; set; }
		public RegistroDTO FotoDTO { get; set; }
		public RegistroDTO LattesDTO { get; set; }
		public RegistroDTO ResearchGateDTO { get; set; }
		public RegistroDTO IDDTO { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Sobrenome { get; set; }
		private LineEdit Apelido { get; set; }
		private Button Editar { get ;set; }
		private TextureButton Foto { get; set; }
		private bool EmEdicao { get; set; }
		private ICadastrarPessoaBLL CadastroPessoaBLL { get; set; }
		private IConsultarPessoaBLL ConsultarPessoaBLL { get; set; }
		private IConsultarTipoBLL TipoBLL { get; set; }
		private IConsultarRegistroBLL ConsultarRegistroBLL { get; set; }
		private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
		private AcceptDialog PopupFeedback { get; set; }
		private FileDialog PopupFoto { get; set; }
		private Popup PopupURL { get; set; }
		private Label TituloURL { get; set; }
		private LineEdit URL { get; set; }
		private string NomePopUp { get ; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			EmEdicao = true;
			DefinirEmEdicao();
			(GetParent() as JanelaBase).PopularConteudo(this);
		}
		public void FecharCTRL()
		{
			Pessoa?.Dispose();
			FotoDTO?.Dispose();
			LattesDTO?.Dispose();
			ResearchGateDTO?.Dispose();
			IDDTO?.Dispose();
			Nome.QueueFree();
			Sobrenome.QueueFree();
			Apelido.QueueFree();
			Foto.QueueFree();
			Editar.QueueFree();
			CadastroPessoaBLL.Dispose();
			ConsultarPessoaBLL.Dispose();
			TipoBLL.Dispose();
			ConsultarRegistroBLL.Dispose();
			CadastrarRegistroBLL.Dispose();
			PopupFeedback.QueueFree();
			PopupFoto.QueueFree();
			PopupURL.QueueFree();
			TituloURL.QueueFree();
			URL.QueueFree();
			NomePopUp = string.Empty;
			NomePopUp = null;
		}
		public void PopularDados(PessoaDTO pessoaDTO)
		{
			Pessoa = pessoaDTO;
			CodPessoa = pessoaDTO.Codigo;
			Nome.Text = pessoaDTO.Nome;
			Apelido.Text = pessoaDTO.Apelido;
			var relacoes = ConsultarPessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(CodPessoa));
			FotoDTO = ObterRegistroEspecifico(relacoes, "Foto");
			LattesDTO = ObterRegistroEspecifico(relacoes, "Lattes");
			ResearchGateDTO = ObterRegistroEspecifico(relacoes, "ResearchGate");
			IDDTO = ObterRegistroEspecifico(relacoes, "ID");
			if (FotoDTO != null)
				Foto.TextureNormal = ImportadorDeBinariosUtil.GerarImagem(FotoDTO.Nome, "jpg", FotoDTO.Conteudo);
			EmEdicao = false;
			DefinirEmEdicao();
		}
		private RegistroDTO ObterRegistroEspecifico(List<RegistroDTO> relacoes, string parametroNoNome)
		{
			return (from relacao in relacoes
				where
					relacao.Nome.Contains(parametroNoNome)
				select
					relacao).FirstOrDefault();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void RealizarInjecaoDeDependencias()
		{
			TipoBLL = new ConsultarTipoBLL();
			CadastroPessoaBLL = new CadastrarPessoaBLL();
			ConsultarPessoaBLL = new ConsultarPessoaBLL();
			ConsultarRegistroBLL = new ConsultarRegistroBLL();
			CadastrarRegistroBLL = new CadastrarRegistroBLL();
		}
		private void PopularNodes()
		{
			CodPessoa = 0;
			Nome = GetNode<LineEdit>("./HBoxContainer/LabelInput/Nome/LineEdit");
			Sobrenome = GetNode<LineEdit>("./HBoxContainer/LabelInput/Sobrenome/LineEdit");
			Apelido = GetNode<LineEdit>("./HBoxContainer/LabelInput/Apelido/LineEdit");
			Editar = GetNode<Button>("./HBoxContainer/LabelInput/HBoxContainer2/BtnEditar");
			Foto = GetNode<TextureButton>("./HBoxContainer/VBoxContainer/MarginContainer/Borda/Foto");

			PopupURL = GetNode<Popup>("../URL");
			PopupFeedback = GetNode<AcceptDialog>("../Atencao");
			PopupFoto = GetNode<FileDialog>("../FileDialog");
			TituloURL = GetNode<Label>("../URL/Nome/Label");
			URL = GetNode<LineEdit>("../URL/Nome/LineEdit");

			PopupFoto.Filters = new string[1] { "*.jpg" };
		}
		private void AtivarEditacao(bool editar)
		{
			EmEdicao = editar;
			DefinirEmEdicao();
		}
		private void DefinirEmEdicao()
		{
			Editar.Text = EmEdicao? "SALVAR" : "EDITAR";
			Foto.Disabled = !EmEdicao;
			Nome.Editable = EmEdicao;
			Sobrenome.Editable = EmEdicao;
			Apelido.Editable = EmEdicao;
		}
		private async void _on_BtnEditar_button_up()
		{
			if (EmEdicao)
				SalvarDadosPessoa();
			EmEdicao = !EmEdicao;
			DefinirEmEdicao();
		}
		private void SalvarDadosPessoa()
		{
			Task.Run(async () => await RelizarEnvioRegistro());
		}
		public async Task RelizarEnvioRegistro()
		{
			try
			{
				if (CodPessoa == 0)
				{
					var preCadastro = RegistrarPessoa(new List<RelacaoDTO>());
					var preCadastroProcessado = preCadastro.Split(" ");
					CodPessoa = Convert.ToInt32(preCadastroProcessado[0]);
				}
				var relacoes = CadastrarDadosComplementares();
				var retorno = RegistrarPessoa(relacoes);
				CallDeferred("Feedback", retorno, true);
			}
			catch(Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private string RegistrarPessoa(List<RelacaoDTO> relacoes)
		{
			//var pessoa = CadastroPessoaBLL.PopularPessoa(Nome.Text, Sobrenome.Text, "Prefiro não Declarar", Apelido.Text, CodPessoa, relacoes);
			return "";//CadastroPessoaBLL.CadastrarPessoa(pessoa);
		}
		private List<RelacaoDTO> CadastrarDadosComplementares()
		{
			var relacaoFoto = ObterRelacao(FotoDTO);
			var relacaoLattes = ObterRelacao(LattesDTO);
			var relacaoResearchGate = ObterRelacao(ResearchGateDTO);
			var relacaoID = ObterRelacao(IDDTO);

			var relacoes = new List<RelacaoDTO>();
			if (relacaoFoto != null)
				relacoes.Add(relacaoFoto);
			if (relacaoLattes != null)
				relacoes.Add(relacaoLattes);
			if (relacaoResearchGate != null)
				relacoes.Add(relacaoResearchGate);
			if (relacaoID != null)
				relacoes.Add(relacaoID);

			return relacoes;
		}
		private RelacaoDTO ObterRelacao(RegistroDTO registroDTO)
		{
			if (registroDTO != null)
			{
				var retorno = CadastrarRegistroBLL.CadastrarRegistro(registroDTO);
				var retornoProcessado = retorno.Split(" ");
				return new RelacaoDTO()
				{
					RegistroPessoaID = CodPessoa,
					RelacaoID = Convert.ToInt32(retornoProcessado[0]),
					TipoRelacao = "Autor"
				};
			}
			return null;
		}
		private RegistroDTO CadastrarRegistro(RegistroDTO registroDTO)
		{
			if (registroDTO != null)
			{
				var retorno = CadastrarRegistroBLL.CadastrarRegistro(registroDTO);
				if (registroDTO.Codigo == 0)
				{
					var retornoProcessado = retorno.Split(" ");
					registroDTO.Codigo = Convert.ToInt32(retornoProcessado[0]);
				}	
			}
			return registroDTO;
		}	
		private void _on_Foto_button_up()
		{
			if (EmEdicao)
				PopupFoto.Popup_();
		}
		private void _on_ID_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(IDDTO, "ID");
			else
				AbrirURL(IDDTO);
		}
		private void _on_Lattes_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(LattesDTO, "Lattes");
			else
				AbrirURL(LattesDTO);
		}
		private void _on_ResearchGate_button_up()
		{
			if (EmEdicao)
				AbirPopupDeAlteracaoURL(ResearchGateDTO, "ResearchGate");
			else
				AbrirURL(ResearchGateDTO);
		}
		private void _on_BtnConexoes_button_up()
		{
			Task.Run(async () => await Dados.BuscarRelacoes(Pessoa, Coluna, this));
		}
		private void _on_FileDialog_file_selected(String path)
		{
			AlterarFotoPerfil(path);
		}
		private void AlterarFotoPerfil(string caminho)
		{
			var imagem = ImportadorDeBinariosUtil.BuscarImagem("", "", caminho, false);
			Foto.TextureNormal = imagem;
			PopularFoto(caminho);
		}
		private void PopularFoto(string caminho)
		{
			var base64 = ImportadorDeBinariosUtil.ObterBase64(caminho);

			if (FotoDTO == null)
				FotoDTO = new RegistroDTO()
				{
					Nome = GerarNomeAleatorio("Foto"),
					Idioma = "Português",
					Tipo = "Imagem JPG",
					Conteudo = base64,
					DataInsercao = DateTime.Now,
					Referencias = new List<RelacaoDTO>()
				};
			else
			{
				FotoDTO.Conteudo = base64;
				FotoDTO.DataInsercao = DateTime.Now;
			}
		}
		private void _on_BtnEditarURL_button_up()
		{
			AlterarURL();
			PopupURL.Hide();
		}
		private void AbrirURL(RegistroDTO registro)
		{
			if (registro != null && !string.IsNullOrEmpty(registro.Conteudo))
				OS.ShellOpen(registro.Conteudo);
		}
		private void AbirPopupDeAlteracaoURL(RegistroDTO registro, string nome)
		{
			TituloURL.Text = "Por favor insira a URL do " + nome;
			NomePopUp = nome;
			URL.Text = registro != null ? registro.Conteudo : string.Empty;
			PopupURL.Popup_();
		}
		private void AlterarURL()
		{
			switch(NomePopUp)
			{
				case ("ID"):
					IDDTO = ProcessarRegistroDTO(IDDTO);
					break;
				case ("Lattes"):
					LattesDTO = ProcessarRegistroDTO(LattesDTO);
					break;
				case ("ResearchGate"):
					ResearchGateDTO = ProcessarRegistroDTO(ResearchGateDTO);
					break;
			}	
		}
		private RegistroDTO ProcessarRegistroDTO(RegistroDTO registroDTO)
		{
			if (registroDTO == null)
				registroDTO = new RegistroDTO()
				{
					Nome = GerarNomeAleatorio(NomePopUp),
					Idioma = "Português",
					Tipo = "Link ou URL",
					Conteudo = URL.Text,
					DataInsercao = DateTime.Now,
					Referencias = new List<RelacaoDTO>()
				};
			else
			{
				registroDTO.Conteudo = URL.Text;
				registroDTO.DataInsercao = DateTime.Now;
			}
			return registroDTO;
		}
		private string GerarNomeAleatorio(string prefixo)
		{
			var contagem = 30 - prefixo.Count();
			return prefixo + Gen.Random.Text.Length(contagem).Invoke();
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			var mensagemFinal = sucesso ? "Retorno: " + mensagem : "Erro: " + mensagem;
			PopupFeedback.DialogText = mensagemFinal;
			PopupFeedback.Popup_();
		}
	}
}
