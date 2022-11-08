using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

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
	private static PackedScene Cidade { get; set; }
	private static Mapa2D MapaLocalidade { get; set; }
	private static PlanoDeRegistros PlanoDeRegistros { get; set; }

	public static Spatial Localidades { get; private set; }
	public static bool AguardandoSelecaoDePonto { get; private set; }
	public static List<TipoDTO> Tipos { get; private set; }

	private static IConsultarCidadeBLL ConsultarCidadeBLL { get; set; }
	private static IConsultarTipoBLL TipoBLL { get; set; }
	
	[Signal] public delegate void DialogoFinalizado();
	[Signal] public delegate void PerguntaRespondida(string resposta);
	[Signal] public delegate void ArquivoEscolhido(string base64);

	private Button NovoRegistroBTN { get; set; }
	private Button NovaPistaVivaBTN { get; set; }
	private Button NovaCidadeBTN { get; set; }
	private Button BuscarBTN { get; set; }
	private ColorRect Veu { get; set; }
	private const int LimiteArquivo = 2097152;

	public override void _Ready()
	{
		ReposicionarTela();
		RealizarInjecaoDependecias();
		PopularNodes();
		AtualizarCidades();
	}
	public override void _Process(float delta)
	{
		NovaCidadeBTN.Disabled = LocalidadeMode() || Veu.Visible;
		NovoRegistroBTN.Disabled = Veu.Visible;
		NovaPistaVivaBTN.Disabled = Veu.Visible;
		BuscarBTN.Disabled = Veu.Visible;
	}
	private void RealizarInjecaoDependecias()
	{
		ConsultarCidadeBLL = new BLL.ConsultarCidadeBLL();
		TipoBLL = new BLL.ConsultarTipoBLL();
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
		NovaCidadeBTN = GetNode<Button>("./Toolbar/InterfaceSobreposta/SideBar/VBoxContainer/NovaCidade");
		NovaPistaVivaBTN = GetNode<Button>("./Toolbar/InterfaceSobreposta/SideBar/VBoxContainer/NovaPistaViva");
		NovoRegistroBTN = GetNode<Button>("./Toolbar/InterfaceSobreposta/SideBar/VBoxContainer/NovoRegistro");
		BuscarBTN = GetNode<Button>("./Toolbar/InterfaceSobreposta/SideBar/VBoxContainer/Buscar");
		Veu = GetNode<ColorRect>("./PlanoDeRegistros/Veu");
		AguardandoSelecaoDePonto = false;
		PlanoDeRegistros = GetNode<PlanoDeRegistros>("./PlanoDeRegistros");
		Tipos = TipoBLL.ListarTipos();
	}
	public static void InstanciarPrimeiraPessoa(PessoaDTO pessoaDTO)
	{
		PlanoDeRegistros.InstanciarPrimeiraPessoa(pessoaDTO);
	}
	public static void InstanciarPrimeiroRegisro(RegistroDTO registroDTO)
	{
		PlanoDeRegistros.InstanciarPrimeiroRegisro(registroDTO);
	}
	public static void InstanciarRelacoes(PessoaDTO pessoaDTO, Vector2 posicao)
	{
		PlanoDeRegistros.InstanciarRelacoes(pessoaDTO, posicao);
	}
	public static void InstanciarReferencias(RegistroDTO registroDTO, Vector2 posicao)
	{
		PlanoDeRegistros.InstanciarReferencias(registroDTO, posicao);
	}
	public static void FecharArvore()
	{
		PlanoDeRegistros.LimparRegistros();
		MapaLocalidade.AtualizarRegistrosLocalizados();
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
	public static void AutalizarRegistrosLocalizados()
	{
		MapaLocalidade.AtualizarRegistrosLocalizados();
	}
	private void _on_CaixaDeDialog_confirmed()
	{
		EmitSignal("DialogoFinalizado");
		AguardandoSelecaoDePonto = CaixaDeDialogo.DialogText.Contains(InterfaceSobreposta.MensagemCidade) || CaixaDeDialogo.DialogText.Contains(InterfaceSobreposta.MensagemRegistro);

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
	private void ReposicionarTela()
	{
		if (OS.GetName() == "Windows")
		{
			var screen_size = OS.GetScreenSize(0);
			var window_size = OS.WindowSize;
			OS.WindowPosition = screen_size * 0.5f - window_size * 0.5f;
		}
	}
}
