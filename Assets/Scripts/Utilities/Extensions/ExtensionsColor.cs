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

    public static bool IsSimilar(this Color self, Color other, float variance = 0.025f) => Mathf.Abs(self.r - other.r) < variance && Mathf.Abs(self.g - other.g) < variance && Mathf.Abs(self.b - other.b) < variance;

    public static Color SetAlpha(this ref Color self, float alpha)
    {
        self.a = alpha;
        return self;
    }
}
