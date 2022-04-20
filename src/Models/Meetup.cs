﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Model for the meetup.json file. The model assigns each meetup an unique meetup ID
    /// location ID with a index, title, date, description and a list of attendees.
    /// </summary>
    public class Meetup
    {
        // Unique id for each meetup
        public string? meetup_id { get; set; }
        // location_id mapped to location model
        public string? location_id { get; set; }
        public int? index { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }
        // collection of attendee attending the event
        public Attendee? Attendees { get; set; }
        public string? Description { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Meetup>(this);
    }
}
