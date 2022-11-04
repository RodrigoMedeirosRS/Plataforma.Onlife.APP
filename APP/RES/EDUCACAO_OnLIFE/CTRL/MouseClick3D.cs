using Godot;
using System;

using DTO;

public class MouseClick3D : Spatial
{	
	private Camera Cam { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	private void PopularNodes()
	{
		Cam = GetNode<Camera>("./Camera");
	}
	private void MoveMouse()
	{

	}
	public override void _PhysicsProcess(float delta)
	{
		CapturarObjectoComClique();
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
