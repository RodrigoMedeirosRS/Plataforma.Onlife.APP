using Godot;
using System;

using DTO;
using BLL.Utils;
using CTRL.Interface;

public class PlanoDeRegistros : CanvasLayer
{
	public Control Container { get; set; }
	private ColorRect Veu { get; set; }
	private Vector2 MouseOffset { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		MouseOffset = new Vector2();
		Container = GetNode<Control>("./Container");
		Veu = GetNode<ColorRect>("./Veu");
	}
	private void AlternarVisiblidade()
	{
		Veu.Visible = Container.GetChildCount() > 0;
		if (!Veu.Visible)
			Container.RectPosition = new Vector2(960, 540);
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
			var janelaCena = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/JanelaRegistro.tscn");
			var janela = InstanciadorUtil.InstanciarObjeto(Container, janelaCena, null);
			(janela as Janela).PopularDados(registroDTO);
		}
	}
	public void InstanciarPrimeiraPessoa(PessoaDTO pessoaDTO)
	{
		if(!Veu.Visible)
		{
			var janelaCena = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/JanelaPessoa.tscn");
			var janela = InstanciadorUtil.InstanciarObjeto(Container, janelaCena, null);
			(janela as JanelaPessoa).DefinirDados(pessoaDTO);
		}
	}
	public void LimparRegistros()
	{
		foreach(var registro in Container.GetChildren())
			(registro as IDisposableCTRL).FecharCTRL();
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
