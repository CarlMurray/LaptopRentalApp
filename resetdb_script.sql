delete from bookings;
delete from laptops;
delete from brands;
DBCC Checkident ("laptops", reseed, 0);
DBCC Checkident ("brands", reseed, 0);
DBCC Checkident ("bookings", reseed, 0);