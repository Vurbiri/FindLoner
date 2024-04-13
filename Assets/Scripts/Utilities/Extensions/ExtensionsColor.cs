using UnityEngine;


public static class ExtensionsColor
{
    public static void Randomize(this ref Color self, float saturationMin = 0.15f, float brightnessMin = 0.15f)
    {
        float saturation, brightness;
        do
        {
            self.r = Random.value;
            self.g = Random.value;
            self.b = Random.value;

            brightness = self.maxColorComponent;
            saturation = brightness == 0 ? 0 : (1f - Mathf.Min(self.r, self.g, self.b) / brightness);
        }
        while (saturation <= saturationMin || brightness <= brightnessMin);

        self.a = 1f;
    }

    public static bool IsSimilar(this Color self, Color other, float variance = 0.025f)
    {
        return (Mathf.Abs(self.g - other.g) < variance && Mathf.Abs(self.b - other.b) < variance) ||
               (Mathf.Abs(self.r - other.r) < variance && (Mathf.Abs(self.g - other.g) < variance || Mathf.Abs(self.b - other.b) < variance));
    }

    //public static float Saturation(this Color self)
    //{
    //    float max = self.maxColorComponent;
    //    if (max == 0) return 0;

    //    float min = Mathf.Min(self.r, self.g, self.b);
    //    return 1f - (min / max);
    //}


    //public static void SetBrightness(this ref Color self, float brightness)
    //{
    //    self.r *= brightness;
    //    self.g *= brightness;
    //    self.b *= brightness;
    //}

    //public static Color RandomRecolor(this Color self, Vector2 rangeR, Vector2 rangeG, Vector2 rangeB)
    //{
    //    self.r *= GetMultiplier(rangeR);
    //    self.g *= GetMultiplier(rangeG);
    //    self.b *= GetMultiplier(rangeB);

    //    return self;

    //    #region Locla Function
    //    static float GetMultiplier(Vector2 range)
    //    {
    //        float spread = Mathf.Abs(range.y - range.x) / 10f;
    //        float value;
    //        do
    //        {
    //            value = range.RandomRange();
    //        }
    //        while (value > 1f - spread && value < 1f + spread);

    //        return value;
    //    }
    //    #endregion
    //}
}
