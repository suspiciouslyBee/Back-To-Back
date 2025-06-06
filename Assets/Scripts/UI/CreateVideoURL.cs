using UnityEngine;
using UnityEngine.Video;

public class CreateVideoURL : MonoBehaviour
{
    [SerializeField] string videoFileName;
    void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            // Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    
}
