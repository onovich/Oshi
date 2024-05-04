using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Oshi {

    public static class SoundApp {

        public static async Task LoadAssets(SoundAppContext ctx) {

            var handle = Addressables.LoadAssetAsync<GameObject>("Sound_AudioSource");
            var prefab = await handle.Task;
            ctx.audioSourcePrefab = prefab.GetComponent<AudioSource>();
            ctx.bgmPlayer = GameObject.Instantiate(ctx.audioSourcePrefab);
            ctx.assetsHandle = handle;

            for (int i = 0; i < ctx.roleGenericPlayer.Length; i++) {
                if (ctx.audioSourcePrefab == null) {
                    continue;
                }
                ctx.roleGenericPlayer[i] = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
            }

            for (int i = 0; i < ctx.blockGenericPlayer.Length; i++) {
                if (ctx.audioSourcePrefab == null) {
                    continue;
                }
                ctx.blockGenericPlayer[i] = GameObject.Instantiate(ctx.audioSourcePrefab, ctx.root);
            }

        }

        public static void ReleaseAssets(SoundAppContext ctx) {
            Addressables.Release(ctx.assetsHandle);
        }

        public static void TearDown(SoundAppContext ctx) {
            BGM_Stop(ctx);
            GameObject.Destroy(ctx.bgmPlayer.gameObject);
            foreach (var player in ctx.roleGenericPlayer) {
                player.Stop();
                GameObject.Destroy(player.gameObject);
            }
            foreach (var player in ctx.blockGenericPlayer) {
                player.Stop();
                GameObject.Destroy(player.gameObject);
            }
        }

        public static void BGM_PlayLoop(SoundAppContext ctx, AudioClip[] clips, float volume) {
            if (ctx.bgmPlayer.clip == null || !ctx.bgmPlayer.isPlaying) {
                ctx.bgmPlayerIndex += 1;
                ctx.bgmPlayerIndex %= clips.Length;
                ctx.bgmPlayer.clip = clips[ctx.bgmPlayerIndex];
                ctx.bgmPlayer.Play();
                ctx.bgmPlayer.loop = true;
            }
            ctx.bgmPlayer.volume = volume;
        }

        public static void BGM_Play(SoundAppContext ctx, AudioClip[] clips, float volume) {
            if (ctx.bgmPlayer.clip == null) {
                Random.InitState(System.DateTime.Now.Millisecond);
                ctx.bgmPlayerIndex = Random.Range(0, clips.Length);
            }
            ctx.bgmPlayer.clip = clips[ctx.bgmPlayerIndex];
            ctx.bgmPlayer.Play();
            ctx.bgmPlayer.volume = volume;
        }

        public static void BGM_Stop(SoundAppContext ctx) {
            ctx.bgmPlayer.Stop();
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
            ctx.bgmPlayer.mute = isMute;
            foreach (var player in ctx.roleGenericPlayer) {
                player.mute = isMute;
            }
            foreach (var player in ctx.blockGenericPlayer) {
                player.mute = isMute;
            }
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