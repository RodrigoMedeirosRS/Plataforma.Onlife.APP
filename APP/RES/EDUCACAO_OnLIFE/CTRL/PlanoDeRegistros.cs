using Godot;
using System;
using System.Collections.Generic;

using DTO;
using BLL.Utils;
using BLL.Interface;
using CTRL.Interface;
using DTO.Dominio;

public class PlanoDeRegistros : CanvasLayer
{
	public Control Container { get; set; }
	private Control First { get; set; }
	private ColorRect Veu { get; set; }
	private Vector2 MouseOffset { get; set; }
	private IConsultarRegistroBLL RegistroBLL { get; set; }
	private IConsultarPessoaBLL PessoaBLL { get; set; }
	private bool FechandoArvore { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
	}
	private void RealizarInjecaoDeDependencias()
	{
		RegistroBLL = new BLL.ConsultarRegistroBLL();
		PessoaBLL = new BLL.ConsultarPessoaBLL();
	}
	private void PopularNodes()
	{
		FechandoArvore = false;
		MouseOffset = new Vector2();
		Container = GetNode<Control>("./Container");
		Veu = GetNode<ColorRect>("./Veu");
		First = GetNode<Control>("./First");
	}
	private void AlternarVisiblidade()
	{
		Veu.Visible = Container.GetChildCount() > 0;
	}
	public override void _Process(float delta)
	{
		AlternarVisiblidade();
		Mover();
	}
	public void InstanciarPrimeiroRegisro(RegistroDTO registroDTO)
	{
		if(!Veu.Visible)
		{
			InstanciarJanelaRegistro(registroDTO, First.RectGlobalPosition);
		}
	}
	public void InstanciarPrimeiraPessoa(PessoaDTO pessoaDTO)
	{
		if(!Veu.Visible)
		{
			InstanciarJanelaPessoa(pessoaDTO, First.RectGlobalPosition);
		}
	}
	private void InstanciarJanelaRegistro(RegistroDTO registroDTO, Vector2 posicao)
	{
		var janelaCena = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/JanelaRegistro.tscn");
		var janela = InstanciadorUtil.InstanciarObjeto(Container, janelaCena, null);
		(janela as Janela).PopularDados(registroDTO);
		(janela as Janela).RectGlobalPosition = posicao - new Vector2(134, 148);
	}
	public void InstanciarReferencias(RegistroDTO registroDTO, Vector2 posicao)
	{
		posicao += new Vector2(434, 148);
		var relacoes = BuscarRelacoes(registroDTO);
		foreach (var referencia in relacoes.Pessoas)
		{
			InstanciarJanelaPessoa(referencia, posicao);
			posicao -= new Vector2(0, 350);
		}
		foreach (var relacao in relacoes.Registros)
		{
			InstanciarJanelaRegistro(relacao, posicao);
			posicao += new Vector2(0, 350);
		}		
	}
	private ReferenciaRetorno BuscarRelacoes(RegistroDTO registroDTO)
	{
		try
		{
			return RegistroBLL.RealizarConsultaDeRegistrosRelacionados(new DTO.Dominio.RelacaoConsulta()
			{
				CodRegistro = registroDTO.Codigo
			});
		}
		catch
		{
			return new ReferenciaRetorno();
		}
	}
	private void InstanciarJanelaPessoa(PessoaDTO pessoaDTO, Vector2 posicao)
	{
		var janelaCena = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/JanelaPessoa.tscn");
		var janela = InstanciadorUtil.InstanciarObjeto(Container, janelaCena, null);
		(janela as JanelaPessoa).DefinirDados(pessoaDTO);
		(janela as JanelaPessoa).RectGlobalPosition = posicao - new Vector2(134, 148);
	}
	public void InstanciarRelacoes(PessoaDTO pessoaDTO, Vector2 posicao)
	{
		posicao += new Vector2(434, 148);

		foreach (var relacao in BuscarRelacoes(pessoaDTO))
		{
			InstanciarJanelaRegistro(relacao, posicao);
			posicao += new Vector2(0, 350);
		}		
	}
	private List<RegistroDTO> BuscarRelacoes(PessoaDTO pessoaDTO)
	{
		try
		{
			return PessoaBLL.RealizarConsultaDeRegistrosRelacionados(new DTO.Dominio.RelacaoConsulta()
			{
				CodRegistro = pessoaDTO.Codigo
			});
		}
		catch
		{
			return new List<RegistroDTO>();
		}
	}
	public void LimparRegistros()
	{
		if (FechandoArvore == false)
		{
			FechandoArvore = true;
			foreach(var registro in Container.GetChildren())
				(registro as IDisposableCTRL).FecharCTRL();
		}
		FechandoArvore = false;
	}
	public void Mover()
	{
		if (Veu.Visible)
		{
			if (Input.IsActionJustPressed("meio"))
				MouseOffset = Container.GetLocalMousePosition();
			if (Input.IsActionPressed("meio"))
				Container.RectGlobalPosition = Container.GetGlobalMousePosition() - MouseOffset;
		}
	}
}
