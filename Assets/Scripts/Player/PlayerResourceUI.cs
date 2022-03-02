using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceUI : MonoBehaviour
{
    public ResourceTypes resourceType;

    private Player player;
    private Text text;

    public void Construct(Player player)
    {
        this.player = player;
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = LargeNumberFormatter.FormatNumber(player.GetNumOfResource(resourceType));
    }
}
