﻿using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class LikesStaging
    {
        public long Id { get; set; }
        public long ChangeRequestId { get; set; }
        public byte LikeStatus { get; set; }
        public string LikedBy { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }

        public ChangeRequestStaging ChangeRequestStaging { get; set; }
    }
}
