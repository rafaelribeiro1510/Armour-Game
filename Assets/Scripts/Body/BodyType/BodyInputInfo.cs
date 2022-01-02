
namespace Body.BodyType
{
    public class BodyInputInfo
    {
        public string EmotionPhysical;
        public string EmotionDisease;
        public int SizePhysical;
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
