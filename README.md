# Laptop Rental App - OOSD Semester 2 CA3
## Carl Murray

### Notes:
- 3 models are used in the project: Laptop, Brand and Booking.
- The Laptop model has a foreign key to the Brand model.
- The Booking model has a foreign key to the Laptop model.
- On startup, the database is seeded with 3 brands, 6 laptops and no bookings.
- When the user selects a brand and booking dates, the "Search" button must be clicked for results to show.
- If any of the form inputs are modified, the search results will be cleared and the user must click "Search" again.
- A laptop cannot be booked if it is already booked for the selected dates.
- 