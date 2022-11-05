using Godot;
using System;
using System.Linq;

using DTO;
using BLL.Utils;

public class Main : Node2D
{
	private static AcceptDialog CaixaDeDialogo { get; set; }
	private static ConfirmationDialog CaixaDePergunta { get; set; }
	private static FileDialog CaixaDeArquivos { get; set; }
	private static CadastroDePistaViva CadastroDePistaViva { get; set; }
	private static CadastroDeCidade CadastroDeCidade { get; set; }
	private const int LimiteArquivo = 2097152;
	private static bool AguardandoSelecaoDePonto { get; set; }

	[Signal] public delegate void DialogoFinalizado();
	[Signal] public delegate void PerguntaRespondida(string resposta);
	[Signal] public delegate void ArquivoEscolhido(string base64);

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
		CadastroDeCidade = GetNode<CadastroDeCidade>("./Interface/Popups/CadastroDeCidade");
		AguardandoSelecaoDePonto = false;
	}
	public static void DispararDialogo(string mensagem)
	{
		CaixaDeDialogo.DialogText = mensagem;
		CaixaDeDialogo.Popup_();
	}
	public static void DispararPergunta(string mensagem)
	{
		CaixaDePergunta.DialogText = mensagem;
		CaixaDePergunta.Popup_();
	}
	public static void DispararArquivo(string[] filtros)
	{
		CaixaDeArquivos.Filters = filtros;
		CaixaDeArquivos.Popup_();
	}
	public static void DispararPistaViva(PessoaDTO pessoaDTO = null)
	{
		AguardandoSelecaoDePonto = false;
		if (pessoaDTO != null)
			CadastroDePistaViva.CarregarEdicao(pessoaDTO);
		else
			CadastroDePistaViva.Popup_();
	}
	public static void DispararLocalidade(Vector3 posicao)
	{
		if (AguardandoSelecaoDePonto)
		{
			AguardandoSelecaoDePonto = false;
			CadastroDeCidade.Popup(posicao);
		}
	}
	private void _on_CaixaDeDialog_confirmed()
	{
		EmitSignal("DialogoFinalizado");
		AguardandoSelecaoDePonto = CaixaDeDialogo.DialogText.Contains("Por favor, clique com o botão direito do mouse sobre o globo marcando a posição aproximada da cidade que você quer cadastrar.");
	}
	private void _on_CaixadeConfirmacao_custom_action(String action)
	{
		EmitSignal("PerguntaRespondida", action.ToString());
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
		if (GetOriginalLengthInBytes(base64) > LimiteArquivo)
			DispararDialogo("Desculpe, infelizmente o arquivo excede o limite de 2mb. Sugerimos que faça o upload na nuvem e compartilhe aqui como link.");
		else
			EmitSignal("ArquivoEscolhido", base64);
	}
	public int GetOriginalLengthInBytes(string base64)
	{
		if (string.IsNullOrEmpty(base64)) { return 0; }

		var characterCount = base64.Length;
		var paddingCount = base64.Substring(characterCount - 2, 2)
									.Count(c => c == '=');
		return (3 * (characterCount / 4)) - paddingCount;
	}
}
