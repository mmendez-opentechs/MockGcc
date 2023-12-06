namespace MockGcc.Service.State
{
    public class State
    {
        /** Mock PersonInfo State */
        public static readonly object MockPersonInfoStateLock = new object();

        public int MockPersonInfoRate { get; set; }

        private int _mockPersonInfoLatency;
        public int MockPersonInfoLatency
        {
            get { return GetMockPersonInfoLatency(); }
            private set { _mockPersonInfoLatency = value; }
        }

        public void SetMockPersonInfoLatency(int latency)
        {
            lock (MockPersonInfoStateLock)
            {
                MockPersonInfoLatency = latency;
            }
        }

        public int GetMockPersonInfoLatency()
        {
            lock (MockPersonInfoStateLock)
            {
                return _mockPersonInfoLatency;
            }
        }

        /** Mock Account State */
        public static readonly object MockAccountStateLock = new object();
        public int MockAccountRate { get; set; }

        private int _mockAccountLatency;
        public int MockAccountLatency {
            get { return GetMockAccountLatency(); }
            private set { _mockAccountLatency = value; }
        }

        public void SetMockAccountLatency(int latency)
        {
            lock (MockAccountStateLock)
            {
                _mockAccountLatency = latency;
            }
        }

        public int GetMockAccountLatency()
        {
            lock (MockAccountStateLock)
            {
                return _mockAccountLatency;
            }
        }
    }
}
