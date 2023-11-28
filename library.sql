-----start--------

--Create database

USE [master]
GO


CREATE DATABASE [Library]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Library', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Library.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Library_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Library_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

----------------------------------------------------------------------------------------------------------------

--Create tables

CREATE TABLE Books (
    book_id INT PRIMARY KEY,
    title VARCHAR(200),
    author VARCHAR(200),
    publication_year INT,
    isbn VARCHAR(20)
);

CREATE TABLE Users (
    user_id INT PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100),
    registration_date DATE
);

CREATE TABLE Borrowed_Books (
    borrow_id INT PRIMARY KEY,
    user_id INT,
    book_id INT,
    borrow_date DATE,
    return_date DATE,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (book_id) REFERENCES Books(book_id)
);

----------------------------------------------------------------------------------------------------

--insert sample data
(1, 'The Great Gatsby', 'F. Scott Fitzgerald', 1925, '978-3-16-148410-0'),
(2, 'To Kill a Mockingbird', 'J.K. Rowling', 1960, '978-0-06-112008-4'),
(3, '1984', 'George Orwell', 1949, '978-0-452-28423-4'),
(4, 'The Catcher in the Rye', 'J.D. Salinger', 1951, '978-0-316-76948-0'),
(5, 'The Hobbit', 'J.R.R. Tolkien', 1937, '978-0-395-15414-4'),
(6, 'The Lord of the Rings', 'J.R.R. Tolkien', 1954, '978-0-395-19395-4'),
(7, 'Pride and Prejudice', 'Jane Austen', 1813, '978-0-19-283355-6'),
(8, 'One Hundred Years of Solitude', 'Gabriel Garcia Marquez', 1967, '978-0-06-088328-7'),
(9, 'Brave New World', 'Aldous Huxley', 1932, '978-0-06-085052-4'),
(10, 'The Odyssey', 'Homer', -800, '978-0-14-044911-2'),
(11, 'The Great Expectations', 'Charles Dickens', 1861, '978-0-14-143956-3'),
(12, 'The Alchemist', 'Paulo Coelho', 1988, '978-0-06-112241-5'),
(13, 'Moby-Dick', 'Herman Melville', 1851, '978-0-14-243724-7'),
(14, 'Crime and Punishment', 'Fyodor Dostoevsky', 1866, '978-0-553-21295-8'),
(15, 'The Road', 'Cormac McCarthy', 2020, '978-0-307-28762-3');


INSERT INTO Users (user_id, first_name, last_name, email, registration_date) VALUES
(1, 'John', 'Doe', 'john.doe@example.com', '2023-01-15'),
(2, 'Jane', 'Smith', 'jane.smith@example.com', '2023-02-20'),
(3, 'Bob', 'Johnson', 'bob.johnson@example.com', '2023-03-10'),
(4, 'Alice', 'Williams', 'alice.williams@example.com', '2023-04-05'),
(5, 'Charlie', 'Brown', 'charlie.brown@example.com', '2023-05-15'),
(6, 'Eva', 'Green', 'eva.green@example.com', '2023-06-20'),
(7, 'Frank', 'Miller', 'frank.miller@example.com', '2023-07-10'),
(8, 'Grace', 'Taylor', 'grace.taylor@example.com', '2023-08-05'),
(9, 'Henry', 'Jones', 'henry.jones@example.com', '2023-09-15'),
(10, 'Ivy', 'Moore', 'ivy.moore@example.com', '2023-10-20'),
(11, 'Jack', 'Roberts', 'jack.roberts@example.com', '2023-11-10'),
(12, 'Kate', 'Johnson', 'kate.johnson@example.com', '2023-12-05'),
(13, 'Liam', 'Clark', 'liam.clark@example.com', '2024-01-15'),
(14, 'Mia', 'Brown', 'mia.brown@example.com', '2024-02-20'),
(15, 'Noah', 'Davis', 'noah.davis@example.com', '2024-03-10');


INSERT INTO Borrowed_Books (borrow_id, user_id, book_id, borrow_date, return_date) VALUES
(1, 1, 1, '2023-01-20', '2023-02-10'),
(2, 2, 3, '2023-02-05', '2023-03-01'),
(3, 1, 2, '2023-03-15', '2023-04-05'),
(4, 2, 5, '2023-04-10', '2023-05-05'),
(5, 3, 1, '2023-05-20', '2023-06-10'),
(6, 4, 12, '2023-06-15', '2023-07-05'),
(7, 4, 3, '2023-07-20', '2023-08-10'),
(8, 4, 7, '2023-08-15', '2023-09-05'),
(9, 9, 14, '2023-09-20', '2023-10-10'),
(10, 10, 1, '2023-10-15', '2023-11-05'),
(11, 11, 6, '2023-11-20', '2023-12-10'),
(12, 12, 2, '2023-12-15', '2024-01-05'),
(13, 13, 4, '2024-01-20', '2024-02-10'),
(14, 14, 11, '2024-02-15', '2024-03-01');

-------------------------------------------------------------------------------------------------

--Write a SQL query to retrieve the top 10 most borrowed books, along with the number of times each book has been borrowed.

--Answer:

--query to retrieve the top 10 most borrowed books, along with the number of times each book has been borrowed
SELECT TOP 10
    b.book_id,
    b.title,
    COUNT(bb.borrow_id) AS borrow_count
FROM
    Books b
JOIN
    Borrowed_Books bb ON b.book_id = bb.book_id
GROUP BY
    b.book_id, b.title
ORDER BY
    borrow_count DESC


