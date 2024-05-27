using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemInboxItem : MonoBehaviour
{
    public async void Redeem(InboxItemContainer item)
    {
        await UserLevelComponent.AddXP(item.Item.XPAmount);
    }
}
