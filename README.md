# NorthRegion

MySQL query
```
CREATE DATABASE Product;

USE Product;

CREATE TABLE NorthRegion
(
    Id              int auto_increment  primary key,
    Name            varchar(255)        not null,
    Description     varchar(255)        null,
    Price           float               not null,
    ExpiredDate     varchar(10)         null,
    ImageFilename   varchar(255)        null,
    Source          varchar(255)        null
);
```