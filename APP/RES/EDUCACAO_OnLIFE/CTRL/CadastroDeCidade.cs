using Godot;
using System;

using DTO;
using BLL.Utils;
using BLL.Interface;

public class CadastroDeCidade : ConfirmationDialog
{
	private ICadastrarCidadeBLL BLL { get; set; }
	private LineEdit Nome { get; set; }
	private RichTextLabel Descricao { get; set; }
	private TextureButton Mapa { get; set; }
	private Texture ImagemPadrao { get; set; }
	private LocalidadeDTO Localidade { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
		LimparTela();
	}
	private void RealizarInjecaoDeDependencias()
	{
		BLL = new BLL.CadastrarCidadeBLL();
	}
	private void PopularNodes()
	{
		Nome = GetNode<LineEdit>("./Control/Nome");
		Descricao = GetNode<RichTextLabel>("./Control/Label/Descricao");
		Mapa = GetNode<TextureButton>("./Control/ImageMapa");
		ImagemPadrao = Mapa.TextureNormal;
		Localidade = new LocalidadeDTO();
	}
	public void LimparTela()
	{
		Nome.Text = string.Empty;
		Descricao.BbcodeText = string.Empty;
		Mapa.TextureNormal = ImagemPadrao;
		Localidade = new LocalidadeDTO();
	}
	private void PopularDTO()
	{
		Localidade.Nome = Nome.Text;
		Localidade.Descricao = Descricao.BbcodeText;
	}
	public void Popup(Vector3 posicao)
	{
		LimparTela();
		Localidade.X = posicao.x;
		Localidade.Y = posicao.y;
		Localidade.Z = posicao.z;
	}
	public void CarregarEdicao(LocalidadeDTO localidade)
	{
		try
		{
			LimparTela();
			this.Popup_();
			Localidade = localidade;
			Nome.Text = localidade.Nome;
			Descricao.BbcodeText = localidade.Descricao;
			if (!string.IsNullOrEmpty(localidade.Mapa))
				Mapa.TextureNormal = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", localidade.Mapa);
		}
		catch
		{
			Main.DispararDialogo("Erro ao carregar perfil");
		}
	}
	private void _on_CadastroDeCidade_confirmed()
	{
		try
		{
			PopularDTO();
			BLL.CadastrarCidade(Localidade);
			LimparTela();
		}
		catch(Exception ex)
		{
			var localidade = Localidade;
			CarregarEdicao(localidade);
			Main.DispararDialogo(ex.Message);
		}
	}
	private void _on_ImageMapa_button_up()
	{
		Main.DispararArquivo(new string[]{ "*.jpg, *.jpeg ; JPG Images" });
	}
	private void _on_Main_ArquivoEscolhido(string base64)
	{
		if (this.Visible)
		{
			Localidade.Mapa = base64;
			Mapa.TextureNormal = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", base64); 
		}
	}
}
