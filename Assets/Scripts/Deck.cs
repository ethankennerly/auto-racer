using UnityEngine;  // Mathf
using System;  // Random

public class Deck
{
	private static System.Random rng = new System.Random();

	/**
	 * Unity Random includes 1.0, which would be out of range.
	 * Would be more usable with generic data-type.
	 */
	public static void Shuffle(float[] deck)
	{
		for (int index = deck.Length - 1; 1 <= index; index--)
		{
			int r = (int) Mathf.Floor((float) (rng.NextDouble() * (index + 1f)));
			float swap = deck[index];
			deck[index] = deck[r];
			deck[r] = swap;
		}
	}

	private int index = -1;
	private int length = -1;
	private float[] cards;

	public void Setup(float[] originals, int copies)
	{
		int original = originals.Length;
		length = copies * original;
		cards = new float[length];
		for (index = 0; index < length; index++)
		{
			cards[index] = originals[index % original];
		}
		Shuffle(cards);
		index = -1;
	}

	public float NextCard()
	{
		index++;
		if (length <= index) {
			Shuffle(cards);
			index = 0;
		}
		return cards[index];
	}
}
