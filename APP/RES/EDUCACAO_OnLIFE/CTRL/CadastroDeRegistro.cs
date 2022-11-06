using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BLL.Interface;
using DTO;
using DTO.Utils;
using BLL.Utils;
using DTO.Dominio;


public class CadastroDeRegistro : ConfirmationDialog
{
	private LineEdit Nome { get; set; }
	private LineEdit Apelido { get; set; }
	private LineEdit ConteudoURL { get; set; }
	private OptionButton Idioma { get; set; }
	private OptionButton Tipo { get; set; }
	private TextEdit Descricao { get; set; }
	private TextEdit ConteudoTexto { get; set; }
	private IConsultarTipoBLL BLLTipo { get; set; }
	private TextureButton ConteudoImagem { get; set; }
	private ICadastrarRegistroBLL CadastrarRegistroBLL { get; set; }
	private Control TextoInput { get; set; }
	private Control ImagemInput { get; set; }
	private Control AudioInput { get; set; }
	private Control ArquivoInput { get; set; }
	private Control URLInput { get; set; }
	private TextureButton AbrirURL { get; set; }
	private TextureButton Download { get; set; }
	private TextureButton PlayBTN { get; set; }
	private TextureButton StopBTN { get; set; }

	private Texture ImagemOriginal { get; set; }
	private List<TipoDTO> Tipos { get; set; }
	private RegistroDTO Registro { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDependencias();
		PopularNodes();
		ExibirInterfaceTextual();
		AtualizarInterface();
	}
	public override void _Process(float delta)
	{
		AbrirURL.Visible = !string.IsNullOrEmpty(ConteudoURL.Text);
		Download.Visible = !string.IsNullOrEmpty(Registro.Conteudo);
		PlayBTN.Visible = !string.IsNullOrEmpty(Registro.Conteudo);
		StopBTN.Visible = !string.IsNullOrEmpty(Registro.Conteudo);
	}
	public void RealizarInjecaoDependencias()
	{
		BLLTipo = new BLL.ConsultarTipoBLL();
		CadastrarRegistroBLL = new BLL.CadastrarRegistroBLL();
	}
	public void PopularNodes()
	{
		Nome = GetNode<LineEdit>("./Control/Nome");
		Apelido = GetNode<LineEdit>("./Control/Apelido");
		Idioma = GetNode<OptionButton>("./Control/Idioma");
		Tipo = GetNode<OptionButton>("./Control/Tipo");
		ConteudoTexto = GetNode<TextEdit>("./Control/TextoInput/Conteudo");
		Descricao = GetNode<TextEdit>("./Control/Label/Descricao");
		TextoInput = GetNode<Control>("./Control/TextoInput");
		ImagemInput = GetNode<Control>("./Control/ImagemInput");
		AudioInput = GetNode<Control>("./Control/AudioInput");
		ArquivoInput = GetNode<Control>("./Control/ArquivoInput");
		URLInput = GetNode<Control>("./Control/URLInput");
		ConteudoURL = GetNode<LineEdit>("./Control/URLInput/URL");
		AbrirURL = GetNode<TextureButton>("./Control/URLInput/Go");
		Download = GetNode<TextureButton>("./Control/ArquivoInput/Download");
		ConteudoImagem = GetNode<TextureButton>("./Control/ImagemInput/ImagemButton");
		PlayBTN = GetNode<TextureButton>("./Control/AudioInput/Play");
		StopBTN = GetNode<TextureButton>("./Control/AudioInput/Stop");

		Registro = new RegistroDTO();
		Tipos = BLLTipo.PopularDropDownTipo(Tipo);
		BLLTipo.PopularDropDownIdioma(Idioma);
		ImagemOriginal = ConteudoImagem.TextureNormal;
	}
	public void CarregarEdicao(RegistroDTO registroDTO)
	{
		this.Popup_();
	}
	private void _on_Tipo_item_selected(int index)
	{
		AtualizarInterface();
	}
	private void AtualizarInterface()
	{
		switch (Tipos[Tipo.Selected].TipoExecucao)
		{
			case (TipoExecucao.Texto):
				ExibirInterfaceTextual();
				break;
			case (TipoExecucao.Imagem):
				ExibirInterfaceImagem();
				break;
			case (TipoExecucao.Audio):
				ExibirInterfaceAudio();
				break;
			case (TipoExecucao.Arquivo):
				ExibirInterfaceArquivo();
				break;
			case (TipoExecucao.URL):
				ExibirInterfaceURL();
				break;
		}
	}

