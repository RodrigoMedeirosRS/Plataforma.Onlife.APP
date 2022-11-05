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
	public void CarregarEdicao(PessoaDTO pistaviva)
	{
		try
		{
			PistaViva = pistaviva;
			Nome.Text = pistaviva.Nome;
			Apelido.Text = pistaviva.Apelido;
			Lattes.Text = pistaviva.Lattes;
			LinkedIn.Text = pistaviva.Lattes;
			ResearchGate.Text = pistaviva.ResearchGate;
			FotoPerfil.TextureNormal = ImportadorDeBinariosUtil.GerarImagem(Nome.Text, ".jpg", pistaviva.Foto);
			this.Popup_();
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
			BLL.CadastrarPessoa(PistaViva);
			LimparTela();
		}
		catch(Exception ex)
		{
			this.Popup_();
			Main.DispararDialogo(ex.Message);
		}
	}
}
