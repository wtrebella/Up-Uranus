Not much to it, to take a gif do something like:

using GifRecorder;
Recorder.Record(imageWidth, imageHeight, duration, framesPerSecond, filename);

Note that it has to keep each uncompressed frame in memory during generation so
it uses quite a bit of memory to do a long gif or a high frame rate. Right now
I'm using 4 seconds with around 10 frames per second and that seems to be a
reasonable compromise on my machine.

I've made every attempt to keep performance impact minimal, but you'll likely
notice some stutter during frame capture - to minimize this run your game at
a lower resolution when you take gifs.

Finally - note that it takes a while for the gif to process, it will print
"done encoding gif" to the console when it has finished, so keep an eye out
for that.
