using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WXRadio.WXSynthesizer
{
    public class Config
    {
        [XmlIgnore]
        public IntPtr CoopControl { get; set; }
        public string VoiceName { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public int CurrentChannel { get; set; } = 99;
        public Location Location { get; set; } = new Location();
        public string AlertType { get; set; } = "Sound";
        public bool ButtonBeeps { get; set; } = true;
        public List<Event> Events { get; set; } = new List<Event>();
    }

    public class Event
    {
        [XmlAttribute]
        public string FullName { get; set; } = "";
        [XmlText]
        public bool Enabled { get; set; }
    }

    public class Location
    {
        public string LocationType { get; set; } = "Any";
        public string LocationName { get; set; } = "";
    }
}
