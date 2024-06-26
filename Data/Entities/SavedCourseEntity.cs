﻿namespace Data.Entities;

public class SavedCourseEntity
{
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
    public string CourseId { get; set; } = null!;
}
