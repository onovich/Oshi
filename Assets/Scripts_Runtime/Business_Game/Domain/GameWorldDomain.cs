using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public static class GameWorldDomain {

        public static void GenCandidate(GameBusinessContext ctx, WorldEntity worldEntity) {

            var rd = ctx.gameEntity.random;
            var blockTypeID = ctx.templateCoreContext.BlockTM_GetRandom(rd).typeID;
            worldEntity.candidateBlockTypeIDs.Enqueue(blockTypeID);

        }

        public static void GenNextFromCandidate(GameBusinessContext ctx, WorldEntity worldEntity) {

            var rd = ctx.gameEntity.random;
            var blockTypeID = worldEntity.candidateBlockTypeIDs.ToArray()[rd.Next(worldEntity.candidateBlockTypeIDs.Count)];
            worldEntity.nextCandidateBlockTypeIDs.Enqueue(blockTypeID);

        }

        public static void SpawnerBlockFromNext(GameBusinessContext ctx, WorldEntity worldEntity) {

            var blockTypeID = worldEntity.nextCandidateBlockTypeIDs.Dequeue();
            var blockEntity = GameFactory.BlockEntity_Spawn(ctx, blockTypeID, new Vector2Int(0, 0));

        }

    }

}