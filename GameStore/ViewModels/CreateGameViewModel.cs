﻿using System;
using System.Collections.Generic;

namespace GameStore.ViewModels
{
    public class CreateGameViewModel
    {
        public string GameName { get; set; }

        public string GameDescription { get; set; }

        public double GamePrice { get; set; }

        public DateTime GameReleaseDate { get; set; }

        public string GameDeveloper { get; set; }

        public string GameImgUrl { get; set; }

        public string GamePublisher { get; set; }

        public List<Guid> GenreIds { get; set; }
    }
}
