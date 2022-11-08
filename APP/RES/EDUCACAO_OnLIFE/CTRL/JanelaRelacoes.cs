using Godot;
using System;
using System.Collections.Generic;

using DTO;
using BLL.Utils;
using BLL.Interface;
using CTRL.Interface;

public class JanelaRelacoes : AcceptDialog, IDisposableCTRL
{
	private IConsultarPessoaBLL BLLPessoa { get; set; }
	private IConsultarRegistroBLL BLLRegistro { get; set; }
	private IConsultarTipoBLL BLLTIpo { get; set; }
	private LineEdit BarraDeBusca { get; set; }
	private OptionButton Idioma { get; set; }
	private VBoxContainer Container { get; set; }
	private List<RelacaoDTO> Relacoes { get; set; }
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
		Relacoes = new List<RelacaoDTO>();
		BarraDeBusca = GetNode<LineEdit>("./Conteudo/BarraDeBusca/LineEdit");
		Idioma = GetNode<OptionButton>("./Conteudo/BarraDeBusca/OptionButton");
		Container = GetNode<VBoxContainer>("./Conteudo/Container/Ordenador");
		BLLTIpo.PopularDropDownIdioma(Idioma);
	}
	public void DefinirDados(List<RelacaoDTO> relacoes)
	{
		Relacoes = relacoes;
		RealizarConsulta();
	}
	private void RealizarConsulta()
	{
		LimparConsulta();
		try
		{
			ListarRegistrosRelacionados();
			RealizarConsultaRegisros();
		}
		catch {}
	}
	private void RealizarConsultaRegisros()
	{
		if (!string.IsNullOrEmpty(BarraDeBusca.Text))
		{
			var registros = new List<RegistroDTO>();
			registros = BLLRegistro.RealizarConsulta(new DTO.Dominio.RegistroConsulta()
			{
				Nome = BarraDeBusca.Text,
				Idioma = Idioma.GetItemText(Idioma.Selected),
				Completo = false
			});
			InstanciarResultados(registros, false);
		}
	}
	private void ListarRegistrosRelacionados()
	{
		var registros = new List<RegistroDTO>();
		foreach (var relacao in Relacoes)
		{
			var retorno = BLLRegistro.ObterRelacao(new DTO.Dominio.RelacaoConsulta()
			{
				CodRegistro = (int)relacao.RelacaoID
			});
			if (retorno != null)
				registros.Add(retorno);
		}
		InstanciarResultados(registros, true);
	}

	private void InstanciarResultados(List<RegistroDTO> registros, bool relacionado)
	{
		foreach(var registro in registros)
		{
			var registroInstanciado = InstanciadorUtil.CarregarCena("res://RES/EDUCACAO_OnLIFE/CENAS/ResultadoRelacao.tscn");
			var registroCena = InstanciadorUtil.InstanciarObjeto(Container, registroInstanciado, null);
			(registroCena as ResultadoRelacao).DefinirDados(registro, relacionado);
		};
	}
	public List<RelacaoDTO> ObterRelacoes()
	{
		AtualizarRelacoes();
		return Relacoes;
	}
	private void AtualizarRelacoes()
	{
		Relacoes = new List<RelacaoDTO>();
		foreach(var relacao in Container.GetChildren())
			if ((relacao as ResultadoRelacao).Relacionado)
				Relacoes.Add((relacao as ResultadoRelacao).Relacao);
	}
	private void LimparConsulta()
	{
		AtualizarRelacoes();
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
	public void FecharCTRL()
	{
		BLLPessoa.Dispose();
		BLLTIpo.Dispose();
		BLLRegistro.Dispose();
		QueueFree();
	}
}
