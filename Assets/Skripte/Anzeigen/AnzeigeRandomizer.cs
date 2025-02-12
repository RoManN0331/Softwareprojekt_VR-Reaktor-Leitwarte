using UnityEngine;
using System.Collections;


//ziehe dieses skript auf das GameObject, das AnzeigeSteuerung.cs als Komponente hat
//dadurch wird die Anzeige zufällig verändert

/// <summary>
/// This class randomizes the values of the AnzeigeSteuerung.cs and AnzeigeSteuerung5.cs scripts.
/// </summary>
public class AnzeigeRandomizer : MonoBehaviour
{


    /// <param name="anzeigeSteuerung">Reference to a AnzeigeSteuerung component</param>
    private AnzeigeSteuerung anzeigeSteuerung;
    /// <param name="anzeigeSteuerung2">Reference to a AnzeigeSteuerung5 component</param>
    private AnzeigeSteuerung5 anzeigeSteuerung2;

    /// <summary>
    /// This method initialises anzeigeSteuerung and anzeigeSteuerung2.
    /// </summary>
    void Start()
    {
        anzeigeSteuerung = GetComponent<AnzeigeSteuerung>();
        if(anzeigeSteuerung == null)
        {
            anzeigeSteuerung2 = GetComponent<AnzeigeSteuerung5>();

            if (anzeigeSteuerung2 != null)
            {
                anzeigeSteuerung2.percentage = Random.Range(40, 70);
                anzeigeSteuerung2.percentage2 = Random.Range(anzeigeSteuerung2.percentage, 90);
                anzeigeSteuerung2.percentage3 = 100;
                anzeigeSteuerung2.CHANGEpercentage = Random.Range(10, 70);
                anzeigeSteuerung2.end_Number = Random.Range(1, 11) * 1000; // Set end_Number to a random value between 1000 and 10000 in increments of 1000
                StartCoroutine(ChangeValuesOverTime2());
            }
        }
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

    /// <summary>
    /// This method sends random input to a display by assigning a random label and interpolating a random change.
    /// </summary>
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

    /// <summary>
    /// This method sends random input to a display by assigning a random label and interpolating a random change.
    /// </summary>
    private IEnumerator ChangeValuesOverTime2()
    {
        while (true)
        {
            float startValue = anzeigeSteuerung2.CHANGEpercentage;
            float endValue = Random.Range(0, anzeigeSteuerung2.percentage2 + 5);
            float duration = Random.Range(5, 70);
            float elapsedTime = 0f;

            // Randomize the text with two random letters
            anzeigeSteuerung2.komponente = GetRandomLetters(2);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                anzeigeSteuerung2.CHANGEpercentage = Mathf.Lerp(startValue, endValue, elapsedTime / duration); // Use Mathf.SmoothStep for quadratic interpolation
                yield return null;
            }

            anzeigeSteuerung2.CHANGEpercentage = endValue;
            yield return new WaitForSeconds(Random.Range(0, 20));
        }
    }

    /// <summary>
    /// This method generates a random string of letters, used as a label for a display in ChangeValuesOverTime() or ChangeValuesOverTime2().
    /// </summary>
    /// <param name="length"> int specifying the length of the random string</param>
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