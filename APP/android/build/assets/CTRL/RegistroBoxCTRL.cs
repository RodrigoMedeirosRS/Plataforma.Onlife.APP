using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.DTO.Utils;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class RegistroBoxCTRL : Panel, IDisposableCTRL
	{
		public int Coluna { get; set; }
		public TabBuscarCTRL TabBuscar { get; set; }
		public TabSonarCTRL TabSonar { get; set; }

		private bool Maximizado { get; set; }
		private Label Nome { get; set; }
		private Label Apelido { get; set; }
		
		private RichTextLabel ConteudoDescricao { get; set; }
		private RichTextLabel ConteudoTextual { get; set; }
		private TextureRect ConteudoImagem { get; set; }
		private AudioStreamPlayer ConteudoAudio { get; set; }
		private FileDialog PopupDeDownload { get; set; }
		
		private Control CampoDescricao { get; set; }
		private Control CampoImagem { get; set; }
		private Control CampoTextual { get; set; }
		private Control CampoAudio { get; set; }
		private Control CampoDownload { get; set; }
		private Control CampoURL { get; set; }
		
		public RegistroDTO Registro { get; set; }
		private List<TipoDTO> Tipos { get; set; }
		private IConsultarTipoBLL ConsultarTipoBLL { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void RealizarInjecaoDeDependencias()
		{
			ConsultarTipoBLL = new ConsultarTipoBLL();
		}
		private void PopularNodes()
		{
			Maximizado = false;
			PopupDeDownload = GetNode<FileDialog>("./PopupDeDownload");

			Nome = GetNode<Label>("./Conteudo/Nome");
			Apelido = GetNode<Label>("./Conteudo/VBoxContainer/Apelido/Conteudo");
			ConteudoDescricao = GetNode<RichTextLabel>("./Conteudo/VBoxContainer/Descricao/ScrollContainer/Conteudo");
			ConteudoTextual = GetNode<RichTextLabel>("./Conteudo/VBoxContainer/Texto/ScrollContainer/Conteudo");
			ConteudoImagem = GetNode<TextureRect>("./Conteudo/VBoxContainer/Imagem/Imagem");
			ConteudoAudio = GetNode<AudioStreamPlayer>("./Conteudo/VBoxContainer/Audio/AudioPlayer");
			
			CampoTextual = GetNode<Control>("./Conteudo/VBoxContainer/Texto");
			CampoImagem = GetNode<Control>("./Conteudo/VBoxContainer/Imagem");
			CampoDescricao = GetNode<Control>("./Conteudo/VBoxContainer/Descricao");
			CampoAudio = GetNode<Control>("./Conteudo/VBoxContainer/Audio");
			CampoDownload = GetNode<Control>("./Conteudo/VBoxContainer/Arquivo");
			CampoURL = GetNode<Control>("./Conteudo/VBoxContainer/URL");

			Tipos = ConsultarTipoBLL.ConsultarTipos();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		public void Preencher(RegistroDTO registroDTO, Vector2 posicao)
		{
			RectPosition = posicao;
			Registro = registroDTO;
			Nome.Text = registroDTO.Nome;
			PopularCampoOpcional(Apelido, registroDTO.Apelido);
			PopularCampoOpcional(ConteudoDescricao, registroDTO.Descricao);
		}
		private void PopularCampoOpcional(Label campo, string conteudo)
		{
			(campo.GetParent() as Control).Visible = !string.IsNullOrEmpty(conteudo);
			campo.Text = conteudo;
		}
		private void PopularCampoOpcional(RichTextLabel campo, string conteudo)
		{
			(campo.GetParent().GetParent() as Control).Visible = !string.IsNullOrEmpty(conteudo);
			campo.BbcodeText = conteudo;
		}
		private void _on_Editar_button_up()
		{
			if (ObterDetalhesTipo(Registro.Tipo).TipoExecucao == TipoExecucao.Audio)
				ConteudoAudio.Stop();
			MainCTRL.EditarRegistro(Registro);
		}
		private void _on_Exibir_button_up()
		{
			if (TabBuscar != null)
				Task.Run(async () => await TabBuscar.BuscarRelacoes(Registro, Coluna, this));
			else
				Task.Run(async () => await TabSonar.BuscarRelacoes(Registro, Coluna, this));
		}
		private void _on_Maximizar_button_up()
		{
			if (Maximizado)
				ExibirDescricao();
			else
				ExibirCampo();
			
		}
		private void _on_Play_button_up()
		{
			ConteudoAudio.Play();
		}
		private void _on_Stop_button_up()
		{
			ConteudoAudio.Stop();
		}
		private void _on_Download_button_up()
		{
			PopupDeDownload.CurrentFile = Registro.Nome + ObterDetalhesTipo(Registro.Tipo).Extensao;
			PopupDeDownload.Popup_();
		}
		private void _on_Acessar_URL_button_up()
		{
			OS.ShellOpen(Registro.Conteudo);
		}
		private void _on_PopupDeDownload_file_selected(String path)
		{
			Task.Run(async () => await SalvarArquivo(path, Registro.Conteudo));
		}
		private async Task SalvarArquivo(string caminho, string base64)
		{
			ImportadorDeBinariosUtil.SalvarBase64(caminho, base64);
		}
		private void ExibirCampo()
		{
			Maximizado = true;
			switch(ObterDetalhesTipo(Registro.Tipo).TipoExecucao)
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
		private void ExibirDescricao()
		{
			Maximizado = false;
			ConteudoAudio.Stop();
			CampoDescricao.Visible = true;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			CampoURL.Visible = false;

			RectMinSize = new Vector2(293, 234);
			RectSize = new Vector2(293, 234);
		}
		private void ExibirRegistroDeArquivo()
		{
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = true;
			CampoURL.Visible = false;
			
			RectMinSize = new Vector2(293, 158);
			RectSize = new Vector2(293, 158);
		}
		private void ExibirRegistroTextual()
		{
			CampoTextual.Visible = true;
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			CampoURL.Visible = false;

			RectMinSize = new Vector2(293, 401);
			RectSize = new Vector2(293, 401);
			
			ConteudoTextual.BbcodeText = Registro.Conteudo;
		}
		private void ExibirRegistroDeAudio()
		{
			CampoTextual.Visible = false;
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoAudio.Visible = true;
			CampoDownload.Visible = false;
			CampoURL.Visible = false;

			var audio = ImportadorDeBinariosUtil.GerarAudio(Registro.Nome, ObterDetalhesTipo(Registro.Tipo).Extensao, Registro.Conteudo);
			ConteudoAudio.Stream = audio;

			RectMinSize = new Vector2(293, 158);
			RectSize = new Vector2(293, 158);
		}
		private void ExibirRegistroImagem()
		{
			CampoImagem.Visible = true;
			CampoDescricao.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			CampoURL.Visible = false;

			var imagem = ImportadorDeBinariosUtil.GerarImagem(Registro.Nome, ObterDetalhesTipo(Registro.Tipo).Extensao, Registro.Conteudo);
			ConteudoImagem.Texture = imagem;

			RectMinSize = new Vector2(293, 401);
			RectSize = new Vector2(293, 401);
		}
		private void ExibirRegistroURL()
		{
			CampoDescricao.Visible = false;
			CampoImagem.Visible = false;
			CampoTextual.Visible = false;
			CampoAudio.Visible = false;
			CampoDownload.Visible = false;
			CampoURL.Visible = true;

			RectMinSize = new Vector2(293, 158);
			RectSize = new Vector2(293, 158);
		}
		public TipoDTO ObterDetalhesTipo(string nomeTipo)
		{
			return (from tipo in Tipos
				where
					tipo.Nome == nomeTipo
				select 
					tipo).FirstOrDefault();
		}
		public void FecharCTRL()
		{
			Nome.QueueFree();
			Apelido.QueueFree();
			PopupDeDownload.QueueFree();
		
			ConteudoDescricao.QueueFree();
			ConteudoTextual.QueueFree();
			ConteudoImagem.QueueFree();
			ConteudoAudio.QueueFree();
			
			CampoDescricao.QueueFree();
			CampoImagem.QueueFree();
			CampoTextual.QueueFree();
			CampoAudio.QueueFree();
			CampoDownload.QueueFree();
			CampoURL.QueueFree();

			foreach (var tipo in Tipos)
				tipo.Dispose();
			Tipos.Clear();	
			Tipos = null;

			if (Registro != null)
				Registro.Dispose();
				
			ConsultarTipoBLL.Dispose();
			QueueFree();
		}
	}
}
