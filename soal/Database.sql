CREATE DATABASE [MBCA]
GO
USE [MBCA]
GO

CREATE TABLE [PhonePrefix](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Prefix]            VARCHAR(200)	    NOT NULL
);
CREATE TABLE [Role](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Name]		        VARCHAR(200)	    NOT NULL
);
CREATE TABLE [User](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Username]	        VARCHAR(200)	    NOT NULL,
	[Password]	        VARCHAR(200)	    NOT NULL,
	[FullName]	        VARCHAR(200)	    NOT NULL,
	[Email]		        VARCHAR(200)	    NOT NULL,
	[PhoneNumber]	    VARCHAR(200)	    NOT NULL,
	[RoleID]		    INT                 NOT NULL,
	[IsActivated]	    BIT	            NOT NULL,

    CONSTRAINT FK_User_Role FOREIGN KEY ([RoleID]) REFERENCES [Role]([ID])
);
CREATE TABLE [EventCategory](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Name]		        VARCHAR(200)	    NOT NULL
);
CREATE TABLE [Event](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Title]			    VARCHAR(200)	    NOT NULL,
	[Description]	    TEXT	            NOT NULL,
	[Date]		        DATE	            NOT NULL,
	[StartTime]	        TIME	            NOT NULL,
	[EndTime]		    TIME	            NOT NULL,
	[Location]		    VARCHAR(200)	    NOT NULL,
	[Initiator]		    VARCHAR(200)	    NOT NULL,
	[Price]		        DECIMAL	            NOT NULL,
	[EventCategoryID]   INT                 NOT NULL,

    CONSTRAINT FK_Event_Category FOREIGN KEY ([EventCategoryID]) REFERENCES [EventCategory]([ID])
);
CREATE TABLE [EventBanner](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[EventID]		    INT	                NOT NULL,
	[Banner]		    TEXT	            NOT NULL,

    CONSTRAINT FK_EventBanner_Event FOREIGN KEY ([EventID]) REFERENCES [Event]([ID])
);
CREATE TABLE [ExhibitCategory](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Name]		        VARCHAR(200)	    NOT NULL
);
CREATE TABLE [Exhibit](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Name]		        VARCHAR(200)	    NOT NULL,
	[Artist]		    VARCHAR(200)	    NOT NULL,
	[TimePeriod]	    VARCHAR(200)	    NOT NULL,
	[Image]		        TEXT	            NOT NULL,
	[ExhibitCategoryID] INT	                NOT NULL,

    CONSTRAINT FK_Exhibit_ExhibitCategory FOREIGN KEY ([ExhibitCategoryID]) REFERENCES [ExhibitCategory]([ID])
);
CREATE TABLE [EventExhibit](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[EventID]		    INT	NOT NULL,
	[ExhibitID]		    INT	NOT NULL,

    CONSTRAINT FK_EventExhibit_Event FOREIGN KEY ([EventID]) REFERENCES [Event]([ID]),
    CONSTRAINT FK_EventExhibit_Exhibit FOREIGN KEY ([ExhibitID]) REFERENCES [Exhibit]([ID])
);
CREATE TABLE [ExhibitTags](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[ExhibitID]		    INT	                NOT NULL,
	[Tag]			    VARCHAR(200)	    NOT NULL,

    CONSTRAINT FK_ExhibitTags_Exhibit FOREIGN KEY ([ExhibitID]) REFERENCES [Exhibit]([ID])
);
CREATE TABLE [OTP](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[UserID]		    INT	                NOT NULL,
	[Code]		        VARCHAR(200)	    NOT NULL,
	[ValidUntil]	    DATETIME	        NOT NULL,

    CONSTRAINT FK_OTP_User FOREIGN KEY ([UserID]) REFERENCES [User]([ID])
);
CREATE TABLE [Promo](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[Code]		        VARCHAR(200)	    NOT NULL,
	[DiscountPercentage]DECIMAL	            NOT NULL,
	[StartDate]	        DATE	            NOT NULL,
	[EndDate]		    DATE	            NOT NULL,
);
CREATE TABLE [Ticket](
	[ID]			    INT				    PRIMARY KEY IDENTITY(1, 1),
	[TransactionDate]	DATETIME	        NOT NULL DEFAULT GETDATE(),
	[EventID]		    INT	                NOT NULL,
	[UserID]		    INT	                NOT NULL,
	[Qty]			    INT	                NOT NULL,
	[PromoID]		    INT		            NULL,
	[TotalPrice]	    DECIMAL             NOT NULL,

    CONSTRAINT FK_Ticket_Event FOREIGN KEY ([EventID]) REFERENCES [Event]([ID]),
    CONSTRAINT FK_Ticket_User FOREIGN KEY ([UserID]) REFERENCES [User]([ID]),
    CONSTRAINT FK_Ticket_Promo FOREIGN KEY ([PromoID]) REFERENCES [Promo]([ID]),
);



