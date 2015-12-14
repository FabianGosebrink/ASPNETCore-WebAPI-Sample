using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using SampleWebApiMVC6.Models;
using SampleWebApiMVC6.Services;
using System.Collections.Generic;
using Microsoft.AspNet.JsonPatch;

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
            return new JsonResult(Singleton.Instance.Houses.Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet("{id:int}", Name = "GetSingleHouse")]
        public IActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntity == null)
            {
                return new HttpNotFoundResult();
            }

            return new JsonResult(_houseMapper.MapToDto(houseEntity));
        }

        [HttpPatch]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<HouseDto> housePatchDocument)
        {
            if (housePatchDocument == null)
            {
                return HttpBadRequest();
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            HouseEntity houseEntity = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            HouseDto existingHouse = _houseMapper.MapToDto(houseEntity);

            housePatchDocument.ApplyTo(existingHouse, ModelState);

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            int index = Singleton.Instance.Houses.FindIndex(x => x.Id == id);
            Singleton.Instance.Houses[index] = _houseMapper.MapToEntity(existingHouse);

            return new JsonResult(existingHouse);
        }

        [HttpPost]
        public IActionResult Create([FromBody] HouseDto houseDto)
        {
            if (houseDto == null)
            {
                return new BadRequestResult();
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            HouseEntity houseEntity = _houseMapper.MapToEntity(houseDto);

            Singleton.Instance.Houses.Add(houseEntity);

            return new CreatedAtRouteResult("GetSingleHouse", new { id = houseEntity.Id }, _houseMapper.MapToDto(houseEntity));
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] HouseDto houseDto)
        {
            if (houseDto == null)
            {
                return new BadRequestResult();
            }

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            HouseEntity houseEntityToUpdate = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntityToUpdate == null)
            {
                return new HttpNotFoundResult();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            //Update to Database --> Is singleton in this case....

            return new JsonResult(_houseMapper.MapToDto(houseEntityToUpdate));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = Singleton.Instance.Houses.FirstOrDefault(x => x.Id == id);

            if (houseEntityToDelete == null)
            {
                return new HttpNotFoundResult();
            }

            Singleton.Instance.Houses.Remove(houseEntityToDelete);

            return new NoContentResult();
        }
    }
}
