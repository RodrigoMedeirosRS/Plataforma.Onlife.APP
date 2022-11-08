using Godot;
using System;
using System.Collections.Generic;

using DTO;
using BLL.Utils;
using BLL.Interface;
using CTRL.Interface;

public class TelaDeBusca : AcceptDialog
{
	private IConsultarPessoaBLL BLLPessoa { get; set; }
	private IConsultarRegistroBLL BLLRegistro { get; set; }
	private IConsultarTipoBLL BLLTIpo { get; set; }
	private LineEdit BarraDeBusca { get; set; }
	private OptionButton Idioma { get; set; }
	private VBoxContainer Container { get; set; } 
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
	}
	private void RealizarInjecaoDeDependencias()
	{
		BLLPessoa = new BLL.ConsultarPessoaBLL();
		BLLTIpo = new BLL.ConsultarTipoBLL();
		BLLRegistro = new BLL.ConsultarRegistroBLL();
	}
	private void PopularNodes()
	{
		BarraDeBusca = GetNode<LineEdit>("./Conteudo/BarraDeBusca/LineEdit");
		Idioma = GetNode<OptionButton>("./Conteudo/BarraDeBusca/OptionButton");
		Container = GetNode<VBoxContainer>("./Conteudo/Container/Ordenador");
		BLLTIpo.PopularDropDownIdioma(Idioma);
	}
	private void RealizarConsulta()
	{
		LimparConsulta();

		var registros = new List<RegistroDTO>();
		var pistasVivas = new List<PessoaDTO>();
		
		try
		{
			registros = BLLRegistro.RealizarConsulta(new DTO.Dominio.RegistroConsulta()
			{
				Nome = BarraDeBusca.Text,
				Idioma = Idioma.GetItemText(Idioma.Selected),
				Completo = false
			});
		}
		catch{}
		try
		{
			 pistasVivas = BLLPessoa.RealizarConsulta(new DTO.Dominio.PessoaConsulta()
			{
				Nome = BarraDeBusca.Text
			});
		}
		catch{}

		InstanciarResultados(registros, pistasVivas);
	}
	private void InstanciarResultados(List<RegistroDTO> registros, List<PessoaDTO> pistasVivas)
	{
		foreach(var registro in registros)
		{
			var registroInstanciado = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/ResultadoBusca.tscn");
			var registroCena = InstanciadorUtil.InstanciarObjeto(Container, registroInstanciado, null);
			(registroCena as ResultadoBusca).DefinirDados(registro);
		};
		foreach(var pistaViva in pistasVivas)
		{
			var pistaVivaInstanciada = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/ResultadoBusca.tscn");
			var pistaVivaCena = InstanciadorUtil.InstanciarObjeto(Container, pistaVivaInstanciada, null);
			(pistaVivaCena as ResultadoBusca).DefinirDados(pistaViva);
		};
	}
	private void LimparConsulta()
	{
		foreach(var resultado in Container.GetChildren())
			(resultado as IDisposableCTRL).FecharCTRL();
	}
	private void _on_Busca_button_up()
	{
		RealizarConsulta();
	}
	private void _on_TelaDeBusca_popup_hide()
	{
		BarraDeBusca.Text = string.Empty;
		LimparConsulta();
	}
}