-- [Role]
SET IDENTITY_INSERT [Role] ON
GO
INSERT INTO [Role] ([ID], [Name]) VALUES
(1, 'Visitor'),
(2, 'Employee');
SET IDENTITY_INSERT [Role] OFF
GO

-- [EventCategory]
SET IDENTITY_INSERT [EventCategory] ON
GO
INSERT INTO [EventCategory] ([ID], [Name]) VALUES
(1, 'Adult'),
(2, 'Children'),
(3, 'All Ages');
SET IDENTITY_INSERT [EventCategory] OFF
GO

-- [User]
SET IDENTITY_INSERT [User] ON
GO
INSERT INTO [User] ([ID], [Username], [Password], [FullName], [Email], [PhoneNumber], [RoleID], [IsActivated]) VALUES
(1, 'johnsmith', '9b8769a4a742959a2d0298c36fb70623f2dfacda8436237df08d8dfd5b37374c', 'John Smith', 'john@example.com', '123-456-7890', 1, 1),
(2, 'emilyjones', '546526ee8366a03b217f250e9916210bec90c72c39d84c9de336682af81ab93e', 'Emily Jones', 'emily@example.com', '234-567-8901', 2, 1),
(3, 'michaelb', 'b2d9458c1aa028dd37883ae4b3184ef57917564d70937a5f1d7cfb0e6c97732c', 'Michael Brown', 'michaelb@example.com', '345-678-9012', 1, 1),
(4, 'sarahw', 'c170bfc3222cb45be21e2738148bccb3e099a6594830613b302c8e3fb7f1b8e8', 'Sarah Williams', 'sarahw@example.com', '456-789-0123', 2, 0),
(5, 'annalee', '83f768aac05b8e3b177ccb54784f3f26f50068264e569b80b7e1f7503357b9ae', 'Anna Lee', 'annalee@example.com', '567-890-1234', 1, 1);
SET IDENTITY_INSERT [User] OFF
GO

-- [Event]
SET IDENTITY_INSERT [Event] ON
GO
INSERT INTO [Event] ([ID], [Title], [Description], [Date], [StartTime], [EndTime], [Location], [Initiator], [Price], [EventCategoryID]) VALUES
(1, 'Art for Adults', 'Exhibition for adults', '2025-08-15', '18:00:00', '21:00:00', 'Art Gallery', 'John Smith', 15.0, 1),
(2, 'Kids Fun Day', 'Fun activities for kids', '2025-08-20', '10:00:00', '15:00:00', 'Community Hall', 'Emily Jones', 5.0, 2),
(3, 'Family Day Out', 'Suitable for all ages', '2025-09-01', '09:00:00', '18:00:00', 'City Park', 'John Smith', 10.0, 3),
(4, 'Modern Art [Exhibit]', 'Contemporary artworks', '2025-09-10', '11:00:00', '19:00:00', 'Modern Art Museum', 'Sarah Williams', 20.0, 1),
(5, 'Children Storytelling', 'Storytelling for children', '2025-08-25', '14:00:00', '16:00:00', 'Library Hall', 'Anna Lee', 7.5, 2);
SET IDENTITY_INSERT [Event] OFF
GO

-- [EventBanner]
SET IDENTITY_INSERT [EventBanner] ON
GO
INSERT INTO [EventBanner] ([ID], [EventID], [Banner]) VALUES
(1, 1, 'art_for_adults_1.jpg'),
(2, 1, 'art_for_adults_2.jpg'),
(3, 2, 'kids_fun_day_1.jpg'),
(4, 2, 'kids_fun_day_2.jpg'),
(5, 2, 'kids_fun_day_3.jpg'),
(6, 3, 'family_day_out_1.jpg'),
(7, 3, 'family_day_out_2.jpg'),
(8, 4, 'modern_art_exhibit_1.jpg'),
(9, 4, 'modern_art_exhibit_2.jpg'),
(10, 5, 'children_storytelling_1.jpg');
SET IDENTITY_INSERT [EventBanner] OFF
GO

