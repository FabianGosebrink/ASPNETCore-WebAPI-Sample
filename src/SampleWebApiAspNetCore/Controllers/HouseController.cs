using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using Newtonsoft.Json;
using AutoMapper;
using SampleWebApiAspNetCore.Entities;
using System;

namespace SampleWebApiAspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class HouseController : Controller
    {
        private readonly IHouseRepository _houseRepository;

        public HouseController(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        [HttpGet]
        public IActionResult GetAllHouses()
        {
            var allHouseEntitys = _houseRepository.GetAll().ToList();

            var allHouseEntitysDto = allHouseEntitys.Select(x => Mapper.Map<HouseDto>(x));

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(new { totalCount = _houseRepository.Count() }));

            return Ok(allHouseEntitysDto);
        }

        [HttpGet("{id:int}", Name = "GetSingleHouse")]
        public IActionResult GetSingle(int id)
        {
            HouseEntity houseEntityFromRepo = _houseRepository.GetSingle(id);

            if (houseEntityFromRepo == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<HouseDto>(houseEntityFromRepo));
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<HouseUpdateDto> housePatchDocument)
        {
            if (housePatchDocument == null)
            {
                return BadRequest();
            }

            var existingHouse = _houseRepository.GetSingle(id);

            if (existingHouse == null)
            {
                return NotFound();
            }

            var houseToPatch = Mapper.Map<HouseUpdateDto>(existingHouse);
            housePatchDocument.ApplyTo(houseToPatch, ModelState);

            TryValidateModel(houseToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(houseToPatch, existingHouse);

            _houseRepository.Update(existingHouse);

            bool result = _houseRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when updating the house with id: {id}");
            }

            return Ok(Mapper.Map<HouseDto>(existingHouse));
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseCreateDto houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest("HouseEntitycreate object was null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HouseEntity toAdd = Mapper.Map<HouseEntity>(houseDto);

            _houseRepository.Add(toAdd);

            bool result = _houseRepository.Save();

            if (!result)
            {
                throw new Exception("something went wrong when adding a new House");
            }
            
            return CreatedAtRoute("GetSingleHouse", new { id = toAdd.Id }, Mapper.Map<HouseDto>(toAdd));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] HouseUpdateDto houseDto)
        {
            if (houseDto == null)
            {
                return BadRequest();
            }

            var existingHouseEntity = _houseRepository.GetSingle(id);

            if (existingHouseEntity == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(houseDto, existingHouseEntity);

            _houseRepository.Update(existingHouseEntity);

            bool result = _houseRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when updating the house with id: {id}");
            }

            return Ok(Mapper.Map<HouseDto>(existingHouseEntity));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var existingHouseEntity = _houseRepository.GetSingle(id);

            if (existingHouseEntity == null)
            {
                return NotFound();
            }

            _houseRepository.Delete(id);

            bool result = _houseRepository.Save();

            if (!result)
            {
                throw new Exception($"something went wrong when deleting the House with id: {id}");
            }

            return NoContent();
        }
    }
}
