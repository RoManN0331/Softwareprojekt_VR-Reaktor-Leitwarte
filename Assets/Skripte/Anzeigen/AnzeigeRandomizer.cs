using UnityEngine;
using System.Collections;


//ziehe dieses skript auf das GameObject, das AnzeigeSteuerung.cs als Komponente hat
//dadurch wird die Anzeige zufällig verändert
public class AnzeigeRandomizer : MonoBehaviour
{
    private AnzeigeSteuerung anzeigeSteuerung;

    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if (anzeigeSteuerung != null)
        {
            anzeigeSteuerung.percentage = Random.Range(40, 70);
            anzeigeSteuerung.percentage2 = Random.Range(anzeigeSteuerung.percentage, 90);
            anzeigeSteuerung.percentage3 = 100;
            anzeigeSteuerung.CHANGEpercentage = Random.Range(10, 70);
            anzeigeSteuerung.end_Number = Random.Range(1, 11) * 1000; // Set end_Number to a random value between 1000 and 10000 in increments of 1000
            StartCoroutine(ChangeValuesOverTime());
        }
    }

    private IEnumerator ChangeValuesOverTime()
    {
        while (true)
        {
            float startValue = anzeigeSteuerung.CHANGEpercentage;
            float endValue = Random.Range(0, anzeigeSteuerung.percentage2 + 5);
            float duration = Random.Range(5, 70);
            float elapsedTime = 0f;

            // Randomize the text with two random letters
            anzeigeSteuerung.komponente = GetRandomLetters(2);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                anzeigeSteuerung.CHANGEpercentage = Mathf.Lerp(startValue, endValue, elapsedTime / duration); // Use Mathf.SmoothStep for quadratic interpolation
                yield return null;
            }

            anzeigeSteuerung.CHANGEpercentage = endValue;
            yield return new WaitForSeconds(Random.Range(0, 20));
        }
    }

    private string GetRandomLetters(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[Random.Range(0, chars.Length)];
        }
        return new string(stringChars);
    }
}