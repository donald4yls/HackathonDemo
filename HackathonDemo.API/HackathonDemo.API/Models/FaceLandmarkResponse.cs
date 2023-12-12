namespace HackathonDemo.API.Models
{
    public class FaceLandmarkResponse
    {
        public bool IsDrowsy { get; set; }

        public string LandmarkedImageName { get; set; }

        public FaceLandmarkResponse()
        {
            
        }

        public FaceLandmarkResponse(bool isDrowsy, string landmarkedImageName)
        {
            this.IsDrowsy = isDrowsy;
            this.LandmarkedImageName = landmarkedImageName;            
        }
    }
}
