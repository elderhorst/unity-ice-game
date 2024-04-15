using UnityEngine;

namespace IceGame
{
	public class Ease
	{
		public static float In(float t)
		{
			return t * t;
		}
		
		public static float Out(float t)
		{
			return (1 - ((1 - t) * (1 - t)));
		}
	}
}