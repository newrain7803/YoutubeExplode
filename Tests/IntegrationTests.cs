﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeExplode.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public async Task YoutubeClient_CheckVideoExistsAsync_Existing_Test()
        {
            var client = new YoutubeClient();
            bool exists = await client.CheckVideoExistsAsync("Te_dGvF6CcE");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task YoutubeClient_CheckVideoExistsAsync_NonExisting_Test()
        {
            var client = new YoutubeClient();
            bool exists = await client.CheckVideoExistsAsync("qld9w0b-1ao");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task YoutubeClient_GetVideoInfoAsync_NonExisting_Test()
        {
            var client = new YoutubeClient();
            await Assert.ThrowsExceptionAsync<VideoNotAvailableException>(() => client.GetVideoInfoAsync("qld9w0b-1ao"));
        }

        [TestMethod]
        public async Task YoutubeClient_GetVideoInfoAsync_Normal_Test()
        {
            // Most common video type

            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("_QdPW8JrYzQ");

            Assert.That.IsSet(videoInfo);
            Assert.AreEqual("_QdPW8JrYzQ", videoInfo.Id);
        }

        [TestMethod]
        public async Task YoutubeClient_GetVideoInfoAsync_Signed_Test()
        {
            // Video that uses signature cipher

            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("9bZkp7q19f0");

            Assert.That.IsSet(videoInfo);
            Assert.AreEqual("9bZkp7q19f0", videoInfo.Id);
        }

        [TestMethod]
        public async Task YoutubeClient_GetVideoInfoAsync_SignedRestricted_Test()
        {
            // Video that uses signature cipher and is also age-restricted

            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("SkRSXFQerZs");

            Assert.That.IsSet(videoInfo);
            Assert.AreEqual("SkRSXFQerZs", videoInfo.Id);
        }

        [TestMethod]
        public async Task YoutubeClient_GetVideoInfoAsync_CannotEmbed_Test()
        {
            // Video that cannot be embedded outside of Youtube

            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("_kmeFXjjGfk");

            Assert.That.IsSet(videoInfo);
            Assert.AreEqual("_kmeFXjjGfk", videoInfo.Id);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_Normal_Test()
        {
            // Playlist created by a user

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("PLI5YfMzCfRtZ8eV576YoY3vIYrHjyVm_e");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("PLI5YfMzCfRtZ8eV576YoY3vIYrHjyVm_e", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.UserMade, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_Large_Test()
        {
            // Playlist created by a user with a lot of videos in it

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("PLWwAypAcFRgKFlxtLbn_u14zddtDJj3mk");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("PLWwAypAcFRgKFlxtLbn_u14zddtDJj3mk", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.UserMade, playlistInfo.Type);
            Assert.IsTrue(1000 <= playlistInfo.VideoIds.Count);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_LargeTruncated_Test()
        {
            // Playlist created by a user with a lot of videos in it

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("PLWwAypAcFRgKFlxtLbn_u14zddtDJj3mk", 2);

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("PLWwAypAcFRgKFlxtLbn_u14zddtDJj3mk", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.UserMade, playlistInfo.Type);
            Assert.IsTrue(1000 >= playlistInfo.VideoIds.Count);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_VideoMix_Test()
        {
            // Playlist generated by Youtube to group similar videos

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("RD1hu8-y6fKg0");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("RD1hu8-y6fKg0", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.VideoMix, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_ChannelMix_Test()
        {
            // Playlist generated by Youtube to group uploads by same channel based on video

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("ULl6WWX-BgIiE");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("ULl6WWX-BgIiE", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.ChannelMix, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_ChannelUploads_Test()
        {
            // Playlist generated by Youtube to group uploads by same channel

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("UUTMt7iMWa7jy0fNXIktwyLA");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("UUTMt7iMWa7jy0fNXIktwyLA", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.ChannelUploads, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_ChannelPopular_Test()
        {
            // Playlist generated by Youtube to group popular uploads by same channel

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("PUTMt7iMWa7jy0fNXIktwyLA");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("PUTMt7iMWa7jy0fNXIktwyLA", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.ChannelPopular, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_Liked_Test()
        {
            // System playlist for videos liked by a user

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("LLEnBXANsKmyj2r9xVyKoDiQ");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("LLEnBXANsKmyj2r9xVyKoDiQ", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.Liked, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetPlaylistInfoAsync_Favorites_Test()
        {
            // System playlist for videos favorited by a user

            var client = new YoutubeClient();
            var playlistInfo = await client.GetPlaylistInfoAsync("FLEnBXANsKmyj2r9xVyKoDiQ");

            Assert.That.IsSet(playlistInfo);
            Assert.AreEqual("FLEnBXANsKmyj2r9xVyKoDiQ", playlistInfo.Id);
            Assert.AreEqual(PlaylistType.Favorites, playlistInfo.Type);
        }

        [TestMethod]
        public async Task YoutubeClient_GetChannelUploadsAsync_Test()
        {
            var client = new YoutubeClient();
            var videoIds = await client.GetChannelUploadsAsync("UC2pmfLm7iq6Ov1UwYrWYkZA");

            Assert.IsNotNull(videoIds);
            Assert.IsTrue(videoIds.Any());
        }

        [TestMethod]
        public async Task YoutubeClient_SearchAsync_Test()
        {
            var client = new YoutubeClient();
            var videoIds = await client.SearchAsync("funny cat videos");

            Assert.IsNotNull(videoIds);
            Assert.IsTrue(videoIds.Any());
        }

        [TestMethod]
        public async Task YoutubeClient_GetMediaStreamAsync_Normal_Test()
        {
            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("_QdPW8JrYzQ");

            var streams = new List<MediaStreamInfo>();
            streams.AddRange(videoInfo.MixedStreams);
            streams.AddRange(videoInfo.AudioStreams);
            streams.AddRange(videoInfo.VideoStreams);

            foreach (var streamInfo in streams)
            {
                using (var stream = await client.GetMediaStreamAsync(streamInfo))
                {
                    Assert.That.IsSet(stream);

                    var buffer = new byte[100];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
        }

        [TestMethod]
        public async Task YoutubeClient_GetMediaStreamAsync_Signed_Test()
        {
            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("9bZkp7q19f0");

            var streams = new List<MediaStreamInfo>();
            streams.AddRange(videoInfo.MixedStreams);
            streams.AddRange(videoInfo.AudioStreams);
            streams.AddRange(videoInfo.VideoStreams);

            foreach (var streamInfo in streams)
            {
                using (var stream = await client.GetMediaStreamAsync(streamInfo))
                {
                    Assert.That.IsSet(stream);

                    var buffer = new byte[100];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
        }

        [TestMethod]
        public async Task YoutubeClient_GetMediaStreamAsync_SignedRestricted_Test()
        {
            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("SkRSXFQerZs");

            var streams = new List<MediaStreamInfo>();
            streams.AddRange(videoInfo.MixedStreams);
            streams.AddRange(videoInfo.AudioStreams);
            streams.AddRange(videoInfo.VideoStreams);

            foreach (var streamInfo in streams)
            {
                using (var stream = await client.GetMediaStreamAsync(streamInfo))
                {
                    Assert.That.IsSet(stream);

                    var buffer = new byte[100];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
        }

        [TestMethod]
        public async Task YoutubeClient_GetMediaStreamAsync_CannotEmbed_Test()
        {
            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("_kmeFXjjGfk");

            var streams = new List<MediaStreamInfo>();
            streams.AddRange(videoInfo.MixedStreams);
            streams.AddRange(videoInfo.AudioStreams);
            streams.AddRange(videoInfo.VideoStreams);

            foreach (var streamInfo in streams)
            {
                using (var stream = await client.GetMediaStreamAsync(streamInfo))
                {
                    Assert.That.IsSet(stream);

                    var buffer = new byte[100];
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
        }

        [TestMethod]
        public async Task YoutubeClient_GetClosedCaptionTrackAsync_Normal_Test()
        {
            var client = new YoutubeClient();
            var videoInfo = await client.GetVideoInfoAsync("_QdPW8JrYzQ");

            var trackInfo = videoInfo.ClosedCaptionTracks.First();
            var track = await client.GetClosedCaptionTrackAsync(trackInfo);

            Assert.That.IsSet(track);
        }
    }
}