using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SampleWebApiMVC6.Models;
using SampleWebApiMVC6.Services;

namespace SampleWebApiMVC6.Controllers
{
    [Route("api/[controller]")]
    public class HouseController : Controller
    {
        private readonly IHouseMapper _houseMapper;

        public HouseController(IHouseMapper houseMapper)
        {
            _houseMapper = houseMapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(Singleton.Instance.Houses.Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet]
        [Route("{id:int}", Name="GetSingleHouse")]
        public IActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntity == null)
            {
                return new HttpNotFoundResult();
            }

            return new ObjectResult(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseDto houseDto)
        {
            if(houseDto == null)
            {
                return HttpBadRequest();
            }

            if(!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            HouseEntity houseEntity = _houseMapper.MapToEntity(houseDto);

            Singleton.Instance.Houses.Add(houseEntity);

            return CreatedAtRoute("GetSingleHouse", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Update(int id, [FromBody] HouseDto houseDto)
        {
            if(houseDto == null)
            {
                return HttpBadRequest();
            }

            if(!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            HouseEntity houseEntityToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntityToUpdate == null)
            {
                return new HttpNotFoundResult();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            //Update to Database --> Is singleton in this case....

            return new ObjectResult(_houseMapper.MapToDto(houseEntityToUpdate));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if(houseEntityToDelete == null)
            {
                return new HttpNotFoundResult();
            }

            Singleton.Instance.Houses.Remove(houseEntityToDelete);

            return new NoContentResult();
        }
    }
}
