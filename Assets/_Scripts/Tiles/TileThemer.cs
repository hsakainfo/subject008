using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileThemer : MonoBehaviour {
    public ThemeManager.TileType tileType = ThemeManager.TileType.floor;

    private ThemeManager themeManager;
    private string theme;

    private bool initialized = false;

    public void SetThemedSprite(string theme) {
        themeManager = GameObject.FindGameObjectWithTag("ThemeManager").GetComponent<ThemeManager>();
        //Bitte an den EventManager anhängen und nicht durch ein weiteres Component lösen
        this.theme = theme;

        SetSprite();
    }

    private void SetSprite() {
        var sprite = themeManager.getSprite(theme, tileType);
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
