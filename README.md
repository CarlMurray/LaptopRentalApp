# Laptop Rental App - OOSD Semester 2 CA3
## Carl Murray - S00248587

### Notes:
- 3 models are used in the project: Laptop, Brand and Booking.
- The Laptop model has a foreign key to the Brand model.
- The Booking model has a foreign key to the Laptop model.
- On startup, the database is seeded with 3 brands, 6 laptops and no bookings.
- When the user selects a brand and booking dates, the "Search" button must be clicked for results to show.
- If any of the form inputs are modified, the search results will be cleared and the user must click "Search" again.
- A laptop cannot be booked if it is already booked for the selected dates.


### Instructions:
- Install Entity Framework Core SQL Server package
- NOTE: The connection string is in ApplicationDbContext.cs and "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true" should be changed as required.
- Run the following commands in the Package Manager Console:
	- Add-Migration InitialCreate
	- Update-Database
- Run the project; the database should be seeded with data once the project is running.
- Test the project as required
- If needed, run resetdb_script.sql to reset the database.