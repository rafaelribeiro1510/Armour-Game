namespace Body.BodyType
{
    public class BodyInputInfo
    {
        public string EmotionPhysical;
        public string EmotionDisease;
        public int SizePhysical;
        public int SizeDisease;

        public BodyInputInfo(string emotionPhysical, string emotionDisease, int sizePhysical, int sizeDisease)
        {
            EmotionPhysical = emotionPhysical;
            EmotionDisease = emotionDisease;
            SizePhysical = sizePhysical;
            SizeDisease = sizeDisease;
        }
    }
}
