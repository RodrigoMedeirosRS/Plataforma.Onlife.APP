using Godot;
using System;
using System.Collections.Generic;

using BLL.Interface;
using DTO;
using DTO.Utils;
using BLL.Utils;
using DTO.Dominio;
using CTRL.Interface;

public class Registro : TextureButton
{
	private LocalidadeDTO Cidade { get; set; }
	private RegistroDTO RegistroDTO { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	public void PopularNodes()
	{
		Cidade = new LocalidadeDTO();
		RegistroDTO = new RegistroDTO();
	}
	public void PouparDadosRegistro(LocalidadeDTO cidade, RegistroDTO registro)
	{
		Cidade = cidade;
		RegistroDTO = registro;
		this.HintTooltip = RegistroDTO.Nome;
	}
	private void _on_TextureButton_button_up()
	{
		// Replace with function body.
	}
}
