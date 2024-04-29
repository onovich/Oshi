using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePathDomain {

        public static PathModel Spawn(GameBusinessContext ctx,
                                       int typeID,
                                       int index,
                                       float speed,
                                       bool isCircleLoop,
                                       bool isPingPongLoop,
                                       Vector2Int[] nodes,
                                       EntityType travelerType,
                                       int travelerIndex) {
            var path = GameFactory.Path_Spawn(ctx.templateInfraContext,
                                              typeID,
                                              index,
                                              speed,
                                              isCircleLoop,
                                              isPingPongLoop,
                                              nodes,
                                              travelerType,
                                              travelerIndex);

            ctx.pathRepo.Add(path);
            return path;
        }

    }

}