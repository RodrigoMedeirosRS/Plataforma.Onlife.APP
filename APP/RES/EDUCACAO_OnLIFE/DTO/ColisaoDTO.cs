using System;
using Godot;

namespace DTO
{
	public class ColisaoDTO
	{
		public ColisaoDTO()
		{
			Posicao = new Vector3(0,0,0);
		}
		public Vector3 Posicao { get; set; }
		public Spatial Ponto { get; set; }
	}
}
