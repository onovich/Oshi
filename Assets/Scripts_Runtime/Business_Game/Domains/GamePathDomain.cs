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