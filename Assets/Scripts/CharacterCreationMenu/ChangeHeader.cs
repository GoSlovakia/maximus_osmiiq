using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ChangeHeader : MonoBehaviour
{
  [SerializeField]
  private LocalizedString header;

  [SerializeField]
  private LocalizeStringEvent localize;

  public void SwapHeader() => localize.StringReference = header;
}
