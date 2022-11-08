using Godot;
using System;
using System.Linq;

using DTO;
using DTO.Utils;
using BLL.Utils;
using BLL.Interface;
using CTRL.Interface;

public class Cabecalho : Control, IDisposableCTRL
{
	private Janela Dados { get; set; }
	private IConsultarRegistroBLL RegistroBLL { get; set; }
	private Label Nome { get; set; }
	private Label Apelido { get; set; }
	private Label Descricao { get; set; }
	private Label Idioma { get; set; }
	private AcceptDialog Registro { get; set; }
	private FileDialog SaveFile { get; set; }
	private Texture ImagemOriginal { get; set; }
	private TextureRect Imagem { get; set; }
	private Label Texto { get; set; }
	private Label URL { get; set; }


	public Control ConteudoTexto { get; set; }
	public Control ConteudoAudio { get; set; }
	public Control ConteudoArquivo { get; set; }
	public Control ConteudoImagem { get; set; }
	public Control ConteudoURL { get; set; }

	private AudioStreamPlayer ReprodutorAudio { get ; set; }
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
	}
	private void RealizarInjecaoDeDependencias()
	{
		RegistroBLL = new BLL.ConsultarRegistroBLL();
	}
	private void PopularNodes()
	{
		Dados = GetParent<Janela>();
		Nome = GetNode<Label>("./Nome/NomeTexto");
		Apelido = GetNode<Label>("./Apelido/ApelidoTexto");
		Descricao = GetNode<Label>("./Descricao/ScrollContainer/DescricaoTexto");
		Idioma = GetNode<Label>("../Conteudo/Registro/Control/Idioma/NomeIdioma");
		Registro = GetNode<AcceptDialog>("../Conteudo/Registro");

		ConteudoTexto = GetNode<Control>("../Conteudo/Registro/Control/Texto");
		ConteudoAudio = GetNode<Control>("../Conteudo/Registro/Control/Audio");
		ConteudoArquivo = GetNode<Control>("../Conteudo/Registro/Control/Arquivo");
		ConteudoImagem = GetNode<Control>("../Conteudo/Registro/Control/Imagem");
		ConteudoURL = GetNode<Control>("../Conteudo/Registro/Control/URL");

		ReprodutorAudio = GetNode<AudioStreamPlayer>("../Conteudo/Registro/Control/Audio/ConteudoAudio");
		Texto = GetNode<Label>("../Conteudo/Registro/Control/Texto/ScrollContainer/CorpoDoTexto");
		Imagem = GetNode<TextureRect>("../Conteudo/Registro/Control/Imagem/ImagemButton");
		ImagemOriginal = Imagem.Texture;
		ConteudoURL = GetNode<Label>("../Conteudo/Registro/Control/URL/URL");
		SaveFile = GetNode<FileDialog>("../Conteudo/SaveDialog");
	}
	private void _on_Janela_DadosCarregados()
	{
		PopularDados();
	}
	public void FecharCTRL()
	{
		Dados.RegistroDTO.Dispose();
		Dados.Alerta.QueueFree();
		Registro.QueueFree();
		RegistroBLL.Dispose();
		Main.FecharArvore();
		GetParent().QueueFree();
	}
	private void PopularDados()
	{
		Nome.Text = Dados.RegistroDTO.Nome;
		Apelido.Text = Dados.RegistroDTO.Apelido;
		Descricao.Text = Dados.RegistroDTO.Descricao;
		Idioma.Text = Dados.RegistroDTO.Idioma;
		AlternarTipoJanela();
	}
	private void AlternarTipoJanela()
	{
		ReprodutorAudio.Stop();
		var tipo = Main.Tipos.FirstOrDefault(tipo => tipo.Nome == Dados.RegistroDTO.Tipo);
		ConteudoTexto.Visible = tipo.TipoExecucao == TipoExecucao.Texto;
		ConteudoAudio.Visible = tipo.TipoExecucao == TipoExecucao.Audio;
		ConteudoArquivo.Visible = tipo.TipoExecucao == TipoExecucao.Arquivo;
		ConteudoImagem.Visible = tipo.TipoExecucao == TipoExecucao.Imagem;
		ConteudoURL.Visible = tipo.TipoExecucao == TipoExecucao.URL;
	}
	private void AtribuirConteudo()
	{
		ValidarConteudoRegistro();
		var tipo = Main.Tipos.FirstOrDefault(tipo => tipo.Nome == Dados.RegistroDTO.Tipo);
		switch(tipo.TipoExecucao)
		{
			case(TipoExecucao.Audio):
				ReprodutorAudio.Stream = ImportadorDeBinariosUtil.GerarAudio("temp", tipo.Extensao, Dados.RegistroDTO.Conteudo);
				break;
			case(TipoExecucao.Imagem):
				Imagem.Texture = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", Dados.RegistroDTO.Conteudo);
				break;
			case(TipoExecucao.Texto):
				Texto.Text = Dados.RegistroDTO.Conteudo;
				break;
			case(TipoExecucao.URL):
				URL.Text = Dados.RegistroDTO.Conteudo;
				break;
		};
	}
	private void ValidarConteudoRegistro()
	{
		if (string.IsNullOrEmpty(Dados.RegistroDTO.Conteudo))
		{
			var registro = RegistroBLL.RealizarConsulta(new DTO.Dominio.RegistroConsulta()
			{
				Nome = Dados.RegistroDTO.Nome,
				Apelido = Dados.RegistroDTO.Apelido,
				Idioma = Dados.RegistroDTO.Idioma,
				Completo = true
			}).FirstOrDefault();
			Dados.RegistroDTO.Conteudo = registro.Conteudo;
		}
	}
	private void _on_Editar_button_up()
	{
		Dados.Edicao = true;
		Dados.Alerta.Popup_();
	}
	
	private void _on_Abrir_button_up()
	{
		AtribuirConteudo();
		Registro.Popup_();
	}
	private void _on_Relacoes_button_up()
	{
		
	}
	private void _on_Registro_popup_hide()
	{
		ReprodutorAudio.Stop();
	}
	private void _on_Alterta_confirmed()
	{
		if (Dados.Edicao)
		{
			ValidarConteudoRegistro();
			Main.DispararRegistro(Dados.RegistroDTO);
		}
		FecharCTRL();
	}
	private void _on_Stop_button_up()
	{
		ReprodutorAudio.Stop();
	}
	private void _on_Play_button_up()
	{
		ReprodutorAudio.Play();
	}
	private void _on_Download_button_up()
	{
		var tipo = Main.Tipos.FirstOrDefault(tipo => tipo.Nome == Dados.RegistroDTO.Tipo);
		SaveFile.Filters = new string[]{ "*" + tipo.Extensao + " ; " + tipo.Nome };
		SaveFile.Popup_();
	}
	private void _on_Go_button_up()
	{
		if(!string.IsNullOrEmpty(Dados.RegistroDTO.Conteudo))
			OS.ShellOpen(Dados.RegistroDTO.Conteudo);
	}
	private void _on_FileDialog_file_selected(String path)
	{
		ImportadorDeBinariosUtil.SalvarBase64(path, Dados.RegistroDTO.Conteudo);
	}
}
