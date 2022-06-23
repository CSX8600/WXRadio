using System;
using System.Collections.Generic;

namespace WXRadio.WeatherManager
{
    public class StormManager
    {
        public event EventHandler<Storm> StormAdded;

        private Dictionary<int, Storm> _stormsByID = new Dictionary<int, Storm>();

        private static StormManager _instance;
        public static StormManager INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StormManager();
                }

                return _instance;
            }
        }

        private StormManager() { }

        public void AddStorm(StormEventInfo stormEventInfo)
        {
            if (_stormsByID.ContainsKey(stormEventInfo.ID))
            {
                return;
            }

            Storm storm = new Storm(stormEventInfo);
            _stormsByID.Add(storm.ID, storm);

            StormAdded?.Invoke(this, storm);
        }

        public void UpdateStorm(StormEventInfo stormEventInfo)
        {
            if (!_stormsByID.ContainsKey(stormEventInfo.ID))
            {
                return;
            }

            _stormsByID[stormEventInfo.ID].UpdateStorm(stormEventInfo);
        }

        public void DeleteStorm(StormEventInfo stormEventInfo)
        {
            if (!_stormsByID.ContainsKey(stormEventInfo.ID))
            {
                return;
            }

            _stormsByID[stormEventInfo.ID].DeleteStorm();
            _stormsByID.Remove(stormEventInfo.ID);
        }

        public IReadOnlyCollection<Storm> GetStorms()
        {
            return _stormsByID.Values;
        }
    }
}
