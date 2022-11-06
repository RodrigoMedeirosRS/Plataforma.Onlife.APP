using Godot;
using System;
using System.Collections.Generic;

using BLL.Interface;
using DTO;
using DTO.Utils;
using BLL.Utils;
using DTO.Dominio;
using CTRL.Interface;

public class Registro : Control, IDisposableCTRL
{
	private RegistroDTO RegistroDTO { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	public void PopularNodes()
	{
		RegistroDTO = new RegistroDTO();
	}
	public void DefinirDadosDoRegistro(RegistroDTO registro)
	{
		RegistroDTO = registro;
		this.HintTooltip = RegistroDTO.Nome;
	}
	public void FecharCTRL()
	{
		RegistroDTO.Dispose();
		this.QueueFree();
	}
	private void _on_TextureButton_button_up()
	{
		// Replace with function body.
	}
}