-- [ExhibitCategory]
SET IDENTITY_INSERT [ExhibitCategory] ON
GO
INSERT INTO [ExhibitCategory] ([ID], [Name]) VALUES
(1, 'Painting'),
(2, 'Sculpture'),
(3, 'Photography'),
(4, 'Installation');
SET IDENTITY_INSERT [ExhibitCategory] OFF
GO

-- [Exhibit]
SET IDENTITY_INSERT [Exhibit] ON
GO
INSERT INTO [Exhibit] ([ID], [Name], [Artist], [TimePeriod], [Image], [ExhibitCategoryID]) VALUES
(1, 'The Blue Rider', 'Wassily Kandinsky', '1911', 'blue_rider.jpg', 1),
(2, 'Modern Sculpture', 'Henry Moore', '1950', 'modern_sculpture.jpg', 2),
(3, 'City Life', 'Jane Doe', '2020', 'city_life.jpg', 3),
(4, 'Sunset Installation', 'Mark Smith', '2023', 'sunset_installation.jpg', 4),
(5, 'Nature Paintings', 'Anna Green', '2018', 'nature_paintings.jpg', 1);
SET IDENTITY_INSERT [Exhibit] OFF
GO

-- [ExhibitTags]
SET IDENTITY_INSERT [ExhibitTags] ON
GO
INSERT INTO [ExhibitTags] ([ID], [ExhibitID], [Tag]) VALUES
(1, 1, 'abstract'),
(2, 1, 'expressionism'),
(3, 2, 'modern'),
(4, 3, 'urban'),
(5, 4, 'installation'),
(6, 5, 'nature'),
(7, 5, 'landscape');
SET IDENTITY_INSERT [ExhibitTags] OFF
GO

-- [EventExhibit]
SET IDENTITY_INSERT [EventExhibit] ON
GO
INSERT INTO [EventExhibit] ([ID], [EventID], [ExhibitID]) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 3, 3),
(4, 4, 4),
(5, 2, 5);
SET IDENTITY_INSERT [EventExhibit] OFF
GO

-- [Promo]
SET IDENTITY_INSERT [Promo] ON
GO
INSERT INTO [Promo] ([ID], [Code], [DiscountPercentage], [StartDate], [EndDate]) VALUES
(1, 'SUMMER20', 20.0, '2025-07-01', '2025-09-01'),
(2, 'KIDS10', 10.0, '2025-07-15', '2025-08-31'),
(3, 'FAMILY15', 15.0, '2025-08-01', '2025-09-30');
SET IDENTITY_INSERT [Promo] OFF
GO

-- [Ticket]
SET IDENTITY_INSERT [Ticket] ON
GO
INSERT INTO [Ticket] ([ID], [TransactionDate], [EventID], [UserID], [Qty], [PromoID], [TotalPrice]) VALUES
(1, '2025-07-26 10:00:00', 1, 1, 2, 1, 24.0),
(2, '2025-07-26 11:30:00', 2, 1, 1, NULL, 5.0),
(3, '2025-07-26 12:00:00', 3, 2, 3, 2, 27.0),
(4, '2025-07-27 09:00:00', 4, 3, 1, 3, 17.0),
(5, '2025-07-27 10:15:00', 5, 5, 2, NULL, 15.0);
SET IDENTITY_INSERT [Ticket] OFF
GO

-- Phone Prefix
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+1');      -- United States
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+44');     -- United Kingdom
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+62');     -- Indonesia
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+91');     -- India
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+81');     -- Japan
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+49');     -- Germany
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+86');     -- China
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+33');     -- France
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+39');     -- Italy
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+7');      -- Russia
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+34');     -- Spain
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+55');     -- Brazil
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+61');     -- Australia
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+27');     -- South Africa
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+63');     -- Philippines
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+65');     -- Singapore
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+66');     -- Thailand
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+82');     -- South Korea
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+90');     -- Turkey
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+370');    -- Lithuania
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+48');     -- Poland
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+351');    -- Portugal
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+46');     -- Sweden
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+41');     -- Switzerland
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+380');    -- Ukraine
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+972');    -- Israel
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+64');     -- New Zealand
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+20');     -- Egypt
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+254');    -- Kenya
INSERT INTO PhonePrefix ([Prefix]) VALUES ('+58');     -- Venezuela