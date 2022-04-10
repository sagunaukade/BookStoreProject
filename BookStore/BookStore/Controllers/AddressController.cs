using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL addressBL;
        public AddressController(IAddressBL addressBL)
        {
            this.addressBL = addressBL;
        }
        [HttpPost("AddAddress")]
        public IActionResult AddAddress(AddressModel address)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var addData = this.addressBL.AddAddress(address, userId);
                if (addData.Equals(" Address Added Successfully"))
                {
                    return this.Ok(new { Status = true, Response = addData });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Response = addData });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { status = false, Response = ex.Message });
            }
        }
        [HttpPut("UpdateAddress")]
        public IActionResult UpdateAddress(AddressModel address,int AddressId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var addData = this.addressBL.UpdateAddress(address,AddressId);
                if (addData != null)
                {
                    return this.Ok(new { Status = true, Message = "Address Updated Successfully", Response = addData });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Enter Correct AddressId or TypeId ", Response = addData });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { status = false, Response = ex.Message });
            }
        }
        [HttpDelete("DeleteAddress")]
        public IActionResult DeleteAddress(int addressId)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (this.addressBL.DeleteAddress(addressId))
                {
                    return this.Ok(new { Status = true, Message = "Address Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Enter Correct Address Id" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { status = false, Response = ex.Message });
            }
        }

    }
}
