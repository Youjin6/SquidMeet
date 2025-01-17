﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Model for the users.json file. The model assigns each user a unique ID with a
    /// username, password, name (first and last name separated by a space), age, gender
    /// and a one sentence user biography that can be displayed on their profile.
    /// </summary>
    public class UserModel
    {
        // Unique ID assigned to a user
        public int? user_id { get; set; }
        // Unique username that the user chooses
        public string? username { get; set; }
        // Password that the user chooses
        public string? password { get; set; }
        // First name and last name separated by a space
        public string? name { get; set; }
        // Age that the user supplies
        public int age { get; set; }
        // Gender that the user supplies as a string
        public string? gender { get; set; }
        // String biography that the user supplies
        public string? bio { get; set; }

        public override string ToString() => JsonSerializer.Serialize<UserModel>(this);
    }
}
