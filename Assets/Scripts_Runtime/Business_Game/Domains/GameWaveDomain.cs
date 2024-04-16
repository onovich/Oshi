
using UnityEngine;

namespace Chouten {

    public static class GameWaveDomain {

        public static void ApplySpawnWaveEnemies(GameBusinessContext ctx, MapEntity map, float dt) {
            var leftWave = map.leftWaveModel;
            var rightWave = map.rightWaveModel;
            map.IncTimer(dt);
            var time = map.timer;
            SpawnWaveEnemies(ctx, map, leftWave, time, true);
            SpawnWaveEnemies(ctx, map, rightWave, time, false);
        }

        static void SpawnWaveEnemies(GameBusinessContext ctx, MapEntity map, WaveModel wave, float time, bool isLeftSide) {

            var currentIndex = wave.waveIndex;
            if (currentIndex >= wave.spawnTimeArr.Length) {
                return;
            }

            var spawnTime = wave.spawnTimeArr[currentIndex];
            if (time < spawnTime) {
                return;
            }

            var roleTypeID = wave.roleTypeIDArr[currentIndex];
            var pos = isLeftSide ? map.leftBound : map.rightBound;
            var direction = isLeftSide ? Vector2.right : Vector2.left;
            GameRoleDomain.Spawn(ctx, roleTypeID, pos, direction);
            wave.IncWaveIndex();

        }

    }

}