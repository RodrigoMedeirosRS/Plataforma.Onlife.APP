using Godot;
using System;
using System.Linq;

using DTO;
using BLL.Utils;
using BLL.Interface;

public class Main : Node2D
{
	private static AcceptDialog CaixaDeDialogo { get; set; }
	private static ConfirmationDialog CaixaDePergunta { get; set; }
	private static FileDialog CaixaDeArquivos { get; set; }
	private static CadastroDePistaViva CadastroDePistaViva { get; set; }
	private static CadastroDeCidade CadastroDeCidade { get; set; }
	private static CadastroDeRegistro CadastroDeRegistro { get; set; }
	private static bool AguardandoSelecaoDePonto { get; set; }
	private static PackedScene Cidade { get; set; }
	private static IConsultarCidadeBLL ConsultarCidadeBLL { get; set; }
	private static Mapa2D MapaLocalidade { get; set; }
	public static Spatial Localidades { get; private set; }

	[Signal] public delegate void DialogoFinalizado();
	[Signal] public delegate void PerguntaRespondida(string resposta);
	[Signal] public delegate void ArquivoEscolhido(string base64);
	private const int LimiteArquivo = 2097152;

	public override void _Ready()
	{
		RealizarInjecaoDependecias();
		PopularNodes();
		AtualizarCidades();
	}
	private void RealizarInjecaoDependecias()
	{
		ConsultarCidadeBLL = new BLL.ConsultarCidadeBLL();
	}
	private void PopularNodes()
	{
		CaixaDeDialogo = GetNode<AcceptDialog>("./Interface/Popups/CaixaDeDialog");
		CaixaDePergunta = GetNode<ConfirmationDialog>("./Interface/Popups/CaixadeConfirmacao");
		CaixaDeArquivos = GetNode<FileDialog>("./Interface/Popups/FileDialog");
		CadastroDePistaViva = GetNode<CadastroDePistaViva>("./Interface/Popups/CadastroDePessoa");
		CadastroDeCidade = GetNode<CadastroDeCidade>("./Interface/Popups/CadastroDeCidade");
		CadastroDeRegistro = GetNode<CadastroDeRegistro>("./Interface/Popups/CadastroDeRegistro");
		Localidades = GetNode<Spatial>("./CanvasLayer/Mapa3D/Globo/Localidades");
		Cidade = BLL.Utils.InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/Cidade.tscn");
		MapaLocalidade = GetNode<Mapa2D>("./Cidade/Mapa2D");
		AguardandoSelecaoDePonto = false;
	}
	public static bool ObterModoDeCarga()
	{
		return CaixaDeArquivos.Mode == FileDialog.ModeEnum.OpenFile;
	}
	public static void DispararDialogo(string mensagem)
	{
		CaixaDeDialogo.DialogText = mensagem;
		CaixaDeDialogo.Popup_();
	}
	public static bool LocalidadeMode()
	{
		return MapaLocalidade.Visible;
	}
	public static void DispararPergunta(string mensagem)
	{
		CaixaDePergunta.DialogText = mensagem;
		CaixaDePergunta.Popup_();
	}
	public static void DispararArquivo(string[] filtros, bool carregarArquivos = true)
	{
		CaixaDeArquivos.Mode = carregarArquivos ? FileDialog.ModeEnum.OpenFile : FileDialog.ModeEnum.SaveFile;
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
	public static void DispararRegistro(RegistroDTO registroDTO = null)
	{
		AguardandoSelecaoDePonto = false;
		if (registroDTO != null)
			CadastroDeRegistro.CarregarEdicao(registroDTO);
		else
			CadastroDeRegistro.Popup_();
	}
	public static void DispararCidade(LocalidadeDTO localidadeDTO)
	{
		MapaLocalidade.Popup(localidadeDTO);
	}
	public static void AtualizarCidades()
	{
		foreach(var cidade in ConsultarCidadeBLL.ListarCidades())
		{
			var posicao = new Vector3(cidade.X, cidade.Y, cidade.Z);
			var cidadeInstanciada = BLL.Utils.InstanciadorUtil.InstanciarObjeto(cidade.Nome, Localidades, Cidade, posicao);
			(cidadeInstanciada as Cidade).DefinirDadosLocalidade(cidade);
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
			if (ObterModoDeCarga())
				ValidarTamanho(path);
			else
				EmitSignal("ArquivoEscolhido", path);
		}
		catch (Exception ex)
		{
			DispararDialogo("Erro: " + ex.Message);
		}
	}
	private void ValidarTamanho(string caminho)
	{
		var base64 = ImportadorDeBinariosUtil.ObterBase64(caminho);
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
