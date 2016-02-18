using System.Linq;
using Microsoft.AspNet.Mvc;
using SampleWebApiMVC6.Models;
using SampleWebApiMVC6.Services;
using Microsoft.AspNet.JsonPatch;
using SampleWebApiMVC6.Repositories;

namespace SampleWebApiMVC6.Controllers
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
            return Ok(_houseRepository.GetAll().Select(x => _houseMapper.MapToDto(x)));
        }

        [HttpGet("{id:int}", Name = "GetSingleHouse")]
        public IActionResult GetSingle(int id)
        {
            HouseEntity houseEntity = _houseRepository.GetSingle(id);

            if (houseEntity == null)
            {
                return new HttpNotFoundResult();
            }

            return Ok(_houseMapper.MapToDto(houseEntity));
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

            HouseEntity houseEntity = _houseRepository.GetSingle(id);

            HouseDto existingHouse = _houseMapper.MapToDto(houseEntity);

            housePatchDocument.ApplyTo(existingHouse, ModelState);

            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            _houseRepository.Update(_houseMapper.MapToEntity(existingHouse));

            return Ok(existingHouse);
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

            _houseRepository.Add(houseEntity);

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

            HouseEntity houseEntityToUpdate = _houseRepository.GetSingle(id);

            if (houseEntityToUpdate == null)
            {
                return new HttpNotFoundResult();
            }

            houseEntityToUpdate.ZipCode = houseDto.ZipCode;
            houseEntityToUpdate.Street = houseDto.Street;
            houseEntityToUpdate.City = houseDto.City;

            //Update to Database --> Is singleton in this case....

            return Ok(_houseMapper.MapToDto(houseEntityToUpdate));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            HouseEntity houseEntityToDelete = _houseRepository.GetSingle(id);

            if (houseEntityToDelete == null)
            {
                return new HttpNotFoundResult();
            }

            _houseRepository.Delete(id);

            return new NoContentResult();
        }
    }
}
