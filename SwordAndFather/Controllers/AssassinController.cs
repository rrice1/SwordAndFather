﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwordAndFather.Models;

namespace SwordAndFather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssassinController : ControllerBase
    {
        CreateAssassinRequestValidator _validator;
        AssassinRepository _repository;

        public AssassinController()
        {
            _validator = new CreateAssassinRequestValidator();
            _repository = new AssassinRepository();
        }
        [HttpPost("register")]
        public ActionResult AddAssassin(CreateAssassinRequest request)
        {
            if (_validator.Validate(request))
            {
                return BadRequest();
            }

            var newAssassin = _repository.AddAssassin(request.CodeName, request.Catchphrase, request.PreferredWeapon);

            return Created($"api/Assassin/{newAssassin.Id}", newAssassin);

        }
    }

    public class AssassinRepository
    {
        static readonly List<Assassin> assassins = new List<Assassin>();

        public Assassin AddAssassin(string codeName, string catchphrase, string preferredWeapon)
        {
            var newAssassin = new Assassin(codeName, catchphrase, preferredWeapon);

            newAssassin.Id = assassins.Count + 1;

            assassins.Add(newAssassin);

            return newAssassin;
        }
    }

    public interface IValidator<TToValidate>
    {
        bool Validate(TToValidate objectToValidate);
    }

    public class CreateAssassinRequestValidator : IValidator<CreateAssassinRequest>
    {
        public bool Validate(CreateAssassinRequest request)
        {
            return !(string.IsNullOrEmpty(request.Catchphrase) ||
                    string.IsNullOrEmpty(request.CodeName) ||
                    string.IsNullOrEmpty(request.PreferredWeapon));
        }
    }
}