using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePathDomain {

        public static PathModel Spawn(GameBusinessContext ctx,
                                       int typeID,
                                       int index,
                                       bool isCircleLoop,
                                       bool isPingPongLoop,
                                       Vector2Int[] nodes,
                                       EntityType travelerType,
                                       int travelerIndex) {
            var path = GameFactory.Path_Spawn(ctx.templateInfraContext,
                                              typeID,
                                              index,
                                              isCircleLoop,
                                              isPingPongLoop,
                                              nodes,
                                              travelerType,
                                              travelerIndex);

            ctx.pathRepo.Add(path);
            return path;
        }

        public static void ApplyMoving(GameBusinessContext ctx, PathModel path, float fixdt, out bool isEnd) {
            var allow = GamePathDomain.CheckTravelerMovable(ctx, path);
            if (!allow) {
                isEnd = true;
                return;
            }

            path.Tick_MoveCarToNext(fixdt, out isEnd);
            if (isEnd) {
                path.PushIndexToNext();
            }
        }

        public static bool CheckTravelerMovable(GameBusinessContext ctx,
                                           PathModel path) {
            var allow = true;
            var travelerType = path.travelerType;
            var travelerIndex = path.travelerIndex;

            // Spike
            if (travelerType == EntityType.Spike) {
                var has = ctx.spikeRepo.TryGetSpike(travelerIndex, out var spike);
                if (!has) {
                    throw new Exception($"Block not found: {travelerIndex}");
                }
                var start = path.GetCurrentNode();
                var end = path.GetNextNode();
                var count = GridUtils.GetCoveredStraightGrid(start, end, GridUtils.temp);
                for (int i = 0; i < count; i++) {
                    var point = GridUtils.temp[i];
                    allow &= CheckSpikeMovable(ctx, path, spike, point);
                }
            }
            return allow;
        }

        static bool CheckSpikeMovable(GameBusinessContext ctx,
                                      PathModel path,
                                      SpikeEntity spike,
                                      Vector2Int point) {
            var allow = true;
            allow &= !ctx.spikeRepo.HasDifferent(point, spike.entityIndex);
            allow &= !ctx.blockRepo.Has(point);
            allow &= !ctx.wallRepo.Has(point);
            allow &= !ctx.roleRepo.Has(point);
            return allow;
        }

        public static void ApplyCarryTraveler(GameBusinessContext ctx,
                                         PathModel path) {
            var travelerType = path.travelerType;
            var travelerIndex = path.travelerIndex;
            if (travelerType == EntityType.Spike) {
                var has = ctx.spikeRepo.TryGetSpike(travelerIndex, out var spike);
                if (!has) {
                    throw new Exception($"Block not found: {travelerIndex}");
                }
                CarrySpike(ctx, path, spike);
            }
        }

        static void CarrySpike(GameBusinessContext ctx,
                               PathModel path,
                               SpikeEntity spike) {
            var car = path.pathCarPos;
            var oldPos = spike.PosInt;
            spike.Pos_SetPos(car);
            ctx.spikeRepo.UpdatePos(oldPos, spike);
        }

    }

}