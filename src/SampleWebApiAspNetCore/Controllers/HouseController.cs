using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using SampleWebApiAspNetCore.Services;

namespace SampleWebApiAspNetCore.Controllers
{
    [Route("api/[controller]")]
    public class HouseController : Controller
    {
        private readonly IHouseMapper _houseMapper;
        private readonly IHouseRepository _houseRepository;

        public HouseController(IHouseMapper houseMapper, IHouseRepository houseRepository)
        {
            _houseMapper = houseMapper;
            _houseRepository = houseRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_houseRepository.GetAll().Select(x => _houseMapper.MapToDto(x)));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id:int}", Name = "GetSingleHouse")]
        public IActionResult GetSingle(int id)
        {
            try
            {
                HouseEntity houseEntity = _houseRepository.GetSingle(id);

                if (houseEntity == null)
                {
                    return NotFound();
                }

                return Ok(_houseMapper.MapToDto(houseEntity));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<HouseDto> housePatchDocument)
        {
            try
            {
                if (housePatchDocument == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                HouseEntity houseEntity = _houseRepository.GetSingle(id);

                if (houseEntity == null)
                {
                    return NotFound();
                }

                HouseDto existingHouse = _houseMapper.MapToDto(houseEntity);

                housePatchDocument.ApplyTo(existingHouse, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _houseRepository.Update(_houseMapper.MapToEntity(existingHouse));

                return Ok(existingHouse);
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseDto houseDto)
        {
            try
            {
                if (houseDto == null)
                {
                    return new BadRequestResult();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                HouseEntity houseEntity = _houseMapper.MapToEntity(houseDto);

                _houseRepository.Add(houseEntity);

                return new CreatedAtRouteResult("GetSingleHouse", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] HouseDto houseDto)
        {
            try
            {
                if (houseDto == null)
                {
                    return new BadRequestResult();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                HouseEntity houseEntityToUpdate = _houseRepository.GetSingle(id);

                if (houseEntityToUpdate == null)
                {
                    return NotFound();
                }

                houseEntityToUpdate.ZipCode = houseDto.ZipCode;
                houseEntityToUpdate.Street = houseDto.Street;
                houseEntityToUpdate.City = houseDto.City;

                _houseRepository.Update(houseEntityToUpdate);

                return Ok(_houseMapper.MapToDto(houseEntityToUpdate));
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                HouseEntity houseEntityToDelete = _houseRepository.GetSingle(id);

                if (houseEntityToDelete == null)
                {
                    return NotFound();
                }

                _houseRepository.Delete(id);

                return new NoContentResult();
            }
            catch (Exception exception)
            {
                //logg exception or do anything with it
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