	private void _on_CadastroDeRegistro_confirmed()
	{
		try
		{
			PopularDTO();
			CadastrarRegistroBLL.CadastrarRegistro(Registro);
			LimparTela();
			Main.AtualizarCidades();
		}
		catch(Exception ex)
		{
			var registro = Registro;
			CarregarEdicao(registro);
			Main.DispararDialogo(ex.Message);
		}
	}
	private void _on_CadastroDeRegistro_about_to_show()
	{
		LimparTela();
	}
	private void _on_ImagemButton_button_up()
	{
		Main.DispararArquivo(new string[]{ "*.jpg, *.jpeg ; JPG Images" });
	}
	private void _on_Main_ArquivoEscolhido(string base64)
	{
		if (this.Visible)
		{
			Registro.Conteudo = base64;
			ConteudoImagem.TextureNormal = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", base64); 
		}
	}
	private void _on_Stop_button_up()
	{
		// Replace with function body.
	}
	private void _on_Play_button_up()
	{
		// Replace with function body.
	}
	private void _on_FileButton_button_up()
	{
		Main.DispararArquivo(new string[]{ "*" + Tipos[Tipo.Selected].Extensao + " ; " + Tipos[Tipo.Selected].Nome });
	}
	private void _on_Download_button_up()
	{
		// Replace with function body.
	}
	private void _on_Go_button_up()
	{
		// Replace with function body.
	}
	public void LimparTela()
	{
		Registro = new RegistroDTO();
		Nome.Text = string.Empty;
		Apelido.Text = string.Empty;
		Descricao.Text = string.Empty;
		ConteudoURL.Text = string.Empty;
		ConteudoTexto.Text = string.Empty;
		ConteudoImagem.TextureNormal = ImagemOriginal;
	}
	private void PopularDTO()
	{
		Registro.Nome = Nome.Text;
		Registro.Apelido = Apelido.Text;
		Registro.Descricao = Descricao.Text;
		Registro.Tipo = Tipo.GetItemText(Tipo.Selected);
		Registro.Idioma = Idioma.GetItemText(Idioma.Selected);
		Registro.DataInsercao = DateTime.Now;
		Registro.Conteudo = PopularConteudoAscii();
	}
	private string PopularConteudoAscii()
	{
		switch(Tipos[Tipo.Selected].TipoExecucao)
		{
			case (TipoExecucao.Texto):
				return ConteudoTexto.Text;
			case (TipoExecucao.URL):
				return ConteudoURL.Text;
			default:
				return Registro.Conteudo;
		}
	}
	private void ExibirInterfaceTextual()
	{
		DescarregarConteudo();
		DesativarVisibilidade();
		TextoInput.Visible = true;
	}
	private void ExibirInterfaceImagem()
	{
		DescarregarConteudo();
		DesativarVisibilidade();
		ImagemInput.Visible = true;
	}
	private void ExibirInterfaceAudio()
	{
		DescarregarConteudo();
		DesativarVisibilidade();
		AudioInput.Visible = true;
	}
	private void ExibirInterfaceArquivo()
	{
		DescarregarConteudo();
		DesativarVisibilidade();
		ArquivoInput.Visible = true;
	}
	private void ExibirInterfaceURL()
	{
		DescarregarConteudo();
		DesativarVisibilidade();
		URLInput.Visible = true;
	}
	private void DescarregarConteudo()
	{
		Registro.Conteudo = string.Empty;
		ConteudoImagem.TextureNormal = ImagemOriginal;
	}
	private void DesativarVisibilidade()
	{
		TextoInput.Visible = false;
		ImagemInput.Visible = false;
		AudioInput.Visible = false;
		ArquivoInput.Visible = false;
		URLInput.Visible = false;
	}
}
