using System.Collections.Generic;

namespace Alter {

    public class WorldEntity {

        public Queue<int> candidateBlockTypeIDs;
        public Queue<int> nextCandidateBlockTypeIDs;

        public WorldEntity() {
            candidateBlockTypeIDs = new Queue<int>(3);
            nextCandidateBlockTypeIDs = new Queue<int>(3);
        }

        public int CandidateBlockTypeIDs_Get(int index) {
            return candidateBlockTypeIDs.ToArray()[index];
        }

        public void Ctor() {

        }

    }

}