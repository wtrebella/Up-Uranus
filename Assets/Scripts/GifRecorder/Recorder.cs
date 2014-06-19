using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace GifRecorder
{
    public class GifImageInfo {
        public Color[] pixels;
        public int width;
        public int height;

        public GifImageInfo(int w, int h) {
            width = w;
            height = h;
        }

        public void ScaleDownTo(int nw, int nh) {
            var newPixels = new Color[nw * nh];
            var xOff = (float)width / (float)nw;
            var yOff = (float)height / (float)nh;
            Debug.Log ("orig size: " + width + "," + height + " - new size: " + nw + "," + nh);
            Debug.Log ("Scaling modifiers: " + xOff + "," + yOff);
            for(var y = 0; y < nh; y++) {
                var origY = Mathf.FloorToInt(y * yOff);
                for(var x = 0; x < nw; x++) {
                    var origX = Mathf.FloorToInt (x * xOff);
                    newPixels[y * nw + x] = pixels[origX + origY * width];
                }
            }
            width = nw;
            height = nh;
            pixels = newPixels;
        }
    }

    public class Recorder : MonoBehaviour
    {
        public static bool IsRecording = false;

        /// <summary>
        /// This is the main method to interact with, everything else should be considered internals
        /// </summary>
        /// <param name="width">The final width of the gif.</param>
        /// <param name="height">The final height of the gif.</param>
        /// <param name="duration">The duration you want to record for in seconds.</param>
        /// <param name="fps">The frames per second you want to capture.</param>
        /// <param name="filename">The filename to save.</param>
        public static void Record (int width, int height, float duration, int fps, string filename)
        {
            if(IsRecording) {
                return;
            }
            var inst = GetInstance ();
            inst.StartRecording (width, height, duration, fps, filename);
        }

        static Recorder GetInstance ()
        {
            var inst = GameObject.FindObjectOfType<Recorder> ();
            if (inst != null) {
                return inst;
            }
            var go = new GameObject ("_recorder");
            inst = go.AddComponent<Recorder> ();
            return inst;
        }

        void StartRecording (int width, int height, float duration, int fps, string filename)
        {
            StartCoroutine (RecordCoroutine (width, height, duration, fps, filename));
        }

        IEnumerator RecordCoroutine (int width, int height, float duration, int fps, string filename)
        {
            IsRecording = true;
            Debug.Log ("Capturing gif");
            var textures = new List<Texture2D> ();
            var images = new List<GifImageInfo>();
            int numFrames = (int)(duration * fps);
            float delay = 1.0f / fps;
            var screenRect = new Rect (0, 0, Screen.width, Screen.height);
            for (var i = 0; i < numFrames; i++) {
                yield return new WaitForEndOfFrame();
                var useBilinear = Screen.width != width || Screen.height != height;
                var tex = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, useBilinear);
                tex.ReadPixels (screenRect, 0, 0);
                textures.Add (tex);
                yield return new WaitForSeconds (delay);
            }

            Debug.Log ("Total frames captured: " + textures.Count);

            Debug.Log ("Converting frames to info blocks");
            for(var i = 0; i < textures.Count; i++) {
                var toConvert = textures[i];
                var image = new GifImageInfo(width, height);
                yield return StartCoroutine (ConvertTexture (toConvert, image, width, height));
                images.Add (image);
            }
            Debug.Log ("Done converting textures to info blocks");
            foreach(var tex in textures) {
                Destroy (tex);
            }
            var thread = new Thread(() => {
                WriteGif (images, fps, filename);
            });
            thread.Start ();
        }

        IEnumerator ConvertTexture(Texture2D toConvert, GifImageInfo image, int width, int height) {
            var line = 0;
            var useBilinear = toConvert.width != width || toConvert.height != height;
            var blockHeight = useBilinear ? 20 : 80;
            image.pixels = new Color[width * height];

            while (line < image.height) {
                if(line + blockHeight >= image.height) {
                    blockHeight = image.height - line;
                }
                if(useBilinear) {
                    var uInc = 1.0f / image.width;
                    var vInc = 1.0f / image.height;
                    ReadBilinearPixels(image, toConvert, line, blockHeight, uInc, vInc);
                }
                else {
                    ReadFullPixels (image, toConvert, line, blockHeight);
                }
                line += blockHeight;
                yield return null;
            }
        }

        void ReadBilinearPixels(GifImageInfo image, Texture2D toConvert, int line, int blockHeight, float uInc, float vInc) {
            for(var y = line; y < line + blockHeight; y++) {
                var v = vInc * y;
                for(var x = 0; x < image.width; x++) {
                    var u = uInc * x;
                    image.pixels[x + y * image.width] = toConvert.GetPixelBilinear(u, v);
                }
            }
        }

        void ReadFullPixels (GifImageInfo image, Texture2D toConvert, int line, int blockHeight) {
            var values = toConvert.GetPixels (0, line, image.width, blockHeight);
            System.Array.Copy (values, 0, image.pixels, image.width * line, values.Length);
        }

        void WriteGif(List<GifImageInfo> textures, int fps, string filename) {
            Debug.Log ("Encoding " + textures.Count + " frames");
            var enc = new AnimatedGifEncoder();
            enc.start (filename);
            enc.setDelay(1000 / fps);
            enc.setRepeat(0);
            for(var i = 0; i < textures.Count; i++) {
                var frame = textures[i];
                enc.addFrame(frame);
                Thread.Sleep (1);
            }
            enc.finish();
            Debug.Log ("Done encoding gif");
            IsRecording = false;
        }
    }
}