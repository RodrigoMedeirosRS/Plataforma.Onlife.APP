using Godot;
using System;

using DTO;
using BLL.Utils;
using CTRL.Interface;

public class PlanoDeRegistros : CanvasLayer
{
	public Control Container { get; set; }
	private Control First { get; set; }
	private ColorRect Veu { get; set; }
	private Vector2 MouseOffset { get; set; }
	private bool FechandoArvore { get; set; }
	public override void _Ready()
	{
		PopularNodes();
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
	private void InstanciarJanelaPessoa(PessoaDTO pessoaDTO, Vector2 posicao)
	{
		var janelaCena = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/JanelaPessoa.tscn");
		var janela = InstanciadorUtil.InstanciarObjeto(Container, janelaCena, null);
		(janela as JanelaPessoa).DefinirDados(pessoaDTO);
		(janela as JanelaPessoa).RectGlobalPosition = posicao - new Vector2(134, 148);
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
