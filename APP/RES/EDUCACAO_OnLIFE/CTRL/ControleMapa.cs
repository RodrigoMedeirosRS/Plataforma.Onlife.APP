using Godot;
using System;

using DTO;
public class ControleMapa : Spatial
{
	private Vector2 Direcao { get; set; }
	private Spatial Globo { get; set; }
	private Sprite Rot { get; set; }
	private Camera Cam { get; set; }
	private VSlider Zoom { get; set; }
	private TextureButton Up { get; set; }
	private TextureButton Down { get; set; }
	private TextureButton Left { get; set; }
	private TextureButton Right { get; set; }

	public override void _Ready()
	{
		PopularNodes();
		DefinirValoresIniciais();
	}
	private void DefinirValoresIniciais()
	{
		Cam.Size = Convert.ToSingle(Zoom.Value);
	}
	private void PopularNodes()
	{
		Rot = GetNode<Sprite>("./Botoes/Sprites/Rotation");
		Globo = GetNode<Spatial>("./Globo");
		Cam = GetNode<Camera>("./Camera");
		Zoom = GetNode<VSlider>("./Botoes/Zoom");
		Up = GetNode<TextureButton>("./Botoes/Up");
		Down = GetNode<TextureButton>("./Botoes/Down");
		Left = GetNode<TextureButton>("./Botoes/Left");
		Right = GetNode<TextureButton>("./Botoes/Right");
	}
	public override void _PhysicsProcess(float delta)
	{
		RotacionarGlobo(delta);
		if (Input.IsActionPressed("clique"))
		{
			var localizacao = CapturarObjectoComClique();
			if (localizacao != null)
				Main.DispararLocalidade(localizacao.Posicao);
		}
	}

	private void RotacionarGlobo(float delta)
	{
		if (Up.Pressed)
			Globo.RotateX(0.5f * delta);
		if (Down.Pressed)
			Globo.RotateX(-0.5f * delta);
		if (Left.Pressed)
			Globo.RotateY(0.5f * delta);
		if (Right.Pressed)
			Globo.RotateY(-0.5f * delta);
	}

	private void _on_Rotacao_value_changed(float value)
	{
		Cam.RotationDegrees = new Vector3(0,0,value);
		Rot.RotationDegrees = value;
	}
	private void _on_Zoom_value_changed(float value)
	{
		Cam.Size = value;
	}
	private ColisaoDTO CapturarObjectoComClique()
	{
		var spacestate = GetWorld().DirectSpaceState;
		var mousePosition = GetViewport().GetMousePosition();
		var rayOrigin = Cam.ProjectRayOrigin(mousePosition);
		var rayEnd = rayOrigin + Cam.ProjectRayNormal(mousePosition) * 2000;
		var intersecao = spacestate.IntersectRay(rayOrigin, rayEnd);

		if (intersecao.Count > 0)
		{
			return new ColisaoDTO()
			{
				Posicao = (Vector3)intersecao["position"],
				Ponto = (Spatial)intersecao["collider"]
			};
		}
		return null;
	}
}
