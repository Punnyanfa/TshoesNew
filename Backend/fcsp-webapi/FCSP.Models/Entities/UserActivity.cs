using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCSP.Models.Entities
{
    public class UserActivity : BaseEntity
    {
        public long UserId { get; set; }
        public long ViewedDesignId { get; set; }
        public string ViewAt { get; set; } = null!;
        public int ViewDuration { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual CustomShoeDesign ViewedDesign { get; set; } = null!;
    }
} 