﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Models
{
    public class GenreModel
    {
        [Key]
        public Guid GenreId { get; set; }

        public string GenreName { get; set; }

        public Guid? ParentId { get; set; }

        public GenreModel Parent { get; set; }

        public ICollection<GenreModel> Children { get; set; } = new List<GenreModel>();

        public IList<GamesAndGenresModel> GameAndGenre { get; set; } = new List<GamesAndGenresModel>();
    }
}