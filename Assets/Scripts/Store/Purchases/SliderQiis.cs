using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderQiis : MonoBehaviour
{
  public Slider slide;
  // Start is called before the first frame update
  async void Start()
  {
    if (PlayerPrefs.GetString("UserQiis") != string.Empty)
      slide.value = int.Parse(PlayerPrefs.GetString("UserQiis"));

    CurrencyData temp = await GetCurrency.GetUserBalance();
    Debug.Log("Quiids " + temp.QI);
    slide.value = int.Parse(temp.QI);
  }
}
