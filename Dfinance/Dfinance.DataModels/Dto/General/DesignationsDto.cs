﻿using System.ComponentModel.DataAnnotations;

namespace Dfinance.DataModels.Dto.General
{
    public  class DesignationsDto
    {
        [Required(ErrorMessage = "Designations Name is mandatory!!")]
        public string Name { get; set; }

    }
}
