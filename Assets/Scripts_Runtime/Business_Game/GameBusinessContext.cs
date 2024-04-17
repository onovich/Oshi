using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class GameBusinessContext {

        // Entity
        public GameEntity gameEntity;
        public PlayerEntity playerEntity;
        public InputEntity inputEntity; // External
        public MapEntity currentMapEntity;

        public RoleRepository roleRepo;
        public BlockRepository blockRepo;
        public SpikeRepository spikeRepo;
        public WallRepository wallRepo;
        public GoalRepository goalRepo;

        // App
        public UIAppContext uiContext;
        public VFXAppContext vfxContext;
        public CameraAppContext cameraContext;

        // Camera
        public Camera mainCamera;

        // Service
        public IDRecordService idRecordService;

        // Infra
        public TemplateInfraContext templateInfraContext;
        public AssetsInfraContext assetsInfraContext;

        // Timer
        public float fixedRestSec;

        // SpawnPoint
        public Vector2 ownerSpawnPoint;

        // TEMP
        public RaycastHit2D[] hitResults;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            playerEntity = new PlayerEntity();
            idRecordService = new IDRecordService();
            roleRepo = new RoleRepository();
            blockRepo = new BlockRepository();
            spikeRepo = new SpikeRepository();
            wallRepo = new WallRepository();
            goalRepo = new GoalRepository();
            hitResults = new RaycastHit2D[100];
        }

        public void Reset() {
            idRecordService.Reset();
            roleRepo.Clear();
            blockRepo.Clear();
            spikeRepo.Clear();
            wallRepo.Clear();
            goalRepo.Clear();
        }

        // Role
        public RoleEntity Role_GetOwner() {
            roleRepo.TryGetRole(playerEntity.ownerRoleEntityID, out var role);
            return role;
        }

        public void Role_ForEach(Action<RoleEntity> onAction) {
            roleRepo.ForEach(onAction);
        }

        public RoleEntity Role_GetNearestEnemy(RoleEntity role) {
            RoleEntity nearest = null;
            var minDistance = float.MaxValue;
            var rolePos = role.Pos;
            roleRepo.ForEach((r) => {
                if (Vector2.Dot(r.faceDir, role.faceDir) >= 0) {
                    return;
                }
                if (r.allyStatus == role.allyStatus.GetOpposite()) {
                    var distance = Vector2.Distance(rolePos, r.Pos);
                    if (distance < minDistance) {
                        minDistance = distance;
                        nearest = r;
                    }
                }
            });
            return nearest;
        }

        // Block
        public void Block_ForEach(Action<BlockEntity> onAction) {
            blockRepo.ForEach(onAction);
        }

    }

}