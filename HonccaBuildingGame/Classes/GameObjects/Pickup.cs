using HonccaBuildingGame.Classes.Pickups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.GameObjects
{
	abstract class Pickup : Animation
	{
		public Pickup(Vector2 _startPosition, Texture2D _texture) : base(_startPosition, _texture)
		{

		}
	}
}
