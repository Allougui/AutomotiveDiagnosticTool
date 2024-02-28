using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Newtonsoft.Json;


namespace AutomotiveDiagnosticTool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class DiagnosticController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public DiagnosticController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("basic")]
        
        public IActionResult GetBasicDiagnostic()
        {
            // Simulate basic diagnostic operation 
            bool diagnosticSuccess = PerformBasicDiagnostic(); 

            if (diagnosticSuccess)
            {
                var diagnosticInfo = new
                {
                    Status = "OK",
                    Message = "Basic diagnostic operation completed successfully",
                    Timestamp = DateTime.UtcNow
                };
                StoreBasicDiagnosticDataInDatabase(diagnosticInfo);

                return Ok(diagnosticInfo);
               
             
            }
            else
            {
                var diagnosticInfo = new
                {
                    Status = "Error",
                    Message = "Basic diagnostic operation failed",
                    Timestamp = DateTime.UtcNow
                };
                return BadRequest(diagnosticInfo);
            }
           
        }

        private void StoreBasicDiagnosticDataInDatabase(object diagnosticInfo)
        {
            // Obtain the authenticated user's UserId
            int userId = GetAuthenticatedUserId();

            // Create a new DiagnosticData object for basic diagnostic
            var diagnosticData = new DiagnosticData
            {
                Details = JsonConvert.SerializeObject(diagnosticInfo), 
                Timestamp = DateTime.UtcNow,
                UserId = userId,  
                DiagnosticType = "Basic" 
            };

            // Save the diagnostic data to the database
            _dbContext.DiagnosticData.Add(diagnosticData);
            _dbContext.SaveChanges();

            
        }

        private bool PerformBasicDiagnostic()
        {
            //Simulate basic diagnostic checks and tasks 
            Random random = new Random();
            int randomNumber = random.Next(1, 100);
            return randomNumber %2 == 0;
        }

        [HttpGet("detailed")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDetailedDiagnostic()
        {
            if (User.IsInRole("Admin"))
            {
                // Simulate detailed diagnostic operation for admin
                bool detailedDiagnosticSuccess = PerformDetailedDiagnostic();

                // Return detailed diagnostic information
                var detailedDiagnosticInfo = new
                {
                    Status = detailedDiagnosticSuccess ? "OK" : "Error",
                    Message = detailedDiagnosticSuccess ? "Detailed diagnostic operation went successfully" : "Detailed diagnostic operation failed",
                    Details = detailedDiagnosticSuccess ? (object)new
                    {
                        EngineHealth = "Good",
                        BatteryStatus = "Fully Charged",
                        TirePressure = new { FrontLeft = 32, FrontRight = 32, RearLeft = 30, RearRight = 30 }
                    } : new
                    {
                        TimestampAttribute = DateTime.UtcNow
                    }

                };

                // Store detailed diagnostic data in the database
                StoreDetailedDiagnosticDataInDatabase(detailedDiagnosticInfo);

                return detailedDiagnosticSuccess ? Ok(detailedDiagnosticInfo) : BadRequest(detailedDiagnosticInfo);
            }
            else
            {
                return Forbid();
            }
        }

        private void StoreDetailedDiagnosticDataInDatabase(object detailedDiagnosticInfo)
        {
            int userId = GetAuthenticatedUserId();
            var diagnosticData = new DiagnosticData
            {
                Details = JsonConvert.SerializeObject(detailedDiagnosticInfo),
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                DiagnosticType = "Detailed"
            };

            _dbContext.DiagnosticData.Add(diagnosticData);
            _dbContext.SaveChanges();
        }


        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return -1;

        }

        private bool PerformDetailedDiagnostic()
        {
            
            Random random = new Random();
            int randomNumber = random.Next(1, 100);

            return randomNumber % 3 == 0;
        }
    }
}
