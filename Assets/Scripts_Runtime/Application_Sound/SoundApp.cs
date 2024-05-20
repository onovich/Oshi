using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Oshi {

    public static class SoundApp {

        public static async Task LoadAssets(SoundAppContext ctx) {

            var handle = Addressables.LoadAssetAsync<GameObject>("Sound_AudioSource");
            var prefab = await handle.Task;
            ctx.audioSourcePrefab = prefab.GetComponent<AudioSource>();
            ctx.assetsHandle = handle;

            for (int i = 0; i < ctx.bgmPlayer.Length; i++) {
                var audio = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
                audio.name = "BGMPlayer - " + i;
                ctx.bgmPlayer[i] = audio;
            }

            for (int i = 0; i < ctx.roleGenericPlayer.Length; i++) {
                var audio = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
                audio.name = "RoleGenericPlayer - " + i;
                ctx.roleGenericPlayer[i] = audio;
            }

            for (int i = 0; i < ctx.blockGenericPlayer.Length; i++) {
                var audio = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
                audio.name = "BlockGenericPlayer - " + i;
                ctx.blockGenericPlayer[i] = audio;
            }

            var bgsAudio = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
            bgsAudio.name = "BGSPlayer";
            ctx.bgsPlayer = bgsAudio;

        }

        public static void ReleaseAssets(SoundAppContext ctx) {
            if (ctx.assetsHandle.IsValid()) {
                Addressables.Release(ctx.assetsHandle);
            }
        }

        public static void TearDown(SoundAppContext ctx) {
            foreach (var player in ctx.bgmPlayer) {
                player?.Stop();
                if (player != null && player.gameObject != null) {
                    GameObject.Destroy(player.gameObject);
                }
            }
            foreach (var player in ctx.roleGenericPlayer) {
                player?.Stop();
                if (player != null && player.gameObject != null) {
                    GameObject.Destroy(player.gameObject);
                }
            }
            foreach (var player in ctx.blockGenericPlayer) {
                player?.Stop();
                if (player != null && player.gameObject != null) {
                    GameObject.Destroy(player.gameObject);
                }
            }
            ctx.bgsPlayer?.Stop();
            if (ctx.bgsPlayer != null && ctx.bgsPlayer.gameObject != null) {
                GameObject.Destroy(ctx.bgsPlayer.gameObject);
            }
        }

        public static void BGM_PlayLoop(SoundAppContext ctx, AudioClip clip, int layer, float volume, bool replay) {
            var player = ctx.bgmPlayer[layer];
            if (player.isPlaying && !replay) {
                return;
            }

            player.clip = clip;
            player.Play();
            player.loop = true;
            player.volume = volume;
        }

        public static void BGM_Stop(SoundAppContext ctx, int layer) {
            var player = ctx.bgmPlayer[layer];
            player.Stop();
        }

        public static void BGS_PlayLoop(SoundAppContext ctx, AudioClip clip, float volume) {
            var player = ctx.bgsPlayer;
            player.clip = clip;
            player.Play();
            player.loop = true;
            player.volume = volume;
        }

        public static void BGS_Stop(SoundAppContext ctx) {
            var player = ctx.bgsPlayer;
            player.Stop();
        }

        public static void Role_Generic(SoundAppContext ctx, AudioClip clip, float volume) {
            PlayWhenFree(ctx, ctx.roleGenericPlayer, clip, volume);
        }

        public static float GetVolume(Vector2 listenerPos, Vector2 hitPos, float thresholdDistance, float volume) {
            float dis = Vector2.Distance(listenerPos, hitPos);
            if (dis >= thresholdDistance) {
                return 0;
            }
            return (1 - dis / thresholdDistance) * volume;
        }

        public static void SetMuteAll(SoundAppContext ctx, bool isMute) {
            foreach (var player in ctx.bgmPlayer) {
                player.mute = isMute;
            }
            foreach (var player in ctx.roleGenericPlayer) {
                player.mute = isMute;
            }
            foreach (var player in ctx.blockGenericPlayer) {
                player.mute = isMute;
            }
            ctx.bgsPlayer.mute = isMute;
        }

        static void PlayWhenFree(SoundAppContext ctx, AudioSource[] players, AudioClip clip, float volume) {
            if (clip == null || volume <= 0) {
                return;
            }
            foreach (var player in players) {
                if (!player.isPlaying) {
                    player.clip = clip;
                    player.Play();
                    player.volume = volume;
                    return;
                }
            }
        }

    }

}