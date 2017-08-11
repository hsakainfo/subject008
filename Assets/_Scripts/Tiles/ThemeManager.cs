using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThemeManager : MonoBehaviour {
    public enum TileType {
        floor,
        wall_bottom,
        wall_top,
        wall_left,
        wall_right,
        wall_top_right_inner,
        wall_bottom_right_inner,
        wall_top_left_inner,
        wall_bottom_left_inner,
        wall_top_right_outer,
        wall_bottom_right_outer,
        wall_top_left_outer,
        wall_bottom_left_outer,
        wall,
        wall_puddle
    }

    public enum Theme {
        dummy,
        beach
    }

    public float chanceForSpecialTile = .05f;

    private Dictionary<string, Dictionary<TileType, List<Sprite>>> themes =
        new Dictionary<string, Dictionary<TileType, List<Sprite>>>();

    private string[] themeNames = {"beach", "ice", "volcano", "forest"};

    private void InitializeThemes() {
        Random.InitState((int) Time.time);

        foreach (var themeName in themeNames) {
            var sprites = new Dictionary<TileType, List<Sprite>>();

            foreach (var tileType in Enum.GetValues(typeof(TileType)).Cast<TileType>()) {
                var spriteVariations = new List<Sprite>();

                var spriteName = "Tiles/" + themeName + "/" + tileType;
                var sprite = Resources.Load<Sprite>(spriteName);

                for (var i = 1; sprite != null; i++) {
                    spriteVariations.Add(sprite);

                    spriteName = "Tiles/" + themeName + "/" + tileType + "_0" + i;
                    sprite = Resources.Load<Sprite>(spriteName);
                }

                sprites.Add(tileType, spriteVariations);
            }

            themes[themeName] = sprites;
        }
    }

    private void Reset() {
        InitializeThemes();
    }

    private void Awake() {
        InitializeThemes();
    }

    public Sprite getSprite(string theme, TileType tileType) {
        if (theme == "")
            theme = "dummy";

        List<Sprite> sprites;
        themes[theme].TryGetValue(tileType, out sprites);

        if (sprites == null || sprites.Count == 0)
            return null;

        if (tileType == TileType.wall_puddle)
            return sprites[Random.Range(0, sprites.Count)];

        if (sprites.Count < 2 || Random.value > chanceForSpecialTile)
            return sprites[0];

        var numberOfSpecialTiles = sprites.Count - 1;

        var specialTileIndex = Random.Range(1, numberOfSpecialTiles + 1);

        return sprites[specialTileIndex];
    }
}
