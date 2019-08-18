create database projectEstimator;
use projectEstimator;
create table projectDetails(
sno int identity(1,1) primary key,
projectName varchar(100) not null,
devActivityHours varchar(max),
Employee varchar(255));

create table users(
sno int identity(1,1) primary key,
userName varchar(255) not null,
password varchar(255) not null);

create table newProjects(
sno int identity(1,1) primary key,
projectName varchar(255) not null,
userComments varchar(255));



