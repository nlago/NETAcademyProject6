USE [CFIdentityDb]
GO

INSERT [dbo].[Categories] ([Name]) VALUES ('Nature');
INSERT [dbo].[Categories] ([Name]) VALUES ('Technology');
INSERT [dbo].[Categories] ([Name]) VALUES ('Health & Fitness');
INSERT [dbo].[Categories] ([Name]) VALUES ('Education');
INSERT [dbo].[Categories] ([Name]) VALUES ('Art');
INSERT [dbo].[Categories] ([Name]) VALUES ('TestCategory');

INSERT [dbo].[Projects] ([Title], [Descr], [Goalfunds], [Creator_Id], [Fundsrecv], [Category_Id], [StartDate], [EndDate]) 
VALUES ('MyFirstProject', 'My first project desc', 10000, 3, 100, 2, '2018/6/1', '2018/9/23');
INSERT [dbo].[Projects] ([Title], [Descr], [Goalfunds], [Creator_Id], [Fundsrecv], [Category_Id], [StartDate], [EndDate]) 
VALUES ('HappyHipo', 'HappyHipo desc', 3000, 4, 100, 3, '2018/6/4', '2018/10/27');

INSERT [dbo].[Updates] ([Project_Id], [Descr], [Timestamp]) 
VALUES (1, 'My first project update desc desc', '2018/6/2');

INSERT [dbo].[Packages] ([Donation_upperlim], [Reward], [Project_Id]) 
VALUES (50, 'Tsixla', 1);
INSERT [dbo].[Packages] ([Donation_upperlim], [Reward], [Project_Id]) 
VALUES (100, 'Glifitzouri', 1);

INSERT [dbo].[Backers_projects] ([User_Id], [Project_Id], [Package_Id]) 
VALUES (3, 1, 1);

select * from categories
select * from Backers_projects
select * from Packages
select * from Photos
select * from Projects
select * from Updates
select * from Videos