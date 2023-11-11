namespace Alter {

    public class GameBusinessContext {

        public GameEntity gameEntity;
        public WorldEntity worldEntity;
        public InputEntity inputEntity;
        public PlayerEntity playerEntity;

        public BlockRepository blockRepository;

        public UIAppContext uiAppContext;
        public AssetsCoreContext assetsCoreContext;
        public TemplateCoreContext templateCoreContext;

        public int idrecord_block;

        public GameBusinessContext() {
            gameEntity = new GameEntity();
            worldEntity = new WorldEntity();
            inputEntity = new InputEntity();
            blockRepository = new BlockRepository();
            idrecord_block = 0;
        }

        public void Player_Set(PlayerEntity playerEntity) {
            this.playerEntity = playerEntity;
        }

        public void Block_Add(BlockEntity block) {
            blockRepository.Add(block);
        }

        public void Block_Remove(BlockEntity block) {
            blockRepository.Remove(block);
        }

        public bool Block_TryGet(int id, out BlockEntity block) {
            return blockRepository.TryGet(id, out block);
        }

        public void Block_ForEach(System.Action<BlockEntity> action) {
            blockRepository.ForEach(action);
        }

        public void Block_Clear() {
            blockRepository.Clear();
        }

        public void ForEach_Block(System.Action<BlockEntity> action) {
            blockRepository.ForEach(action);
        }

    }

}