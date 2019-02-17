using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;

namespace SpotifyHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            var spotify = GetSpotifyApi();

            spotify.OnTrackChange += spotify_OnTrackChange;

            IntervalWait(1000);
        }

        private static SpotifyLocalAPI GetSpotifyApi()
        {
            var spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                Console.WriteLine("Launching Spotify...");
                SpotifyLocalAPI.RunSpotify();
                while (!SpotifyLocalAPI.IsSpotifyRunning())
                {
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine("Connnecting to Spotify...");

            spotify.Connect();
            spotify.ListenForEvents = true;
            Console.WriteLine("Connected to Spotify!");

            return spotify;
        }

        private static void spotify_OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            Track currentTrack = e.NewTrack;
            log(currentTrack);
        }

        private static void log(Track track)
        {
            var trackName = track.TrackResource.Name;
            Console.WriteLine("Logging track - " + trackName);

            bool appendToFile = true;
            ;
            using (var writer = new StreamWriter("spotify_history.bin", appendToFile))
            {
                var trackString = JsonConvert.SerializeObject(track);
                writer.WriteLine(DateTime.Now + "," + trackString);
            }
        }

        private static void IntervalWait(int milliseconds)
        {
            while (true)
            {
                Thread.Sleep(milliseconds);
            }
        }
    }
}
