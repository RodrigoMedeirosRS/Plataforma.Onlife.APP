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
	private TextureButton Button { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	public void PopularNodes()
	{
		RegistroDTO = new RegistroDTO();
		Button = GetNode<TextureButton>("./TextureButton");
	}
	public void DefinirDadosDoRegistro(RegistroDTO registro)
	{
		RegistroDTO = registro;
		Button.HintTooltip = RegistroDTO.Nome;
	}
	public void FecharCTRL()
	{
		RegistroDTO.Dispose();
		Button.QueueFree();
		this.QueueFree();
	}
	private void _on_TextureButton_button_up()
	{
		Main.InstanciarPrimeiroRegisro(RegistroDTO);
	}
}
