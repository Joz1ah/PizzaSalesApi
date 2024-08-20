# Pizza Sales API

This project is a RESTful API for importing, storing, and retrieving pizza sales data. It was developed as part of a 24-hour C# .NET API challenge.

## Features

- Import sales data from CSV files
- Store data in a structured database
- Retrieve imported data through API endpoints
- Pagination support for large datasets
- Flexible data model parsing

## Technology Stack

- C# .NET
- ASP.NET Core Web API
- Entity Framework Core
- CsvHelper

## Project Structure

- `Controllers/`: Contains the SalesController for managing API endpoints
- `Models/`: Includes data models (Pizza, PizzaType, Order, OrderDetail)
- `Data/`: Houses the SalesDbContext for database operations

## Setup

1. Clone the repository
2. Ensure you have .NET SDK installed (version X.X or later)
3. Navigate to the project directory
4. Run `dotnet restore` to restore dependencies
5. Update the connection string in `appsettings.json` to point to your database
6. Run `dotnet ef database update` to create the database schema
7. Run `dotnet run` to start the API

## API Endpoints

### Import CSV Data
- **POST** `/api/Sales/Import`
  - Uploads a CSV file
  - Returns the file path of the uploaded file

### Retrieve Imported Data
- **GET** `/api/Sales/ImportedData`
  - Query Parameters:
    - `filePath`: Path of the imported CSV file
    - `model`: Type of data to retrieve (Pizzas, PizzaTypes, Orders, OrderDetails)
    - `page`: Page number for pagination (default: 1)
    - `pageSize`: Number of records per page (default: 1000)
  - Returns paginated data based on the specified model

## Usage Example

1. Import CSV data > Execute
2. Copy File Path
3. Retrieve Imported data > Paste File Path > Input Mode Type > Adjust Page size and Page number

## Error Handling

The API includes robust error handling and logging. Errors are returned with appropriate HTTP status codes and descriptive messages.

## Future Improvements

- Implement authentication and authorization
- Add more complex querying capabilities
- Introduce caching for frequently accessed data

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
