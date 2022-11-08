using Godot;
using System;

using DTO;
using BLL.Utils;
using BLL.Interface;

public class CadastroDePistaViva : ConfirmationDialog
{
	private ICadastrarPessoaBLL BLL { get; set; }
	private LineEdit Nome { get; set; }
	private LineEdit Apelido { get; set; }
	private LineEdit Lattes { get; set; }
	private LineEdit ResearchGate { get; set; }
	private LineEdit LinkedIn { get; set; }
	private TextureButton FotoPerfil { get; set; }
	private Texture FotoPadrao { get; set; }
	private PessoaDTO PistaViva { get; set; }
	private JanelaRelacoes JanelaRelacoes { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
		LimparTela();
	}
	private void RealizarInjecaoDeDependencias()
	{
		BLL = new BLL.CadastrarPessoaBLL();
	}
	private void PopularNodes()
	{
		Nome = GetNode<LineEdit>("./Control/Nome");
		Apelido = GetNode<LineEdit>("./Control/Apelido");
		Lattes = GetNode<LineEdit>("./Control/Lattes");
		ResearchGate = GetNode<LineEdit>("./Control/ResearchGate");
		LinkedIn = GetNode<LineEdit>("./Control/LinkedIn");
		FotoPerfil = GetNode<TextureButton>("./Control/FotoDePerfil");
		JanelaRelacoes = GetNode<JanelaRelacoes>("./Popup/JanelaRelacoes");
		FotoPadrao = FotoPerfil.TextureNormal;
		PistaViva = new PessoaDTO();
	}
	public void LimparTela()
	{
		Nome.Text = string.Empty;
		Apelido.Text = string.Empty;
		Lattes.Text = string.Empty;
		ResearchGate.Text = string.Empty;
		LinkedIn.Text = string.Empty;
		FotoPerfil.TextureNormal = FotoPadrao;
		PistaViva = new PessoaDTO();
	}
	private void PopularDTO()
	{
		PistaViva.Nome = Nome.Text;
		PistaViva.Apelido = Apelido.Text;
		PistaViva.Lattes = Lattes.Text;
		PistaViva.ResearchGate = ResearchGate.Text;
		PistaViva.LinkedIn = LinkedIn.Text;
	}
	public void CarregarEdicao(PessoaDTO pistaviva)
	{
		try
		{
			this.Popup_();
			PistaViva = pistaviva;
			Nome.Text = pistaviva.Nome;
			Apelido.Text = pistaviva.Apelido;
			Lattes.Text = pistaviva.Lattes;
			LinkedIn.Text = pistaviva.Lattes;
			ResearchGate.Text = pistaviva.ResearchGate;
			if (!string.IsNullOrEmpty(pistaviva.Foto))
				FotoPerfil.TextureNormal = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", pistaviva.Foto);
				
		}
		catch
		{
			Main.DispararDialogo("Erro ao carregar perfil");
		}
	}
	private void _on_CadastroDePessoa_confirmed()
	{
		try
		{
			PopularDTO();
			BLL.CadastrarPessoa(PistaViva);
			LimparTela();
		}
		catch(Exception ex)
		{
			var pessoa = PistaViva;
			CarregarEdicao(pessoa);
			Main.DispararDialogo(ex.Message);
		}
	}
	private void _on_CadastroDePessoa_about_to_show()
	{
		LimparTela();
	}
	private void _on_FotoDePerfil_button_up()
	{
		Main.DispararArquivo(new string[]{ "*.jpg, *.jpeg ; JPG Images" });
	}
	private void _on_Main_ArquivoEscolhido(string base64)
	{
		if (this.Visible)
		{
			PistaViva.Foto = base64;
			FotoPerfil.TextureNormal = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", base64); 
		}
	}
	private void _on_JanelaRelacoes_confirmed()
	{
		PistaViva.Relacoes = JanelaRelacoes.ObterRelacoes();
	}
	private void _on_Relacoes_button_up()
	{
		JanelaRelacoes.Popup_();
		JanelaRelacoes.DefinirDados(PistaViva.Relacoes);
		JanelaRelacoes.RectGlobalPosition = this.RectGlobalPosition;
	}
}
