using System.Collections.Generic;

public class TemplateCoreContext {

    Dictionary<int, BlockTM> blockTMs;

    public TemplateCoreContext() {
        blockTMs = new Dictionary<int, BlockTM>();
    }

    public void BlockTM_Add(int key, BlockTM BlockTM) {
        blockTMs.Add(key, BlockTM);
    }

    bool BlockTM_TryGet(int key, out BlockTM BlockTM) {
        return blockTMs.TryGetValue(key, out BlockTM);
    }

}