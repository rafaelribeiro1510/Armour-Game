
using Unity.Plastic.Newtonsoft.Json;

namespace Body.BodyType
{
    public class BodyInputInfo
    {
        [JsonProperty("EmotionPhysical")]
        public string EmotionPhysical;
        [JsonProperty("EmotionDisease")]
        public string EmotionDisease;
        [JsonProperty("SizePhysical")]
        public int SizePhysical;
        [JsonProperty("SizeDisease")]
        public int SizeDisease;

        public BodyInputInfo(int sizePhysical, int sizeDisease, string emotionPhysical, string emotionDisease)
        {
            SizePhysical = sizePhysical;
            SizeDisease = sizeDisease;
            EmotionPhysical = emotionPhysical;
            EmotionDisease = emotionDisease;
        }
    }
}
