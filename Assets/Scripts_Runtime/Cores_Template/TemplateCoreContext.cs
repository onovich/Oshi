using System;
using System.Collections.Generic;
using System.Linq;

namespace Alter {

    public class TemplateCoreContext {

        Dictionary<int, BlockTM> blockTMs;

        public TemplateCoreContext() {
            blockTMs = new Dictionary<int, BlockTM>();
        }

        public void BlockTM_Add(int key, BlockTM BlockTM) {
            blockTMs.Add(key, BlockTM);
        }

        public bool BlockTM_TryGet(int key, out BlockTM BlockTM) {
            return blockTMs.TryGetValue(key, out BlockTM);
        }

        public BlockTM BlockTM_GetRandom(Random rd) {
            int index = rd.Next(blockTMs.Count);
            return blockTMs.Values.ElementAt(index);
        }

    }

}