--SELECT Clause:
--Selects the book id(b.book_id) book title (b.title) and the count of borrowings (COUNT(bb.borrow_id)).
--and limit the result to the top 10 rows
--FROM Clause:
--Specifies the tables involved in the query (books b).
--JOIN Clause:
--Establishes a join between the books and borrowed_books tables based on the book_id column.
--GROUP BY Clause:
--Groups the result set by the unique identifier of each book (b.book_id) and (b.title)
--ORDER BY Clause:
--Orders the result set in descending order based on the count of borrowings (borrow_count), ensuring the most borrowed books come first.


--------------------------------------------------------------------------------------------------------------------

--Create a stored procedure that calculates the average number of days a book is borrowed before being returned. 
--The procedure should take a book_id as input and return the average number of days.

--Defines the stored procedure named CalculateAverageBorrowTime that takes an @book_id parameter.

CREATE PROCEDURE CalculateAverageBorrowTime
    @book_id INT
AS
BEGIN  --begining of the stored procedure

    --Prevents the count of the number of rows affected by a Transact-SQL statement from being returned.
    SET NOCOUNT ON;

    --to store the calculated average duration
    DECLARE @avg_duration FLOAT;

    --Marks the beginning of the block where potential errors will be caught.
    BEGIN TRY

        -- Calculate the average duration of borrowing for the specified book
        -- and assigns it to the @avg_duration variable.

        SELECT @avg_duration = AVG(DATEDIFF(DAY, bb.borrow_date, bb.return_date))
        FROM Borrowed_Books bb
        WHERE bb.book_id = @book_id AND bb.return_date IS NOT NULL;
        -- Return the calculated average duration
        SELECT ISNULL(@avg_duration, 0) AS average_duration;

    --Marks the ending of the block where potential errors will be caught.
    END TRY

    --Marks the beginning of the block to handle errors.
    BEGIN CATCH
        -- Handle any errors  during the execution
        SELECT ERROR_MESSAGE() AS ErrorMessage;
    --Marks the ending of the block to handle errors.
    END CATCH;
END; --end of the stored procedure

--Execute the stored procedure for a specific book (e.g., book_id = 1)
EXEC CalculateAverageBorrowTime @book_id = 1;

---------------------------------------------------------------------------------------------------

--Write a query to find the user who has borrowed the most books from the library.

--Answer:

--Query to find the user who has borrowed the most books from the library
SELECT TOP 1
    u.user_id,
    u.first_name,
    u.last_name,
    COUNT(bb.borrow_id) AS books_borrowed
FROM
    Users u
JOIN
    Borrowed_Books bb ON u.user_id = bb.user_id
GROUP BY
    u.user_id, u.first_name, u.last_name
ORDER BY
    books_borrowed DESC;

--SELECT Clause:
--Selects the user id(user_id) first name (first_name) last name(last_name) 
--and the count of borrowings (COUNT(bb.borrow_id)) and limit the result to the top 1 row
--FROM Clause:
--Specifies the tables involved in the query (Users u).
--JOIN Clause:
--Establishes a join between the user and borrowed_books tables based on the user_id column.
--GROUP BY Clause:
--Groups the result set by the unique identifier of each user (u.user_id) , (u.first_name) and (u.last_name)
--ORDER BY Clause:
--Orders the result set in descending order based on the count of borrowings (borrow_count), 
--ensuring the most borrowed books come first.

----------------------------------------------------------------------------------------------------------

--Create an index on the publication_year column of the books table to improve query performance.

--Answer:
--Create index on publication_year on the Book table named idx_production_year
CREATE INDEX idx_publication_year ON Books (publication_year);


--------------------------------------------------------------------------------------------------------------

--Write a SQL query to find all books published in the year 2020 that have not been borrowed by any user.

--Answer:

--Query to find all books published in the year 2020 with no borrowers
SELECT
    b.book_id ,
    title,
    author,
    publication_year,
    isbn
    
FROM
    Books b
LEFT JOIN  -- always shows all column from table Books whether matchinng records found or not
    Borrowed_Books bb ON b.book_id = bb.book_id
WHERE
    b.publication_year = 2020 --books published in the year 2020
    AND bb.borrow_id IS NULL; --books with no borrow_id(no entry in Borrowed_Books)


-----------------------------------------------------------------------------------------------------------

--Design a SQL query that lists users who have borrowed books published by a specific author (e.g., "J.K. Rowling").

--Answer:

--Query to lists users who have borrowed books published by author 'J.K. Rowling'
SELECT
    u.user_id,        --list user details from table Users u
    u.first_name,
    u.last_name
FROM
    Users u
JOIN                  --Join User table with Borrowed_Book table to find matches on user_id column
    Borrowed_Books bb ON u.user_id = bb.user_id
JOIN
    Books b ON bb.book_id = b.book_id ----Join User table with Book table to find matches on book_id column
WHERE
    b.author = 'J.K. Rowling'; -- show only  matches with author 'Harper Lee' from the result

----------------------------------------------------------------------------------------------------------------------
--Create a trigger that automatically updates the return_date in the borrowed_books table to the current date when a book is returned

--Create a trigger named UpdateReturnDate on Table Borrowed_Books column return_date when  a book is returned
CREATE TRIGGER UpdateReturnDate
ON Borrowed_Books
AFTER UPDATE
AS
BEGIN
    IF UPDATE(return_date)  --check if return_date column already updated
    BEGIN
        UPDATE bb     -- Update the 'return_date' to the current date and time for relevant records       
        SET bb.return_date = GETDATE()  
        FROM Borrowed_Books bb
        INNER JOIN inserted i ON bb.borrow_id = i.borrow_id  --match with relevant records (inserted virtual table) to update
        WHERE i.return_date IS NOT NULL;                     --update only return_date is not null in inserted virtual table
    END
END;

--example
--update borrowed_books SET return_date = GETDATE()-10 where borrow_id=1 
--the trigger will update automatically the return_date to GETDATE() after the above update


---end-------