public class Deck
{
	// TODO
	public static void Shuffle(float[] deck)
	{
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
