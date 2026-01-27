-damage-popup-component-type =
    { $setting ->
        [combined] Combined
        [total] Total
        [delta] Delta
        [hit] Hit
       *[other] Unknown
    }

damage-popup-component-switched = Target set to type: { $setting ->
        [combined] { -damage-popup-component-type(setting: "combined") }
        [total] { -damage-popup-component-type(setting: "total") }
        [delta] { -damage-popup-component-type(setting: "delta") }
        [hit] { -damage-popup-component-type(setting: "hit") }
       *[other] { -damage-popup-component-type(setting: "other") }
    }
