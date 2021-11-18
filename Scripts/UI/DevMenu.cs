using System.Collections;
using System.Collections.Generic;
using Classes.Player;
using UnityEngine;

public sealed class DevMenu : MonoBehaviour
{
    [SerializeField] private Character player;

    public void ChangeForm()
    {
        player.Health =
            player.Health >= 75.0001f
                ? (74.9f)
                : (player.Health >= 50.0001f
                    ? (49.9f)
                    : (player.Health >= 25.0001f
                        ? (24.9f)
                        : (player.MaxHealth)));
    }
}
