create database projectEstimator;
use projectEstimator;
create table projectDetails(
sno int identity(1,1) primary key,
projectName varchar(100) not null,
devActivityHours varchar(max),
Employee varchar(255));

select * from projectDetails;

{"ProjectName":"adfasd","SQLCount":"1","SQLHours":"1",
"UICount":"1","UIHours":"1","ControllerCount":"1",
"ControllerHours":"1","UnitTestCount":"1","UnitTestHours":"1",
"TechnicalTestCount":"1","TechnicalTestHours":"1",
"BugCounts":"1","BugHours":"1"}
