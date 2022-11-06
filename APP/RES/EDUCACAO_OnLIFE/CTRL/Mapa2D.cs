using Godot;
using System;
using System.Collections.Generic;

using BLL.Interface;
using DTO;
using DTO.Utils;
using BLL.Utils;
using DTO.Dominio;
using CTRL.Interface;

public class Mapa2D : Control
{
	private LocalidadeDTO Cidade { get; set; }
	private TextureRect Mapa { get; set; }
	private Control Registros { get; set; }
	private Texture MapaOriginal { get; set; }

	private IConsultarCidadeBLL CidadeBLL { get; set; }
	private IConsultarRegistroBLL RegistroBLL { get; set; }
	public override void _Ready()
	{
		PopularNodes();
		RealizarInjecaoDependecias();	
	}
	public void PopularNodes()
	{
		Cidade = new LocalidadeDTO();
		Mapa = GetNode<TextureRect>("./Mapa");
		Registros = GetNode<Control>("./Registros");
		MapaOriginal = Mapa.Texture;
	}
	private void RealizarInjecaoDependecias()
	{
		CidadeBLL = new BLL.ConsultarCidadeBLL();
		RegistroBLL = new BLL.ConsultarRegistroBLL();
	}
	public override void _Process(float delta)
	{
		if (Main.LocalidadeMode() && Main.AguardandoSelecaoDePonto && Input.IsActionPressed("clique"))
		{
			var mousePosition = GetViewport().GetMousePosition() + this.RectPosition;
			var registro = new RegistroDTO()
			{
				Latitude = mousePosition.x,
				Longitude = mousePosition.y
			};
			Main.DispararRegistro(registro);
		}
	}
	public void Popup(LocalidadeDTO cidade)
	{
		LimparRegistrosLocalizados();
		DefinirDadosLocalidade(cidade);
		this.Visible = true;
	}
	public void DefinirDadosLocalidade(LocalidadeDTO cidade)
	{
		try
		{
			ConsultarCidade(cidade);
			PopularRegistrosLocalizados();
			Mapa.Texture = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", Cidade.Mapa);
		}
		catch
		{
			Main.DispararDialogo("Erro interno no sistema");
		}
	}
	private void ConsultarCidade(LocalidadeDTO cidade)
	{
		Cidade = CidadeBLL.ConsultarCidade(new LocalidadeConsulta()
		{
			Codigo = cidade.Codigo,
			Nome = cidade.Nome,
			Completo = true
		});
	}
	private void PopularRegistrosLocalizados()
	{

	}
	private void LimparRegistrosLocalizados()
	{
		foreach(var registro in Registros.GetChildren())
			(registro as IDisposableCTRL).FecharCTRL();
	}
	private void _on_Button_button_up()
	{
		this.Visible = false;
	}
}
