CREATE DATABASE BookingManagement;
GO
drop database BookingManagement
USE BookingManagement;
GO

CREATE TABLE Campuses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Code NVARCHAR(50) UNIQUE NOT NULL,
    Location NVARCHAR(255),
	CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
);

CREATE TABLE Departments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Code NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(500),
    CampusId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_Departments_Campus FOREIGN KEY (CampusId) REFERENCES Campuses(Id)
);

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(20),
    Password NVARCHAR(255) NOT NULL,
    CampusId INT NOT NULL,
	RoleId INT NOT NULL,
    DepartmentId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_Users_Campus FOREIGN KEY (CampusId) REFERENCES Campuses(Id),
    CONSTRAINT FK_Users_Department FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
	CONSTRAINT FK_Users_Role FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

CREATE TABLE Rooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    Location NVARCHAR(255),
    CampusId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_Rooms_Campus FOREIGN KEY (CampusId) REFERENCES Campuses(Id)
);

CREATE TABLE Slots (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
);

CREATE TABLE Booking (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
	Status INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_Booking_User FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE BookingDetail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL,
    BookingDate DATE NOT NULL,
    SlotId INT NOT NULL,
    RoomId INT NOT NULL,
    Status INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_BookingDetail_Booking FOREIGN KEY (BookingId) REFERENCES Booking(Id),
    CONSTRAINT FK_BookingDetail_Slot FOREIGN KEY (SlotId) REFERENCES Slots(Id),
    CONSTRAINT FK_BookingDetail_Room FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
);

CREATE TABLE ApprovalHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CampusId INT NOT NULL,
    DepartmentId INT NOT NULL,
    UserBookingId INT NOT NULL,
    BookingId INT NOT NULL,
    BookingDetailId INT NOT NULL,
    HeadDepartmentId INT NOT NULL,
    ApprovalByHeadDepartment BIT NOT NULL,
    ReasonByHeadApproval NVARCHAR(500),
    ManagerId INT NOT NULL,
    ApprovalByManager BIT NOT NULL,
    ReasonByManager NVARCHAR(500),
	CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    DeleteAt DATETIME NULL
    CONSTRAINT FK_ApprovalHistory_Campus FOREIGN KEY (CampusId) REFERENCES Campuses(Id) ,
    CONSTRAINT FK_ApprovalHistory_Department FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    CONSTRAINT FK_ApprovalHistory_User FOREIGN KEY (UserBookingId) REFERENCES Users(Id),
    CONSTRAINT FK_ApprovalHistory_Booking FOREIGN KEY (BookingId) REFERENCES Booking(Id),
    CONSTRAINT FK_ApprovalHistory_BookingDetail FOREIGN KEY (BookingDetailId) REFERENCES BookingDetail(Id),
    CONSTRAINT FK_ApprovalHistory_HeadDepartment FOREIGN KEY (HeadDepartmentId) REFERENCES Users(Id),
    CONSTRAINT FK_ApprovalHistory_Manager FOREIGN KEY (ManagerId) REFERENCES Users(Id)
);
