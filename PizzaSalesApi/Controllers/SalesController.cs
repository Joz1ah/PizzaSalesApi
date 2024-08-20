using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using PizzaSalesApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;

namespace PizzaSalesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SalesController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SalesController(IConfiguration configuration, ILogger<SalesController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // POST: api/Sales/Import
        // This endpoint allows users to upload a CSV file.
        // The file is saved to the server's file system.
        [HttpPost("Import")]
        public IActionResult ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required.");
            }

            try
            {
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "files", file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Ok(new { FilePath = filePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while importing the CSV file");
                return StatusCode(500, $"An error occurred while importing the CSV file: {ex.Message}");
            }
        }

        // GET: api/Sales/ImportedData?filePath={filePath}&model={model}
        // This endpoint retrieves data from the imported CSV file.
        // It supports pagination through the 'page' and 'pageSize' parameters.
        [HttpGet("ImportedData")]
        public IActionResult GetImportedData(string filePath, string model, int page = 1, int pageSize = 1000)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return BadRequest($"{model}.csv file not exist.");
            }

            if (string.IsNullOrEmpty(model))
            {
                return BadRequest("Model type is required.");
            }

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Read();
                csv.ReadHeader();

                var records = new List<object>();
                int currentRecord = 0;
                int startRecord = (page - 1) * pageSize;
                int endRecord = startRecord + pageSize;

                while (csv.Read())
                {
                    if (currentRecord >= startRecord && currentRecord < endRecord)
                    {
                        switch (model)
                        {
                            case "Pizzas":
                                records.Add(csv.GetRecord<Pizza>());
                                break;
                            case "PizzaTypes":
                                records.Add(csv.GetRecord<PizzaType>());
                                break;
                            case "Orders":
                                records.Add(csv.GetRecord<Order>());
                                break;
                            case "OrderDetails":
                                records.Add(csv.GetRecord<OrderDetail>());
                                break;
                        }
                    }
                    currentRecord++;
                    if (currentRecord >= endRecord)
                    {
                        break;
                    }
                }

                return Ok(records);
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, "Error parsing CSV");
                return BadRequest($"Error parsing CSV: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
                return StatusCode(500, "An error occurred while retrieving data.");
            }
        }

        // GET: api/Sales/RecordCount?filePath={filePath}&model={model}
        // This endpoint returns the count of records in the specified CSV file.
        [HttpGet("RecordCount")]
        public IActionResult GetRecordCount(string filePath, string model)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return BadRequest($"{model}.csv file not exist.");
            }

            if (string.IsNullOrEmpty(model))
            {
                return BadRequest("Model type is required.");
            }

            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                csv.Read();
                csv.ReadHeader();

                int recordCount = 0;

                while (csv.Read())
                {
                    switch (model)
                    {
                        case "Pizzas":
                            csv.GetRecord<Pizza>();
                            break;
                        case "PizzaTypes":
                            csv.GetRecord<PizzaType>();
                            break;
                        case "Orders":
                            csv.GetRecord<Order>();
                            break;
                        case "OrderDetails":
                            csv.GetRecord<OrderDetail>();
                            break;
                    }
                    recordCount++;
                }

                return Ok(new { Model = model, RecordCount = recordCount });
            }
            catch (CsvHelperException ex)
            {
                _logger.LogError(ex, "Error parsing CSV");
                return BadRequest($"Error parsing CSV: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving record count");
                return StatusCode(500, "An error occurred while retrieving record count.");
            }
        }

        // DELETE: api/Sales/DeleteFile?filePath={filePath}
        // This endpoint deletes the specified CSV file from the server.
        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return BadRequest("File not found.");
            }

            try
            {
                System.IO.File.Delete(filePath);
                return Ok("File deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the file");
                return StatusCode(500, $"An error occurred while deleting the file: {ex.Message}");
            }
        }

        // Future Additions:
        // 1. Add an endpoint to update a specific record in the CSV file.
        // 2. Add an endpoint to delete a specific record from the CSV file.
        // 3. Implement authentication and authorization for the endpoints.
        // 4. Add validation for the CSV file format and content.
        // 5. Implement caching for frequently accessed data.
        // 6. Add support for different file storage options (e.g., cloud storage).
    }
}
