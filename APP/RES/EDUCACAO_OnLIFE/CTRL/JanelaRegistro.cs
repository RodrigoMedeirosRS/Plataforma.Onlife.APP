using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using DTO;
using BLL;
using BLL.Utils;
using DTO.Utils;
using BLL.Interface;
using CTRL.Interface;

namespace Onlife.CTRL
{
	public class JanelaRegistro : MarginContainer, IDisposableCTRL
	{
		public int Coluna { get; set; }
		public DadosCTRL Dados { get; set; }
		public int CodigoRegistro { get; set; }
		public RegistroDTO Registro { get; set; }
		private bool Maximizado { get; set; }
		private bool EmEdicao { get; set; }
		private List<TipoDTO> Tipos { get; set; }
		private TipoDTO TipoSelecionado { get; set; }
		private IConsultarTipoBLL BLLTipo { get; set; }
		private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
		private IConsultarRegistroBLL ConsultarRegistroBLL { get; set; }
		private Control Janela { get; set; }
		private VBoxContainer Container { get; set; }
		private VBoxContainer ConteudoTXTContainer { get; set; }
		private VBoxContainer ConteudoAUDIOContainer { get; set; }
		private VBoxContainer ConteudoURLContainer { get; set; }
		private VBoxContainer ConteudoBINContainer { get; set; }
		private VBoxContainer ConteudoIMGContainer { get; set; }
		private VBoxContainer ConteudoTipoContainer { get; set; }
		private LineEdit Nome { get; set; }
		private LineEdit Apelido { get; set; }
		private LineEdit Localizacao { get; set; }
		private TextEdit Descricao { get; set; }
		private LineEdit ConteudoTXT { get; set; }
		private LineEdit URL { get; set; }
		private TextureButton IMG { get; set; }
		private Button BtnEditar { get; set; }
		private Button BtnMaximizar { get; set; }
		private Button BtnAlterarAudio { get; set; }
		private Button BtnAlterarBinario { get; set; }
		private OptionButton TipoDropdown { get; set; }
		private AudioStreamPlayer AudioPlayer { get; set; }
		private FileDialog PopupDeArquivo { get; set; }
		private FileDialog PopupDeGravacao { get; set; }
		private AcceptDialog PopupDeFeedback { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
			(GetParent() as JanelaBase).PopularConteudo(this);
			AplicarMaximizar();
			AlternarEdicao(true, false);
		}
		public void FecharCTRL()
		{
			BLLTipo.Dispose();
			ConsultarRegistroBLL.Dispose();
			CadastrarRegistroBLL.Dispose();
			TipoSelecionado.Dispose();
			foreach (var tipo in Tipos)
				tipo.Dispose();
			Tipos.Clear();
			Tipos = null;


			ConteudoTXTContainer.QueueFree();
			ConteudoAUDIOContainer.QueueFree();
			ConteudoURLContainer.QueueFree();
			ConteudoBINContainer.QueueFree();
			ConteudoIMGContainer.QueueFree();
			ConteudoTipoContainer.QueueFree();
			BtnMaximizar.QueueFree();
			BtnEditar.QueueFree();
			BtnAlterarAudio.QueueFree();
			BtnAlterarBinario.QueueFree();
			TipoDropdown.QueueFree();
			Nome.QueueFree();
			Apelido.QueueFree();
			Localizacao.QueueFree();
			Descricao.QueueFree();
			ConteudoTXT.QueueFree();
			URL.QueueFree();
			IMG.QueueFree();
			AudioPlayer.QueueFree();
			PopupDeArquivo.QueueFree();
			PopupDeGravacao.QueueFree();
			PopupDeFeedback.QueueFree();
		}
		private void RealizarInjecaoDeDependencias()
		{
			BLLTipo = new ConsultarTipoBLL();
			CadastrarRegistroBLL = new CadastrarRegistroBLL();
			ConsultarRegistroBLL = new ConsultarRegistroBLL();
		}
		private void PopularNodes()
		{
			Container = GetNode<VBoxContainer>("./VBoxContainer");
			Janela = GetNode<Control>("../Control");
			ConteudoTXTContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoTXT");
			ConteudoAUDIOContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoAUDIO");
			ConteudoURLContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoURL");
			ConteudoBINContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoBIN");
			ConteudoIMGContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoIMG");
			ConteudoTipoContainer = GetNode<VBoxContainer>("./VBoxContainer/ConteudoTipo");
			BtnEditar = GetNode<Button>("./VBoxContainer/Comandos/BtnEditar");
			BtnMaximizar = GetNode<Button>("./VBoxContainer/Comandos/BtnExibir");
			BtnAlterarAudio = GetNode<Button>("./VBoxContainer/ConteudoAUDIO/Controles/BtnAlterar");
			BtnAlterarBinario = GetNode<Button>("./VBoxContainer/ConteudoBIN/Controles/BtnAlterar");
			Nome = GetNode<LineEdit>("./VBoxContainer/Nome/LineEdit");
			Apelido = GetNode<LineEdit>("./VBoxContainer/Apelido/LineEdit");
			Localizacao = GetNode<LineEdit>("./VBoxContainer/Localizacao/LineEdit");
			Descricao = GetNode<TextEdit>("./VBoxContainer/Descricao/LineEdit");
			ConteudoTXT = GetNode<LineEdit>("./VBoxContainer/ConteudoTXT/LineEdit");
			URL = GetNode<LineEdit>("./VBoxContainer/ConteudoURL/LineEdit");
			IMG = GetNode<TextureButton>("./VBoxContainer/ConteudoIMG/Borda/Imagem");
			AudioPlayer = GetNode<AudioStreamPlayer>("./AudioPlayer");
			PopupDeArquivo = GetNode<FileDialog>("../FileDialog");
			PopupDeGravacao = GetNode<FileDialog>("../SaveDialog");
			PopupDeFeedback = GetNode<AcceptDialog>("../Atencao");
			TipoDropdown = GetNode<OptionButton>("./VBoxContainer/ConteudoTipo/Borda/TipoDropdown");
			Maximizado = false;
			EmEdicao = false;
			ObterTipos();
		}
		private void ObterTipos()
		{
			try
			{
				Tipos = BLLTipo.PopularDropDownTipo(TipoDropdown);
				ObterDadosExtensao(TipoDropdown.GetItemText(0));
			}
			catch (Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		public override void _Process(float delta)
		{
			AjustarAltura();
		}
		private void _on_BtnEditar_button_up()
		{
			AlternarEdicao(!EmEdicao, true);
		}
		private void _on_BtnExibir_button_up()
		{
			try
			{
				AlterarMaximizar();
			}
			catch (Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private void _on_BtnConexoes_button_up()
		{
			Task.Run(async () => await Dados.BuscarRelacoes(Registro, Coluna, this));
		}
		private void _on_OptionButton_item_selected(int index)
		{
			ObterDadosExtensao(TipoDropdown.GetItemText(index));
			ExibirCampo();
		}
		private void _on_Imagem_button_up()
		{
			PopupDeArquivo.Popup_();
		}
		private void _on_BtnDOWNLOAD_button_up()
		{
			PopupDeGravacao.Popup_();
		}
		private void _on_BtnAcessar_button_up()
		{
			OS.ShellOpen(URL.Text);
		}
		private void _on_BtnPlay_button_up()
		{
			AudioPlayer.Play();
		}
		private void _on_BtnStop_button_up()
		{
			AudioPlayer.Stop();
		}
		private void _on_BtnAlterar_button_up()
		{
			PopupDeArquivo.Popup_();
		}
		private void _on_FileDialog_file_selected(String path)
		{
			try
			{
				Registro = PopularRegistro();
				Registro.Conteudo = ImportadorDeBinariosUtil.ObterBase64(path);
				if (TipoSelecionado.TipoExecucao == TipoExecucao.Imagem && !string.IsNullOrEmpty(Registro.Nome))
					IMG.TextureNormal = ImportadorDeBinariosUtil.GerarImagem(Registro.Nome, TipoSelecionado.Extensao, Registro.Conteudo);
				if (TipoSelecionado.TipoExecucao == TipoExecucao.Audio && !string.IsNullOrEmpty(Registro.Nome))
					AudioPlayer.Stream = ImportadorDeBinariosUtil.GerarAudio(Registro.Nome, TipoSelecionado.Extensao, Registro.Conteudo);
			}
			catch (Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private void _on_SaveDialog_file_selected(String path)
		{
			try
			{
				if (!string.IsNullOrEmpty(Registro?.Conteudo))
					ImportadorDeBinariosUtil.SalvarBase64(path, Registro.Conteudo);
			}
			catch (Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private void AjustarAltura()
		{
			if (Janela.RectMinSize.y < Container.RectSize.y + 40)
			{
				var tamanho = new Vector2(500, Container.RectSize.y + 40);
				Janela.RectMinSize = tamanho;
				Janela.RectSize = tamanho;
				this.RectMinSize = tamanho;
				this.RectSize = tamanho;
			}
		}
		private void AlterarMaximizar()
		{
			Maximizado = !Maximizado;
			BtnMaximizar.Text = Maximizado ? "RESUMIR" : "EXIBIR";
			AplicarMaximizar();
		}
		private void AplicarMaximizar()
		{
			AudioPlayer.Stop();
			if (Maximizado)
				ExibirCampo();
			else
			{
				AlternarEdicao(false, false);
				OcultarTodos();
			}
		}
		private void ExibirCampo()
		{
			Maximizado = true;
			switch (TipoSelecionado.TipoExecucao)
			{
				case TipoExecucao.Audio:
					ExibirRegistroDeAudio();
					break;
				case TipoExecucao.Imagem:
					ExibirRegistroImagem();
					break;
				case TipoExecucao.Texto:
					ExibirRegistroTextual();
					break;
				case TipoExecucao.Arquivo:
					ExibirRegistroDeArquivo();
					break;
				case TipoExecucao.URL:
					ExibirRegistroURL();
					break;
			}
		}
		private void ExibirRegistroDeAudio()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Show();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroImagem()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Show();
			BtnEditar.Show();
		}
		private void ExibirRegistroTextual()
		{
			ConteudoTXTContainer.Show();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroDeArquivo()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Show();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void ExibirRegistroURL()
		{
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Show();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Show();
		}
		private void OcultarTodos()
		{
			AudioPlayer.Stop();
			ConteudoTXTContainer.Hide();
			ConteudoAUDIOContainer.Hide();
			ConteudoURLContainer.Hide();
			ConteudoBINContainer.Hide();
			ConteudoIMGContainer.Hide();
			BtnEditar.Hide();
		}
		private void AlternarEdicao(bool edicao, bool salvar)
		{
			AudioPlayer.Stop();
			AtivarComandos(edicao);
			BtnEditar.Text = EmEdicao ? "SALVAR" : "EDITAR";
			if (EmEdicao)
			{
				BtnAlterarAudio.Show();
				BtnAlterarBinario.Show();
				ConteudoTipoContainer.Show();
			}
			else
			{
				if (salvar)
					SalvarRegistro();
				BtnAlterarAudio.Hide();
				BtnAlterarBinario.Hide();
				ConteudoTipoContainer.Hide();
			}
		}
		private void AtivarComandos(bool ativar)
		{
			EmEdicao = ativar;
			Nome.Editable = ativar;
			Apelido.Editable = ativar;
			Localizacao.Editable = ativar;
			Descricao.Readonly = !ativar;
			ConteudoTXT.Editable = ativar; ;
			URL.Editable = ativar;
			IMG.Disabled = !ativar;
		}
		private void ObterDadosExtensao(string nomeTipo)
		{
			TipoSelecionado = (from tipo in Tipos where tipo.Nome == nomeTipo select tipo).FirstOrDefault();
		}
		private void SalvarRegistro()
		{
			Task.Run(async () => await RelizarEnvioRegistro());
		}
		public async Task RelizarEnvioRegistro()
		{
			try
			{
				Registro = PopularRegistro();
				var retorno = CadastrarRegistroBLL.CadastrarRegistro(Registro);

				if (CodigoRegistro == 0)
				{
					var retornoProcessado = retorno.Split(" ");
					CodigoRegistro = Convert.ToInt32(retornoProcessado[0]);
				}
				CallDeferred("Feedback", retorno, true);
			}
			catch (Exception ex)
			{
				CallDeferred("Feedback", ex.Message, false);
			}
		}
		private RegistroDTO PopularRegistro()
		{
			return new RegistroDTO()
			{
				Codigo = CodigoRegistro,
				Nome = Nome.Text,
				Apelido = Apelido.Text,
				Idioma = "Português",
				Tipo = TipoSelecionado.Nome,
				Conteudo = ObterConteudo(),
				Descricao = Descricao.Text,
				DataInsercao = DateTime.Now,
				Referencias = new List<RelacaoDTO>()
			};
		}
		public void PopularDados(RegistroDTO registroDTO)
		{
			Registro = registroDTO;
			CodigoRegistro = registroDTO.Codigo;
			Nome.Text = registroDTO.Nome;
			TipoSelecionado = ObterDetalhesTipo(registroDTO.Tipo);
			Descricao.Text = registroDTO.Descricao;

			switch (TipoSelecionado.TipoExecucao)
			{
				case TipoExecucao.Audio:
					AudioPlayer.Stream = ImportadorDeBinariosUtil.GerarAudio(Registro.Nome, TipoSelecionado.Extensao, Registro.Conteudo);
					break;
				case TipoExecucao.Imagem:
					IMG.TextureNormal = ImportadorDeBinariosUtil.GerarImagem(Registro.Nome, TipoSelecionado.Extensao, Registro.Conteudo);
					break;
				case TipoExecucao.Texto:
					ConteudoTXT.Text = registroDTO.Conteudo;
					break;
				case TipoExecucao.Arquivo:
					break;
				case TipoExecucao.URL:
					URL.Text = registroDTO.Conteudo;
					break;
			}
			AlternarEdicao(false, false);
		}
		public TipoDTO ObterDetalhesTipo(string nomeTipo)
		{
			return (from tipo in Tipos
					where
						tipo.Nome == nomeTipo
					select
						tipo).FirstOrDefault();
		}
		private string ObterConteudo()
		{
			switch (TipoSelecionado.TipoExecucao)
			{
				case TipoExecucao.Audio:
					return string.IsNullOrEmpty(Registro?.Conteudo) ? string.Empty : Registro.Conteudo;
				case TipoExecucao.Imagem:
					return string.IsNullOrEmpty(Registro?.Conteudo) ? string.Empty : Registro.Conteudo;
				case TipoExecucao.Texto:
					return ConteudoTXT.Text;
				case TipoExecucao.Arquivo:
					return string.IsNullOrEmpty(Registro?.Conteudo) ? string.Empty : Registro.Conteudo;
				case TipoExecucao.URL:
					return URL.Text;
				default:
					return string.Empty;
			}
		}
		private void Feedback(string mensagem, bool sucesso)
		{
			var mensagemFinal = sucesso ? "Retorno: " + mensagem : "Erro: " + mensagem;
			PopupDeFeedback.DialogText = mensagemFinal;
			PopupDeFeedback.Popup_();
		}
	}
}
