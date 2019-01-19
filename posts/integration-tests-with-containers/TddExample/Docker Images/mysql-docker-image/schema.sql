CREATE DATABASE TestDb;

USE TestDb;

CREATE TABLE Books (
    Id varchar(36),
    Title varchar(255),
    Author varchar(255) NULL,
    Isbn varchar(255) NULL,
    NumPages int,

    PRIMARY KEY (Id)
);

INSERT INTO Books (Id, Title, Author, Isbn, NumPages)
VALUES ('409b0915-b494-4993-9211-a533fb78f70d', 'Clean Code', 'Robert C. Martin', '978-0131177055', 464);

INSERT INTO Books (Id, Title, Author, Isbn, NumPages)
VALUES ('95aedbbc-e385-4762-b513-5b579cd0ac64', 'Breakfast of Champions', 'Kurt Vonnegut', '978-1501263378', 378);