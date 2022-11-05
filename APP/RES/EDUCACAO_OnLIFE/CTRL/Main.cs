using Godot;
using System;

using DTO;
using BLL.Utils;

public class Main : Node2D
{
	private static AcceptDialog CaixaDeDialogo { get; set; }
	private static ConfirmationDialog CaixaDePergunta { get; set; }
	private static FileDialog CaixaDeArquivos { get; set; }
	private static CadastroDePistaViva CadastroDePistaViva { get; set; }
	public static bool AguardandoDialogo { get; set; }
	public static string AguardandoPergunta { get; set; }
	public static string AguardandoArquivo { get; set; }

	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		CaixaDeDialogo = GetNode<AcceptDialog>("./Interface/Popups/CaixaDeDialog");
		CaixaDePergunta = GetNode<ConfirmationDialog>("./Interface/Popups/CaixadeConfirmacao");
		CaixaDeArquivos = GetNode<FileDialog>("./Interface/Popups/FileDialog");
		CadastroDePistaViva = GetNode<CadastroDePistaViva>("./Interface/Popups/CadastroDePessoa");
		AguardandoDialogo = false;
		AguardandoPergunta = "inicial";
		AguardandoArquivo = "inicial";
	}
	public static void DispararDialogo(string mensagem)
	{
		AguardandoDialogo = true;
		CaixaDeDialogo.DialogText = mensagem;
		CaixaDeDialogo.Popup_();
	}
	public static void DispararPergunta(string mensagem)
	{
		AguardandoPergunta = string.Empty;
		CaixaDePergunta.DialogText = mensagem;
		CaixaDePergunta.Popup_();
	}
	public static void DispararArquivo(string[] filtros)
	{
		AguardandoArquivo = string.Empty;
		CaixaDeArquivos.Filters = filtros;
		CaixaDeArquivos.Popup_();
	}
	public static void DispararPistaViva(PessoaDTO pessoaDTO = null)
	{
		if (pessoaDTO != null)
			CadastroDePistaViva.CarregarEdicao(pessoaDTO);
		else
			CadastroDePistaViva.Popup_();
	}
	private void _on_CaixaDeDialog_confirmed()
	{
		AguardandoDialogo = false;
	}
	private void _on_CaixadeConfirmacao_custom_action(String action)
	{
		AguardandoPergunta = action;
	}
	private void _on_FileDialog_file_selected(String path)
	{
		try
		{
			ValidarTamanho(ImportadorDeBinariosUtil.ObterBase64(path));
		}
		catch (Exception ex)
		{
			DispararDialogo("Erro: " + ex.Message);
		}
	}
	private void ValidarTamanho(string base64)
	{
		var bytes = (base64.Length - 814) / 1.37;
		if (bytes > 2048)
		{
			DispararDialogo("Desculpe, infelizmente o arquivo excede o limite de 2mb. Sugerimos que faça o upload na nuvem e compartilhe aqui como link.");
			AguardandoArquivo = "inicial";
		}
		else
		{
			AguardandoArquivo = base64;
		}
	}
}
